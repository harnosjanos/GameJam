using Godot;

public partial class Projectile : Area2D
{
    public Vector2 Direction;

    public bool Pierce = false;

    public bool IsChargedProjectile = false;

    public bool IsNovaProjectile = false;

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
                !IsNovaProjectile &&
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

            // csak ez marad a speciális rész
            if (!seahorse.IsJumping)
                return;

            Vector2 hitPosition =
                seahorse.GlobalPosition;

            ulong hitEnemyId =
                seahorse.GetInstanceId();

            if (
                SkillManager.Instance.AttackLevel >= 6 &&
                !IsNovaProjectile &&
                !exploded &&
                !seahorse.ChargedNovaTriggered
            )
            {
                seahorse.ChargedNovaTriggered = true;

                exploded = true;

                CallDeferred(
                    nameof(SpawnChargedNova),
                    hitPosition,
                    hitEnemyId
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
            if (
                IgnoredEnemyId != 0 &&
                posseidon.GetInstanceId() == IgnoredEnemyId
            )
                return;
                
            if (
                SkillManager.Instance.AttackLevel >= 6 &&
                !IsNovaProjectile &&
                !exploded
            )
            {
                exploded = true;

                CallDeferred(
                    nameof(SpawnChargedNova),
                    posseidon.GlobalPosition,
                    posseidon.GetInstanceId()
                );
            }

            posseidon.TakeDamage(Damage);

            if (!Pierce)
                QueueFree();

            return;
        }

        // WAVE
        if (area is WaveAttack wave)
        {
            if (
                IgnoredEnemyId != 0 &&
                wave.GetInstanceId() == IgnoredEnemyId
            )
                return;

            if (
                SkillManager.Instance.AttackLevel >= 6 &&
                !IsNovaProjectile &&
                !exploded
            )
            {
                exploded = true;

                CallDeferred(
                    nameof(SpawnChargedNova),
                    wave.GlobalPosition,
                    wave.GetInstanceId()
                );
            }

            wave.TakeDamage(Damage);

            if (!Pierce)
                QueueFree();

            return;
        }

        // RAIN
        if (area is RainCloud cloud)
        {
            if (
                IgnoredEnemyId != 0 &&
                cloud.GetInstanceId() == IgnoredEnemyId
            )
                return;

            if (
                SkillManager.Instance.AttackLevel >= 6 &&
                !IsNovaProjectile &&
                !exploded
            )
            {
                exploded = true;

                CallDeferred(
                    nameof(SpawnChargedNova),
                    cloud.GlobalPosition,
                    cloud.GetInstanceId()
                );
            }

            cloud.TakeDamage(Damage);

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

            projectile.IsNovaProjectile = true;

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