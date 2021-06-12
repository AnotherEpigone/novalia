using SadRogue.Primitives;

namespace Novalia.Ui
{
    public static class ColorHelper
    {
        public static Color PlayerNameBlue => new Color(140, 180, 190);

        // stat bars
        public static Color ManaBlue => new Color(45, 105, 175);
        public static Color DepletedManaBlue => new Color(10, 25, 45);
        public static Color HealthRed => new Color(135, 0, 0);
        public static Color DepletedHealthRed => new Color(30, 0, 0);

        // highlights
        public static Color RedHighlight => new Color(280, 0, 0, 150);
        public static Color YellowHighlight => new Color(280, 180, 0, 150);
        public static Color WhiteHighlight => new Color(255, 255, 255, 150);
        public static Color GreyHighlight => new Color(200, 200, 200, 200);
        public static Color DarkGreyHighlight => new Color(100, 100, 100, 100);

        // Gui general
        public static Color ControlBack => new Color(24, 24, 24);
        public static Color ControlBackDark => new Color(18, 18, 18);
        public static Color Text => new Color(180, 180, 180);
        public static Color TextBright => BayeuxParchment2;
        public static Color SelectedBackground => DarkGrey2;
        public static Color EnemyName => EnemyRed;
        public static Color ImportantAction => ActionYellow;
        public static Color GoblinGreen => BayeuxGreen;

        // colors
        public static Color BayeuxParchment => new Color(217, 209, 159);
        public static Color BayeuxParchment2 => new Color(206, 196, 132);
        public static Color BayeuxGold => new Color(229, 179, 4);
        public static Color BayeuxLightGreen => new Color(179, 219, 155);
        public static Color BayeuxGreen => new Color(33, 61, 30);
        public static Color BayeuxBlue => new Color(0, 65, 89);
        public static Color BayeuxLightBlue => new Color(131, 191, 204);
        public static Color BayeuxRed => new Color(177, 35, 0);
        public static Color BayeuxRed2 => new Color(160, 47, 15);
        public static Color BayeuxRed3 => new Color(146, 54, 31);

        public static Color BayeuxDarkGreen => new Color(41, 57, 36);

        public static Color ArchDarkBrown => new Color(89, 64, 45);

        // Applications
        public static Color DarkWood => new Color(36, 31, 27);
        public static Color DarkWood2 => new Color(51, 34, 24);
        public static Color DarkGrey => new Color(66, 66, 66);
        public static Color DarkGrey2 => new Color(50, 50, 50);
        public static Color EnemyRed => new Color(225, 15, 0);
        public static Color ActionYellow => new Color(157, 113, 0);
        public static Color StoryBlue => BayeuxLightBlue;
        public static Color ItemGrey => Color.DarkGray;

        public static string GetParserString(string text, Color color)
        {
            return $"[c:r f:{color.ToParser()}]{text}[c:u]";
        }
    }
}
