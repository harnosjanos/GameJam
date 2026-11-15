using Godot;

public partial class Tutorial_ChildPhoneix : Node2D
{
	private Sprite2D _head;
	private Marker2D _frontMarker;

	public override void _Ready()
	{
		_head = GetNode<Sprite2D>("Head");
		_frontMarker = GetNode<Marker2D>("Head/Marker2D");
	}

	public override void _Process(double delta)
	{
		UpdateHead();
	}

	private void UpdateHead()
	{
		Vector2 direction = (GetGlobalMousePosition() - _head.GlobalPosition) * -1;

		_head.GlobalRotation = direction.Angle() - _frontMarker.Rotation;
		_head.FlipV = direction.X < 0;
	}
}