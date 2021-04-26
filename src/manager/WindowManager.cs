using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GYARTE.manager 
{
    public static class WindowManager
    {
        private  static RenderTarget2D TargetResolution;
        private  static int WindowWidth;
        private static int WindowHeight;
        private static Vector2 WindowScale;
        private static Vector2 WindowCenter => new Vector2((float) WindowWidth / 2, (float) WindowHeight / 2);
        

        // public int ScreenWidth = Game1.graphics.PreferredBackBufferWidth;
        // public int screenHeight = Game1.graphics.PreferredBackBufferHeight;
        public static WindowConfiguration WindowInit(Settings settings, GraphicsDeviceManager graphicsDeviceManager, GraphicsDevice graphicsDevice)
        {
            if (settings.Fullscreen)
            {
                graphicsDeviceManager.IsFullScreen = true;
                WindowWidth = 1280;
                WindowHeight = 720;
            }
            else
            {
                WindowWidth = settings.WindowX;
                WindowHeight = settings.WindowY; 
            }
            
            WindowScale = new Vector2((float) WindowWidth / 1280, (float)WindowHeight / 720);
            
            TargetResolution = new RenderTarget2D(graphicsDevice, 1280, 720);

            if (Math.Abs((float) WindowWidth / WindowHeight - 16f / 9f) > 0.0001)
            {
                throw new Exception("Window ratio != 16:9");
            }
            
            graphicsDeviceManager.PreferredBackBufferWidth = settings.WindowX; 
            graphicsDeviceManager.PreferredBackBufferHeight = settings.WindowY;
            
            graphicsDeviceManager.ApplyChanges();

            var config = new WindowConfiguration(
                TargetResolution, 
                settings.Fullscreen, 
                settings.WindowX, 
                settings.WindowY);

            return config;
        }
    }

    public struct WindowConfiguration
    {
        public WindowConfiguration(RenderTarget2D targetResolution, bool fullscreen, int width, int height)
        {
            TargetResolution = targetResolution;
            WindowWidth = width;
            WindowHeight = height;
            Fullscreen = fullscreen;
        }

        public void UpdateFullscreen(Settings settings, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphicsDeviceManager)
        {
            if (settings.Fullscreen)
            {
                graphicsDeviceManager.IsFullScreen = true;

                WindowWidth = 1280;
                WindowHeight = 720;

                Fullscreen = true;
            }
            else
            {
                graphicsDeviceManager.IsFullScreen = false;

                WindowWidth = settings.WindowX;
                WindowHeight = settings.WindowY;

                Fullscreen = false;
            }
            
            TargetResolution = new RenderTarget2D(graphicsDevice, 1280, 720);
            
            graphicsDeviceManager.PreferredBackBufferWidth = settings.WindowX; 
            graphicsDeviceManager.PreferredBackBufferHeight = settings.WindowY;
            
            graphicsDeviceManager.ApplyChanges();

        }
        public RenderTarget2D TargetResolution;
        public int WindowWidth;
        public int WindowHeight;
        public bool Fullscreen;
        public Vector2 WindowCenter => new Vector2((float) WindowWidth / 2, (float) WindowHeight / 2);
        public Vector2 WindowScale => new Vector2((float) WindowWidth / 1280, (float) WindowHeight / 720);
        
    }
}