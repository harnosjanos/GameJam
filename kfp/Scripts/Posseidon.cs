using Godot;
using System.Threading.Tasks;

public partial class Posseidon : Node2D
{
    [Export]
    public int Health = 400;

    private ProgressBar healthBar;

    private Vector2 centerPos;
    private float moveTime;

    private PackedScene waveScene;

    private WaveAttack currentWave;

    public override void _Ready()
    {
        AddToGroup("boss");
        GetNode<Area2D>("Hitbox")
            .AddToGroup("enemy");

        centerPos = GlobalPosition;

        healthBar =
            GetNode<ProgressBar>(
                "ProgressBar"
            );

        healthBar.MaxValue =
            Health;

        healthBar.Value =
            Health;

        waveScene =
            GD.Load<PackedScene>(
                "res://Scenes/WaveAttack.tscn"
            );
        
        CallDeferred(
            nameof(StartAttackLoop)
        );
    }

    public override void _Process(
        double delta
    )
    {
        moveTime +=
            (float)delta;

        float x =
            Mathf.Sin(
                moveTime
            ) * 250f;

        float y =
            Mathf.Sin(
                moveTime * 2f
            ) * 30f;

        GlobalPosition =
            centerPos +
            new Vector2(x, y);
    }

    private async Task DoLeftWaveAttack()
    {
        Player player =
            GetTree()
            .GetFirstNodeInGroup("player")
            as Player;

        player.MoveToMarker("Right");

        await ToSignal(
            GetTree().CreateTimer(1.0),
            SceneTreeTimer.SignalName.Timeout
        );
        if (!IsInsideTree())
            return;

        SpawnWaveFromLeft();

        while (currentWave != null)
        {
            await ToSignal(
                GetTree(),
                SceneTree.SignalName.ProcessFrame
            );
            if (!IsInsideTree())
                return;
        }
    }

    private async Task DoRightWaveAttack()
    {
        Player player =
            GetTree()
            .GetFirstNodeInGroup("player")
            as Player;

        player.MoveToMarker("Left");

        await ToSignal(
            GetTree().CreateTimer(1.0),
            SceneTreeTimer.SignalName.Timeout
        );
        if (!IsInsideTree())
            return;

        SpawnWaveFromRight();

        while (currentWave != null)
        {
            await ToSignal(
                GetTree(),
                SceneTree.SignalName.ProcessFrame
            );
            if (!IsInsideTree())
                return;
        }
    }

    private void SpawnWaveFromLeft()
    {
        WaveAttack wave =
            waveScene
            .Instantiate<WaveAttack>();

        Marker2D spawnPoint =
            GetTree()
            .CurrentScene
            .GetNode<Marker2D>(
                "LeftWaveSpawn"
            );

        wave.GlobalPosition =
            spawnPoint.GlobalPosition;

        wave.Direction =
            Vector2.Right;

        wave.Scale =
            Vector2.One;

        currentWave = wave;

        GetTree()
            .CurrentScene
            .AddChild(wave);
    }

    private void SpawnWaveFromRight()
    {
        WaveAttack wave =
            waveScene
            .Instantiate<WaveAttack>();

        Marker2D spawnPoint =
            GetTree()
            .CurrentScene
            .GetNode<Marker2D>(
                "RightWaveSpawn"
            );

        wave.GlobalPosition =
            spawnPoint.GlobalPosition;

        wave.Direction =
            Vector2.Left;

        wave.Scale =
            new Vector2(-1, 1);

        currentWave = wave;

        GetTree()
            .CurrentScene
            .AddChild(wave);
    }

    public void TakeDamage(
        int damage
    )
    {
        Health -= damage;

        healthBar.Value =
            Health;

        if (Health <= 0)
        {
            GD.Print(
                "POSEIDON DEAD"
            );
            GetTree().ChangeSceneToFile(
                "res://Scenes/Main_Menu.tscn"
            );

            QueueFree();
        }
    }

    public void EndCurrentAttack()
    {
        currentWave = null;
    }

