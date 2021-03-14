﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GYARTE.gameObjects.entity;
using GYARTE.main.gameComponents;
using GYARTE.misc;


namespace GYARTE.manager
{
    public static class InputManager
    {
        private static KeyboardState _keyboardState;
        private static readonly Dictionary<Keys, bool> HasBeenPressed = new Dictionary<Keys, bool>();

        private static void PressedOnce(Keys key, Execution execution)
        {
            HasBeenPressed[key] = IfPressedOnce(key, execution);
        }
        private static bool IfPressedOnce(Keys key, Execution execution)
        {
            if (_keyboardState.IsKeyDown(key) && !HasBeenPressed[key])
            {
                execution.Invoke();
                
                return true;
            }
            
            return _keyboardState.IsKeyDown(key) && HasBeenPressed[key];
        }
        internal static bool IfPressedOnce(List<Keys> keys, Execution execution)
        {
            throw new NotImplementedException();
            /*
            var key = keys.Find(key =>
            {
                return _keyboardState.IsKeyDown(key);
            });
            
            if (_keyboardState.IsKeyDown(key) && !HasBeenPressed[key])
            {
                execution.Invoke();
                
                return true;
            }
            return _keyboardState.IsKeyDown(key) && HasBeenPressed[key];
            */
        }
        
        public static void InputCheck(Player player, GameState gameState, Settings settings)
        {
            _keyboardState = Keyboard.GetState();
            
            PressedOnce(Keys.Escape, () =>
            {
                GameComponents.GameState = GameComponents.GameState == GameState.Run ? GameState.PauseMenu : GameState.Run;
                player.Velocity.X = 0; 
            });
            
            /*
             * _keyboardState.IsKeyDown(Keys.Space) ||
             * _keyboardState.IsKeyDown(Keys.Up) ||
             * _keyboardState.IsKeyDown(Keys.Down) || 
             * _keyboardState.IsKeyDown(Keys.S) ||
             * _keyboardState.IsKeyDown(Keys.W)
             */

            switch (gameState) 
            {
                case GameState.Run:
                    PressedOnce(Keys.Space, () =>
                    {
                        GameComponents.GDirection *= (int) player.Velocity.Y == 0 ? -1 : 1;
                    });
                
                    player.Direction.X = _keyboardState.IsKeyDown(Keys.A) || _keyboardState.IsKeyDown(Keys.Left) 
                        ? -1
                        : player.Direction.X = _keyboardState.IsKeyDown(Keys.D) || _keyboardState.IsKeyDown(Keys.Right) 
                            ? 1
                            : player.Direction.X;

                    player.Velocity.X = _keyboardState.IsKeyDown(Keys.A) || _keyboardState.IsKeyDown(Keys.Left) 
                        ? -player.Speed
                        : player.Velocity.X = _keyboardState.IsKeyDown(Keys.D) || _keyboardState.IsKeyDown(Keys.Right) 
                            ? player.Speed
                            : 0f;
                    break;
            }
        }
    }
}