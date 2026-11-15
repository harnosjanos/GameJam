using Godot;

public partial class Player : Node2D
{
    [Export]
    public PackedScene ProjectileScene;

    protected Sprite2D head;

    protected int maxMana = 10;
    protected int mana = 10;
    [Export] protected Label manaText;

    protected ProgressBar manaBar;

    protected bool reloading = false;

    protected bool charging = false;
    protected float chargeTime = 0f;

    protected int maxHealth = 10;
    protected int health = 10;
    [Export] protected Label healthText;

    protected ProgressBar healthBar;

    private Vector2 targetPosition;

    private bool movingToTarget = false;

    [Export]
    public float SlideSpeed = 600f;

    private Sprite2D body;
    private Sprite2D nest;
    private Sprite2D mountain;

    public override void _Ready()
    {
        targetPosition =
            GlobalPosition;

        head = GetNode<Sprite2D>("Head");

        // HP fejlesztés
        maxHealth = 10 + (SkillManager.Instance.HpLevel * 2);
        health = maxHealth;
        healthText.Text = $"{health}/{maxHealth}";

        // Mana fejlesztés
        maxMana = 10 + (SkillManager.Instance.ManaLevel * 2);
        mana = maxMana;
        manaText.Text = $"{mana}/{maxMana}";

        InitializeMana();
        InitializeHealth();

        body =
            GetNode<Sprite2D>("Body");

        nest =
            GetNode<Sprite2D>("Nest");

        mountain =
            GetNode<Sprite2D>("Mountain");
    }

    public override void _Process(double delta)
    {
        UpdateHead();

        HandleAttackInput(delta);

        UpdateManaBar();
        UpdateHealthBar();
        manaText.Text = $"{mana}/{maxMana}";
        healthText.Text = $"{health}/{maxHealth}";

        if (movingToTarget)
        {
            GlobalPosition =
                GlobalPosition.MoveToward(
                    targetPosition,
                    SlideSpeed *
                    (float)delta
                );

            if (
                GlobalPosition.DistanceTo(
                    targetPosition
                ) < 5f
            )
            {
                GlobalPosition =
                    targetPosition;

                movingToTarget = false;
            }
        }

    }

    public void MoveToPosition(
        Vector2 position
    )
    {
        targetPosition =
            position;

        movingToTarget = true;
    }

    public void MoveToMarker(
        string markerName
    )
    {
        Marker2D marker =
            GetTree()
            .CurrentScene
            .GetNode<Marker2D>(
                markerName
            );

        MoveToPosition(
            marker.GlobalPosition
        );
    }

    public void ShowPhoenix()
    {
        body.Visible = true;
        head.Visible = true;
    }

    public void HidePhoenix()
    {
        body.Visible = false;
        head.Visible = false;
    }

    public int Health { get => health; }
}