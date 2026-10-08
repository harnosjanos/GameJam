using Godot;

public partial class Player
{
    private void InitializeHealth()
    {
        health = maxHealth;

        healthBar = GetNode<ProgressBar>("HealthBar");

        healthBar.MaxValue = maxHealth;
        healthBar.Value = health;
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.Value = health;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        GD.Print("Player HP: " + health);

        if (health <= 0)
        {
            GD.Print("GAME OVER");
        }
    }
}