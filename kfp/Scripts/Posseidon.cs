using Godot;

public partial class Posseidon : Node2D
{
    [Export]
    public int Health = 500;

    private ProgressBar healthBar;
    private Timer attackTimer;

    private Vector2 centerPos;
    private float moveTime;

    private PackedScene waveScene;

    public override void _Ready()
    {
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

    private void OnAttackTimerTimeout()
    {
        if (GD.Randf() < 0.5f)
        {
            SpawnWaveFromLeft();
        }
        else
        {
            SpawnWaveFromRight();
        }

        GD.Print(
            "POSEIDON ATTACK"
        );
    }

    private void SpawnWaveFromLeft()
    {
        GD.Print("WAVE FROM LEFT");
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

        wave.GetNode<Sprite2D>("Sprite2D")
            .FlipH = true;

        GetTree()
            .CurrentScene
            .AddChild(wave);
    }

    private void SpawnWaveFromRight()
    {
        GD.Print("WAVE FROM RIGHT");
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

        wave.GetNode<Sprite2D>("Sprite2D")
            .FlipH = false;

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

            QueueFree();
        }
    }
}