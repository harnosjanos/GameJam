using Godot;

public partial class Tentacle : Area2D
{
    [Export]
    public int Health = 10;

    public override void _Ready()
    {
        AddToGroup("enemy");
        AddToGroup("tentacle");
        ZIndex = -1;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            QueueFree();
        }
    }
}