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
	"Nagyszerűen csináltad!",
	"Erőteljesebb támadást a hosszabb suhintással tudsz végrehajtani",
	"Nagyon hasznos, ha sok ellenfél közelít hozzád.",
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

	[Export] private AnimatedSprite2D Demon1;
	[Export] private AnimatedSprite2D Demon2;

	private Area2D Demon1Hitbox;
	private Area2D Demon2Hitbox;


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
		Text = "Nyomb meg 3 másodpercig a bal egérgombot.";
		Visible = true;
		VisibleCharacters = Text.Length;

		ChildPhoenix.EnableChargedAttack();

		/*tween = CreateTween();
		tween.TweenProperty(
			FadeOverlay, "color", new Color(0, 0, 0, 1), 2.0f
			);
		tween.TweenCallback(Callable.From(() =>
		{
			GetTree().ChangeSceneToFile("res://Scenes/Main_Menu.tscn");
		}));*/

	}

	private void OnChargedAttackFired()
	{
		ConfigFile config = new ConfigFile();

		config.SetValue(
			"Progress",
			"TutorialCompleted",
			true
		);

		config.Save("user://save.cfg");
		
		tween = CreateTween();
		tween.TweenProperty(
			FadeOverlay,
			"color",
			new Color(0, 0, 0, 1),
			2.0f
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
		Demon1.Visible = true;
		Demon2.Visible = true;

		Demon1Hitbox.Monitorable = true;
		Demon2Hitbox.Monitorable = true;

		Demon1.Play("default");
		Demon2.Play("default");

		Demon1.Modulate = new Color(
			Demon1.Modulate.R, Demon1.Modulate.G, Demon1.Modulate.B, 0.0f);
		Demon2.Modulate = new Color(
			Demon2.Modulate.R, Demon2.Modulate.G, Demon2.Modulate.B, 0.0f);

		Tween demon1Tween = CreateTween();
		Tween demon2Tween = CreateTween();


		demon1Tween.TweenProperty(Demon1, "modulate:a", 1.0f, 2.0f);

		demon2Tween.TweenProperty(Demon2, "modulate:a", 1.0f, 2.0f);

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
		tween.TweenProperty(this, "visible_characters", Text.Length, 3.0f);
		tween.TweenInterval(2.0f);
		tween.TweenCallback(Callable.From(() =>
		{
			motherActual++;
			ShowPlayerShotSentence();
		}));



	}


	public override void _Ready()
	{
		Demon1Hitbox = GetNode<Area2D>("../Demon1/Area2D");
		Demon2Hitbox = GetNode<Area2D>("../Demon2/Area2D");

		Demon1Hitbox.AddToGroup("tutorial_demon");
		Demon2Hitbox.AddToGroup("tutorial_demon");

		Demon1Hitbox.Monitorable = false;
		Demon2Hitbox.Monitorable = false;

		Text = "";
		Visible = false;

		EggHatching.AnimationFinished += OnHatchingFinished;
		EggHatching.Visible = true;
		EggHatching.Frame = 0;
		EggHatching.Play("EggHatch");

		ChildPhoenix.ChargedAttackFired += OnChargedAttackFired;
	}
	public override void _Process(double enable)
	{

	}
}
