using Godot;

public partial class Main : Node2D
{
    private float timer;

    private bool finished;

    public override void _Process(double delta)
    {
        if (finished)
            return;

        timer += (float)delta;

        if (timer >= 10f)
        {
            finished = true;

            SkillManager.Instance.CurrentStage = 2;

            GetTree().ChangeSceneToFile(
                "res://Scenes/Map2.tscn"
            );
        }
    }
}