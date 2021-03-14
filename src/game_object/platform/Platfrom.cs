using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.main.gameComponents;
using GYARTE.manager;

namespace GYARTE.gameObjects.platform
{
    public struct PlatformGrid
    {
        public PlatformGrid(Texture2D platform, float ScreenX = 1280f, float ScreenY = 720f)
        {
            Xgrid = new int[(int) Math.Ceiling(ScreenX/platform.Width)];
            Ygrid = new int[(int) Math.Ceiling(ScreenY/platform.Height)];

            for (int i=0; i<Math.Ceiling(ScreenX / platform.Width); i++)
            {
                Xgrid[i] = i * platform.Width;
            }

            for (int i = 0; i < Math.Ceiling(ScreenY / platform.Height); i++)
            {
                Ygrid[i] = i * platform.Height;
            }
        }

        public Vector2 Grid(int x, int y)
        {
            return new Vector2(Xgrid[x], Ygrid[y]);
        }
        
        public readonly int[] Xgrid;
        public readonly int[] Ygrid;
        public readonly int MaxX => Xgrid.Length - 1;
        public readonly int MaxY => Ygrid.Length - 1;
        public readonly Vector2 GridDimension => new Vector2(Xgrid.Length, Ygrid.Length);
    }
    
    public class Platform : GameObject
    {
        private readonly bool _isVisible;
        private readonly Rectangle _spriteRect;
        public new Rectangle Rect => new Rectangle(
            (int)Position.X, 
            (int)Position.Y, 
            128,
            72);

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