using Godot;

public partial class Enemy : CharacterBody2D
{
    [Export]
    public float Speed = 100f;

    [Export]
    public int Health = 3;

    private ProgressBar healthBar;

    private Player player;

    public override void _Ready()
    {
        player = GetTree()
            .GetFirstNodeInGroup("player") as Player;

        healthBar = GetNode<ProgressBar>("HealthBar");

        healthBar.MaxValue = Health;
        healthBar.Value = Health;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (player == null)
            return;

        Vector2 direction =
            (player.GlobalPosition - GlobalPosition)
            .Normalized();

        Velocity = direction * Speed;

        MoveAndSlide();

        if (GlobalPosition.DistanceTo(player.GlobalPosition) < 20)
        {
            player.TakeDamage(Health);

            QueueFree();
        }

        if (healthBar != null)
        {
            healthBar.Value = Health;   
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        GD.Print("Enemy HP: " + Health);

        if (Health <= 0)
        {
            QueueFree();
        }
    }
}