    private async Task DoRainAttack()
    {

        DestroyAllRainClouds();

        Player player =
            GetTree()
            .GetFirstNodeInGroup("player")
            as Player;

        player.MoveToMarker("Center");

        await ToSignal(
            GetTree().CreateTimer(1.0),
            SceneTreeTimer.SignalName.Timeout
        );
        if (!IsInsideTree())
            return;

        SpawnRainCloud();

        await ToSignal(
            GetTree().CreateTimer(8.0),
            SceneTreeTimer.SignalName.Timeout
        );
        if (!IsInsideTree())
            return;

        while (
            GetTree()
                .GetNodesInGroup(
                    "rain_cloud"
                ).Count > 0
        )
        {
            await ToSignal(
                GetTree(),
                SceneTree.SignalName.ProcessFrame
            );
            if (!IsInsideTree())
                return;
        }
    }

    private void SpawnRainCloud()
    {
        PackedScene cloudScene =
            GD.Load<PackedScene>(
                "res://Scenes/RainCloud.tscn"
            );

        RainCloud cloud =
            cloudScene.Instantiate<RainCloud>();

        Marker2D center =
            GetTree()
            .CurrentScene
            .GetNode<Marker2D>("Center");

        cloud.GlobalPosition =
            center.GlobalPosition +
            new Vector2(0, -250);

        GetTree()
            .CurrentScene
            .AddChild(cloud);
    }


    private void DestroyAllRainClouds()
    {
        foreach (
            RainCloud cloud
            in GetTree()
            .GetNodesInGroup(
                "rain_cloud"
            )
        )
        {
            cloud.RemovedByBoss = true;

            cloud.QueueFree();
        }
    }

    private void SpawnTentacles()
    {
        PackedScene tentacleScene =
            GD.Load<PackedScene>(
                "res://Scenes/Tentacle.tscn"
            );

        Tentacle leftTentacle =
            tentacleScene.Instantiate<Tentacle>();

        Tentacle rightTentacle =
            tentacleScene.Instantiate<Tentacle>();

        Marker2D left =
            GetTree()
            .CurrentScene
            .GetNode<Marker2D>("Center");

        leftTentacle.GlobalPosition =
            left.GlobalPosition +
            new Vector2(-260, 170);
        leftTentacle.RotationDegrees = 45;

        rightTentacle.GlobalPosition =
            left.GlobalPosition +
            new Vector2(260, 170);
        rightTentacle.Scale =
            new Vector2(-1, 1);
        rightTentacle.RotationDegrees = -45;

        GetTree()
            .CurrentScene
            .CallDeferred(
                Node.MethodName.AddChild,
                leftTentacle
            );

        GetTree()
            .CurrentScene
            .CallDeferred(
                Node.MethodName.AddChild,
                rightTentacle
            );
    }

    private async Task DoTentacleAttack()
    {
        SpawnTentacles();

        await ToSignal(
            GetTree(),
            SceneTree.SignalName.ProcessFrame
        );
        if (!IsInsideTree())
            return;

        while (
            GetTree()
                .GetNodesInGroup(
                    "tentacle"
                ).Count > 0
        )
        {
            await ToSignal(
                GetTree(),
                SceneTree.SignalName.ProcessFrame
            );
            if (!IsInsideTree())
                return;
        }
    }

    private async Task AttackLoop()
    {
        while (
            IsInsideTree() &&
            Health > 0
        )
        {
            int attack =
                (int)GD.RandRange(0, 3);

            switch (attack)
            {
                case 0:
                    await DoLeftWaveAttack();
                    if (!IsInsideTree())
                        return;
                    break;

                case 1:
                    await DoRightWaveAttack();
                    if (!IsInsideTree())
                        return;
                    break;

                case 2:
                    await DoRainAttack();
                    if (!IsInsideTree())
                        return;
                    break;

                case 3:
                    await DoTentacleAttack();
                    if (!IsInsideTree())
                        return;
                    break;
            }

            await ToSignal(
                GetTree().CreateTimer(2.0),
                SceneTreeTimer.SignalName.Timeout
            );
            if (!IsInsideTree())
                return;
        }
    }

    private void StartAttackLoop()
    {
        _ = AttackLoop();
    }
}