using Godot;

public partial class Player
{
    private void HandleAttackInput(double delta)
    {
        if (Input.IsActionJustPressed("shoot"))
        {
            charging = true;
            chargeTime = 0f;
        }

        if (Input.IsActionPressed("shoot"))
        {
            chargeTime += (float)delta;
        }

        if (Input.IsActionJustReleased("shoot"))
        {
            if (chargeTime >= 1.0f)
            {
                ChargedAttack();
            }
            else
            {
                Shoot();
            }

            charging = false;
            chargeTime = 0f;
        }
    }

    private async void Shoot()
    {
        if (reloading)
            return;

        if (attackAnimating)
            return;

        if (mana <= 0)
            return;

        attackAnimating = true;

        Body.Play("attack");

        await ToSignal(
            Body,
            AnimatedSprite2D.SignalName.AnimationFinished
        );

        Body.Play("idle");

        Vector2 spawnPosition =
            GlobalPosition +
            new Vector2(0, -50);

        Vector2 baseDirection =
            (GetGlobalMousePosition() - spawnPosition)
            .Normalized();

        int projectileCount = 1;

        if (SkillManager.Instance.AttackLevel >= 2)
            projectileCount++;

        if (SkillManager.Instance.AttackLevel >= 4)
            projectileCount++;

        if (projectileCount == 1)
        {
            SpawnProjectile(baseDirection);
        }
        else if (projectileCount == 2)
        {
            SpawnProjectile(baseDirection.Rotated(-0.15f));
            SpawnProjectile(baseDirection.Rotated(0.15f));
        }
        else if (projectileCount == 3)
        {
            SpawnProjectile(baseDirection);
            SpawnProjectile(baseDirection.Rotated(-0.25f));
            SpawnProjectile(baseDirection.Rotated(0.25f));
        }

        mana--;

        GD.Print("Mana: " + mana + "/" + maxMana);

        if (mana <= 0)
        {
            if (SkillManager.Instance.ManaLevel >= 6)
            {
                TriggerManaUltimate();
            }

            Reload();
        }
        attackAnimating = false;
    }

    private void SpawnProjectile(Vector2 direction)
    {
        Projectile projectile =
            ProjectileScene.Instantiate<Projectile>();

        projectile.Damage =
            GetNormalDamage();

        projectile.IsChargedProjectile = false;

        projectile.GlobalPosition =
            GlobalPosition + new Vector2(0, -50);

        projectile.Direction =
            direction;

        projectile.Rotation =
            direction.Angle();

        GetTree().CurrentScene.AddChild(projectile);
    }

    private void ChargedAttack()
    {
        if (reloading)
            return;

        if (mana < 5)
            return;

        mana -= 5;

        int projectileCount = 10 + (SkillManager.Instance.ChargeLevel / 2);

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
                ProjectileScene.Instantiate<Projectile>();
            
            projectile.Damage =
                GetChargedDamage();
            
            projectile.IsChargedProjectile = true;
            
            projectile.Pierce =
                SkillManager.Instance.ChargeLevel >= 6;

            projectile.GlobalPosition =
                GlobalPosition + new Vector2(0, -50);

            projectile.Direction =
                direction.Normalized();

            projectile.Rotation =
                direction.Angle();

            GetTree().CurrentScene.AddChild(projectile);
        }

        GD.Print("Charged Attack!");
        GD.Print("Mana: " + mana + "/" + maxMana);

        if (mana <= 0)
        {
            if (SkillManager.Instance.ManaLevel >= 6)
            {
                TriggerManaUltimate();
            }

            Reload();
        }
    }

    private void UltimateChargedAttack()
    {
        int projectileCount = 10 +
            (SkillManager.Instance.ChargeLevel / 2);

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
                ProjectileScene.Instantiate<Projectile>();
                projectile.IsChargedProjectile = true;

            projectile.Damage =
                GetChargedDamage();

            projectile.GlobalPosition =
                GlobalPosition + new Vector2(0, -50);

            projectile.Direction =
                direction.Normalized();

            projectile.Rotation =
                direction.Angle();

            projectile.Pierce =
                SkillManager.Instance.ChargeLevel >= 6;

            GetTree().CurrentScene.AddChild(projectile);
        }
    }

    private async void TriggerManaUltimate()
    {
        await ToSignal(
            GetTree().CreateTimer(0.1),
            SceneTreeTimer.SignalName.Timeout
        );

        UltimateChargedAttack();
    }
}