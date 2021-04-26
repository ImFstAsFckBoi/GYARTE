#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.gameObjects.platform;
using GYARTE.main.gameComponents;
using GYARTE.misc;
using System.Linq;
using GYARTE.manager;
namespace GYARTE.gameObjects.entity
{
    public class Player : LivingEntity
    {
        public int _charge = 100;


        public Player(Texture2D sprite, Vector2 startPosition, float speed, int startHp, bool isAffectedByGravity = true,
            bool isAffectedByGravityDirection = false, Vector2? spriteSize = null, bool animator = false) 
            : base(sprite, startPosition, startHp, speed, isAffectedByGravity, isAffectedByGravityDirection, spriteSize, animator)
        {
           
        }

        public void Update(float g, int gDirection, GameTime gameTime, IEnumerable<Platform> platforms, List<LivingEntity> targets)
        {
            GameComponents.InputManager.MouseIO.WhenPressed(Buttons.LeftButton, () =>
            {

                if (_charge <= 0)
                {
                    _charge = -50;
                    return;
                }
                _charge -= 1;
                
                double deltaX = GameComponents.InputManager.MouseIO.Position.X - Hitbox.Center.X;
                double deltaY = GameComponents.InputManager.MouseIO.Position.Y - Hitbox.Center.Y;

                double alpha = Math.Atan2(deltaY, deltaX);

                GameComponents.DrawManager.NewDrawCall.Sprite(new Vector2(Hitbox.Center.X, Hitbox.Center.Y), GameComponents.SpriteTable["Beam"], origin: new Vector2(0, 50), rotation: (float) alpha, priority: 1);

                Line line = new Line(Hitbox.Center, GameComponents.InputManager.MouseIO.Position);
                foreach (LivingEntity target in targets.Where((x) => line.Intersects(x.Hitbox)))
                {
                    
                    target.HP -= 5;
                }
            }, () =>
            {
                _charge += _charge >= 100? 0: 1;
            });

                
            base.Update(g, gDirection, gameTime, platforms);
            
            DrawHealthbar(new Vector2(5, 5), new Vector2(1.5f, 1.5f));
        }

        public void DrawHealthbar(Vector2 position, Vector2 scale)
        {
            GameComponents.DrawManager.NewDrawCall.Sprite(position, (Texture2D) GameComponents.SpriteTable["HealthBar"], scale: scale, priority: -1, sourceRectangle: new Rectangle(0, 0, (int) (HP * 2), 20));
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