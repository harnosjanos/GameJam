using Godot;

public partial class RainCloud : Area2D
{
    [Export]
    public int Health = 8;

    private Timer rainTimer;

    private PackedScene rainDropScene;

    private Player player;

    public override void _Ready()
    {
        AddToGroup("enemy");

        player =
            GetTree()
            .GetFirstNodeInGroup("player")
            as Player;

        rainTimer =
            GetNode<Timer>("RainTimer");

        rainDropScene =
            GD.Load<PackedScene>(
                "res://Scenes/RainDrop.tscn"
            );

        rainTimer.Timeout +=
            SpawnRainDrop;

        rainTimer.Start();
    }

    private void SpawnRainDrop()
    {
        PackedScene scene;

        if (GD.Randf() < 0.05f)
        {
            scene =
                GD.Load<PackedScene>(
                    "res://Scenes/HorseDrop.tscn"
                );
        }
        else
        {
            scene =
                rainDropScene;
        }

        Area2D drop =
            scene.Instantiate<Area2D>();

        drop.GlobalPosition =
            GlobalPosition +
            new Vector2(
                (float)GD.RandRange(-150, 150),
                0
            );

        GetTree()
            .CurrentScene
            .AddChild(drop);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        GD.Print(
            $"Cloud HP: {Health}/10"
        );

        if (Health <= 0)
        {
            Posseidon boss =
                GetTree()
                .GetFirstNodeInGroup("boss")
                as Posseidon;

            if (boss != null)
            {
                boss.CloudDestroyed();
            }

            QueueFree();
        }
    }
}