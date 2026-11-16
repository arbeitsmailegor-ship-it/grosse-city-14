using System.Numerics;
using Content.Client.Resources;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.IoC;
using Robust.Shared.Maths;

namespace Content.Client.UIpatch;

/// <summary>
/// Левая информационная панель лобби в стиле Dark Military Sci-Fi
/// </summary>
public sealed partial class LobbyInfoPanel : Robust.Client.UserInterface.Control
{
    [Dependency] private IResourceCache _cache = default!;

    // Цвета из технического описания
    private static readonly Color PanelBg = Color.FromHex("#101214");
    private static readonly Color InnerBg = Color.FromHex("#151719");
    private static readonly Color BorderColor = Color.FromHex("#383B3F");
    private static readonly Color PrimaryText = Color.FromHex("#E0E2E5");
    private static readonly Color SecondaryText = Color.FromHex("#9DA2A8");
    private static readonly Color IconColor = Color.FromHex("#C7CBD0");
    private static readonly Color AccentRed = Color.FromHex("#EF202D");
    private static readonly Color DarkRed = Color.FromHex("#421719");
    private static readonly Color SeparatorColor = Color.FromHex("#292C30");

    private Font? _titleFont;
    private Font? _regularFont;

    // Свойства для отображения данных
    public string ServerName { get; set; } = "MyServer";
    public string RoundStatus { get; set; } = "Раунд ещё не начался";
    public string RoundTimer { get; set; } = "1:53";
    public bool ShowTimer { get; set; } = true;

    public int PlayersInRound { get; set; } = 0;
    public string StationsInfo { get; set; } = "1/1";
    public string CurrentRound { get; set; } = "#9";

    public string CurrentMap { get; set; } = "Dv";

    public string GameMode { get; set; } = "Ролевой";
    public string GameModeDescription { get; set; } = "Обычная жизнь в оккупированном\nмире, где бюрократия приходит\nпозже, чем пули.";

    public string AdminInfo { get; set; } = "Админ server: EndV0r";

    public LobbyInfoPanel()
    {
        IoCManager.InjectDependencies(this);
        _titleFont = _cache.GetFont("/Fonts/NotoSans/NotoSans-Bold.ttf", 27);
        _regularFont = _cache.GetFont("/Fonts/NotoSans/NotoSans-Regular.ttf", 17);
    }

    protected override void Draw(DrawingHandleScreen handle)
    {
        base.Draw(handle);

        var size = PixelSize;
        var width = size.X;
        var height = size.Y;

        // Параметры скошенных углов
        float cornerCut = 15f;

        // Рисуем основной фон панели со скошенными углами
        DrawBeveledRect(handle, Vector2.Zero, new Vector2(width, height), PanelBg, cornerCut);

        // Рисуем границу
        DrawBeveledRectOutline(handle, Vector2.Zero, new Vector2(width, height), BorderColor, cornerCut, 2f);

        // Красная подсветка верхнего левого угла
        DrawRedGlowTopLeft(handle, cornerCut);

        // Диагональные декоративные полосы справа сверху
        DrawDiagonalStripes(handle, width);

        // Красная подсветка нижнего левого угла
        DrawRedGlowBottomLeft(handle, height, cornerCut);

        // Декоративные полосы внизу слева
        DrawBottomLeftStripes(handle, height);

        // Рисуем содержимое
        DrawContent(handle, width, height);
    }

    private void DrawBeveledRect(DrawingHandleScreen handle, Vector2 pos, Vector2 size, Color color, float cornerCut)
    {
        // Создаем полигон со скошенными углами
        var points = new[]
        {
            new Vector2(pos.X + cornerCut, pos.Y), // Верх левый (после среза)
            new Vector2(pos.X + size.X - cornerCut, pos.Y), // Верх правый (до среза)
            new Vector2(pos.X + size.X, pos.Y + cornerCut), // Правый верхний (после среза)
            new Vector2(pos.X + size.X, pos.Y + size.Y - cornerCut), // Правый нижний (до среза)
            new Vector2(pos.X + size.X - cornerCut, pos.Y + size.Y), // Низ правый (после среза)
            new Vector2(pos.X + cornerCut, pos.Y + size.Y), // Низ левый (до среза)
            new Vector2(pos.X, pos.Y + size.Y - cornerCut), // Левый нижний (после среза)
            new Vector2(pos.X, pos.Y + cornerCut) // Левый верхний (до среза)
        };

        // Рисуем заполненный полигон (разбиваем на треугольники)
        for (int i = 1; i < points.Length - 1; i++)
        {
            handle.DrawPrimitives(DrawPrimitiveTopology.TriangleList, new[] { points[0], points[i], points[i + 1] }, color);
        }
    }

