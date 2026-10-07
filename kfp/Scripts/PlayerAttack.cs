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

    private void Shoot()
    {
        if (reloading)
            return;

        if (mana <= 0)
            return;

        Projectile projectile =
            ProjectileScene.Instantiate<Projectile>();

        projectile.GlobalPosition = GlobalPosition;

        projectile.Direction =
            (GetGlobalMousePosition() - GlobalPosition)
            .Normalized();

        projectile.Rotation =
            projectile.Direction.Angle();

        GetTree().CurrentScene.AddChild(projectile);

        mana--;

        if (mana <= 0)
        {
            Reload();
        }
    }

    private void ChargedAttack()
    {
        if (reloading)
            return;

        if (mana < 5)
            return;

        mana -= 5;

        int projectileCount = 10;

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

            projectile.GlobalPosition = GlobalPosition;

            projectile.Direction = direction;

            projectile.Rotation =
                direction.Angle();

            GetTree().CurrentScene.AddChild(projectile);
        }

        if (mana <= 0)
        {
            Reload();
        }
    }
}