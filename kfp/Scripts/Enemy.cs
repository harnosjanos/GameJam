using Godot;

public partial class Enemy : CharacterBody2D
{
    [Export]
    public float Speed = 100f;

    [Export]
    public int Health = 3;

    public bool ChargedNovaTriggered = false;

    private ProgressBar healthBar;

    private Player player;
    private AnimatedSprite2D enemySprite;
    private bool attacking;
    private const string AttackAnimationName = "Attack";

    private void OnAttackAnimationFinished()
    {
        if (attacking && enemySprite.Animation == AttackAnimationName)
            QueueFree();
    }
    public override void _Ready()
    {
        player = GetTree()
            .GetFirstNodeInGroup("player") as Player;

        healthBar = GetNode<ProgressBar>("HealthBar");

        foreach (Node child in GetChildren())
        {
            if (child is AnimatedSprite2D sprite)
            {
                enemySprite = sprite;
                break;
            }
        }

        healthBar.MaxValue = Health;
        healthBar.Value = Health;

        enemySprite.AnimationFinished += OnAttackAnimationFinished;

    }



    public override void _PhysicsProcess(double delta)
    {
        if (player == null || attacking)
            return;

        Vector2 direction =
            (player.GlobalPosition - GlobalPosition)
            .Normalized();

        Velocity = direction * Speed;

        MoveAndSlide();

        if (GlobalPosition.DistanceTo(player.GlobalPosition) < 20)
        {
            attacking = true;
            Velocity = Vector2.Zero;
            player.TakeDamage(Health);
            enemySprite.Play(AttackAnimationName);
            return;
        }

        if (healthBar != null)
        {
            healthBar.Value = Health;
        }

        ChargedNovaTriggered = false;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        GD.Print("Enemy HP: " + Health);

        if (Health <= 0)
        {
            SkillManager.Instance.AddKill();

            QueueFree();
        }
    }
}
