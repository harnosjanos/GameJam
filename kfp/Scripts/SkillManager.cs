using Godot;

public partial class SkillManager : Node
{
    public static SkillManager Instance { get; private set; }

    public int AvailablePoints = 30;
    public int KillCount = 0;
    public int NextPointAt = 5;

    public int HpLevel = 0;
    public int ManaLevel = 0;
    public int AttackLevel = 0;
    public int ChargeLevel = 0;

    public int CurrentStage = 1;

    public override void _Ready()
    {
        Instance = this;
    }

    public void ResetSkills()
    {
        AvailablePoints = 0;

        KillCount = 0;
        NextPointAt = 5;

        HpLevel = 0;
        ManaLevel = 0;
        AttackLevel = 0;
        ChargeLevel = 0;
    }

    public void AddKill()
    {
        KillCount++;

        if (KillCount >= NextPointAt)
        {
            AvailablePoints++;

            NextPointAt =
                NextPointAt * 2 +
                (NextPointAt / 2);
        }
    }
}