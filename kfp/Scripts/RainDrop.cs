using Godot;

public partial class RainDrop : Area2D
{
    [Export]
    public int Damage = 1;

    [Export]
    public float Speed = 200f;

    private Player player;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;

        player =
            GetTree()
            .GetFirstNodeInGroup("player")
            as Player;
    }

    public override void _Process(
        double delta
    )
    {
        GlobalPosition +=
            Vector2.Down *
            Speed *
            (float)delta;
    }

    private void OnAreaEntered(
        Area2D area
    )
    {
        if (!area.IsInGroup("player"))
            return;

        player.TakeDamage(
            Damage
        );

        QueueFree();
    }
}