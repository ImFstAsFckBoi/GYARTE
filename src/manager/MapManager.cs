using System;
using System.Collections.Generic;
using GYARTE.main.gameComponents;
using Microsoft.Xna.Framework;

namespace GYARTE.manager
{
    public static class MapManager
    {
        public static Dictionary<string, string> LoadedRooms = new Dictionary<string, string>();
        
        public static void Draw(Vector2 currentLvlID)
        {
            //1238
            //string ex = "0.0.A.0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789";
          
            GameComponents.DrawManager.NewDrawCall.Sprite(new Vector2(1110, 10), GameComponents.SpriteTable["MapBack"], scale: new Vector2(3, 3), priority: -10);
            
            for (int i=0; i<3; i++)
            {
                for (int j=0; j<3; j++)
                {
                    try 
                    {
                        int y = 0;

                        int x = Convert.ToInt16(LoadedRooms[$"{currentLvlID.X - 1 + i}.{currentLvlID.Y + 1 - j}"], 16);
                        for (int _ = x; x > 4; x -= 4) { y++; }

                        GameComponents.DrawManager.NewDrawCall.Sprite(
                        new Vector2(1114 + 32*i, 14 +32*j), 
                        GameComponents.SpriteTable["MapSheet"], 
                        sourceRectangle: new Rectangle(x * 8, y * 8, 8, 8), 
                        scale: new Vector2(4, 4), priority: -11);
                    } 
                    catch (KeyNotFoundException)
                    {
                        continue;
                    }
                    
                }
            }
            
            /*
            GameComponents.DrawManager.NewDrawCall.Sprite(new Vector2(1114, 14), GameComponents.SpriteTable["MapSheet"], sourceRectangle: new Rectangle(x * 8, y * 8, 8, 8), scale: new Vector2(4, 4), priority: -11);
            GameComponents.DrawManager.NewDrawCall.Sprite(new Vector2(1114 + 16 * 2, 14), GameComponents.SpriteTable["MapSheet"], sourceRectangle: new Rectangle(x * 8, y * 8, 8, 8), scale: new Vector2(4, 4), priority: -11);
            GameComponents.DrawManager.NewDrawCall.Sprite(new Vector2(1114 + 32 * 2, 14), GameComponents.SpriteTable["MapSheet"], sourceRectangle: new Rectangle(x * 8, y * 8, 8, 8), scale: new Vector2(4, 4), priority: -11);
            GameComponents.DrawManager.NewDrawCall.Sprite(new Vector2(1114, 14 + 16 * 2), GameComponents.SpriteTable["MapSheet"], sourceRectangle: new Rectangle(x * 8, y * 8, 8, 8), scale: new Vector2(4, 4), priority: -11);
            GameComponents.DrawManager.NewDrawCall.Sprite(new Vector2(1114 + 16 * 2, 14 + 16 * 2), GameComponents.SpriteTable["MapSheet"], sourceRectangle: new Rectangle(x * 8, y * 8, 8, 8), scale: new Vector2(4, 4), priority: -11);
            GameComponents.DrawManager.NewDrawCall.Sprite(new Vector2(1114 + 32 * 2, 14 + 16 * 2), GameComponents.SpriteTable["MapSheet"], sourceRectangle: new Rectangle(x * 8, y * 8, 8, 8), scale: new Vector2(4, 4), priority: -11);
            GameComponents.DrawManager.NewDrawCall.Sprite(new Vector2(1114, 14 + 32 * 2), GameComponents.SpriteTable["MapSheet"], sourceRectangle: new Rectangle(x * 8, y * 8, 8, 8), scale: new Vector2(4, 4), priority: -11);
            GameComponents.DrawManager.NewDrawCall.Sprite(new Vector2(1114 + 16 * 2, 14 + 32 * 2), GameComponents.SpriteTable["MapSheet"], sourceRectangle: new Rectangle(x * 8, y * 8, 8, 8), scale: new Vector2(4, 4), priority: -11);
            GameComponents.DrawManager.NewDrawCall.Sprite(new Vector2(1114 + 32 * 2, 14 + 32 * 2), GameComponents.SpriteTable["MapSheet"], sourceRectangle: new Rectangle(x * 8, y * 8, 8, 8), scale: new Vector2(4, 4), priority: -11);
            */
        }
    }
}