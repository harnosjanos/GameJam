using Godot;
using System;
using System.Collections.Generic;

public partial class Dialog : Label
{


	List<string> dialogs = new()
		{
			"Kedvesem most hogy megszülettél megtanulunk harcolni,",
			"Ebben a világban sok veszély vár rád",
			"Először próbálj meg támadni ahogyan én"
		};
	int actual = 0;
	Tween tween;
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
			float wait_time = 4.0f;
			tween = CreateTween();

			tween.TweenProperty(this, "visible_characters", Text.Length, time);
			tween.TweenInterval(wait_time);

			tween.TweenCallback(Callable.From(NextSentence));
		}
	}
	public override void _Ready()
	{
		DialogStart(actual);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
