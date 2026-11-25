using Godot;
using System;

public partial class ThunderBolt : Area2D
{
	[Export] public int Damage = 5;
	[Export] public float Speed = 300f;
	[Export] public float Lifetime = 4f;

	public Vector2 Direction = Vector2.Down;

	private double lifetimeElapsed;
	private Player player;

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("lightning");
		player = GetTree().GetFirstNodeInGroup("player") as Player;
	}

	public override void _Process(double delta)
	{
		lifetimeElapsed += delta;
		if (lifetimeElapsed >= Lifetime)
		{
			QueueFree();
			return;
		}

		GlobalPosition += Direction * Speed * (float)delta;
	}

	private void OnAreaEntered(Area2D area)
	{
		if (!area.IsInGroup("player"))
			return;

		player?.TakeDamage(Damage);
		QueueFree();
	}
}
