using Content.Client.Resources;
using Content.Client.Stylesheets;
using Content.Client.Stylesheets.Stylesheets;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Maths;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.UIpatch;

/// <summary>
/// Lobby-only visual language: dark side panels, dossier typography and controls.
/// These live in the theme rather than in a stylesheet local to <c>LobbyGui</c>,
/// so the lobby keeps a consistent Combine-inspired look.
/// </summary>
[CommonSheetlet]
public sealed class LobbySheetlet<T> : Sheetlet<T> where T : PalettedStylesheet
{
    public const string Panel = "LobbyPanel";
    public const string MenuPanel = "PvpMenuPanel";
    public const string Divider = "LobbyDivider";
    public const string Accent = "LobbyAccent";
    public const string ActionDock = "LobbyActionDock";
    public const string TopBar = "LobbyTopBar";
    public const string TopBarDivider = "LobbyTopBarDivider";
    public const string CharacterCard = "LobbyCharacterCard";
    public const string DossierPortrait = "LobbyDossierPortrait";
    public const string DossierHeader = "LobbyDossierHeader";
    public const string DossierCode = "LobbyDossierCode";
    public const string DossierName = "LobbyDossierName";
    public const string DossierMetaLabel = "LobbyDossierMetaLabel";
    public const string DossierMetaValue = "LobbyDossierMetaValue";
    public const string DossierPortraitTop = "LobbyDossierPortraitTop";
    public const string DossierStatus = "LobbyDossierStatus";
    public const string Hint = "LobbyHint";
    public const string ButtonPrimary = "LobbyButtonPrimary";
    public const string ButtonSecondary = "LobbyButtonSecondary";
    public const string ButtonDanger = "LobbyButtonDanger";
    public const string ButtonBottomBar = "LobbyButtonBottomBar";
    public const string TextTitle = "LobbyTextTitle";
    public const string TextSection = "LobbyTextSection";
    public const string TextBody = "LobbyTextBody";
    public const string TextMuted = "LobbyTextMuted";
    public const string TextAccent = "LobbyTextAccent";
    public const string TextTime = "LobbyTextTime";
    public const string TextMode = "LobbyTextMode";
    public const string TextButton = "LobbyTextButton";
    public const string TextReady = "LobbyTextReady";
    public const string TopServerName = "LobbyTopServerName";
    public const string TopBarSub = "LobbyTopBarSub";
    public const string TopBarTime = "LobbyTopBarTime";
    public const string Icon = "LobbyIcon";
    public const string TopBarText = "LobbyTopBarText";
    public const string TopBarTextSecondary = "LobbyTopBarTextSecondary";
    public const string TopBarIcon = "LobbyTopBarIcon";
    public const string TopBarIconSmall = "LobbyTopBarIconSmall";

    // Combine/Military Sci-Fi цветовая палитра
    private static readonly Color PanelBg = Color.FromHex("#101214").WithAlpha(0.95f);      // Основной фон панелей
    private static readonly Color PanelBgSecondary = Color.FromHex("#191B1E").WithAlpha(0.95f); // Вторичный фон
    private static readonly Color PanelBorder = Color.FromHex("#34383D").WithAlpha(0.85f);  // Тонкие границы
    private static readonly Color AccentRed = Color.FromHex("#E51E2A");                      // Основной красный акцент
    private static readonly Color AccentRedGlow = Color.FromHex("#FF2635").WithAlpha(0.6f); // Акцентное свечение
    private static readonly Color MenuBg = Color.FromHex("#242629").WithAlpha(0.95f);       // Дополнительный фон
    private static readonly Color DividerColor = Color.FromHex("#34383D").WithAlpha(0.5f);  // Тонкие разделители
    private static readonly Color TextPrimary = Color.FromHex("#E0E1E3");                    // Основной текст
    private static readonly Color TextSecondary = Color.FromHex("#969BA2");                  // Вторичный текст
    private static readonly Color TextDisabled = Color.FromHex("#656A70");                   // Неактивные элементы
    private static readonly Color ButtonHover = Color.FromHex("#E51E2A").WithAlpha(0.15f);  // Hover эффект
    private static readonly Color ButtonPressed = Color.FromHex("#E51E2A").WithAlpha(0.30f); // Pressed эффект
    private static readonly Color ButtonDark = Color.FromHex("#0E0F11").WithAlpha(0.95f);   // Темно-серые кнопки (почти черные)
    private static readonly Color ButtonDarkBorder = Color.FromHex("#2A2C2F").WithAlpha(0.8f); // Граница темных кнопок

    // GolosText в репозитории отсутствует, поэтому используем штатный NotoSans
    // (кириллица поддерживается, файлы уже есть в Resources/Fonts/NotoSans).
    private const string LobbyFontRegularPath = "/Fonts/NotoSans/NotoSans-Regular.ttf";
    private const string LobbyFontSemiBoldPath = "/Fonts/NotoSans/NotoSans-Bold.ttf";
    private const string LobbyFontBoldPath = "/Fonts/NotoSans/NotoSans-Bold.ttf";

