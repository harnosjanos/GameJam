using Godot;
using System;

public partial class EnemySpawner : Node
{
	[Export] float radius = 150;
	[Export] PackedScene enemy;
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
		Enemy enemyCopy = enemy.Instantiate<Enemy>();
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
				GD.Print($"x nem megfelelő {x}");
			}
			else
			{
				xOk = true;
			}

			if (playerSpawnY - radius < y && y < playerSpawnY - radius && 400 < y)
			{
				y = rand.Next(0, 500);
				GD.Print("y nem megfelelő");
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
