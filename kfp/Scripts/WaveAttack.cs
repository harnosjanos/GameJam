using Godot;

public partial class WaveAttack : Area2D
{
    [Export]
    public int Health = 20;

    [Export]
    public float Speed = 75f;

    public Vector2 Direction =
        Vector2.Left;

    private Player player;

    public override void _Ready()
    {
        AddToGroup("enemy");

        AreaEntered += OnAreaEntered;

        player =
            GetTree()
            .GetFirstNodeInGroup("player")
            as Player;
    }

    private void OnAreaEntered(
        Area2D area
    )
    {
        GD.Print("WAVE TOUCHED: " + area.Name);

        if (!area.IsInGroup("player"))
            return;

        bool survived =
            player.Health > Health;

        player.TakeDamage(Health);

        if (survived)
        {
            player.MoveToMarker("Center");

            Posseidon boss =
                GetTree()
                .GetFirstNodeInGroup("boss")
                as Posseidon;

            boss.EndCurrentAttack();
        }

        QueueFree();
    }

    public override void _Process(
        double delta
    )
    {
        GlobalPosition +=
            Direction *
            Speed *
            (float)delta;
    }

    public void TakeDamage(
        int damage
    )
    {
        Health -= damage;

        GD.Print(
            $"Wave HP: {Health}/20"
        );

        if (Health <= 0)
        {
            if (IsInstanceValid(player))
            {
                player.MoveToMarker("Center");
            }

            Posseidon boss =
                GetTree()
                .GetFirstNodeInGroup("boss")
                as Posseidon;

            if (boss != null)
            {
                boss.EndCurrentAttack();
            }

            QueueFree();
        }
    }
}