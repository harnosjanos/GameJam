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
        GetNode<Button>("FEJLESZT").Pressed += OnFejlesztPressed;
        GetNode<Button>("FOLYTAT").Pressed += OnFolytatPressed;
        GetNode<Button>("MENU").Pressed += OnMenuPressed;

        UpdateUI();
    }

    private void UpdateUI()
    {
        pointsLabel.Text =
            "Pontok: " + SkillManager.Instance.AvailablePoints;

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