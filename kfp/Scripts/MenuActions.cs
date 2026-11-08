using Godot;
using System;

public partial class MenuActions : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void _on_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes//Main.tscn");
	}

	private void _on_tutorial_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes//Tutorial.tscn");
	}

	private void _on_settings_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes//Settings.tscn");
	}

	private void _on_back_to_menu_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Main_Menu.tscn");
	}

	private void _on_exit_button_pressed()
	{
		GetTree().Quit();
	}
}
