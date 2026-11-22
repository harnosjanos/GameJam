using Godot;
using System;

public partial class Map2EnemySpawner : Node
{
    [Export]
    public double SpawnInterval = 2.0;

    private double elapsed;

    private PackedScene seahorseScene;
    private PackedScene rayScene;
    private PackedScene fishScene;
    private PackedScene hippocampusScene;

    Player player;

    public override void _Ready()
    {
        seahorseScene =
            GD.Load<PackedScene>(
                "res://Scenes/Seahorse.tscn"
            );

        rayScene =
            GD.Load<PackedScene>(
                "res://Scenes/Ray.tscn"
            );

        fishScene =
            GD.Load<PackedScene>(
                "res://Scenes/Fish.tscn"
            );
        
        hippocampusScene =
            GD.Load<PackedScene>(
                "res://Scenes/Hippocampus.tscn"
            );

        player =
            GetTree()
            .GetFirstNodeInGroup("player")
            as Player;
    }

    public override void _Process(
        double delta
    )
    {
        elapsed += delta;

        if (elapsed >= SpawnInterval)
        {
            elapsed -= SpawnInterval;

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int type =
            (int)GD.RandRange(0, 3);

        switch (type)
        {
            case 0:
                SpawnSeahorse();
                break;

            case 1:
                SpawnRay();
                break;
            
            case 2:
                SpawnFish();
                break;
            
            case 3:
                SpawnHippocampus();
                break;
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
        AnimatedSprite2D enemySprite = enemy.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        enemySprite.FlipH = enemy.Position.X < player.Position.X;

        GetTree()
            .CurrentScene
            .AddChild(enemy);
    }

    private void SpawnRay()
    {
        Ray enemy =
            rayScene
            .Instantiate<Ray>();

        Random rand = new();

        int x = rand.Next(0, 1920);
        int y = rand.Next(0, 1080);

        bool spawned = false;

        while (!spawned)
        {
            if (
                player.GlobalPosition.DistanceTo(
                    new Vector2(x, y)
                ) < 300
            )
            {
                x = rand.Next(0, 1920);
                y = rand.Next(50, 350);
            }
            else
            {
                spawned = true;
            }
        }

        enemy.GlobalPosition =
            new Vector2(x, y);

        AnimatedSprite2D sprite =
            enemy.GetNode<AnimatedSprite2D>(
                "AnimatedSprite2D"
            );

        sprite.FlipH =
            player.GlobalPosition.X >
            enemy.GlobalPosition.X;

        GetTree()
            .CurrentScene
            .AddChild(enemy);
    }

    private void SpawnFish()
    {
        Fish enemy =
            fishScene
            .Instantiate<Fish>();

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
        AnimatedSprite2D enemySprite = enemy.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        enemySprite.FlipH = enemy.Position.X < player.Position.X;

        GetTree()
            .CurrentScene
            .AddChild(enemy);
    }

    private void SpawnHippocampus()
    {
        Hippocampus enemy =
            hippocampusScene
            .Instantiate<Hippocampus>();

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
        AnimatedSprite2D enemySprite = enemy.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        enemySprite.FlipH = enemy.Position.X < player.Position.X;

        GetTree()
            .CurrentScene
            .AddChild(enemy);
    }
}