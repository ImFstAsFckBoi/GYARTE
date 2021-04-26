using System.Net.Mime;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.manager;
using MonoManager;
using GYARTE.gameObjects.entity;
using GYARTE.menu;
using GYARTE.misc;
namespace GYARTE.main.gameComponents
{
    public static partial class GameComponents
    {
        public static void Initialize(GraphicsDevice gDevice, GraphicsDeviceManager gManager, ContentManager content)
        {
            var rng = new Random();
            GameState = new GameState();

            LoadContent(content);
            
            InputManager = new InputManager();
            MenuManager = new MenuManager();
            
            MenuManager.AddMenu(new Menu("PAUSE", new List<MenuItem>()
                {
                    {new MenuItem("RESUME", () => 
                        {
                            GameState = GameState.Run; 
                        })},

                    { new MenuItem("RETURN", () => 
                        {
                            GameState = GameState.MainMenu; 
                        })},

                    { new MenuItem("Amogus", () => 
                        {                            
                            Player1 = new Player(SpriteTable["RedSus"], Player1.Position, Settings.PlayerSpeed, 100,  true, true, new Vector2(94, 122), true);
                        })},

                    { new MenuItem("Kooky", () => 
                        {                            
                            Player1 = new Player(SpriteTable["EnemySprite"], Player1.Position, Settings.PlayerSpeed, 100,  true, true, new Vector2(80, 116), true);
                        })}
                }), 
                GameState.PauseMenu);
            
            MenuManager.AddMenu(new Menu("MAIN MENU", new List<MenuItem>()
            {
                { new MenuItem("PLAY", () => 
                {
                    GameState = GameState.Run;
                })},

                 { new MenuItem("QUIT", () => {
                     Environment.Exit(0);
                 })}
            }),
                GameState.MainMenu);
            

            LevelManager = new LevelManager((Texture2D)SpriteTable["PlatformSprite"], EnemyTemplate.LoadManifest(content), Vector2.Zero);

            Settings = SettingsManager.LoadSettings();
            WindowConfig = WindowManager.WindowInit(Settings, gManager, gDevice);

            DrawManager = new DrawManager(gDevice, WindowConfig);

            Player1 = new Player(SpriteTable["PlayerSprite"], new Vector2(600, 400), Settings.PlayerSpeed, 100,  true, true, new Vector2(60, 99), true);
        
               

            //Enemy2 = new Floater(SpriteTable["EyeSprite"], new Vector2(200, 200), 0, 100, SpriteTable["ShotSprite"]);
            //Enemies.Add(Enemy1);
            //Enemies.Add(Enemy2);

            //Coin = new DynamicItem((Texture2D) SpriteTable["CoinSprite"], PlatformGrid.Grid(3, 2), true, target => target.Hp += 100);
            //Items.Add(Coin);
            /*Items.Add(new DynamicItem((Texture2D)SpriteTable["CoinSprite"], new Vector2(rng.Next(WindowConfig.WindowWidth),
                    rng.Next(WindowConfig.WindowHeight)), true,
                (target) => target.Hp += 100));
            Items.Add(new DynamicItem((Texture2D)SpriteTable["CoinSprite"], new Vector2(rng.Next(WindowConfig.WindowWidth),
                    rng.Next(WindowConfig.WindowHeight)), true,
                (target) => target.Hp += 100));
            Items.Add(new DynamicItem((Texture2D)SpriteTable["CoinSprite"], new Vector2(rng.Next(WindowConfig.WindowWidth),
                    rng.Next(WindowConfig.WindowHeight)), true,
                (target) => target.Hp += 100));*/


            GlobalG = Settings.GravitySpeed;
        }
        private static void LoadContent(ContentManager content)
        {
            string IMGDIR = "./assets/img";
            string FONTDIR = "./assets/font";
            SpriteTable = new Dictionary<string, Texture2D>()
            { // {"-----------", content.Load<Texture2D/SpriteFont>(@$"{IMGDIR/FONTDIR}/-----------")}
                {"PlayerSprite", content.Load<Texture2D>(@$"{IMGDIR}/player_sprite_sheet")},
                {"PlatformSprite", content.Load<Texture2D>(@$"{IMGDIR}/platform_sprite_sheet")},
                {"EnemySprite", content.Load<Texture2D>(@$"{IMGDIR}/jubngkook_spritesheet")}, 
                {"BackgroundSprite", content.Load<Texture2D>(@$"{IMGDIR}/background")},
                {"CoinSprite", content.Load<Texture2D>(@$"{IMGDIR}/dogecoin")},
                {"EyeSprite", content.Load<Texture2D>(@$"{IMGDIR}/eye")},
                {"ShotSprite", content.Load<Texture2D>(@$"{IMGDIR}/shot")},
                {"HealthBar", content.Load<Texture2D>(@$"{IMGDIR}/healthbar")},
                {"HealthBarOutline", content.Load<Texture2D>(@$"{IMGDIR}/healthbar_outline")},
                {"PauseMenuSprite", content.Load<Texture2D>(@$"{IMGDIR}/menu")},
                {"ShadingSprite", content.Load<Texture2D>(@$"{IMGDIR}/shading")},
                {"MainMenuBackground", content.Load<Texture2D>(@$"{IMGDIR}/mainmenu")},
                {"RedSus", content.Load<Texture2D>(@$"{IMGDIR}/amogus_sprietsheet")},
                {"MapBack", content.Load<Texture2D>(@$"{IMGDIR}/mapback")},
                {"MapSheet", content.Load<Texture2D>(@$"{IMGDIR}/mapspritesheet")},
                {"Beam", content.Load<Texture2D>(@$"{IMGDIR}/beam")}
            };

            FontTable = new Dictionary<string, SpriteFont>()
            {
                {"Arial16Bold", content.Load<SpriteFont>(@$"{FONTDIR}/Arial_16_Bold")}
            };

            /*
                jungkook:   (80, 116)
                amogus:     (94, 122)
                player:     (60, 99)
            */
        }
    }
}
