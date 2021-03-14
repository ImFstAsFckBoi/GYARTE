using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYARTE.gameObjects.entity;

namespace GYARTE.main.gameComponents
{
    public static partial class GameComponents
    {
        private static readonly List<Enemy> Enemies = new List<Enemy>();
        private static readonly List<DynamicItem> Items = new List<DynamicItem>();
        private static Player Player1;
        private static Enemy Enemy1;
        public static Enemy Enemy2;
        public static DynamicItem Coin;

    }
}
