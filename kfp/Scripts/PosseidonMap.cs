using Godot;

public partial class PosseidonMap : Node2D
{
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
}