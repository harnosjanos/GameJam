using System;
using Godot;

public partial class Tutorial_ChildPhoneix : Node2D
{
	public event Action ShotFinished; //ez egy event
	public event Action ChargedAttackFired;
	private Sprite2D _head;
	private Marker2D _frontMarker;

	private bool canUseChargedAttack;
	private bool chargedAttackUsed;
	private bool isCharging;
	private float chargeTime;

	private const float ChargedAttackHoldDuration = 3.0f;

	bool shooted; //ez egy logikai változó

	bool canShoot;

	public void EnableShooting()
	{
		canShoot = true;
	}

	public void EnableChargedAttack()
	{
		canUseChargedAttack = true;
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
		ShotFinished?.Invoke();
	}

	private void FireChargedAttack()
	{
		if (!canUseChargedAttack || chargedAttackUsed)
			return;

		chargedAttackUsed = true;
		canUseChargedAttack = false;

		const int projectileCount = 10;
		Vector2 spawnPosition = _frontMarker.GlobalPosition;
		PackedScene projectileScene = GD.Load<PackedScene>(
			"res://Scenes/Projectile.tscn"
		);

		for (int i = 0; i < projectileCount; i++)
		{
			float angle = Mathf.Tau * i / projectileCount;
			Vector2 direction = new Vector2(
				Mathf.Cos(angle),
				Mathf.Sin(angle)
			);

			Projectile projectile = projectileScene.Instantiate<Projectile>();
			projectile.IsChargedProjectile = true;
			projectile.GlobalPosition = spawnPosition;
			projectile.Direction = direction;
			projectile.Rotation = direction.Angle();

			GetTree().CurrentScene.AddChild(projectile);
		}

		ChargedAttackFired?.Invoke();
	}

	public override void _Input(InputEvent @event)
	{

		if (@event is InputEventMouseButton chargedMouseButton &&
				chargedMouseButton.ButtonIndex == MouseButton.Left &&
				canUseChargedAttack &&
				!chargedAttackUsed)
		{
			if (chargedMouseButton.Pressed)
			{
				isCharging = true;
				chargeTime = 0.0f;
			}
			else if (isCharging)
			{
				isCharging = false;

				if (chargeTime >= ChargedAttackHoldDuration)
					FireChargedAttack();
			}

			return;
		}

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

		if (isCharging && Input.IsMouseButtonPressed(MouseButton.Left))
			chargeTime += (float)delta;
	}

	private void UpdateHead()
	{
		Vector2 direction = (GetGlobalMousePosition() - _head.GlobalPosition) * -1;

		_head.GlobalRotation = direction.Angle() - _frontMarker.Rotation;
		_head.FlipV = direction.X < 0;
	}
}