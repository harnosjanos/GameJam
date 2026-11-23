using Godot;
using System.Threading.Tasks;

public partial class Zeusz : Node2D
{
	[Export] public float LightningMinInterval = 2.5f;
	[Export] public float LightningMaxInterval = 5.0f;
	[Export] public float TeleportMinInterval = 9.0f;
	[Export] public float TeleportMaxInterval = 15.0f;
	[Export] public int Health = 100;

	private PackedScene lightningScene;
	private Timer lightningTimer;
	private Timer teleportTimer;
	private ColorRect flashRect;
	private ProgressBar healthBar;
	private bool teleporting;
	private bool atLeftPosition;
	private bool defeated;

	public override void _Ready()
	{
		AddToGroup("boss");
		GetNode<Area2D>("Area2D").AddToGroup("enemy");
		healthBar = GetNode<ProgressBar>("HealthBar");
		healthBar.MaxValue = Health;
		healthBar.Value = Health;

		lightningScene = GD.Load<PackedScene>("res://Scenes/ThunderBolt.tscn");

		CanvasLayer flashLayer = new();
		flashRect = new ColorRect
		{
			Color = new Color(1f, 1f, 1f, 1f),
			MouseFilter = Control.MouseFilterEnum.Ignore,
			Visible = false
		};
		flashRect.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
		flashLayer.AddChild(flashRect);
		AddChild(flashLayer);

		lightningTimer = CreateTimer(LightningMinInterval, LightningMaxInterval);
		lightningTimer.Timeout += SpawnLightning;

		teleportTimer = CreateTimer(TeleportMinInterval, TeleportMaxInterval);
		teleportTimer.Timeout += OnTeleportTimerTimeout;

		GlobalPosition = GetTopRightPosition();
		atLeftPosition = false;
	}

	private Timer CreateTimer(float minimum, float maximum)
	{
		Timer timer = new();
		AddChild(timer);
		timer.WaitTime = (float)GD.RandRange(minimum, maximum);
		timer.OneShot = true;
		timer.Start();
		return timer;
	}

	private void ResetTimer(Timer timer, float minimum, float maximum)
	{
		timer.WaitTime = (float)GD.RandRange(minimum, maximum);
		timer.Start();
	}

	private void SpawnLightning()
	{
		Player player = GetTree().GetFirstNodeInGroup("player") as Player;
		if (player == null || lightningScene == null)
		{
			ResetTimer(lightningTimer, LightningMinInterval, LightningMaxInterval);
			return;
		}

		ThunderBolt bolt = lightningScene.Instantiate<ThunderBolt>();
		bolt.GlobalPosition = GlobalPosition;
		bolt.Direction = (player.GlobalPosition - GlobalPosition).Normalized();
		bolt.Rotation = bolt.Direction.Angle() + Mathf.Pi / 2f;
		GetTree().CurrentScene.AddChild(bolt);

		ResetTimer(lightningTimer, LightningMinInterval, LightningMaxInterval);
	}

	private async void OnTeleportTimerTimeout()
	{
		if (teleporting)
			return;

		teleporting = true;

		await FlashAsync();
		Visible = false;
		await WaitAsync(0.10);

		Vector2 target = atLeftPosition ? GetTopRightPosition() : GetTopLeftPosition();
		GlobalPosition = target;
		atLeftPosition = !atLeftPosition;

		await WaitAsync(0.10);
		Visible = true;
		await FlashAsync();

		teleporting = false;
		ResetTimer(teleportTimer, TeleportMinInterval, TeleportMaxInterval);
	}

	private async Task FlashAsync()
	{
		flashRect.Visible = true;
		await WaitAsync(0.12);
		flashRect.Visible = false;
	}

	private async Task WaitAsync(double seconds)
	{
		await ToSignal(GetTree().CreateTimer(seconds), SceneTreeTimer.SignalName.Timeout);
	}

	private Vector2 GetTopRightPosition()
	{
		Rect2 viewport = GetViewport().GetVisibleRect();
		return new Vector2(viewport.Size.X * 0.82f, viewport.Position.Y + 100f);
	}

	private Vector2 GetTopLeftPosition()
	{
		Rect2 viewport = GetViewport().GetVisibleRect();
		return new Vector2(viewport.Size.X * 0.18f, viewport.Position.Y + 100f);
	}

	public void TakeDamage(int damage)
	{
		if (defeated)
			return;

		Health -= damage;
		healthBar.Value = Health;
		GD.Print($"Zeusz HP: {Health}");

		if (Health <= 0)
		{
			defeated = true;
			SkillManager.Instance.AddKill();

			SkillManager.Instance.CurrentStage = 3;
			GetTree().ChangeSceneToFile("res://Scenes/Map2.tscn");
		}
	}
}
