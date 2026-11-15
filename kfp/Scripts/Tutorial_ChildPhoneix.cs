using Godot;

public partial class Tutorial_ChildPhoneix : Node2D
{
	private Sprite2D _head;
	private Marker2D _frontMarker;

	bool shooted;

	bool canShoot;

	public void EnableShooting()
	{
		canShoot = true;
	}
	public void ShootOnce()
	{
		if (!canShoot || shooted)
			return;

		shooted = true;

		Vector2 spawnPosition = _frontMarker.GlobalPosition;
		Vector2 direction = (GetGlobalMousePosition() - spawnPosition).Normalized();

		Projectile projectile = GD.Load<PackedScene>(
			"res://Scenes/Projectile.tscn"
		).Instantiate<Projectile>();

		projectile.IsChargedProjectile = false;
		projectile.GlobalPosition = spawnPosition;
		projectile.Direction = direction;
		projectile.Rotation = direction.Angle();

		GetTree().CurrentScene.AddChild(projectile);
	}

	public override void _Input(InputEvent @event)
	{
		if (!canShoot || shooted)
		{
			return;
		}

		if (@event is InputEventMouseButton mouseButton &&
		mouseButton.ButtonIndex == MouseButton.Left &&
		mouseButton.Pressed)
		{
			ShootOnce();
		}
	}

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