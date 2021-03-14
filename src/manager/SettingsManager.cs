using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GYARTE.manager
{
    public struct Settings
    {
        public Settings(bool fullscreen, int windowX, int windowY, int playerSpeed, int enemySpeed, int gravitySpeed, bool showDiagnostics)
        {
            Fullscreen = fullscreen;
            WindowX = windowX;
            WindowY = windowY;
            PlayerSpeed = playerSpeed;
            EnemySpeed = enemySpeed;
            GravitySpeed = gravitySpeed;
            ShowDiagnostics = showDiagnostics;
        }
        
        public Settings(int fullscreen, int windowX, int windowY, int playerSpeed, int enemySpeed, int gravitySpeed, int showDiagnostics) 
            : this(fullscreen == 1, windowX, windowY, playerSpeed, enemySpeed, gravitySpeed, showDiagnostics == 1)
        {
        }
        
        public Settings(Dictionary<string, int> json) 
            : this(json["Fullscreen"], json["WindowX"], json["WindowY"], json["PlayerSpeed"], json["EnemySpeed"], json["GravitySpeed"], json["ShowDiagnostics"])
        {
        }

        public Dictionary<string, int> JsonStructure()
        {
            return new Dictionary<string, int>  () //TODO: STRUCT
            {
                {"Fullscreen", Fullscreen ? 1 : 0},
                {"WindowX", WindowX}, //1280, 1920, 2560, 3840
                {"WindowY", WindowY}, //720, 1080, 1440, 2160
                {"PlayerSpeed", PlayerSpeed},
                {"EnemySpeed", EnemySpeed},
                {"GravitySpeed", GravitySpeed},
                {"ShowDiagnostics", 0}
            
            };
        }

        public bool Fullscreen { get; set; }
        public int WindowX;
        public int WindowY;
        public int PlayerSpeed;
        public int EnemySpeed;
        public int GravitySpeed;
        public bool ShowDiagnostics;
    }
    
    public static class SettingsManager
    {
        //DEFAULT SETTINGS
        private static readonly Settings DefaultSettings = new Settings(
            false, 
            1280, 
            720, 
            600, 
            200, 
            800,
            false);

        public static readonly Dictionary<string, int> DefaultSettingsJson = new Dictionary<string, int>()
        {
            {"Fullscreen", 0},
            {"WindowX", 1280}, //1280, 1920, 2560, 3840
            {"WindowY", 720}, //720, 1080, 1440, 2160
            {"PlayerSpeed", 600},
            {"EnemySpeed", 200},
            {"GravitySpeed", 800},
            {"ShowDiagnostics", 0}
        };
        
        public static Settings LoadSettings()
        {
            try
            {
                return new Settings(
                    JsonConvert.DeserializeObject<Dictionary<string, int>>(
                        File.ReadAllText(@"./settings.json")));
            }
            catch (FileNotFoundException)
            {
                try
                {
                    SaveSettings(DefaultSettings);
                    return new Settings(
                        JsonConvert.DeserializeObject<Dictionary<string, int>>(
                            File.ReadAllText(@"./settings.json")));
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR WHEN WRITING NEW SETTINGS FILE, USING TEMP DEFAULT: " + e);
                    return DefaultSettings;
                }
            }
        }

        public static void ApplySettings()
        {
            //TODO: IMPLEMENT
        }
        public static void SaveSettings(Settings settings)
        {
            File.WriteAllText(
                @"./settings.json", 
                JsonConvert
                    .SerializeObject(settings.JsonStructure(), Formatting.Indented));
        }
    }
}