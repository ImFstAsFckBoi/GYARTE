using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.main.gameComponents;

namespace GYARTE.gameObjects
{
    public abstract class GameObject
    {
        public Vector2 Position;
        public Vector2 SpriteSize; 
        public float Width => SpriteSize.X;
        public float Height => SpriteSize.Y;
        public Rectangle Hitbox => new Rectangle(
            (int) Position.X,
            (int) Position.Y,
            (int) Width,
            (int) Height);
        
        protected readonly Texture2D Sprite;

        protected GameObject(Texture2D sprite, Vector2 startPosition, Vector2? spriteSize = null)
        {
            Sprite = sprite;
            Position = startPosition;
            SpriteSize = spriteSize ?? new Vector2(sprite.Width, sprite.Height);
        }

        public virtual void Draw()
        {
            GameComponents.DrawManager.NewDrawCall.Sprite(Position, Sprite, priority: 50);
        }

        public virtual void Update()
        {
            Draw();
        }
    }
}