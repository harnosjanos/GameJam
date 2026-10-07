using Godot;

public partial class Player
{
    private async void Reload()
    {
        reloading = true;

        Visible = false;

        await ToSignal(
            GetTree().CreateTimer(3.0),
            SceneTreeTimer.SignalName.Timeout
        );

        mana = maxMana;

        Visible = true;

        reloading = false;
    }
}