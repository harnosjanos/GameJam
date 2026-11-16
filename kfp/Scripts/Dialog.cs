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
			
			//teszt: "azz"
		};
	int actual = 0;
	Tween tween;

	[Export] private AnimatedSprite2D MotherPhoenixSprite;

	[Export] private string NextAnimation = "Attack";

	[Export] private AnimatedSprite2D EggHatching;

	[Export] private Node2D FeatherPosition;

	[Export] private Tutorial_ChildPhoneix ChildPhoenix;


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

	private void OnAttackAnimationFinished()
	{
		if (MotherPhoenixSprite.Animation != NextAnimation)
			return;

		MotherPhoenixSprite.AnimationFinished -= OnAttackAnimationFinished;
		ChildPhoenix.EnableShooting();
		MotherPhoenixSprite.Play("default");

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

	void OnHatchingFinished()
	{
		if (EggHatching.Animation != "EggHatch")
		{
			return;
		}

		EggHatching.Visible = false;

		float wait_time = 2.0f;
		tween = CreateTween();
		tween.TweenInterval(wait_time);

		tween.TweenCallback(Callable.From(() =>
		{
			Visible = true;
			DialogStart(actual);
		}));

	}
	void DialogStart(int index)
	{
		if (index < dialogs.Count)
		{
			Text = dialogs[index];
			VisibleCharacters = 0;
			float time = 4.0f;
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


			MotherPhoenixSprite.AnimationFinished -= OnAttackAnimationFinished;
			MotherPhoenixSprite.AnimationFinished += OnAttackAnimationFinished;

			MotherPhoenixSprite.Play(NextAnimation);
		}
	}
	public override void _Ready()
	{
		Text = "";
		Visible = false;

		EggHatching.AnimationFinished += OnHatchingFinished;
		EggHatching.Visible = true;
		EggHatching.Frame = 0;
		EggHatching.Play("EggHatch");

	}
	public override void _Process(double enable)
	{

	}
}
