using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.menu;
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
        public static MenuManager MenuManager;
    }
}