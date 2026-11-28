using Godot;

public partial class Map2 : Node2D
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
                "res://Kepek/Stage2Map/SandHill.png"
            )
        );

        player.GetNode<Sprite2D>("Mountain")
            .Position += new Vector2(50, 0);
    }

    public override void _Process(
        double delta
    )
    {
        if (finished)
            return;

        timer += (float)delta;

        if (SkillManager.Instance.BossReached)
        {
            if (timer >= 60f)
            {
                finished = true;

                GetTree().ChangeSceneToFile(
                    "res://Scenes/PosseidonMap.tscn"
                );
            }
        }

        if (timer >= 120f)
        {
            finished = true;

            //SkillManager.Instance.CurrentStage = 4;

            GetTree().ChangeSceneToFile(
                "res://Scenes/PosseidonMap.tscn"
            );
        }
    }
}