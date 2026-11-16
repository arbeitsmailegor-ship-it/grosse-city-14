using System.Numerics;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Maths;

namespace Content.Client.UIpatch;

/// <summary>
/// Верхняя статусная панель в стиле Combine
/// </summary>
public sealed class CombineTopBar : PanelContainer
{
    private static readonly Color BgColor = Color.FromHex("#0A0C0E").WithAlpha(0.95f);
    private static readonly Color BorderColor = Color.FromHex("#34383D").WithAlpha(0.5f);

    public CombineTopBar()
    {
        // Тема не должна перекрывать собственную отрисовку верхней панели.
        PanelOverride = new StyleBoxFlat(Color.Transparent);
    }

    protected override void Draw(DrawingHandleScreen handle)
    {
        var size = new Vector2(PixelSize.X, PixelSize.Y);

        // Тёмный фон
        handle.DrawRect(new UIBox2(0, 0, size.X, size.Y), BgColor);

        // Тонкая нижняя граница
        handle.DrawRect(new UIBox2(0, size.Y - 1, size.X, size.Y), BorderColor);

        base.Draw(handle);
    }
}
