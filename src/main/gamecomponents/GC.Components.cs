using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.manager;

namespace GYARTE.main.gameComponents
{
    public enum GameState
    {
        MainMenu,
        PauseMenu,
        Run
    }

    public static partial class GameComponents
    {
        public static GameState GameState;
        public static float GlobalG;
        public static int GDirection = 1;
        public static Dictionary<string, Texture2D> SpriteTable;
        public static Dictionary<string, SpriteFont> FontTable;
        public static Settings Settings;
        public static WindowConfiguration WindowConfig;
    }
}