using Godot;

public partial class SkillManager : Node
{
    public static SkillManager Instance { get; private set; }

    public int AvailablePoints = 5;

    public int HpLevel = 0;
    public int ManaLevel = 0;
    public int AttackLevel = 0;
    public int ChargeLevel = 0;

    public override void _Ready()
    {
        Instance = this;
    }

    public void ResetSkills()
    {
        AvailablePoints = 0;

        HpLevel = 0;
        ManaLevel = 0;
        AttackLevel = 0;
        ChargeLevel = 0;
    }
}