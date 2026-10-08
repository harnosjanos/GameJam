using Godot;

public partial class Projectile : Area2D
{
    public Vector2 Direction;

    public float Speed = 600f;

    public int Damage = 1;

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
        if (area.IsInGroup("enemy"))
        {
            Enemy enemy = area.GetParent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
            }

            QueueFree();
        }
    }
}