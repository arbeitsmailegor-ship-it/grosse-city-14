using System.Numerics;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Shared.Maths;

namespace Content.Client.UIpatch;

/// <summary>
/// Cinematic scrim for the lobby background. A flat dark overlay hides the artwork;
/// this keeps the centre readable but lets the background breathe through a
/// directional gradient, then frames it with a soft top and bottom vignette.
/// </summary>
public sealed class LobbyScrim : Robust.Client.UserInterface.Control
{
    private const int HorizontalSteps = 80;
    private const int BottomSteps = 28;
    private const int TopSteps = 18;

    private const float LeftAlpha = 0.86f;
    private const float LeftMidAlpha = 0.38f;
    private const float CenterAlpha = 0.22f;
    private const float RightAlpha = 0.52f;

    public LobbyScrim()
    {
        MouseFilter = MouseFilterMode.Ignore;
    }

    protected override void Draw(DrawingHandleScreen handle)
    {
        var size = new Vector2(PixelSize.X, PixelSize.Y);
        if (size.X <= 0f || size.Y <= 0f)
            return;

        DrawHorizontalGradient(handle, size);
        DrawBottomVignette(handle, size);
        DrawTopVignette(handle, size);
    }

    private static void DrawHorizontalGradient(DrawingHandleScreen handle, Vector2 size)
    {
        var step = size.X / HorizontalSteps;

        for (var i = 0; i < HorizontalSteps; i++)
        {
            var x0 = i * step;
            var x1 = i == HorizontalSteps - 1 ? size.X : (i + 1) * step;
            var alpha = HorizontalAlpha((i + 0.5f) / HorizontalSteps);

            handle.DrawRect(new UIBox2(x0, 0, x1, size.Y), Color.Black.WithAlpha(alpha));
        }
    }

    private static void DrawBottomVignette(DrawingHandleScreen handle, Vector2 size)
    {
        var height = MathF.Min(size.Y * 0.34f, 260f);
        var step = height / BottomSteps;

        for (var i = 0; i < BottomSteps; i++)
        {
            var t = (i + 0.5f) / BottomSteps;
            var y0 = size.Y - height + i * step;
            var y1 = i == BottomSteps - 1 ? size.Y : y0 + step;
            var alpha = 0.42f * Smooth(t);

            handle.DrawRect(new UIBox2(0, y0, size.X, y1), Color.Black.WithAlpha(alpha));
        }
    }

    private static void DrawTopVignette(DrawingHandleScreen handle, Vector2 size)
    {
        var height = MathF.Min(size.Y * 0.14f, 120f);
        var step = height / TopSteps;

        for (var i = 0; i < TopSteps; i++)
        {
            var t = (i + 0.5f) / TopSteps;
            var y0 = i * step;
            var y1 = i == TopSteps - 1 ? height : y0 + step;
            var alpha = 0.20f * (1f - Smooth(t));

            handle.DrawRect(new UIBox2(0, y0, size.X, y1), Color.Black.WithAlpha(alpha));
        }
    }

    private static float HorizontalAlpha(float x)
    {
        if (x < 0.4f)
            return Lerp(LeftAlpha, LeftMidAlpha, Smooth(x / 0.4f));

        if (x < 0.7f)
            return Lerp(LeftMidAlpha, CenterAlpha, Smooth((x - 0.4f) / 0.3f));

        return Lerp(CenterAlpha, RightAlpha, Smooth((x - 0.7f) / 0.3f));
    }

    private static float Smooth(float t)
    {
        t = Math.Clamp(t, 0f, 1f);
        return t * t * (3f - 2f * t);
    }

    private static float Lerp(float from, float to, float t)
    {
        return from + (to - from) * t;
    }
}
