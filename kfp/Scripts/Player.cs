using Godot;

public partial class Player : Node2D
{
    [Export]
    public PackedScene ProjectileScene;

    protected Sprite2D head;

    protected int maxMana = 10;
    protected int mana = 10;

    protected bool reloading = false;

    protected bool charging = false;
    protected float chargeTime = 0f;

    public override void _Ready()
    {
        mana = maxMana;

        head = GetNode<Sprite2D>("Head");
    }

    public override void _Process(double delta)
    {
        UpdateHead();

        HandleAttackInput(delta);
    }
}