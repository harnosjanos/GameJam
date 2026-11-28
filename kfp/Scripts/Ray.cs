using Godot;

public partial class Ray : CharacterBody2D
{
    [Export]
    public float Speed = 100f;

    [Export]
    public int Health = 5;

    public bool ChargedNovaTriggered = false;

    private ProgressBar healthBar;

    private Player player;

    private AnimatedSprite2D sprite;

    public override void _Ready()
    {
        player = GetTree()
            .GetFirstNodeInGroup("player")
            as Player;

        AddToGroup("enemy");

        GetNode<Area2D>("Hitbox")
            .AddToGroup("enemy");

        healthBar =
            GetNode<ProgressBar>(
                "HealthBar"
            );

        healthBar.MaxValue =
            Health;

        healthBar.Value =
            Health;
        
        sprite =
            GetNode<AnimatedSprite2D>(
                "AnimatedSprite2D"
                );

        sprite.Play("default");

        GD.Print(
            sprite.IsPlaying()
        );
    }

    public override void _PhysicsProcess(
        double delta
    )
    {
        if (player == null)
            return;

        Vector2 direction =
            (player.GlobalPosition - GlobalPosition)
            .Normalized();

        Velocity =
            direction * Speed;

        MoveAndSlide();

        if (
            GlobalPosition.DistanceTo(
                player.GlobalPosition
            ) < 20
        )
        {
            player.TakeDamage(Health);

            QueueFree();
        }

        if (healthBar != null)
        {
            healthBar.Value =
                Health;
        }

        ChargedNovaTriggered = false;
    }

    public void TakeDamage(
        int damage
    )
    {
        Health -= damage;

        GD.Print(
            "Ray HP: " + Health
        );

        if (Health <= 0)
        {
            SkillManager.Instance.AddKill();

            QueueFree();
        }
    }
}