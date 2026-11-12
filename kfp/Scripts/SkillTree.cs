
using Godot;

public partial class SkillTree : Node2D
{
    private Label descLabel;
    private Label pointsLabel;
    private Button fejlesztButton;

    private string selectedSkill = "";

    // Fejlesztési ágak adatai
    private readonly string[] hpEffects =
    {
        "MoreHp1", "MoreHp2", "MoreHp3",
        "MoreHp4", "MoreHp5", "LessDmgTaken"
    };

    private readonly string[] manaEffects =
    {
        "MoreMana1", "MoreMana2", "MoreMana3",
        "MoreMana4", "MoreMana5", "AoeAfterRevive"
    };

    private readonly string[] attackEffects =
    {
        "MoreAtk1", "OneExtraFeather1", "MoreAtk2",
        "OneExtraFeather2", "MoreAtk3", "ExplodingFeather"
    };

    private readonly string[] chargeEffects =
    {
        "MoreAtk1", "MoreFeathers1", "MoreAtk2",
        "MoreFeathers2", "MoreAtk3", "PiercingAtk"
    };

    private readonly string[] hpUpgrades =
    {
        "HpUpgraded1", "HpUpgraded2", "HpUpgraded3",
        "HpUpgraded4", "HpUpgraded5", "HpUpgraded6"
    };

    private readonly string[] manaUpgrades =
    {
        "ManaUpgrade1", "ManaUpgrade2", "ManaUpgrade3",
        "ManaUpgrade4", "ManaUpgrade5", "ManaUpgrade6"
    };

    private readonly string[] attackUpgrades =
    {
        "NaUpgrade1", "NaUpgrade2", "NaUpgrade3",
        "NaUpgrade4", "NaUpgrade5", "NaUpgrade6"
    };

    private readonly string[] chargeUpgrades =
    {
        "CaUpgrade1", "CaUpgrade2", "CaUpgrade3",
        "CaUpgrade4", "CaUpgrade5", "CaUpgrade6"
    };


    public override void _Ready()
    {
        descLabel = GetNode<Label>("DESC");
        pointsLabel = GetNode<Label>("PONTOK");

        fejlesztButton = GetNode<Button>("FEJLESZT");

        GetNode<Button>("FOLYTAT").Pressed += OnFolytatPressed;
        GetNode<Button>("MENU").Pressed += OnMenuPressed;

        fejlesztButton.Pressed += OnFejlesztPressed;

        // HP
        RegisterButtons("Hp ág", "HP");

        // MANA
        RegisterButtons("Mana ág", "MANA");

        // ATTACK
        RegisterButtons("Attack ág", "ATTACK");

        // CHARGED ATTACK
        RegisterButtons("Charge ág", "CHARGE");

        UpdateUI();
    }


    // Gombok automatikus regisztrálása
    private void RegisterButtons(string branch, string prefix)
    {
        for (int i = 1; i <= 6; i++)
        {
            int level = i;

            GetNode<Button>($"{branch}/{prefix}{level}")
                .Pressed += () => SelectSkill($"{prefix}{level}");
        }
    }


    // Fejlesztés kiválasztása
    private void SelectSkill(string skillId)
    {
        if (selectedSkill == skillId)
        {
            OnFejlesztPressed();
            return;
        }

        selectedSkill = skillId;

        UpdateUI();
    }


    // Teljes UI frissítése
    private void UpdateUI()
    {
        pointsLabel.Text =
            "Kill: " + SkillManager.Instance.KillCount +
            "\nPontok: " + SkillManager.Instance.AvailablePoints +
            "\nKövetkező pont: " + SkillManager.Instance.NextPointAt;

        UpdateHpTree();
        UpdateManaTree();
        UpdateAttackTree();
        UpdateChargeTree();

        UpdateDescription();
    }


    // Közös ikonfrissítő metódus
    private void UpdateBranch(
        string branch,
        string[] effects,
        string[] upgrades,
        int level)
    {
        for (int i = 0; i < 6; i++)
        {
            GetNode<Sprite2D>($"{branch}/{effects[i]}")
                .Modulate = level >= i + 1
                ? Colors.White
                : Colors.Gray;

            GetNode<Sprite2D>($"{branch}/{upgrades[i]}")
                .Visible = level >= i + 1;
        }
    }


    // HP
    private void UpdateHpTree()
    {
        UpdateBranch(
            "Hp ág",
            hpEffects,
            hpUpgrades,
            SkillManager.Instance.HpLevel
        );
    }


    // MANA
    private void UpdateManaTree()
    {
        UpdateBranch(
            "Mana ág",
            manaEffects,
            manaUpgrades,
            SkillManager.Instance.ManaLevel
        );
    }


    // ATTACK
    private void UpdateAttackTree()
    {
        UpdateBranch(
            "Attack ág",
            attackEffects,
            attackUpgrades,
            SkillManager.Instance.AttackLevel
        );
    }


    // CHARGED ATTACK
    private void UpdateChargeTree()
    {
        UpdateBranch(
            "Charge ág",
            chargeEffects,
            chargeUpgrades,
            SkillManager.Instance.ChargeLevel
        );
    }