    private void DrawBeveledRectOutline(DrawingHandleScreen handle, Vector2 pos, Vector2 size, Color color, float cornerCut, float thickness)
    {
        var points = new[]
        {
            new Vector2(pos.X + cornerCut, pos.Y),
            new Vector2(pos.X + size.X - cornerCut, pos.Y),
            new Vector2(pos.X + size.X, pos.Y + cornerCut),
            new Vector2(pos.X + size.X, pos.Y + size.Y - cornerCut),
            new Vector2(pos.X + size.X - cornerCut, pos.Y + size.Y),
            new Vector2(pos.X + cornerCut, pos.Y + size.Y),
            new Vector2(pos.X, pos.Y + size.Y - cornerCut),
            new Vector2(pos.X, pos.Y + cornerCut)
        };

        // Рисуем линии между точками
        for (int i = 0; i < points.Length; i++)
        {
            var start = points[i];
            var end = points[(i + 1) % points.Length];
            DrawLine(handle, start, end, color, thickness);
        }
    }

    private void DrawLine(DrawingHandleScreen handle, Vector2 start, Vector2 end, Color color, float thickness)
    {
        var direction = end - start;
        var length = direction.Length();
        var normalized = direction / length;
        var perpendicular = new Vector2(-normalized.Y, normalized.X) * (thickness / 2f);

        var p1 = start + perpendicular;
        var p2 = start - perpendicular;
        var p3 = end - perpendicular;
        var p4 = end + perpendicular;

        handle.DrawPrimitives(DrawPrimitiveTopology.TriangleList, new[] { p1, p2, p3 }, color);
        handle.DrawPrimitives(DrawPrimitiveTopology.TriangleList, new[] { p1, p3, p4 }, color);
    }

    private void DrawRedGlowTopLeft(DrawingHandleScreen handle, float cornerCut)
    {
        // Красная линия по верхнему краю
        DrawLine(handle, new Vector2(cornerCut, 0), new Vector2(120, 0), AccentRed, 2f);

        // Красная линия по левому краю
        DrawLine(handle, new Vector2(0, cornerCut), new Vector2(0, 80), AccentRed, 2f);

        // Свечение (полупрозрачные линии)
        var glowColor = AccentRed with { A = 0.3f };
        DrawLine(handle, new Vector2(cornerCut, 0), new Vector2(120, 0), glowColor, 4f);
        DrawLine(handle, new Vector2(0, cornerCut), new Vector2(0, 80), glowColor, 4f);
    }

    private void DrawDiagonalStripes(DrawingHandleScreen handle, float width)
    {
        // Две диагональные полосы в правом верхнем углу
        var stripeColor = DarkRed with { A = 0.6f };
        float stripeWidth = 8f;
        float stripeLength = 60f;
        float angle = MathF.PI / 4f; // 45 градусов

        // Первая полоса
        var start1 = new Vector2(width - 70, 20);
        var end1 = start1 + new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * stripeLength;
        DrawThickLine(handle, start1, end1, stripeColor, stripeWidth);

        // Вторая полоса
        var start2 = new Vector2(width - 55, 20);
        var end2 = start2 + new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * stripeLength;
        DrawThickLine(handle, start2, end2, stripeColor, stripeWidth);
    }

    private void DrawThickLine(DrawingHandleScreen handle, Vector2 start, Vector2 end, Color color, float thickness)
    {
        var direction = end - start;
        var length = direction.Length();
        var normalized = direction / length;
        var perpendicular = new Vector2(-normalized.Y, normalized.X) * (thickness / 2f);

        var p1 = start + perpendicular;
        var p2 = start - perpendicular;
        var p3 = end - perpendicular;
        var p4 = end + perpendicular;

        handle.DrawPrimitives(DrawPrimitiveTopology.TriangleList, new[] { p1, p2, p3 }, color);
        handle.DrawPrimitives(DrawPrimitiveTopology.TriangleList, new[] { p1, p3, p4 }, color);
    }

    private void DrawRedGlowBottomLeft(DrawingHandleScreen handle, float height, float cornerCut)
    {
        // Красная линия по левому краю (нижняя часть)
        DrawLine(handle, new Vector2(0, height - 80), new Vector2(0, height - cornerCut), AccentRed, 2f);

        // Красная линия по нижнему краю
        DrawLine(handle, new Vector2(cornerCut, height), new Vector2(80, height), AccentRed, 2f);

        // Свечение
        var glowColor = AccentRed with { A = 0.3f };
        DrawLine(handle, new Vector2(0, height - 80), new Vector2(0, height - cornerCut), glowColor, 4f);
        DrawLine(handle, new Vector2(cornerCut, height), new Vector2(80, height), glowColor, 4f);
    }

