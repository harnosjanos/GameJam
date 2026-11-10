
using Godot;

public partial class Projectile : Area2D
{
    public Vector2 Direction;

    public bool Pierce = false;

    public bool CanExplode = true;

    public float Speed = 600f;

    public int Damage = 1;

    public float ExplosionRadius = 150f;

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

        Enemy enemy = area.GetParent<Enemy>();

        if (enemy == null)
            return;

        // Alap sebzés az eltalált ellenfélnek
        enemy.TakeDamage(Damage);

        // Robbanás csak akkor, ha engedélyezve van
        if (
            CanExplode &&
            SkillManager.Instance.AttackLevel >= 6 &&
            !exploded
        )
        {
            exploded = true;
            Explode(enemy);
        }

        if (!Pierce)
        {
            QueueFree();
        }
    }

    private void Explode(Enemy originalEnemy)
    {
        // Robbanási effekt létrehozása
        ExplosionEffect effect = new ExplosionEffect();

        effect.MaxRadius = ExplosionRadius;
        effect.Duration = 0.3f;
        effect.PixelSize = 8f;

        GetTree().CurrentScene.AddChild(effect);

        effect.GlobalPosition = GlobalPosition;

        // Körön belüli ellenfelek sebzése
        foreach (Node node in GetTree().GetNodesInGroup("enemy"))
        {
            Enemy otherEnemy = node.GetParent<Enemy>();

            if (otherEnemy == null)
                continue;

            if (otherEnemy == originalEnemy)
                continue;

            float distance = GlobalPosition.DistanceTo(
                otherEnemy.GlobalPosition
            );

            if (distance <= ExplosionRadius)
            {
                otherEnemy.TakeDamage(Damage);
            }
        }
    }
}
