#nullable enable
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.gameObjects.platform;
using GYARTE.main.gameComponents;
using GYARTE.manager;

namespace GYARTE.gameObjects.entity
{
    
    
    public class Player : LivingEntity
    {

        public Player(Texture2D sprite, Vector2 startPosition, float speed, int startHp, bool isAffectedByGravity = true,
            bool isAffectedByGravityDirection = false, Vector2? spriteSize = null, bool animator = false) 
            : base(sprite, startPosition, startHp, speed, isAffectedByGravity, isAffectedByGravityDirection, spriteSize, animator)
        {
           
        }

        public override void Update(float g, int gDirection, GameTime gameTime, IEnumerable<Platform> platforms, LivingEntity target)
        {
            Attack(target);
            base.Update(g, gDirection, gameTime, platforms);
            
            DrawHealthbar(new Vector2(5, 5), new Vector2(1.5f, 1.5f));
        }

        public void DrawHealthbar(Vector2 position, Vector2 scale)
        {
            GameComponents.DrawManager.NewDrawCall.Sprite(position, (Texture2D) GameComponents.SpriteTable["HealthBar"], scale: scale, priority: -1, sourceRectangle: new Rectangle(0, 0, (int) (Hp * 2), 20));
            GameComponents.DrawManager.NewDrawCall.Sprite(position, (Texture2D) GameComponents.SpriteTable["HealthBarOutline"], scale: scale, priority: -1);
        }
        
        protected override void Attack(LivingEntity target, GameTime? gameTime = null)
        {
            /*
            if (Rect.Intersects(target.Rect) && Rect.Bottom < target.Rect.Center.Y)
            {
                target.HP = 0;
            }
            */
        }
    }
}