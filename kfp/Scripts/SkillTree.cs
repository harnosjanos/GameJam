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

        UpdateUI();
    }

    private void UpdateUI()
    {
        pointsLabel.Text =
            $"Pontok: {SkillManager.Instance.AvailablePoints}";
    }

    public void OnHpPressed()
    {
        selectedSkill = 0;

        descLabel.Text =
            $"+2 HP\nJelenlegi szint: {SkillManager.Instance.HpLevel}";
    }

    public void OnFejlesztPressed()
    {
        if (SkillManager.Instance.AvailablePoints <= 0)
            return;

        if (selectedSkill == 0)
        {
            SkillManager.Instance.HpLevel++;
            SkillManager.Instance.AvailablePoints--;

            OnHpPressed();
            UpdateUI();
        }
    }

    public void OnFolytatPressed()
    {
        GetTree().ChangeSceneToFile(
            "res://Scenes/Main.tscn"
        );
    }

    public void OnMenuPressed()
    {
        SkillManager.Instance.ResetSkills();

        GetTree().ChangeSceneToFile(
            "res://Scenes/Main_Menu.tscn"
        );
    }
}