using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.main.gameComponents;
using System.Collections.Generic;
using GYARTE.gameObjects.platform;
using System;
namespace GYARTE.gameObjects.entity.enemy
{


    public abstract class Enemy : LivingEntity
    {
        protected Enemy(Texture2D sprite, Vector2 startPosition, float speed, int startHp, bool isAffectedByGravity = true,
            bool isAffectedByGravityDirection = false, Vector2? spriteSize = null, bool animator = false)
            : base(sprite, startPosition, startHp, speed, isAffectedByGravity, isAffectedByGravityDirection, spriteSize, animator) { }

        protected override void Attack(LivingEntity target, GameTime gameTime = null)
        {
            if (!Rect.Intersects(target.Rect))
                return;


            if ((int)target.Velocity.Y != 0)
                return; //TODO FIX

            target.Hp -= 20;

            target.Position += new Vector2(100 * Direction.X, -100);
        }

        public override void Update(float g, int gDirection, GameTime gameTime, IEnumerable<Platform> platforms, GameObject target)
        {
            //Direction = new Vector2(Velocity.X/Math.Abs(Velocity.X), IsAffectedByGravityDirection? GameComponents.GDirection : 1);
            base.Update(g, gDirection, gameTime, platforms, target);
        }
    }

    public class Goblin : Enemy
    {
        public Goblin(Texture2D sprite, Vector2 startPosition, float speed, int startHp, bool isAffectedByGravity = true,
            bool isAffectedByGravityDirection = false, Vector2? spriteSize = null, bool animator = false)
            : base(sprite, startPosition, speed, startHp, isAffectedByGravity, isAffectedByGravityDirection, spriteSize, animator) { }
    }
}