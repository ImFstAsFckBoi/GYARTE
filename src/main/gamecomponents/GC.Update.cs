using System;
using System.Linq;
using Microsoft.Xna.Framework;
using GYARTE.manager;

namespace GYARTE.main.gameComponents
{
    public static partial class GameComponents
    {
        public static void Run(GameTime gameTime)
        {
            DrawManager.NewDrawCall.Sprite(Vector2.Zero, SpriteTable["BackgroundSprite"], priority: 150);

                Player1.Update(GlobalG, GDirection, gameTime, LevelManager.CurrentLevel.Platforms, Enemy1);

                LevelManager.Update(Player1);
                
                foreach (var i in Enemies)
                    i.Update(GlobalG, GDirection, gameTime, LevelManager.CurrentLevel.Platforms, Player1);
        
        
                foreach (var i in Items)
                {
                    i.Update(GlobalG, GDirection, gameTime, LevelManager.CurrentLevel.Platforms, Player1);
                }
        
                CullingManager.HpCulling(Enemies.ToList()); 
                CullingManager.HpCulling(Player1);
                CullingManager.ItemCulling(Items);
                
                InputManager.InputCheck(Player1, GameState, Settings);

                DrawManager.NewDrawCall.Sprite(
                    new Vector2(
                        Player1.Rect.X - 3000 / 2,
                        Player1.Rect.Y - 1688 / 2),
                    SpriteTable["ShadingSprite"], priority: 0);

                Diagnostics(gameTime);
        }
        public static void PauseMenu(GameTime gameTime)
        {
            DrawManager.NewDrawCall.Sprite(Vector2.Zero, SpriteTable["BackgroundSprite"], priority: 150);

            Player1.Draw();
            Player1.DrawHealthbar(new Vector2(5, 5), new Vector2(1.5f, 1.5f));

            LevelManager.CurrentLevel.DrawLevel();
                    
            foreach (var i in Enemies)
                i.Draw();
            
            
            foreach (var i in Items)
            {
                i.Draw();
            }
                    
            DrawManager.NewDrawCall.Sprite(Vector2.Zero, SpriteTable["PauseMenuSprite"], priority: -1);

            InputManager.InputCheck(Player1, GameState, Settings);

            DrawManager.NewDrawCall.Sprite(
                new Vector2(
                    Player1.Rect.X - 3000 / 2,
                    Player1.Rect.Y - 1688 / 2),
                SpriteTable["ShadingSprite"], priority: 0);

        }
        public static void MainMenu(GameTime gameTime)
        {
            DrawManager.NewDrawCall.Sprite(Vector2.Zero, SpriteTable["MainMenuBackground"], priority: 100);
        }
        private static void Diagnostics(GameTime gameTime)
        {
            if (!Settings.ShowDiagnostics)
                return;

            try
            {
                DrawManager.NewDrawCall.Text(FontTable["Arial16Bold"],"FPS: " + (int) (1/gameTime.ElapsedGameTime.TotalSeconds), new Vector2(5, 40), priority: -1, alignment: TextDrawCall.Alignment.TopLeft);
                DrawManager.NewDrawCall.Text(FontTable["Arial16Bold"],"TPF: " + (int) (gameTime.ElapsedGameTime.TotalMilliseconds) + " ms", new Vector2(5, 60), priority: -1, alignment: TextDrawCall.Alignment.TopLeft);
                DrawManager.NewDrawCall.Text(FontTable["Arial16Bold"], "ROOM: " + (int)(LevelManager.CurrentLevel.LevelID.X) + " - " + LevelManager.CurrentLevel.LevelID.Y, new Vector2(5, 80), priority: -1, alignment: TextDrawCall.Alignment.TopLeft);
                DrawManager.NewDrawCall.Text(FontTable["Arial16Bold"], "X: " + (int) Player1.Position.X + " Y: " + (int) Player1.Position.Y, new Vector2(5, 100), priority: -1, alignment: TextDrawCall.Alignment.TopLeft);
            }
            catch (DivideByZeroException)
            {
                DrawManager.NewDrawCall.Text(FontTable["Arial16Bold"],"FPS: ---", new Vector2(10, 10), priority: -1);
            }
        }
    }
}