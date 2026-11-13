using Godot;

public partial class Projectile : Area2D
{
    public Vector2 Direction;

    public bool Pierce = false;

    public bool IsChargedProjectile = false;

    public float Speed = 600f;

    public int Damage = 1;

    public ulong IgnoredEnemyId = 0;

    private bool exploded = false;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    public override void _Process(double delta)
    {
        Position += Direction * Speed * (float)delta;
    }

    private void OnAreaEntered(Area2D area)
    {
        GD.Print(area.Name);

        if (!area.IsInGroup("enemy"))
            return;

        Node parent = area.GetParent();

        // HARPY
        if (parent is Enemy enemy)
        {
            if (
                IgnoredEnemyId != 0 &&
                enemy.GetInstanceId() == IgnoredEnemyId
            )
                return;

            Vector2 hitPosition =
                enemy.GlobalPosition;

            ulong hitEnemyId =
                enemy.GetInstanceId();

            if (
                SkillManager.Instance.AttackLevel >= 6 &&
                !IsChargedProjectile &&
                !exploded &&
                !enemy.ChargedNovaTriggered
            )
            {
                enemy.ChargedNovaTriggered = true;

                exploded = true;

                CallDeferred(
                    nameof(SpawnChargedNova),
                    hitPosition,
                    hitEnemyId
                );
            }

            enemy.TakeDamage(Damage);

            if (!Pierce)
                QueueFree();

            return;
        }

        // SEAHORSE
        if (parent is SeahorseEnemy seahorse)
        {
            if (
                IgnoredEnemyId != 0 &&
                seahorse.GetInstanceId() == IgnoredEnemyId
            )
                return;

            if (!seahorse.IsJumping)
                return;

            if (
                SkillManager.Instance.AttackLevel >= 6 &&
                !IsChargedProjectile &&
                !exploded &&
                !seahorse.ChargedNovaTriggered
            )
            {
                seahorse.ChargedNovaTriggered = true;

                exploded = true;

                CallDeferred(
                    nameof(SpawnChargedNova),
                    seahorse.GlobalPosition,
                    seahorse.GetInstanceId()
                );
            }

            seahorse.TakeDamage(Damage);

            if (!Pierce)
                QueueFree();

            return;
        }

        // POSSEIDON
        if (parent is Posseidon posseidon)
        {
            posseidon.TakeDamage(Damage);

            if (!Pierce)
                QueueFree();

            return;
        }

        // WAVE
        if (area is WaveAttack wave)
        {
            wave.TakeDamage(Damage);

            if (!Pierce)
                QueueFree();

            return;
        }
    }

    private void SpawnChargedNova(
        Vector2 position,
        ulong ignoredEnemyId
    )
    {
        PackedScene projectileScene =
            GD.Load<PackedScene>(
                "res://Scenes/Projectile.tscn"
            );

        int projectileCount =
            10 + (SkillManager.Instance.ChargeLevel / 2);

        for (int i = 0; i < projectileCount; i++)
        {
            float angle =
                Mathf.Tau * i / projectileCount;

            Vector2 direction =
                new Vector2(
                    Mathf.Cos(angle),
                    Mathf.Sin(angle)
                );

            Projectile projectile =
                projectileScene.Instantiate<Projectile>();

            projectile.IsChargedProjectile = true;

            projectile.IgnoredEnemyId =
                ignoredEnemyId;

            projectile.Pierce =
                SkillManager.Instance.ChargeLevel >= 6;

            projectile.GlobalPosition =
                position;

            projectile.Direction =
                direction.Normalized();

            projectile.Rotation =
                direction.Angle();

            GetTree()
                .CurrentScene
                .CallDeferred(
                    Node.MethodName.AddChild,
                    projectile
                );
        }
    }
}