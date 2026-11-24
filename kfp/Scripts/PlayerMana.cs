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
        head.Visible = false;
        Body.Play("mana_empty");

        await ToSignal(
            GetTree().CreateTimer(3f),
            SceneTreeTimer.SignalName.Timeout
        );

        mana = maxMana;

        Body.PlayBackwards("mana_empty");

        await ToSignal(
            Body,
            AnimatedSprite2D.SignalName.AnimationFinished
        );

        Body.Play("idle");

        head.Visible = true;

        reloading = false;
    }
}