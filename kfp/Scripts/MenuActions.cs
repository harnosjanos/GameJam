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

private void _on_exit_button_pressed()
{
    GetTree().Quit();
}
}
