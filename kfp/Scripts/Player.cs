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

    private AnimatedSprite2D body;
    private Sprite2D mountain;
    
    protected bool attackAnimating = false;

    private AnimatedSprite2D shieldEffect;
    private bool shieldStarted = false;

    protected AudioStreamPlayer2D loves;
    protected AudioStreamPlayer2D serul;
    protected AudioStreamPlayer2D talalat;

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
            GetNode<AnimatedSprite2D>("Body");
  

        mountain =
            GetNode<Sprite2D>("Mountain");
        
        Body.Play("idle");

        shieldEffect =
            GetNode<AnimatedSprite2D>("ShieldEffect");

        shieldEffect.Visible = false;

        loves =
            GetNode<AudioStreamPlayer2D>("loves");

        serul =
            GetNode<AudioStreamPlayer2D>("serul");
        
        talalat =
            GetNode<AudioStreamPlayer2D>("talalat");
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

    private int GetNormalDamage()
    {
        int damage = 1;

        damage += SkillManager.Instance.AttackDamageLevel;

        return damage;
    }

    private int GetChargedDamage()
    {
        int damage = 1;

        damage += SkillManager.Instance.ChargedDamageLevel;

        return damage;
    }

    public void SetMountainTexture(
        Texture2D texture
    )
    {
        mountain.Texture = texture;
    }

    public AnimatedSprite2D Body => body;

    public int Health { get => health; }

    public void PlayHitSound()
    {
        talalat.Play();
    }
}