    private void DrawBottomLeftStripes(DrawingHandleScreen handle, float height)
    {
        // Маленькие диагональные полосы внизу слева
        var stripeColor = Color.FromHex("#1A1C1E");
        float stripeWidth = 3f;
        float stripeLength = 15f;
        float angle = MathF.PI / 4f;

        for (int i = 0; i < 3; i++)
        {
            var start = new Vector2(20 + i * 8, height - 25);
            var end = start + new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * stripeLength;
            DrawThickLine(handle, start, end, stripeColor, stripeWidth);
        }
    }

    private void DrawContent(DrawingHandleScreen handle, float width, float height)
    {
        if (_titleFont == null || _regularFont == null)
            return;

        float leftMargin = 25f;
        float currentY = 35f;

        // Заголовок
        handle.DrawString(_titleFont, new Vector2(leftMargin, currentY), $"Лобби: {ServerName}", PrimaryText);
        currentY += 45f;

        // Статус раунда
        handle.DrawString(_regularFont, new Vector2(leftMargin, currentY), RoundStatus, SecondaryText);
        currentY += 25f;

        // Таймер раунда
        if (ShowTimer)
        {
            var timerText = "Раунд начнётся через: ";
            handle.DrawString(_regularFont, new Vector2(leftMargin, currentY), timerText, SecondaryText);
            var timerWidth = MeasureString(timerText, _regularFont);
            handle.DrawString(_regularFont, new Vector2(leftMargin + timerWidth, currentY), RoundTimer, AccentRed);
            currentY += 30f;
        }

        // Разделитель
        DrawSeparator(handle, leftMargin, width - leftMargin - 25f, currentY);
        currentY += 20f;

        // Блок информации об игроках
        DrawIconTextLine(handle, "👥", $"Игроков в раунде: {PlayersInRound}", leftMargin, currentY, _regularFont);
        currentY += 28f;
        DrawIconTextLine(handle, "👤", $"Станций: {StationsInfo}", leftMargin, currentY, _regularFont);
        currentY += 28f;
        DrawIconTextLine(handle, "🕐", $"Текущий раунд: {CurrentRound}", leftMargin, currentY, _regularFont);
        currentY += 30f;

        // Разделитель
        DrawSeparator(handle, leftMargin, width - leftMargin - 25f, currentY);
        currentY += 20f;

        // Текущая карта
        DrawIconTextLine(handle, "🗺", $"Текущая карта: {CurrentMap}", leftMargin, currentY, _regularFont);
        currentY += 30f;

        // Разделитель
        DrawSeparator(handle, leftMargin, width - leftMargin - 25f, currentY);
        currentY += 20f;

        // Рекомендуемый режим
        DrawIconTextLine(handle, "⭐", "Рекомендуемый режим:", leftMargin, currentY, _regularFont, AccentRed);
        currentY += 25f;
        handle.DrawString(_regularFont, new Vector2(leftMargin + 35, currentY), GameMode, AccentRed);
        currentY += 30f;

        // Описание режима
        var descLines = GameModeDescription.Split('\n');
        foreach (var line in descLines)
        {
            handle.DrawString(_regularFont, new Vector2(leftMargin + 35, currentY), line, SecondaryText);
            currentY += 22f;
        }
        currentY += 10f;

        // Разделитель
        DrawSeparator(handle, leftMargin, width - leftMargin - 25f, currentY);
        currentY += 20f;

        // Информация об администраторе
        DrawIconTextLine(handle, "🛡", $"Админ: {AdminInfo}", leftMargin, currentY, _regularFont, AccentRed);
    }

    private void DrawSeparator(DrawingHandleScreen handle, float x, float width, float y)
    {
        DrawLine(handle, new Vector2(x, y), new Vector2(x + width, y), SeparatorColor, 1f);
    }

    private void DrawIconTextLine(DrawingHandleScreen handle, string icon, string text, float x, float y, Font font, Color? textColor = null)
    {
        var color = textColor ?? SecondaryText;

        // Иконка (упрощенная - в реальности нужны SVG или Font Awesome)
        handle.DrawString(font, new Vector2(x, y), icon, IconColor);

        // Текст
        handle.DrawString(font, new Vector2(x + 35, y), text, color);
    }

    private float MeasureString(string text, Font font)
    {
        // Упрощенная версия - в реальности нужно использовать font.GetMetrics
        return text.Length * 10f;
    }

    protected override Vector2 MeasureOverride(Vector2 availableSize)
    {
        return new Vector2(426, 590);
    }
}
