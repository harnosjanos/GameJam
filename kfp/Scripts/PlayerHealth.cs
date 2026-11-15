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
        if (
            SkillManager.Instance.HpLevel >= 6
            &&
            charging
        )
        {
            damage = (int)Mathf.Floor(damage / 2);
        }

        health -= damage;

        GD.Print("Player HP: " + health);

        if (health <= 0)
        {
            GD.Print("GAME OVER");

            CallDeferred(
                nameof(OpenSkillTree)
            );
        }
    }
    
    private void OpenSkillTree()
    {
        GetTree().ChangeSceneToFile(
            "res://Scenes/SkillTree.tscn"
        );
    }
}