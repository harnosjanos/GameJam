using Godot;
using System;

public partial class Player : Node2D
{
    [Export]
    public PackedScene ProjectileScene;

    private int maxMana = 10;
    private int mana = 10;

    private bool reloading = false;

    public override void _Ready()
    {
        mana = maxMana;
    }

    public override void _Process(double delta)
    {
        if (!reloading)
        {
            LookAt(GetGlobalMousePosition());
        }

        if (Input.IsActionJustPressed("shoot"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (reloading)
            return;

        if (mana <= 0)
            return;

        Projectile projectile =
            ProjectileScene.Instantiate<Projectile>();

        projectile.GlobalPosition = GlobalPosition;

        projectile.Direction =
            (GetGlobalMousePosition() - GlobalPosition)
            .Normalized();

        GetTree().CurrentScene.AddChild(projectile);

        mana--;

        GD.Print("Mana: " + mana + "/" + maxMana);

        if (mana <= 0)
        {
            Reload();
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