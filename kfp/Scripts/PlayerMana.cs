using Godot;

public partial class Player
{
    private void InitializeMana()
    {
        mana = maxMana;

        manaBar = GetNode<ProgressBar>("ManaBar");

        manaBar.MaxValue = maxMana;
        manaBar.Value = mana;
    }

    private void UpdateManaBar()
    {
        if (manaBar != null)
        {
            manaBar.Value = mana;
        }
    }

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

        GD.Print("Mana refilled!");
    }
}