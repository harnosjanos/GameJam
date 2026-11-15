using Godot;

public partial class Map2 : Node2D
{
    private float timer;

    private bool finished;

    public override void _Process(
        double delta
    )
    {
        if (finished)
            return;

        timer += (float)delta;

        if (timer >= 20f)
        {
            finished = true;

            SkillManager.Instance.CurrentStage = 3;

            GetTree().ChangeSceneToFile(
                "res://Scenes/PosseidonMap.tscn"
            );
        }
    }
}