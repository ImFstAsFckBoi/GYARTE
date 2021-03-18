using Microsoft.Xna.Framework;
using GYARTE.main.gameComponents;
namespace GYARTE.main
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics;
        public static bool SCF = false; //TODO: BYE!
        
        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        
        protected override void Initialize()
        {
            GameComponents.Initialize(GraphicsDevice, Graphics, Content);

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            
        }
        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            
            switch (GameComponents.GameState)
            {

                case GameState.Run:
                    GameComponents.Run(gameTime);
                    break;

                case GameState.PauseMenu:
                    GameComponents.PauseMenu(gameTime);
                    break;

                case GameState.MainMenu:
                    GameComponents.MainMenu(gameTime);
                    break;
            }

            if (SCF) //FIX
            {
                GameComponents.WindowConfig.UpdateFullscreen(GameComponents.Settings, GraphicsDevice, Graphics);
                SCF = false;
                System.Diagnostics.Debug.WriteLine("SCF!!");
                System.Diagnostics.Debug.WriteLine(GameComponents.Settings.Fullscreen);
            } 
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GameComponents.DrawManager.DrawQueuedCalls();

            base.Draw(gameTime);
        }
        
    }
}
