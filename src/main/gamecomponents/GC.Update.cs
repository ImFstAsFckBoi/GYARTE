﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;
using GYARTE.manager;
using GYARTE.gameObjects.entity;
using Microsoft.Xna.Framework.Input;
namespace GYARTE.main.gameComponents
{
    public static partial class GameComponents
    {
        public static void BaseUpdate(GameTime gameTime)
        {
            InputManager.Update();

            InputManager.KeyboardIO.WhenPressedOnce(Keys.Escape, () =>
            {
                switch(GameComponents.GameState)
                {
                    case GameState.Run:
                        GameState = GameState.PauseMenu;
                        Player1.Velocity.X = 0; 
                        break;

                    case GameState.PauseMenu:
                        GameState = GameState.Run;
                        break;
                    case GameState.MainMenu:
                        Environment.Exit(0);
                        break;

                }
                
            });
        }
        public static void Run(GameTime gameTime)
        {
            
            DrawManager.NewDrawCall.Sprite(Vector2.Zero, SpriteTable["BackgroundSprite"], priority: 150);

            Player1.Update(GlobalG, GDirection, gameTime, LevelManager.CurrentLevel.Platforms, LevelManager.CurrentLevel.Enemies.Cast<LivingEntity>().ToList());

            LevelManager.Update(Player1);
            
            foreach (var i in LevelManager.CurrentLevel.Enemies)
                i.Update(GlobalG, GDirection, gameTime, LevelManager.CurrentLevel.Platforms, Player1);
    
    
    
    
            CullingManager.HpCulling(LevelManager.CurrentLevel.Enemies); 
            CullingManager.HpCulling(Player1);
            
            
           

            InputManager.KeyboardIO.WhenPressedOnce(Keys.Space, () =>
            {
                GDirection *= Player1.Velocity.Y == 0 ? -1 : 1;
            });


            // Shift for running
            bool tmp_flg = false;
            InputManager.KeyboardIO.WhenPressed(Keys.LeftShift, () =>
            {
                Player1.Speed = Settings.PlayerSpeed * 2;
                tmp_flg = true;
            });

            // Alt for sneaking
            InputManager.KeyboardIO.WhenPressed(Keys.LeftAlt, () =>
            {
                Player1.Speed = Settings.PlayerSpeed * 1 / 2;
                tmp_flg = true;
            });
            
            if(!tmp_flg)
            {
                Player1.Speed = Settings.PlayerSpeed;
            }

            tmp_flg = false;
            InputManager.KeyboardIO.WhenPressed(Keys.A, () =>
            {
                Player1.Direction.X = -1;
                Player1.Velocity.X = -Player1.Speed;
                tmp_flg = true;
            });

            InputManager.KeyboardIO.WhenPressed(Keys.D, () =>
            {
                Player1.Direction.X = 1;
                Player1.Velocity.X = Player1.Speed;
                tmp_flg = true;
            });

            if(!tmp_flg)
            {
                Player1.Velocity.X = 0;
            }

            /*
            DrawManager.NewDrawCall.Sprite(
                new Vector2(
                    Player1.Hitbox.X - 3000 / 2,
                    Player1.Hitbox.Y - 1688 / 2),
                SpriteTable["ShadingSprite"], priority: 0);
            */
            Diagnostics(gameTime);
            BaseUpdate(gameTime);
        }
        public static void PauseMenu(GameTime gameTime)
        {
            
            DrawManager.NewDrawCall.Sprite(Vector2.Zero, SpriteTable["BackgroundSprite"], priority: 150);

            Player1.Draw();
            Player1.DrawHealthbar(new Vector2(5, 5), new Vector2(1.5f, 1.5f));

            LevelManager.CurrentLevel.DrawLevel();
                    
            foreach (var i in LevelManager.CurrentLevel.Enemies)
                i.Draw();
            
            
           
                    
            DrawManager.NewDrawCall.Sprite(Vector2.Zero, SpriteTable["PauseMenuSprite"], priority: -1);
            MenuManager.Update();
            
            DrawManager.NewDrawCall.Sprite(
                new Vector2(
                    Player1.Hitbox.X - 3000 / 2,
                    Player1.Hitbox.Y - 1688 / 2),
                SpriteTable["ShadingSprite"], priority: 0);
            
           BaseUpdate(gameTime);
        }
        public static void MainMenu(GameTime gameTime)
        {
            
            DrawManager.NewDrawCall.Sprite(Vector2.Zero, SpriteTable["MainMenuBackground"], priority: 100);
            GameComponents.MenuManager.Update();
            BaseUpdate(gameTime);
        }
        private static void Diagnostics(GameTime gameTime)
        {
            if (!Settings.ShowDiagnostics)
                return;

            DrawManager.NewDrawCall.Text(FontTable["Arial16Bold"],"TPF: " + (int) (gameTime.ElapsedGameTime.TotalMilliseconds) + " ms", new Vector2(5, 60), priority: -1, alignment: TextDrawCall.Alignment.TopLeft);
            DrawManager.NewDrawCall.Text(FontTable["Arial16Bold"], "ROOM: " + (int)(LevelManager.CurrentLevel.LevelID.X) + " | " + LevelManager.CurrentLevel.LevelID.Y, new Vector2(5, 80), priority: -1, alignment: TextDrawCall.Alignment.TopLeft);
            DrawManager.NewDrawCall.Text(FontTable["Arial16Bold"], "X: " + (int) Player1.Position.X + " Y: " + (int) Player1.Position.Y, new Vector2(5, 100), priority: -1, alignment: TextDrawCall.Alignment.TopLeft);
             DrawManager.NewDrawCall.Text(FontTable["Arial16Bold"], "CHARGE: " + Player1._charge, new Vector2(5, 120), priority: -1, alignment: TextDrawCall.Alignment.TopLeft);
            try
            {
                DrawManager.NewDrawCall.Text(FontTable["Arial16Bold"],"FPS: " + (int) (1/gameTime.ElapsedGameTime.TotalSeconds), new Vector2(5, 40), priority: -1, alignment: TextDrawCall.Alignment.TopLeft);
            }
            catch (DivideByZeroException)
            {
                DrawManager.NewDrawCall.Text(FontTable["Arial16Bold"],"FPS: ---", new Vector2(10, 10), priority: -1);
            }
        }
    }
}