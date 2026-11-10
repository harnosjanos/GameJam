
using Godot;

public partial class ExplosionEffect : Node2D
{
    public float MaxRadius = 150f;

    public float Duration = 0.3f;

    public float PixelSize = 8f;

    private float elapsed = 0f;

    private float currentRadius = 0f;

    public override void _Process(double delta)
    {
        elapsed += (float)delta;

        float progress = Mathf.Clamp(
            elapsed / Duration,
            0f,
            1f
        );

        currentRadius = MaxRadius * progress;

        QueueRedraw();

        if (progress >= 1f)
        {
            QueueFree();
        }
    }

    public override void _Draw()
    {
        float alpha = 1f - (elapsed / Duration);

        Color explosionColor = new Color(
            1f,
            0.4f,
            0.05f,
            alpha
        );

        for (float x = -MaxRadius; x <= MaxRadius; x += PixelSize)
        {
            for (float y = -MaxRadius; y <= MaxRadius; y += PixelSize)
            {
                Vector2 center = new Vector2(
                    x + PixelSize / 2f,
                    y + PixelSize / 2f
                );

                if (center.Length() <= currentRadius)
                {
                    DrawRect(
                        new Rect2(x, y, PixelSize, PixelSize),
                        explosionColor
                    );
                }
            }
        }
    }
}
