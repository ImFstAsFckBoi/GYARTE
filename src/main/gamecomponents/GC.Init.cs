using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.manager;
using GYARTE.gameObjects.entity;

/*
<Compile Include="Properties\AssemblyInfo.cs" />
    <Compile Include="src\game_object\entity\enemy\Enemy.cs" />
    <Compile Include="src\game_object\entity\enemy\Floater.cs" />
    <Compile Include="src\game_object\entity\Entity.cs" />
    <Compile Include="src\game_object\entity\Item.cs" />
    <Compile Include="src\game_object\entity\Player.cs" />
    <Compile Include="src\game_object\GameObject.cs" />
    <Compile Include="src\game_object\platform\Platfrom.cs" />
    <Compile Include="src\level\Level.cs" />
    <Compile Include="src\main\Game1.cs" />
    <Compile Include="src\main\GC.Compontents.cs" />
    <Compile Include="src\main\GC.Init.cs" />
    <Compile Include="src\main\GC.Managers.cs" />
    <Compile Include="src\main\GC.Objects.cs" />
    <Compile Include="src\main\GC.Update.cs" />
    <Compile Include="src\main\Program.cs" />
    <Compile Include="src\manager\CullingManager.cs" />
    <Compile Include="src\manager\InputManager.cs" />
    <Compile Include="src\manager\DrawManager.cs" />
    <Compile Include="src\manager\LevelManager.cs" />
    <Compile Include="src\manager\SettingsManager.cs" />
    <Compile Include="src\manager\WindowManager.cs" />
    <Compile Include="src\menu\IMenuItem.cs" />
    <Compile Include="src\menu\Menu.cs" />
    <Compile Include="src\menu\MenuManager.cs" />
    <Compile Include="src\misc\General.cs" />
*/
namespace GYARTE.main.gameComponents
{
    public static partial class GameComponents
    {
        public static void Initialize(GraphicsDevice gDevice, GraphicsDeviceManager gManager, ContentManager content)
        {
            var rng = new Random();

            GameState = new GameState();
            GameState = GameState.PauseMenu;
            // TODO: Add your initialization logic here
            LoadContent(content);


            LevelManager = new LevelManager((Texture2D)SpriteTable["PlatformSprite"], Vector2.Zero);

            Settings = SettingsManager.LoadSettings();
            WindowConfig = WindowManager.WindowInit(Settings, gManager, gDevice);

            DrawManager = new DrawManager(gDevice, WindowConfig);

            Player1 = new Player((Texture2D)SpriteTable["PlayerSprite"], new Vector2(400, 400), 400, 100, isAffectedByGravityDirection: true, spriteSize: new Vector2(60, 99));

            Enemy1 = new Goblin((Texture2D)SpriteTable["EnemySprite"], new Vector2(600, 200), Settings.EnemySpeed, 100);

            //Enemy2 = new Floater((Texture2D) SpriteTable["EyeSprite"], new Vector2( 300, 500), 0, 100, (Texture2D) SpriteTable["ShotSprite"], isAffectedByGravity: false);
            Enemies.Add(Enemy1);
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
            { // {"-----------", Content.Load<Texture2D>(@$"{IMGDIR/FONTDIR}/-----------")},
                {"PlayerSprite", content.Load<Texture2D>(@$"{IMGDIR}/player_sprite_sheet")},
                {"PlatformSprite", content.Load<Texture2D>(@$"{IMGDIR}/platform_sprite_sheet")},
                {"EnemySprite", content.Load<Texture2D>(@$"{IMGDIR}/enemy")},
                {"BackgroundSprite", content.Load<Texture2D>(@$"{IMGDIR}/background")},
                {"CoinSprite", content.Load<Texture2D>(@$"{IMGDIR}/dogecoin")},
                {"EyeSprite", content.Load<Texture2D>(@$"{IMGDIR}/eye")},
                {"ShotSprite", content.Load<Texture2D>(@$"{IMGDIR}/shot")},
                {"HealthBar", content.Load<Texture2D>(@$"{IMGDIR}/healthbar")},
                {"HealthBarOutline", content.Load<Texture2D>(@$"{IMGDIR}/healthbar_outline")},
                {"PauseMenuSprite", content.Load<Texture2D>(@$"{IMGDIR}/menu")},
                {"ShadingSprite", content.Load<Texture2D>(@$"{IMGDIR}/shading")},
                {"MainMenuBackground", content.Load<Texture2D>(@$"{IMGDIR}/mainmenu")}
            };

            FontTable = new Dictionary<string, SpriteFont>()
            {
                {"Arial16Bold", content.Load<SpriteFont>(@$"{FONTDIR}/Arial_16_Bold")}
            };
        }
    }
}
