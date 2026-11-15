using Godot;
using System;

public partial class Feather : Node2D
{
	[Export] private float acceleration = 600.0f;

	[Export] private Vector2 direction = new Vector2(1, 0);

	public override void _Process(double delta)
	{
		Position += direction * acceleration * (float)delta;


		if (GlobalPosition.X > 2000f)
		{
			QueueFree();
		}
	}
}