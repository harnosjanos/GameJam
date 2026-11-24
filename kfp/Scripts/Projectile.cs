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

        if (area.IsInGroup("tutorial_demon"))
        {
            area.GetParent().QueueFree();
            QueueFree();
            return;
        }

        //-----------------------------------------------------------
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
                !IsChargedProjectile &&
                !exploded &&
                !enemy.ChargedNovaTriggered &&
                CanSpawnNova()
            )
            {
                enemy.ChargedNovaTriggered = true;

                exploded = true;

                TriggerNovaCooldown();

                CallDeferred(
                    nameof(SpawnChargedNova),
                    hitPosition,
                    hitEnemyId
                );
            }

            GD.Print(
                $"Harpy Damage: {Damage}"
            );

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

            Vector2 hitPosition =
                seahorse.GlobalPosition;

            ulong hitEnemyId =
                seahorse.GetInstanceId();

            if (
                SkillManager.Instance.AttackLevel >= 6 &&
                !IsNovaProjectile &&
                !IsChargedProjectile &&
                !exploded &&
                !seahorse.ChargedNovaTriggered &&
                CanSpawnNova()
            )
            {
                seahorse.ChargedNovaTriggered = true;

                exploded = true;

                TriggerNovaCooldown();

                CallDeferred(
                    nameof(SpawnChargedNova),
                    hitPosition,
                    hitEnemyId
                );
            }

            GD.Print(
                $"Seahorse Damage: {Damage}"
            );

            seahorse.TakeDamage(Damage);

            if (!Pierce)
                QueueFree();

            return;
        }

        // ZEUSZ
        if (parent is Zeusz zeusz)
        {
            if (
                IgnoredEnemyId != 0 &&
                zeusz.GetInstanceId() == IgnoredEnemyId
            )
                return;

            zeusz.TakeDamage(Damage);

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
                !IsChargedProjectile &&
                !exploded &&
                CanSpawnNova()
            )
            {
                exploded = true;

                TriggerNovaCooldown();

                CallDeferred(
                    nameof(SpawnChargedNova),
                    posseidon.GlobalPosition,
                    posseidon.GetInstanceId()
                );
            }

            GD.Print(
                $"Posseidon Damage: {Damage}"
            );

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
                !IsChargedProjectile &&
                !exploded &&
                CanSpawnNova()
            )
            {
                exploded = true;

                TriggerNovaCooldown();

                CallDeferred(
                    nameof(SpawnChargedNova),
                    wave.GlobalPosition,
                    wave.GetInstanceId()
                );
            }

            GD.Print(
                $"Wave Damage: {Damage}"
            );

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
                !IsChargedProjectile &&
                !exploded &&
                CanSpawnNova()
            )
            {
                exploded = true;

                TriggerNovaCooldown();

                CallDeferred(
                    nameof(SpawnChargedNova),
                    cloud.GlobalPosition,
                    cloud.GetInstanceId()
                );
            }

            GD.Print(
                $"Cloud Damage: {Damage}"
            );

            cloud.TakeDamage(Damage);

            if (!Pierce)
                QueueFree();

            return;
        }

        //TENTACLE
        if (area is Tentacle tentacle)
        {
            if (
                IgnoredEnemyId != 0 &&
                tentacle.GetInstanceId() == IgnoredEnemyId
            )
                return;

            if (
                SkillManager.Instance.AttackLevel >= 6 &&
                !IsNovaProjectile &&
                !IsChargedProjectile &&
                !exploded &&
                CanSpawnNova()
            )
            {
                exploded = true;

                TriggerNovaCooldown();

                CallDeferred(
                    nameof(SpawnChargedNova),
                    tentacle.GlobalPosition,
                    tentacle.GetInstanceId()
                );
            }

            tentacle.TakeDamage(Damage);

            if (!Pierce)
                QueueFree();

            return;
        }

        //RAY
        if (parent is Ray ray)
        {
            if (
                IgnoredEnemyId != 0 &&
                ray.GetInstanceId() == IgnoredEnemyId
            )
                return;

            if (
                SkillManager.Instance.AttackLevel >= 6 &&
                !IsNovaProjectile &&
                !IsChargedProjectile &&
                !exploded &&
                !ray.ChargedNovaTriggered &&
                CanSpawnNova()
            )
            {
                ray.ChargedNovaTriggered = true;

                exploded = true;

                TriggerNovaCooldown();

                CallDeferred(
                    nameof(SpawnChargedNova),
                    ray.GlobalPosition,
                    ray.GetInstanceId()
                );
            }

            ray.TakeDamage(Damage);

            if (!Pierce)
                QueueFree();

            return;
        }

        //Fish
        if (parent is Fish fish)
        {
            if (
                IgnoredEnemyId != 0 &&
                fish.GetInstanceId() == IgnoredEnemyId
            )
                return;

            if (!fish.IsJumping)
                return;

            Vector2 hitPosition =
                fish.GlobalPosition;

            ulong hitEnemyId =
                fish.GetInstanceId();

            if (
                SkillManager.Instance.AttackLevel >= 6 &&
                !IsNovaProjectile &&
                !IsChargedProjectile &&
                !exploded &&
                !fish.ChargedNovaTriggered &&
                CanSpawnNova()
            )
            {
                fish.ChargedNovaTriggered = true;

                exploded = true;

                TriggerNovaCooldown();

                CallDeferred(
                    nameof(SpawnChargedNova),
                    hitPosition,
                    hitEnemyId
                );
            }

            fish.TakeDamage(Damage);

            if (!Pierce)
                QueueFree();

            return;
        }

        //Hippocampus
        if (parent is Hippocampus hippocampus)
        {
            if (
                IgnoredEnemyId != 0 &&
                hippocampus.GetInstanceId() == IgnoredEnemyId
            )
                return;

            if (!hippocampus.IsJumping)
                return;

            Vector2 hitPosition =
                hippocampus.GlobalPosition;

            ulong hitEnemyId =
                hippocampus.GetInstanceId();

            if (
                SkillManager.Instance.AttackLevel >= 6 &&
                !IsNovaProjectile &&
                !IsChargedProjectile &&
                !exploded &&
                !hippocampus.ChargedNovaTriggered &&
                CanSpawnNova()
            )
            {
                hippocampus.ChargedNovaTriggered = true;

                exploded = true;

                TriggerNovaCooldown();

                CallDeferred(
                    nameof(SpawnChargedNova),
                    hitPosition,
                    hitEnemyId
                );
            }

            hippocampus.TakeDamage(Damage);

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

            projectile.Damage = Damage;
            GD.Print(
                $"Nova Damage: {projectile.Damage}"
            );

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

    private Player GetPlayer()
    {
        return GetTree()
            .GetFirstNodeInGroup("player")
            as Player;
    }

    private bool CanSpawnNova()
    {
        return Time.GetTicksMsec() / 1000.0
            >= SkillManager.Instance.LastAttackNovaTime + 3.0;
    }

    private void TriggerNovaCooldown()
    {
        SkillManager.Instance.LastAttackNovaTime =
            Time.GetTicksMsec() / 1000.0;
    }
}