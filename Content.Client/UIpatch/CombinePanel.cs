using System.Numerics;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Maths;

namespace Content.Client.UIpatch;

/// <summary>
/// Панель в стиле Combine с скошенными углами и декоративными элементами.
/// </summary>
public sealed class CombinePanel : Container
{
    private const float BevelSize = 8f;

    public Color BackgroundColor { get; set; } = Color.FromHex("#101214").WithAlpha(0.93f);
    public Color BorderColor { get; set; } = Color.FromHex("#2C3035").WithAlpha(0.9f);
    public Color AccentColor { get; set; } = Color.FromHex("#E51E2A");
    public bool ShowTopRightDecor { get; set; } = true;
    public bool ShowBottomLeftDecor { get; set; } = true;
    public bool ShowLeftAccent { get; set; } = false;
    public bool ShowRightAccent { get; set; } = false;

    protected override void Draw(DrawingHandleScreen handle)
    {
        var size = new Vector2(PixelSize.X, PixelSize.Y);

        DrawBeveledBackground(handle, size);
        DrawBeveledBorder(handle, size);

        if (ShowTopRightDecor)
            DrawCornerSlashes(handle, size, topRight: true);

        if (ShowBottomLeftDecor)
            DrawCornerSlashes(handle, size, topRight: false);

        if (ShowLeftAccent)
            handle.DrawRect(new UIBox2(0, BevelSize, 2f, size.Y - BevelSize), AccentColor);

        if (ShowRightAccent)
            handle.DrawRect(new UIBox2(size.X - 2f, BevelSize, size.X, size.Y - BevelSize), AccentColor);

        base.Draw(handle);
    }

    private void DrawBeveledBackground(DrawingHandleScreen handle, Vector2 size)
    {
        var points = new[]
        {
            new Vector2(BevelSize, 0),
            new Vector2(size.X - BevelSize, 0),
            new Vector2(size.X, BevelSize),
            new Vector2(size.X, size.Y - BevelSize),
            new Vector2(size.X - BevelSize, size.Y),
            new Vector2(BevelSize, size.Y),
            new Vector2(0, size.Y - BevelSize),
            new Vector2(0, BevelSize),
        };

        handle.DrawPrimitives(DrawPrimitiveTopology.TriangleFan, points, BackgroundColor);
    }

    private void DrawBeveledBorder(DrawingHandleScreen handle, Vector2 size)
    {
        var points = new[]
        {
            new Vector2(BevelSize, 0),
            new Vector2(size.X - BevelSize, 0),
            new Vector2(size.X, BevelSize),
            new Vector2(size.X, size.Y - BevelSize),
            new Vector2(size.X - BevelSize, size.Y),
            new Vector2(BevelSize, size.Y),
            new Vector2(0, size.Y - BevelSize),
            new Vector2(0, BevelSize),
            new Vector2(BevelSize, 0),
        };

        for (var i = 0; i < points.Length - 1; i++)
            handle.DrawLine(points[i], points[i + 1], BorderColor);
    }

    /// <summary>
    /// Рисует три параллельные красные насечки в углу панели — фирменный
    /// элемент Combine из референса.
    /// </summary>
    private void DrawCornerSlashes(DrawingHandleScreen handle, Vector2 size, bool topRight)
    {
        const float slashLength = 22f;
        const float slashThickness = 3f;
        const float spacing = 7f;

        for (var i = 0; i < 3; i++)
        {
            Vector2 start;
            Vector2 end;

            if (topRight)
            {
                start = new Vector2(size.X - 40f - i * spacing, 2f + i * spacing);
                end = start + new Vector2(slashLength, slashLength);
            }
            else
            {
                start = new Vector2(18f + i * spacing, size.Y - 2f - i * spacing);
                end = start + new Vector2(slashLength, -slashLength);
            }

            DrawThickLine(handle, start, end, AccentColor.WithAlpha(0.75f - i * 0.2f), slashThickness);
        }
    }

    private static void DrawThickLine(DrawingHandleScreen handle, Vector2 start, Vector2 end, Color color,
        float thickness)
    {
        var direction = end - start;
        if (direction.LengthSquared() <= 0.0001f)
            return;

        var normal = Vector2.Normalize(new Vector2(-direction.Y, direction.X)) * (thickness / 2f);
        handle.DrawPrimitives(DrawPrimitiveTopology.TriangleList,
            new[] { start + normal, end + normal, end - normal }, color);
        handle.DrawPrimitives(DrawPrimitiveTopology.TriangleList,
            new[] { start + normal, end - normal, start - normal }, color);
    }
}
