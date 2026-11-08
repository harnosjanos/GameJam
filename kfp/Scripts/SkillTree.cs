using Godot;

public partial class SkillTree : Node2D
{
    private Label descLabel;
    private Label pointsLabel;

    private int selectedSkill = -1;

    public override void _Ready()
    {
        descLabel = GetNode<Label>("DESC");
        pointsLabel = GetNode<Label>("PONTOK");

        GetNode<Button>("HP").Pressed += OnHpPressed;
        GetNode<Button>("MANA").Pressed += OnManaPressed;
        GetNode<Button>("CHARGE").Pressed += OnChargePressed;
        GetNode<Button>("ATTACK").Pressed += OnAttackPressed;
        GetNode<Button>("FEJLESZT").Pressed += OnFejlesztPressed;
        GetNode<Button>("FOLYTAT").Pressed += OnFolytatPressed;
        GetNode<Button>("MENU").Pressed += OnMenuPressed;

        UpdateUI();
    }

    private void UpdateUI()
    {
        pointsLabel.Text =
            "Kill: " +
            SkillManager.Instance.KillCount +
            "\nPontok: " +
            SkillManager.Instance.AvailablePoints;

        if (selectedSkill == 0)
        {
            if (SkillManager.Instance.HpLevel < 5)
            {
                descLabel.Text =
                    "+2 HP\n" +
                    "Jelenlegi szint: " +
                    SkillManager.Instance.HpLevel +
                    "/5";
            }
            else
            {
                descLabel.Text =
                    "HP ULTIMATE\n" +
                    "Charged Attack közben pajzs";
            }
        }

        else if (selectedSkill == 1)
        {
            if (SkillManager.Instance.ManaLevel < 5)
            {
                descLabel.Text =
                    "+2 Mana\n" +
                    "Jelenlegi szint: " +
                    SkillManager.Instance.ManaLevel +
                    "/5";
            }
            else
            {
                descLabel.Text =
                    "MANA ULTIMATE\n" +
                    "Mana elfogyásakor automatikus Charged Attack";
            }
        }

        else if (selectedSkill == 2)
        {
            if (SkillManager.Instance.ChargeLevel < 5)
            {
                if (SkillManager.Instance.ChargeLevel % 2 == 0)
                {
                    descLabel.Text =
                        "+1 Charged Damage";
                }
                else
                {
                    descLabel.Text =
                        "+1 Charged Projectile";
                }
            }
            else
            {
                descLabel.Text =
                    "CHARGED ULTIMATE\n" +
                    "Charged Attack Pierce";
            }
        }

        else if (selectedSkill == 3)
        {
            if (SkillManager.Instance.AttackLevel < 5)
            {
                if (SkillManager.Instance.AttackLevel % 2 == 0)
                {
                    descLabel.Text =
                        "+1 Attack Damage";
                }
                else
                {
                    descLabel.Text =
                        "+1 Projectile";
                }
            }
            else
            {
                descLabel.Text =
                    "ATTACK ULTIMATE\n" +
                    "Bounce";
            }
        }

        else
        {
            descLabel.Text =
                "Válassz fejlesztési ágat";
        }
    }

    private void OnHpPressed()
    {
        selectedSkill = 0;
        UpdateUI();
    }

    private void OnManaPressed()
    {
        selectedSkill = 1;
        UpdateUI();
    }

    private void OnChargePressed()
    {
        GD.Print("CHARGE");
        selectedSkill = 2;
        UpdateUI();
    }

    private void OnAttackPressed()
    {
        GD.Print("ATTACK");
        selectedSkill = 3;
        UpdateUI();
    }

    private void OnFejlesztPressed()
    {
        if (SkillManager.Instance.AvailablePoints <= 0)
            return;

        if (selectedSkill == 0)
        {
            if (SkillManager.Instance.HpLevel < 6)
            {
                SkillManager.Instance.HpLevel++;

                SkillManager.Instance.AvailablePoints--;

                UpdateUI();
            }
        }

        else if (selectedSkill == 1)
        {
            if (SkillManager.Instance.ManaLevel < 6)
            {
                SkillManager.Instance.ManaLevel++;

                SkillManager.Instance.AvailablePoints--;

                UpdateUI();
            }
        }

        else if (selectedSkill == 2)
        {
            if (SkillManager.Instance.ChargeLevel < 6)
            {
                SkillManager.Instance.ChargeLevel++;
                SkillManager.Instance.AvailablePoints--;
                UpdateUI();
            }
        }

        else if (selectedSkill == 3)
        {
            if (SkillManager.Instance.AttackLevel < 6)
            {
                SkillManager.Instance.AttackLevel++;
                SkillManager.Instance.AvailablePoints--;
                UpdateUI();
            }
        }
    }

    private void OnFolytatPressed()
    {
        GetTree().ChangeSceneToFile(
            "res://Scenes/Main.tscn"
        );
    }

    private void OnMenuPressed()
    {
        SkillManager.Instance.ResetSkills();

        GetTree().ChangeSceneToFile(
            "res://Scenes/Main_Menu.tscn"
        );
    }
}