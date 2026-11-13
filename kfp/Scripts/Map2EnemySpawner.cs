using Godot;

public partial class Map2EnemySpawner : Node
{
    [Export]
    public double SpawnInterval = 2.0;

    private double elapsed;

    private PackedScene seahorseScene;

    public override void _Ready()
    {
        seahorseScene =
            GD.Load<PackedScene>(
                "res://Scenes/Seahorse.tscn"
            );
    }

    public override void _Process(
        double delta
    )
    {
        elapsed += delta;

        if (elapsed >= SpawnInterval)
        {
            elapsed -= SpawnInterval;

            SpawnSeahorse();
        }
    }

    private void SpawnSeahorse()
    {
        SeahorseEnemy enemy =
            seahorseScene
            .Instantiate<SeahorseEnemy>();

        float x =
            (float)GD.RandRange(
                50,
                1870
            );

        enemy.GlobalPosition =
            new Vector2(
                x,
                500
            );

        GetTree()
            .CurrentScene
            .AddChild(enemy);
    }
}