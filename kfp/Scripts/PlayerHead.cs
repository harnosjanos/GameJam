using Godot;

public partial class Player
{
    private void UpdateHead()
    {
        if (reloading)
            return;

        Vector2 direction =
            GetGlobalMousePosition() - GlobalPosition;

        head.Rotation = direction.Angle();

        head.FlipV = direction.X < 0;
    }
}