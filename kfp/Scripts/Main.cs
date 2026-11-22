using Godot;

public partial class Main : Node2D
{
    private float timer;

    private bool finished;

    public override void _Ready()
    {
        Player player =
            GetTree()
            .GetFirstNodeInGroup("player")
            as Player;

        player.SetMountainTexture(
            GD.Load<Texture2D>(
                "res://Kepek/FirstMap-20261005T154640Z-1-001/FirstMap/Stage1MapFirstHalf/Hill.png"
            )
        );
    }

    public override void _Process(double delta)
    {
        if (finished)
            return;

        timer += (float)delta;

        if (timer >= 20f)
        {
            finished = true;

            SkillManager.Instance.CurrentStage = 2;

            GetTree().ChangeSceneToFile(
                "res://Scenes/ZeuszMap.tscn"
            );
        }
    }
}