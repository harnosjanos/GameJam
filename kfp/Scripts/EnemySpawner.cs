using Godot;
using System;

public partial class EnemySpawner : Node
{
	[Export] float radius = 150;
	[Export] double SpawnInterval { get; set; } = 2.0;
	Player player;
	float playerSpawnX;
	float playerSpawnY;
	double elapsed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetTree()
			.GetFirstNodeInGroup("player") as Player;

		playerSpawnX = player.Position.X;
		playerSpawnY = player.Position.Y;
	}

	public void EnemySpawn()
	{
		Random random = new();
		Enemy enemyCopy;
		switch (random.Next(2))
		{
			case 0:
				enemyCopy = GD.Load<PackedScene>("res://Scenes/Harpy.tscn").Instantiate<Enemy>();
				GD.Print("harpy 0");
				break;
			default:
				enemyCopy = GD.Load<PackedScene>("res://Scenes/Harpy.tscn").Instantiate<Enemy>();
				GD.Print("harpy default");
				break;
		}
		GetTree().CurrentScene.AddChild(enemyCopy);

		Random rand = new Random();
		int x = rand.Next(0, 1000);
		int y = rand.Next(0, 500);
		bool spawned = false;
		bool xOk = false;
		bool yOk = false;
		while (!spawned)
		{
			if (playerSpawnX - radius < x && x < playerSpawnX + radius)
			{
				x = rand.Next(0, 1000);
			}
			else
			{
				xOk = true;
			}

			if (playerSpawnY - radius < y && y < playerSpawnY - radius && 400 < y)
			{
				y = rand.Next(0, 500);
			}
			else
			{
				yOk = true;
			}

			if (xOk && yOk)
			{
				spawned = true;
			}
		}

		AnimatedSprite2D enemySprite = enemyCopy.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		enemySprite.FlipH = playerSpawnX < x;
		enemyCopy.GlobalPosition = new Vector2(x, y);
	}

	public override void _Process(double delta)
	{
		elapsed += delta;

		if (0 < player.Health && SpawnInterval <= elapsed)
		{
			elapsed -= SpawnInterval;
			EnemySpawn();
		}
	}
}
