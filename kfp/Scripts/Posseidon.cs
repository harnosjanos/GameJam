using Godot;
using System.Threading.Tasks;

public partial class Posseidon : Node2D
{
    [Export]
    public int Health = 400;

    private ProgressBar healthBar;
    private Timer attackTimer;

    private Vector2 centerPos;
    private float moveTime;

    private PackedScene waveScene;

    private WaveAttack currentWave;

    private bool attackInProgress = false;

    private bool rainAttackActive = false;

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

        attackTimer =
            GetNode<Timer>(
                "AttackTimer"
            );

        attackTimer.Timeout +=
            OnAttackTimerTimeout;

        attackTimer.Start();

        healthBar.MaxValue =
            Health;

        healthBar.Value =
            Health;

        waveScene =
            GD.Load<PackedScene>(
                "res://Scenes/WaveAttack.tscn"
            );
        
        waveScene =
            GD.Load<PackedScene>(
                "res://Scenes/WaveAttack.tscn"
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
            ) * 60f;

        GlobalPosition =
            centerPos +
            new Vector2(x, y);
    }

    private async void OnAttackTimerTimeout()
    {
        if (attackInProgress)
            return;

        attackInProgress = true;

        int attack =
            (int)GD.RandRange(0, 3);

        switch (attack)
        {
            case 0:
                await DoLeftWaveAttack();
                break;

            case 1:
                await DoRightWaveAttack();
                break;

            case 2:
                await DoRainAttack();
                break;
            
            case 3:
                await DoTentacleAttack();
                break;
        }
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

        SpawnWaveFromLeft();
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

        SpawnWaveFromRight();
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

    public async void EndCurrentAttack()
    {
        currentWave = null;

        await ToSignal(
            GetTree().CreateTimer(3.0),
            SceneTreeTimer.SignalName.Timeout
        );

        attackInProgress = false;
    }

    private async Task DoRainAttack()
    {
        rainAttackActive = true;
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

        SpawnRainCloud();

        await ToSignal(
            GetTree().CreateTimer(8.0),
            SceneTreeTimer.SignalName.Timeout
        );

        rainAttackActive = false;

        attackInProgress = false;
    }

    private void SpawnRainCloud()
    {
        PackedScene cloudScene =
            GD.Load<PackedScene>(
                "res://Scenes/RainCloud.tscn"
            );

        string[] markers =
        {
            "Left",
            "Center",
            "Right"
        };

        foreach (string markerName in markers)
        {
            RainCloud cloud =
                cloudScene
                .Instantiate<RainCloud>();

            Marker2D marker =
                GetTree()
                .CurrentScene
                .GetNode<Marker2D>(
                    markerName
                );

            cloud.GlobalPosition =
                marker.GlobalPosition +
                new Vector2(0, -250);

            GetTree()
                .CurrentScene
                .AddChild(cloud);
        }
    }

    public void CloudDestroyed()
    {
        if (!rainAttackActive)
            return;

        int cloudCount =
            GetTree()
            .GetNodesInGroup(
                "rain_cloud"
            ).Count;

        if (cloudCount <= 1)
        {
            rainAttackActive = false;
            attackInProgress = false;
        }
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
            new Vector2(-250, 120);
        leftTentacle.RotationDegrees = 45;

        rightTentacle.GlobalPosition =
            left.GlobalPosition +
            new Vector2(250, 120);
        rightTentacle.Scale =
            new Vector2(-1, 1);
        rightTentacle.RotationDegrees = -45;

        GetTree()
            .CurrentScene
            .AddChild(leftTentacle);

        GetTree()
            .CurrentScene
            .AddChild(rightTentacle);
    }

    private async Task DoTentacleAttack()
    {
        SpawnTentacles();

        await ToSignal(
            GetTree().CreateTimer(8.0),
            SceneTreeTimer.SignalName.Timeout
        );

        foreach (
            Tentacle tentacle
            in GetTree().GetNodesInGroup(
                "tentacle"
            )
        )
        {
            tentacle.QueueFree();
        }

        attackInProgress = false;
    }
}