using Godot;

public partial class WaveAttack : Area2D
{
    [Export]
    public int Health = 8;

    [Export]
    public float Speed = 75f;

    public Vector2 Direction = Vector2.Left;

    private Player player;

    private Sprite2D sprite;

    public override void _Ready()
    {
        AddToGroup("enemy");

        player =
            GetTree()
            .GetFirstNodeInGroup("player")
            as Player;
    }

    public override void _Process(double delta)
    {
        GlobalPosition +=
            Direction *
            Speed *
            (float)delta;
            
        if (
            player != null &&
            GlobalPosition.DistanceTo(
                player.GlobalPosition
            ) < 120
        )
        {
            player.TakeDamage(Health);

            QueueFree();
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        GD.Print(
            "Wave HP: " +
            Health +
            "/20"
        );

        if (Health <= 0)
        {
            QueueFree();
        }
    }
}