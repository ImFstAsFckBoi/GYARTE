using System.Collections.Generic;
using GYARTE.gameObjects.entity;
using GYARTE.gameObjects.entity.enemy;

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
