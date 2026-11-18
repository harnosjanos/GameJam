using Godot;
using System;
using System.Collections.Generic;

public partial class Dialog : Label
{


	List<string> dialogs = new()
	{

		"Üdvözöllek a világunkban kincsem,","nagyon vártam már hogy megszüless",
		"A főnixek igen magasztos teremtmények,","hiszen mi vagyunk a remény",
		"Bárki próbál elpusztítani minket","mi a hamvainkból is feltámadunk",
		"A későbbiekben meg fogod ezt tapasztalni,","mivel sok rosszakaró van a világunkban",
		"A képességeid azonban mindig erősödni fognak ha teszel róla,","de jól gondold meg miben fejlődsz",
		"Szeretnélek korán felkészíteni arra, hogy meg tudd magad védeni.",
		"A szárnyaid segítségével tollakat tudsz elhajítani.",
		"Vigyázz mert nekünk sincs belőle végtelen","így jól oszd be a támadásaidkor",
		"Kezdetben próbáld meg ezt az egyszerű támadást!"

		//"azz"
	};

	List<string> playerShotDialogs = new()
{
	"Nagyszerű!",
	"Erőteljesebb támadást a hosszabb suhintással tudsz végrehajtani."
};

	int playerActual = 0;
	int motherActual = 0;
	Tween tween;
	[Export] private ColorRect FadeOverlay;

	[Export] private AnimatedSprite2D MotherPhoenixSprite;

	[Export] private string NextAnimation = "Attack";

	[Export] private AnimatedSprite2D EggHatching;

	[Export] private AnimatedSprite2D MotherGrabbed;

	[Export] private Node2D FeatherPosition;

	[Export] private Tutorial_ChildPhoneix ChildPhoenix;


	private PackedScene FeatherTexture;

	bool shooted;

	bool motherGrabbed;
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

		Text = "Nyomd meg a bal egérgombot a lövéshez!";
		Visible = true;
		VisibleCharacters = 0;

		tween = CreateTween();
		tween.TweenProperty(this, "visible_characters", Text.Length, 2.0f);

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


	private void OnPlayerShotFinished()
	{
		Text = "Nagyszerű! Erőteljesebb támadást a hosszabb suhintással tudsz végrehajtani.";
		Visible = true;
		VisibleCharacters = 0;

		tween = CreateTween();
		tween.TweenProperty(this, "visible_characters", Text.Length, 2.0f);


	}

	void NextSentence()
	{
		motherActual++;
		DialogStart(motherActual);
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
			DialogStart(motherActual);
		}));

	}
	void DialogStart(int index)
	{
		if (index < dialogs.Count)
		{
			Text = dialogs[index];
			VisibleCharacters = 0;
			float time = 3.0f;
			float wait_time = 2.0f;
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

			//ChildPhoenix.ShotFinished += OnPlayerShotFinished;
			ChildPhoenix.ShotFinished += OnPlayerShot;
			MotherPhoenixSprite.Play(NextAnimation);
		}
	}

	private void OnPlayerShot()
	{
		motherActual = 0;
		ShowPlayerShotSentence();


	}

	void OnMotherGrabbedFinished()
	{
		MotherGrabbed.AnimationFinished -= OnMotherGrabbedFinished;

		tween = CreateTween();
		tween.TweenProperty(
			FadeOverlay, "color", new Color(0, 0, 0, 1), 2.0f
			);
		tween.TweenCallback(Callable.From(() =>
		{
			GetTree().ChangeSceneToFile("res://Scenes/Main_Menu.tscn");
		}));

	}
	void PlayMotherGrabbed()
	{
		if (motherGrabbed)
		{
			return;
		}
		MotherGrabbed.Visible = true;

		motherGrabbed = true;
		MotherPhoenixSprite.Visible = false;
		MotherGrabbed.AnimationFinished += OnMotherGrabbedFinished;

		MotherGrabbed.Play("GrabPhoneix");
	}
	private void ShowPlayerShotSentence()
	{


		if (motherActual >= playerShotDialogs.Count)
		{
			PlayMotherGrabbed();
			Text = "";
			return;
		}

		Text = playerShotDialogs[motherActual];
		Visible = true;
		VisibleCharacters = 0;

		tween = CreateTween();
		tween.TweenProperty(this, "visible_characters", Text.Length, 2.0f);
		tween.TweenInterval(2.0f);
		tween.TweenCallback(Callable.From(() =>
		{
			motherActual++;
			ShowPlayerShotSentence();
		}));



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
