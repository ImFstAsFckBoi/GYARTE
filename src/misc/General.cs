using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.gameObjects;
using GYARTE.gameObjects.entity;
using GYARTE.main.gameComponents;
using GYARTE.manager;
using GameObject = GYARTE.gameObjects.GameObject;

namespace GYARTE.misc
{    
    public delegate void Execution();
    
    public class Shot : GameObject
    {
        public readonly int Damage;
        private Vector2 _vector;
        private readonly LivingEntity _target;
        public double TTL;
        public double TimeOfBirth;
        
        public double Age(GameTime gameTime) => gameTime.TotalGameTime.TotalMilliseconds - TimeOfBirth;

        public Shot(Texture2D sprite, Vector2 startPosition, int damage, double ttlms, GameTime gameTime, LivingEntity target, Vector2? spriteSize = null) : base(sprite, startPosition, spriteSize)
        {
            Damage = damage;
            _target = target;
            TTL = ttlms;
            TimeOfBirth = gameTime.TotalGameTime.TotalMilliseconds;
            
            double alpha = Math.Atan2(Position.X - target.Position.X, Position.Y - target.Position.Y) + Math.PI;
            _vector = new Vector2((float) Math.Sin(alpha), (float) Math.Cos(alpha));
        }

        public override void Draw()
        {
            GameComponents.DrawManager.NewDrawCall.Sprite(Position, Sprite, priority: 50);
        }
        
        public void Update(GameTime gameTime)
        {
            if (Vector2.Distance(Position, _target.Position) < 300)
            {
                double alpha = Math.Atan2(Position.X - _target.Position.X, Position.Y - _target.Position.Y) + Math.PI;
                _vector = (49 * _vector + new Vector2((float) Math.Sin(alpha), (float) Math.Cos(alpha))) / 50;
            }
            
            Position += _vector * 1000 *(float) gameTime.ElapsedGameTime.TotalSeconds;
            
            base.Update();
        }

        public bool HitScan()
        {
            if (!Rect.Intersects(_target.Rect)) return false;
            return true;
        } 
        
    }
}