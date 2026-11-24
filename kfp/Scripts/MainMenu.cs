using Godot;

public partial class MainMenu : CanvasLayer
{
    public override void _Ready()
    {
        ConfigFile config = new ConfigFile();
        //Tutorial reset
        /*
        config.SetValue(
            "Progress",
            "TutorialCompleted",
            false
        );
        config.Save("user://save.cfg");
        */
        bool tutorialCompleted = false;

        if (
            config.Load("user://save.cfg")
            == Error.Ok
        )
        {
            tutorialCompleted = (bool)
            config.GetValue(
                "Progress",
                "TutorialCompleted",
                false
            );
        }

        GetNode<Sprite2D>(
            "Control/TutorialIncomplete"
        ).Visible = !tutorialCompleted;
    }
}