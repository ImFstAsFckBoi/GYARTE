using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.gameObjects.entity;
using GYARTE.main.gameComponents;

namespace GYARTE.manager
{
    public delegate Rectangle GetSubSprite();
    public class Animator
    {
        private Entity _entity;
        private int _walkCycleCounter = 0;
        private Vector2 _direction => _entity.Direction;
        private Texture2D _spriteSheet;
        private Vector2 _spriteSize;
        public Animator(Entity entity, Texture2D spriteSheet, Vector2 spriteSize)
        {
            _entity = entity;
            _spriteSheet = spriteSheet;
            _spriteSize = spriteSize;
        }

        private Rectangle GetSubSpriteRect()
        {
            int x = (int) _direction.X == 1 ? 0 : _spriteSheet.Width - (int)_spriteSize.X;      // 100% matte
            int y = (int) _direction.Y == 1 ? 0 : _spriteSheet.Height / 2;                      // Inge switch case eller if bs
            int w = (int) _spriteSize.X;
            int h = (int) _spriteSize.Y;



            if (_entity.Velocity.X != 0 && _entity.Velocity.Y == 0)
            {
                _walkCycleCounter += 1;

                if (_walkCycleCounter < 5)
                {
                    x += (int) (_spriteSize.X * _direction.X);
                }
                else if (_walkCycleCounter < 10)
                {
                    x += (int) (2 * _spriteSize.X * _direction.X);
                }
                else
                {
                    _walkCycleCounter = 0;
                }
            }
            else
            {
                _walkCycleCounter = 0;
            }

            return new Rectangle(x, y, w, h);
        }
        
        public void Draw(Vector2? scale = null, float rotation = 0, int priority = 50)
        {
            GameComponents.DrawManager.NewDrawCall.Sprite(_entity.Position, _spriteSheet, sourceRectangle: GetSubSpriteRect(), scale: scale, rotation: rotation);
        }
    }
}

/* protected Rectangle GetSubSpriteRect()
        {
            int x = (int)Direction.X == 1 ? 0 : Sprite.Width - (int)SpriteSize.X;
            int y = (int)Direction.Y == 1 ? 0 : Sprite.Height / 2;
            int w = (int)SpriteSize.X;
            int h = (int)SpriteSize.Y;

            if (Velocity.X != 0 && Velocity.Y == 0)
            {
                WalkCycle += 1;

                if (WalkCycle < 5)
                {
                    x += (int)SpriteSize.X * (int)Direction.X;
                }
                else if (WalkCycle < 10)
                {
                    x += 2 * (int)SpriteSize.X * (int)Direction.X;
                }
                else
                {
                    WalkCycle = 0;
                }
            }

            return new Rectangle(x, y, w, h);
        }*/