    // Kiválasztott ág szintjének lekérése
    private int GetBranchLevel(string prefix)
    {
        switch (prefix)
        {
            case "HP":
                return SkillManager.Instance.HpLevel;

            case "MANA":
                return SkillManager.Instance.ManaLevel;

            case "ATTACK":
                return SkillManager.Instance.AttackLevel;

            case "CHARGE":
                return SkillManager.Instance.ChargeLevel;

            default:
                return 0;
        }
    }


    // Fejlesztési szint beállítása
    private void SetBranchLevel(string prefix, int level)
    {
        switch (prefix)
        {
            case "HP":
                SkillManager.Instance.HpLevel = level;
                break;

            case "MANA":
                SkillManager.Instance.ManaLevel = level;
                break;

            case "ATTACK":
                SkillManager.Instance.AttackLevel = level;
                break;

            case "CHARGE":
                SkillManager.Instance.ChargeLevel = level;
                break;
        }
    }


    // Fejlesztések leírásai
    private string GetSkillDescription(string prefix, int level)
    {
        switch (prefix)
        {
            case "HP":
                if (level == 6)
                    return "ULTIMATE\n\nCharged Attack közben\nkevesebb sebzést kapsz.";

                return "+2 Maximum HP";

            case "MANA":
                if (level == 6)
                    return "ULTIMATE\n\nMana elfogyásakor\nautomatikus Charged Attack.";

                return "+2 Maximum Mana";

            case "ATTACK":
                switch (level)
                {
                    case 1:
                    case 3:
                    case 5:
                        return "Nagyobb Attack sebzés.";

                    case 2:
                    case 4:
                        return "Egy extra Feather az Attackhoz.";

                    case 6:
                        return "ULTIMATE\n\nExploding Feather.";

                }
                break;

            case "CHARGE":
                switch (level)
                {
                    case 1:
                    case 3:
                    case 4:
                        return "Több Feather a Charged Attackhoz.";
                    case 2:
                    case 5:
                        return "Nagyobb Charged Attack sebzés.";
                    case 6:
                        return "ULTIMATE\n\nPiercing Attack.";
                }
                break;
        }

        return "Fejlesztés";
    }


    // Kiválasztott fejlesztés leírása
    private void UpdateDescription()
    {
        fejlesztButton.Visible = false;

        if (selectedSkill == "")
        {
            descLabel.Text = "Válassz egy fejlesztést.";
            return;
        }

        string prefix = "";

        if (selectedSkill.StartsWith("HP"))
            prefix = "HP";

        else if (selectedSkill.StartsWith("MANA"))
            prefix = "MANA";

        else if (selectedSkill.StartsWith("ATTACK"))
            prefix = "ATTACK";

        else if (selectedSkill.StartsWith("CHARGE"))
            prefix = "CHARGE";

        if (prefix == "")
            return;

        int.TryParse(
            selectedSkill.Substring(prefix.Length),
            out int skillNumber
        );

        int currentLevel = GetBranchLevel(prefix);

        string status;

        if (currentLevel >= skillNumber)
        {
            status = "Feloldva";
        }
        else if (currentLevel == skillNumber - 1)
        {
            status = SkillManager.Instance.AvailablePoints > 0
                ? "Fejleszthető"
                : "Nincs elég fejlesztési pontod.";
        }
        else
        {
            status = "Előfeltétel: " +
                prefix + (skillNumber - 1);
        }

        descLabel.Text =
            GetSkillDescription(prefix, skillNumber) +
            "\n\n" +
            status;

        fejlesztButton.Visible =
            currentLevel == skillNumber - 1 &&
            SkillManager.Instance.AvailablePoints > 0;
    }


    // Fejlesztés megvásárlása
    private void OnFejlesztPressed()
    {
        if (SkillManager.Instance.AvailablePoints <= 0)
            return;

        if (selectedSkill == "")
            return;

        string prefix = "";

        if (selectedSkill.StartsWith("HP"))
            prefix = "HP";

        else if (selectedSkill.StartsWith("MANA"))
            prefix = "MANA";

        else if (selectedSkill.StartsWith("ATTACK"))
            prefix = "ATTACK";

        else if (selectedSkill.StartsWith("CHARGE"))
            prefix = "CHARGE";

        if (prefix == "")
            return;

        int.TryParse(
            selectedSkill.Substring(prefix.Length),
            out int skillNumber
        );

        int currentLevel = GetBranchLevel(prefix);

        // Csak a következő fejlesztés vásárolható meg
        if (currentLevel != skillNumber - 1)
            return;

        SetBranchLevel(prefix, currentLevel + 1);

        SkillManager.Instance.AvailablePoints--;

        selectedSkill = "";

        UpdateUI();
    }


    // Folytatás
    private void OnFolytatPressed()
    {
        GetTree().ChangeSceneToFile(
            "res://Scenes/Main.tscn"
        );
    }


    // Vissza a menübe
    private void OnMenuPressed()
    {
        SkillManager.Instance.ResetSkills();

        GetTree().ChangeSceneToFile(
            "res://Scenes/Main_Menu.tscn"
        );
    }
}
