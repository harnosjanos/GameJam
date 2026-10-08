using Godot;

public partial class Player : Node2D
{
    [Export]
    public PackedScene ProjectileScene;

    protected Sprite2D head;

    protected int maxMana = 10;
    protected int mana = 10;

    protected ProgressBar manaBar;

    protected bool reloading = false;

    protected bool charging = false;
    protected float chargeTime = 0f;

    protected int maxHealth = 10;
    protected int health = 10;

    protected ProgressBar healthBar;

    public override void _Ready()
    {
        head = GetNode<Sprite2D>("Head");

        InitializeMana();
        InitializeHealth();
    }

    public override void _Process(double delta)
    {
        UpdateHead();

        HandleAttackInput(delta);

        UpdateManaBar();
        UpdateHealthBar();
    }
}