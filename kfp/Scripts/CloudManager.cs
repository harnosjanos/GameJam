using Godot;
using System.Collections.Generic;

public partial class CloudManager : Control
{
	private const float CloudSpeed = 10.0f;
	private const float CloudSpawnInterval = 4.0f;
	private const float MinimumSameCloudGap = 80.0f;

	private readonly List<Node2D> activeClouds = new();
	private readonly Dictionary<Node2D, int> activeCloudTypes = new();
	private PackedScene[] cloudScenes;
	private float spawnCountdown;

	public override void _Ready()
	{
		string[] scenePaths =
		{
			"res://Scenes/Tutorial_Cloud1.tscn",
			"res://Scenes/Tutorial_Cloud2.tscn",
			"res://Scenes/Tutorial_Cloud3.tscn"
		};

		cloudScenes = new PackedScene[scenePaths.Length];
		for (int i = 0; i < scenePaths.Length; i++)
		{
			cloudScenes[i] = GD.Load<PackedScene>(scenePaths[i]);
			if (cloudScenes[i] == null)
			{
				GD.PushError($"Could not load cloud scene: {scenePaths[i]}");
				cloudScenes = new PackedScene[0];
				return;
			}
		}

		SpawnCloud(true);
		spawnCountdown = CloudSpawnInterval;
	}

	public override void _Process(double delta)
	{
		float frameTime = (float)delta;
		spawnCountdown -= frameTime;
		if (spawnCountdown <= 0.0f)
		{
			spawnCountdown = SpawnCloud() ? CloudSpawnInterval : 0.5f;
		}

		float viewportWidth = GetViewportRect().Size.X;
		for (int i = activeClouds.Count - 1; i >= 0; i--)
		{
			Node2D cloud = activeClouds[i];
			cloud.Position += Vector2.Right * CloudSpeed * frameTime;

			Control visual = cloud.GetChild<Control>(0);
			if (cloud.Position.X + visual.Position.X > viewportWidth)
			{
				cloud.QueueFree();
				activeClouds.RemoveAt(i);
				activeCloudTypes.Remove(cloud);
			}
		}
	}

	private bool SpawnCloud(bool startVisible = false)
	{
		List<int> eligibleScenes = new();
		for (int sceneIndex = 0; sceneIndex < cloudScenes.Length; sceneIndex++)
		{
			bool hasEnoughGap = true;
			foreach (Node2D activeCloud in activeClouds)
			{
				if (activeCloudTypes[activeCloud] != sceneIndex)
				{
					continue;
				}

				Control activeVisual = activeCloud.GetChild<Control>(0);
				float activeCloudLeft = activeCloud.Position.X + activeVisual.Position.X;
				if (activeCloudLeft < MinimumSameCloudGap)
				{
					hasEnoughGap = false;
					break;
				}
			}

			if (hasEnoughGap)
			{
				eligibleScenes.Add(sceneIndex);
			}
		}

		if (eligibleScenes.Count == 0)
		{
			return false;
		}

		int eligibleIndex = (int)GD.RandRange(0, eligibleScenes.Count - 1);
		int sceneIndexToSpawn = eligibleScenes[eligibleIndex];
		Node2D cloud = cloudScenes[sceneIndexToSpawn].Instantiate<Node2D>();
		Control visual = cloud.GetChild<Control>(0);

		AddChild(cloud);
		float cloudLeft = startVisible ? 0.0f : -visual.Size.X;
		cloud.Position = new Vector2(cloudLeft - visual.Position.X, 0.0f);
		activeClouds.Add(cloud);
		activeCloudTypes.Add(cloud, sceneIndexToSpawn);
		return true;
	}
}
