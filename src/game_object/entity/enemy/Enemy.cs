using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

            int direction = target.Position.X < Position.X ? -1 : 1;

            if ((int)target.Velocity.Y != 0)
                return; //TODO FIX

            target.Hp -= 20;

            target.Position += new Vector2(100 * direction, -100);
        }
    }

    public class Goblin : Enemy
    {
        public Goblin(Texture2D sprite, Vector2 startPosition, float speed, int startHp, bool isAffectedByGravity = true,
            bool isAffectedByGravityDirection = false, Vector2? spriteSize = null, bool animator = false)
            : base(sprite, startPosition, speed, startHp, isAffectedByGravity, isAffectedByGravityDirection, spriteSize, animator) { }
    }
}