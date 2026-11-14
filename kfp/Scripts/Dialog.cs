using Godot;
using System;
using System.Collections.Generic;

public partial class Dialog : Label
{


	List<string> dialogs = new()
		{
			"Kedvesem most hogy megszülettél megtanulunk harcolni.",
			"Ebben a világban sok veszély vár rád",
			"Először próbálj meg támadni ahogyan én"
		};
	int actual = 0;
	Tween tween;

	[Export] private AnimatedSprite2D MotherPhoenixSprite;

	[Export] private string NextAnimation = "Attack";

	[Export] private Node2D FeatherPosition;

	private PackedScene FeatherTexture;

	bool shooted;
	private void OnFrameChanged()
	{

		if (MotherPhoenixSprite.Animation != NextAnimation || MotherPhoenixSprite.Frame != 7 || shooted)
		{
			return;
		}
		shooted = true;
		MotherPhoenixSprite.FrameChanged -= OnFrameChanged;
		Shoot();
	}

	private void Shoot()
	{

		Node2D tollSprite = GD.Load<PackedScene>("res://Scenes/MotherAttackFeather.tscn").Instantiate<Node2D>();

		tollSprite.GlobalPosition = FeatherPosition.GlobalPosition;

		GetTree().CurrentScene.AddChild(tollSprite);

		Tween repulesTween = CreateTween();
		repulesTween.TweenProperty(tollSprite, "global_position", tollSprite.GlobalPosition + new Vector2(1000, 0), 1.5f);
		repulesTween.TweenCallback(Callable.From(() => tollSprite.QueueFree()));

	}

	void NextSentence()
	{
		actual++;
		DialogStart(actual);
	}
	void DialogStart(int index)
	{
		if (index < dialogs.Count)
		{
			Text = dialogs[index];
			VisibleCharacters = 0;
			float time = 5.0f;
			float wait_time = 3.0f;
			tween = CreateTween();

			tween.TweenProperty(this, "visible_characters", Text.Length, time);
			tween.TweenInterval(wait_time);

			tween.TweenCallback(Callable.From(NextSentence));
		}
		else
		{
			shooted = false;
			MotherPhoenixSprite.FrameChanged -= OnFrameChanged;
			MotherPhoenixSprite.FrameChanged += OnFrameChanged;
			MotherPhoenixSprite.Play(NextAnimation);
		}
	}
	public override void _Ready()
	{
		DialogStart(actual);

	}
}