    public override StyleRule[] GetRules(T sheet, object config)
    {
        return
        [
            // Основные панели с угловатым дизайном и красными акцентами
            E<PanelContainer>()
                .Class(Panel)
                .Panel(new RoundedStyleBox
                {
                    BackgroundColor = PanelBg,
                    BorderColor = PanelBorder,
                    BorderThickness = 1f,  // Тонкая граница
                    CornerRadius = 2f,     // Минимальное скругление
                    Padding = new Thickness(12f, 10f, 12f, 10f),
                }),
            // Верхняя статусная панель
            E<PanelContainer>()
                .Class(TopBar)
                .Panel(new RoundedStyleBox
                {
                    BackgroundColor = PanelBg.WithAlpha(0.90f),
                    BorderColor = PanelBorder.WithAlpha(0.6f),
                    BorderThickness = 1f, // Тонкая нижняя граница (визуально)
                    CornerRadius = 0f,
                }),
            // Разделитель в верхней панели
            E<PanelContainer>()
                .Class(TopBarDivider)
                .Panel(new StyleBoxFlat(AccentRed.WithAlpha(0.4f))),
            // Карточка персонажа
            E<PanelContainer>()
                .Class(CharacterCard)
                .Panel(new RoundedStyleBox
                {
                    BackgroundColor = PanelBgSecondary,
                    BorderColor = PanelBorder,
                    BorderThickness = 1f,
                    CornerRadius = 2f,
                    Padding = new Thickness(0f),
                }),
            // Досье: портретная рамка
            E<PanelContainer>()
                .Class(DossierPortrait)
                .Panel(new RoundedStyleBox
                {
                    BackgroundColor = Color.FromHex("#0A0B0D"),
                    BorderColor = PanelBorder,
                    BorderThickness = 1f,
                    CornerRadius = 0f,
                    Padding = new Thickness(0f),
                }),
            // Тонкие серые разделители
            E<PanelContainer>()
                .Class(Divider)
                .Panel(new StyleBoxFlat(DividerColor)),
            // Красный акцент - короткая линия под заголовком
            E<PanelContainer>()
                .Class(Accent)
                .Panel(new StyleBoxFlat(AccentRed)),
            // Док-панель действий с красной рамкой
            E<PanelContainer>()
                .Class(ActionDock)
                .Panel(new RoundedStyleBox
                {
                    BackgroundColor = PanelBg,
                    BorderColor = AccentRed,
                    BorderThickness = 2f,  // Яркая красная рамка
                    CornerRadius = 2f,     // Угловатый дизайн
                    Padding = new Thickness(12f, 12f, 12f, 12f),
                }),
            // Подсказка внизу слева
            E<PanelContainer>()
                .Class(Hint)
                .Panel(new RoundedStyleBox
                {
                    BackgroundColor = PanelBg.WithAlpha(0.90f),
                    BorderColor = PanelBorder.WithAlpha(0.7f),
                    BorderThickness = 1f,
                    CornerRadius = 3f,
                    Padding = new Thickness(10f, 6f, 10f, 6f),
                }),
            // Вложенные меню
            E<PanelContainer>()
                .Class(MenuPanel)
                .Panel(new RoundedStyleBox
                {
                    BackgroundColor = MenuBg,
                    BorderColor = PanelBorder,
                    BorderThickness = 1f,
                    CornerRadius = 2f,
                    Padding = new Thickness(10f, 8f, 10f, 8f),
                }),

            // Красный акцент для команд атакующих
            E<ContainerButton>()
                .Class("PvpTeamAttackers")
                .ParentOf(E<Label>())
                .FontColor(AccentRed),
            // Синий акцент для защитников (Combine)
            E<ContainerButton>()
                .Class("PvpTeamDefenders")
                .ParentOf(E<Label>())
                .FontColor(Color.FromHex("#5A9BD5")),  // Приглушённый синий
            // Компактные кнопки классов
            E<ContainerButton>()
                .Class("PvpClassButton")
                .ParentOf(E<Label>())
                .Font(sheet.BaseFont.GetFont(10))
                .AlignMode(Label.AlignMode.Left)
                .FontColor(TextPrimary),

            // === СТИЛИ ТЕКСТА И ИКОНОК ===

            // Иконки в информационных блоках
            E<Label>()
                .Class(Icon)
                .Font(sheet.BaseFont.GetFont(14)),

            // Верхняя панель: основной текст
            E<Label>()
                .Class(TopBarText)
                .Font(ResCache.GetFont(LobbyFontRegularPath, 13))
                .FontColor(TextPrimary),

            // Верхняя панель: вторичный текст
            E<Label>()
                .Class(TopBarTextSecondary)
                .Font(ResCache.GetFont(LobbyFontRegularPath, 12))
                .FontColor(TextSecondary),

            // Верхняя панель: иконка логотипа
            E<Label>()
                .Class(TopBarIcon)
                .Font(sheet.BaseFont.GetFont(18))
                .FontColor(AccentRed),

            // Верхняя панель: маленькая иконка
            E<Label>()
                .Class(TopBarIconSmall)
                .Font(sheet.BaseFont.GetFont(12))
                .FontColor(TextSecondary),

            // === ТЕКСТ ЛОББИ ===
            E<Label>()
                .Class(TextTitle)
                .Font(ResCache.GetFont(LobbyFontSemiBoldPath, 22))
                .FontColor(TextPrimary),
            E<Label>()
                .Class(TextSection)
                .Font(ResCache.GetFont(LobbyFontSemiBoldPath, 15))
                .FontColor(TextPrimary),
            E<Label>()
                .Class(TextBody)
                .Font(ResCache.GetFont(LobbyFontRegularPath, 13))
                .FontColor(Color.FromHex("#C3C8CE")),
            // Описание режима — RichTextLabel, поэтому нужен отдельный селектор,
            // иначе оно рисуется дефолтным шрифтом темы.
            E<RichTextLabel>()
                .Class(TextBody)
                .Font(ResCache.GetFont(LobbyFontRegularPath, 13))
                .Prop(Label.StylePropertyFontColor, Color.FromHex("#C3C8CE")),
            E<Label>()
                .Class(TextMuted)
                .Font(ResCache.GetFont(LobbyFontRegularPath, 12))
                .FontColor(TextSecondary),
            E<Label>()
                .Class(TextAccent)
                .Font(ResCache.GetFont(LobbyFontSemiBoldPath, 12))
                .FontColor(AccentRed),
            E<Label>()
                .Class(TextTime)
                .Font(ResCache.GetFont(LobbyFontSemiBoldPath, 14))
                .FontColor(AccentRed),
            E<Label>()
                .Class(TextMode)
                .Font(ResCache.GetFont(LobbyFontSemiBoldPath, 15))
                .FontColor(AccentRed),
            E<Label>()
                .Class(TextButton)
                .Font(ResCache.GetFont(LobbyFontSemiBoldPath, 13))
                .FontColor(Color.FromHex("#D7DADE")),
            E<Label>()
                .Class(TextReady)
                .Font(ResCache.GetFont(LobbyFontBoldPath, 13))
                .FontColor(Color.FromHex("#F2F3F5")),
            E<Label>()
                .Class(TopServerName)
                .Font(ResCache.GetFont(LobbyFontSemiBoldPath, 16))
                .FontColor(TextPrimary),
            E<Label>()
                .Class(TopBarSub)
                .Font(ResCache.GetFont(LobbyFontRegularPath, 12))
                .FontColor(TextSecondary),
            E<Label>()
                .Class(TopBarTime)
                .Font(ResCache.GetFont(LobbyFontRegularPath, 12))
                .FontColor(TextPrimary),

            // === ДОСЬЕ ===
            E<Label>()
                .Class(DossierHeader)
                .Font(ResCache.GetFont(LobbyFontSemiBoldPath, 18))
                .FontColor(TextPrimary),
            E<Label>()
                .Class(DossierCode)
                .Font(ResCache.GetFont(LobbyFontRegularPath, 10))
                .FontColor(TextSecondary),
            E<Label>()
                .Class(DossierName)
                .Font(ResCache.GetFont(LobbyFontBoldPath, 16))
                .FontColor(TextPrimary),
            E<Label>()
                .Class(DossierMetaLabel)
                .Font(ResCache.GetFont(LobbyFontSemiBoldPath, 10))
                .FontColor(Color.FromHex("#7E848B")),
            E<Label>()
                .Class(DossierMetaValue)
                .Font(ResCache.GetFont(LobbyFontSemiBoldPath, 13))
                .FontColor(Color.FromHex("#D7DADE")),
            E<Label>()
                .Class(DossierPortraitTop)
                .Font(ResCache.GetFont(LobbyFontSemiBoldPath, 10))
                .FontColor(TextSecondary),
            E<Label>()
                .Class(DossierStatus)
                .Font(ResCache.GetFont(LobbyFontSemiBoldPath, 10))
                .FontColor(AccentRed),

            // Тёмная кнопка для UICommandButton (кнопки Combine рисуются сами)
            E<ContainerButton>()
                .Class(ButtonSecondary)
                .Box(new RoundedStyleBox
                {
                    BackgroundColor = ButtonDark,
                    BorderColor = ButtonDarkBorder,
                    BorderThickness = 1f,
                    CornerRadius = 0f,
                }),
        ];
    }
}
