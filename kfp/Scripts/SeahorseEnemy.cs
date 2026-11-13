using Godot;

public partial class SeahorseEnemy : CharacterBody2D
{
    [Export]
    public int Health = 3;

    [Export]
    public float JumpDuration = 2.5f;

    [Export]
    public float JumpHeight = 400f;

    [Export]
    public float SwimSpeed = 120f;

    [Export]
    public float NoJumpRadius = 200f;

    private ProgressBar healthBar;

    private Player player;

    private Vector2 startPos;
    private Vector2 targetPos;

    private float jumpTimer = 0f;

    private bool isJumping = false;

    private int swimDirection = 1;

    private float jumpCooldown;

    private Sprite2D sprite;

    public override void _Ready()
    {
        GetNode<Area2D>("Hitbox")
            .AddToGroup("enemy");

        player =
            GetTree()
            .GetFirstNodeInGroup("player")
            as Player;

        healthBar =
            GetNode<ProgressBar>("ProgressBar");

        sprite =
            GetNode<Sprite2D>("Sprite2D");

        healthBar.MaxValue = Health;
        healthBar.Value = Health;

        swimDirection =
            GD.Randf() < 0.5f
            ? -1
            : 1;

        jumpCooldown =
            (float)GD.RandRange(
                5f,
                10f
            );
    }

    public override void _PhysicsProcess(
        double delta
    )
    {
        if (player == null)
            return;

        if (isJumping)
        {
            UpdateJump(
                (float)delta
            );
        }
        else
        {
            UpdateSwimming(
                (float)delta
            );
        }

        healthBar.Value = Health;
    }

    private void UpdateSwimming(
        float delta
    )
    {
        GlobalPosition +=
            new Vector2(
                swimDirection *
                SwimSpeed *
                delta,
                0
            );

        if (sprite != null)
        {
            sprite.FlipH =
                swimDirection < 0;
        }

        if (GlobalPosition.X < 50)
        {
            swimDirection = 1;
        }

        if (GlobalPosition.X > 950)
        {
            swimDirection = -1;
        }

        jumpCooldown -= delta;

        if (jumpCooldown > 0)
            return;

        float distanceToPlayer =
            Mathf.Abs(
                player.GlobalPosition.X -
                GlobalPosition.X
            );

        if (
            distanceToPlayer <
            NoJumpRadius
        )
        {
            jumpCooldown = 1f;
            return;
        }

        StartJump();
    }

    private void StartJump()
    {
        isJumping = true;

        jumpTimer = 0f;

        startPos =
            GlobalPosition;

        targetPos =
            player.GlobalPosition;
    }

    private void UpdateJump(
        float delta
    )
    {
        jumpTimer += delta;

        float t =
            jumpTimer /
            JumpDuration;

        if (t >= 1f)
        {
            if (
                GlobalPosition.DistanceTo(
                    player.GlobalPosition
                ) < 40
            )
            {
                player.TakeDamage(
                    Health
                );
            }

            QueueFree();

            return;
        }

        Vector2 pos =
            startPos.Lerp(
                targetPos,
                t
            );

        pos.Y -=
            Mathf.Sin(
                t * Mathf.Pi
            ) *
            JumpHeight;

        GlobalPosition = pos;
    }

    public void TakeDamage(
        int damage
    )
    {
        Health -= damage;

        if (Health <= 0)
        {
            SkillManager.Instance
                .AddKill();

            QueueFree();
        }
    }
}