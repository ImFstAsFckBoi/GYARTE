using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.gameObjects.platform;
using GYARTE.main.gameComponents;
using GYARTE.manager;
using GYARTE.misc;

namespace GYARTE.gameObjects.entity.enemy
{
    public class Floater : Enemy
    {
        private readonly List<Shot> _shots = new List<Shot>();
        private readonly Texture2D _shotSprite;

        public Floater(Texture2D sprite, Vector2 startPosition, float speed, int startHp, Texture2D shotSprite,
            bool isAffectedByGravity = false, bool isAffectedByGravityDirection = false) : base(sprite, startPosition,
            speed, startHp, isAffectedByGravity, isAffectedByGravityDirection)
        {
            _shotSprite = shotSprite;
        }

        public override void Update(float g, int gDirection, GameTime gameTime, IEnumerable<Platform> platforms, LivingEntity target)
        {
            foreach (var i in _shots.ToList())
            {
                if (i.Position.X < 0 ||
                    i.Position.Y < 0 ||
                    i.Position.Y > 1280 ||
                    i.Position.X > 1280 ||
                    i.HitScan() ||
                    i.Age(gameTime) > i.TTL)
                {
                    target.Hp -= i.Damage;
                    _shots.Remove(i);
                } 
                    
                
                if (i.Rect.Intersects(Rect))
                    Hp = 0;

                i.Update(gameTime);
            }

            Draw(gameTime);
            Attack(target, gameTime);
            //base.Update(g, gDirection, gameTime, null!, target);
        }

        private void Draw(GameTime gameTime)
        {
            GameComponents.DrawManager.NewDrawCall.Sprite(
                new Vector2(Position.X, 
                    (float) (Position.Y + 25 * Math.Sin(gameTime.TotalGameTime.TotalSeconds))), 
                Sprite);
        }

        protected override void Attack(LivingEntity target, GameTime gameTime = null)
        {
            if (_shots.Count < 1) _shots.Add(new Shot(_shotSprite, Position, 5, 10000, gameTime, target));
        }
    }
}