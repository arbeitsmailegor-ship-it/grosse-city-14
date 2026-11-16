using System.Numerics;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Maths;

namespace Content.Client.UIpatch;

/// <summary>
/// Кнопка в стиле Combine: тёмная панель с красной рамкой, без скруглений.
/// </summary>
public sealed class CombineButton : Button
{
    public enum CombineButtonStyle
    {
        Primary,
        Secondary,
        Danger,
    }

    private CombineButtonStyle _buttonStyle = CombineButtonStyle.Primary;

    public CombineButtonStyle ButtonStyle
    {
        get => _buttonStyle;
        set
        {
            if (_buttonStyle == value)
                return;

            _buttonStyle = value;
            UpdateVisuals();
        }
    }

    private static readonly Color PrimaryTop = Color.FromHex("#3A0A0E");
    private static readonly Color PrimaryBottom = Color.FromHex("#130406");
    private static readonly Color PrimaryBorder = Color.FromHex("#7C131A");
    private static readonly Color AccentRed = Color.FromHex("#E51E2A");
    private static readonly Color AccentRedGlow = Color.FromHex("#FF2635");
    private static readonly Color SecondaryBg = Color.FromHex("#101215");
    private static readonly Color BorderGray = Color.FromHex("#2A2D31");
    private static readonly Color DangerBg = Color.FromHex("#0D0E10");
    private static readonly Color ReadyTop = Color.FromHex("#D91C28");
    private static readonly Color ReadyTopHover = Color.FromHex("#FF3340");
    private static readonly Color ReadyBottom = Color.FromHex("#6B0C12");
    private static readonly Color ReadyEdge = Color.FromHex("#FF7A82");

    public CombineButton()
    {
        // Убираем стандартную тему, чтобы кнопка рисовалась полностью в стиле лобби.
        StyleBoxOverride = new StyleBoxFlat(Color.Transparent);
    }

    protected override void Draw(DrawingHandleScreen handle)
    {
        var size = new Vector2(PixelSize.X, PixelSize.Y);
        if (size.X <= 0f || size.Y <= 0f)
            return;

        var hovered = IsHovered;
        var pressed = DrawMode == DrawModeEnum.Pressed;
        var toggled = ToggleMode && Pressed && !Disabled;

        switch (_buttonStyle)
        {
            case CombineButtonStyle.Primary:
                DrawPrimary(handle, size, hovered, pressed, toggled);
                break;
            case CombineButtonStyle.Secondary:
                DrawSecondary(handle, size, hovered, pressed);
                break;
            case CombineButtonStyle.Danger:
                DrawDanger(handle, size, hovered, pressed);
                break;
        }
    }

    private static void DrawFill(DrawingHandleScreen handle, UIBox2 box, Color color)
    {
        handle.DrawRect(box, color);
    }

    private static void DrawBorder(DrawingHandleScreen handle, Vector2 size, float thickness, Color color)
    {
        handle.DrawRect(new UIBox2(0, 0, size.X, thickness), color);
        handle.DrawRect(new UIBox2(0, size.Y - thickness, size.X, size.Y), color);
        handle.DrawRect(new UIBox2(0, 0, thickness, size.Y), color);
        handle.DrawRect(new UIBox2(size.X - thickness, 0, size.X, size.Y), color);
    }

    private static void DrawPrimary(DrawingHandleScreen handle, Vector2 size, bool hovered, bool pressed, bool toggled)
    {
        if (toggled)
        {
            DrawReady(handle, size, hovered);
            return;
        }

        var top = pressed ? PrimaryTop.WithAlpha(0.75f) : hovered ? PrimaryTop : PrimaryTop.WithAlpha(0.92f);
        var bottom = pressed ? PrimaryBottom.WithAlpha(0.75f) : PrimaryBottom;

        // Горизонтальный градиент: плотный красный слева уходит в почти чёрный справа.
        const int steps = 12;
        for (var i = 0; i < steps; i++)
        {
            var t = i / (float) steps;
            var x1 = size.X * t;
            var x2 = size.X * (t + 1f / steps);
            var color = Color.InterpolateBetween(top, bottom, t);
            DrawFill(handle, new UIBox2(x1, 0, x2, size.Y), color);
        }

        // Красная полоса слева, как в референсе.
        DrawFill(handle, new UIBox2(0, 0, 3f, size.Y), AccentRed);
        DrawFill(handle, new UIBox2(3f, 0, 10f, size.Y), AccentRed.WithAlpha(0.18f));

        DrawBorder(handle, size, 1f, hovered ? AccentRedGlow : PrimaryBorder);

        if (hovered)
            DrawFill(handle, new UIBox2(1, 1, size.X - 1, size.Y - 1), AccentRed.WithAlpha(0.08f));
    }

    /// <summary>
    /// The persistent "ready" state. It needs to read differently from a dark red
    /// idle primary button, otherwise the toggle only changes the label text.
    /// </summary>
    private static void DrawReady(DrawingHandleScreen handle, Vector2 size, bool hovered)
    {
        const int steps = 18;
        var top = hovered ? ReadyTopHover : ReadyTop;

        for (var i = 0; i < steps; i++)
        {
            var t = i / (float) steps;
            var y1 = size.Y * t;
            var y2 = size.Y * (t + 1f / steps);
            var color = Color.InterpolateBetween(top, ReadyBottom, t);
            DrawFill(handle, new UIBox2(0, y1, size.X, y2), color);
        }

        DrawFill(handle, new UIBox2(0, 0, 3f, size.Y), ReadyEdge);
        DrawFill(handle, new UIBox2(3f, 0, 10f, size.Y), ReadyEdge.WithAlpha(0.18f));
        DrawBorder(handle, size, 1f, ReadyEdge.WithAlpha(hovered ? 1f : 0.8f));
        DrawFill(handle, new UIBox2(1f, 1f, size.X - 1f, 2f), Color.White.WithAlpha(0.25f));

        if (hovered)
            DrawFill(handle, new UIBox2(1f, 1f, size.X - 1f, size.Y - 1f), Color.White.WithAlpha(0.06f));
    }

    private static void DrawSecondary(DrawingHandleScreen handle, Vector2 size, bool hovered, bool pressed)
    {
        var background = pressed ? SecondaryBg.WithAlpha(0.7f) : SecondaryBg.WithAlpha(0.92f);
        DrawFill(handle, new UIBox2(0, 0, size.X, size.Y), background);

        if (hovered)
            DrawFill(handle, new UIBox2(0, 0, size.X, size.Y), AccentRed.WithAlpha(0.07f));

        DrawBorder(handle, size, 1f, hovered ? AccentRed.WithAlpha(0.65f) : BorderGray);
    }

    private static void DrawDanger(DrawingHandleScreen handle, Vector2 size, bool hovered, bool pressed)
    {
        DrawFill(handle, new UIBox2(0, 0, size.X, size.Y),
            pressed ? DangerBg.WithAlpha(0.7f) : DangerBg.WithAlpha(0.92f));

        if (hovered)
            DrawFill(handle, new UIBox2(0, 0, size.X, size.Y), AccentRed.WithAlpha(0.1f));

        DrawBorder(handle, size, 1f, hovered ? AccentRedGlow : AccentRed.WithAlpha(0.8f));
    }

    private void UpdateVisuals()
    {
        // Визуальное состояние обновится автоматически при следующем кадре.
    }
}
