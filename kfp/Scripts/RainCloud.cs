using Godot;

public partial class RainCloud : Area2D
{
    [Export]
    public int Health = 8;

    public bool RemovedByBoss = false;

    private Timer rainTimer;

    private PackedScene rainDropScene;

    private Player player;

    private float moveTimer = 0f;

    [Export]
    public float MoveSpeed = 20f;

    private Vector2 startPosition;

    public override void _Ready()
    {
        AddToGroup("enemy");
        AddToGroup("rain_cloud");

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

        startPosition = GlobalPosition;
    }

    public override void _Process(
        double delta
    )
    {
        moveTimer += (float)delta;

        GlobalPosition =
            startPosition +
            new Vector2(
                Mathf.Sin(moveTimer) * 350f,
                0
            );
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
        if (Health <= 0)
        {
            Posseidon boss =
                GetTree()
                .GetFirstNodeInGroup("boss")
                as Posseidon;

            if (
                boss != null &&
                !RemovedByBoss
            )
            QueueFree();
        }
    }
}