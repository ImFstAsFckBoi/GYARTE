using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.main.gameComponents;
using GYARTE.manager;

namespace GYARTE.gameObjects.platform
{
    public class Platform : GameObject
    {
        private readonly bool _isVisible;
        private readonly Rectangle _spriteRect;
        

        public Platform(Texture2D sprite, Vector2 startPosition,Vector2 spriteCords, Vector2 spriteSize, bool isVisible = true) 
            : base(sprite, startPosition, spriteSize)
        {
            _isVisible = isVisible;
            _spriteRect = new Rectangle((int) spriteCords.X, (int) spriteCords.Y, (int) spriteSize.X, (int) spriteSize.Y);
        }
        
        public override void Draw()
        {
            if (!_isVisible) return;
            
            GameComponents.DrawManager.NewDrawCall.Sprite(Position, Sprite, sourceRectangle: _spriteRect);
        }
    }
}