﻿using System.Security.AccessControl;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.gameObjects.entity;

namespace GYARTE.manager
{
      public class LevelManager
    {
        public List<Level> LoadedLevels; //C; R; L; T; B;
        public Level CurrentLevel;
        private readonly Texture2D _spriteSheet;
        
        
        public LevelManager(Texture2D spriteSheet, Vector2 curLevelID)
        {
            _spriteSheet = spriteSheet;
            
            LoadedLevels = new List<Level>()
            {
                {LoadLevel(curLevelID)}, 
                {LoadLevel(new Vector2(curLevelID.X + 1, curLevelID.Y))},
                {LoadLevel(new Vector2(curLevelID.X - 1, curLevelID.Y))},
                {LoadLevel(new Vector2(curLevelID.X, curLevelID.Y + 1))},
                {LoadLevel(new Vector2(curLevelID.X, curLevelID.Y - 1))}
            };

            CurrentLevel = LoadedLevels[0];
        }

        public void Update(Player p)
        {
            CurrentLevel.UpdateLevel();

            if (p.Rect.Center.X < 0)
            {
                StageNewLevel("LEFT", p);
            } 
            else if (p.Rect.Center.X > 1280)
            {
                StageNewLevel("RIGHT", p);
            }
            else if (p.Rect.Center.Y < 0)
            {
                StageNewLevel("TOP", p);
            }
            else if (p.Rect.Center.Y > 720)
            {
                StageNewLevel("BOTTOM", p);
            }

        }
        
        public Level LoadLevel(Vector2 curLevelID) //ASYNC???
        {
            Level lvl;
            try
            {
                lvl = ReadNewLevel(curLevelID);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory("./levels");
                WriteNewLevel(curLevelID);
                lvl = ReadNewLevel(curLevelID);
            }
            catch (System.Exception)
            {
                WriteNewLevel(curLevelID);
                lvl = ReadNewLevel(curLevelID);
            }
           
            return lvl;
        }

        private void WriteNewLevel(Vector2 lvlID)//TODO: SEEDING?
        {
            Random rand = new Random();
            StreamWriter sw = new StreamWriter(@$"./levels/.{lvlID.X}_{lvlID.Y}.room");
            /*
            for (int _ = 0; _ < 100; _++)
            {
                sw.Write(rand.Next(0, 2));
            }
            */
            string EChex = Convert.ToString(rand.Next(16), 16);
            string ECbin = Convert.ToString(Convert.ToInt32(EChex, 16), 2).PadLeft(4, '0');
            sw.Write($"{lvlID.X}.{lvlID.Y}.{EChex}.1111{(ECbin[2] == '0'? "00" : "11")}1111{ECbin[1]}00000000{ECbin[0]}{ECbin[1]}00000000{ECbin[0]}{ECbin[1]}00000000{ECbin[0]}{ECbin[1]}00000000{ECbin[0]}{ECbin[1]}00000000{ECbin[0]}{ECbin[1]}00000000{ECbin[0]}{ECbin[1]}00000000{ECbin[0]}{ECbin[1]}00000000{ECbin[0]}1111{(ECbin[3] == '0'? "00" : "11")}1111");
            sw.Close();
        }

        private Level ReadNewLevel(Vector2 lvlID)
        {
            StreamReader sr = new StreamReader(@$"./levels/.{lvlID.X}_{lvlID.Y}.room");
            string[] room = sr.ReadLine().Split('.');
            System.Diagnostics.Debug.WriteLine(Convert.ToString(Convert.ToInt32(room[2], 16), 2).PadLeft(4, '0'));
            Level lvl = new Level(room[3], _spriteSheet, lvlID);
            sr.Close();
            return lvl;
        }
        private void StageNewLevel(string code, Player p) 
        {
            Level prev = CurrentLevel;

            switch(code.ToUpper()) //C; R; L; T; B;
            {
                case "RIGHT":
                    
                    CurrentLevel = LoadedLevels[1];

                    LoadedLevels[0] = CurrentLevel;
                    LoadedLevels[1] = LoadLevel(new Vector2(CurrentLevel.LevelID.X + 1, CurrentLevel.LevelID.Y));
                    LoadedLevels[2] = prev;
                    LoadedLevels[3] = LoadLevel(new Vector2(CurrentLevel.LevelID.X, CurrentLevel.LevelID.Y + 1));
                    LoadedLevels[4] = LoadLevel(new Vector2(CurrentLevel.LevelID.X, CurrentLevel.LevelID.Y - 1));

                    p.Position = new Vector2(10, p.Position.Y);
                    break;

                case "LEFT":
                   
                    CurrentLevel = LoadedLevels[2];

                    LoadedLevels[0] = CurrentLevel;
                    LoadedLevels[1] = prev;
                    LoadedLevels[2] = LoadLevel(new Vector2(CurrentLevel.LevelID.X - 1, CurrentLevel.LevelID.Y));
                    LoadedLevels[3] = LoadLevel(new Vector2(CurrentLevel.LevelID.X, CurrentLevel.LevelID.Y + 1));
                    LoadedLevels[4] = LoadLevel(new Vector2(CurrentLevel.LevelID.X, CurrentLevel.LevelID.Y - 1));

                    p.Position = new Vector2(1280 - p.Rect.Width - 10, p.Position.Y);
                    break;

                case "TOP":
                    CurrentLevel = LoadedLevels[3];

                    LoadedLevels[0] = CurrentLevel;
                    LoadedLevels[1] = LoadLevel(new Vector2(CurrentLevel.LevelID.X + 1, CurrentLevel.LevelID.Y));
                    LoadedLevels[2] = LoadLevel(new Vector2(CurrentLevel.LevelID.X - 1, CurrentLevel.LevelID.Y));
                    LoadedLevels[3] = LoadLevel(new Vector2(CurrentLevel.LevelID.X, CurrentLevel.LevelID.Y + 1));
                    LoadedLevels[4] = prev;

                    p.Position = new Vector2(p.Position.X, 720 - 10 - p.Rect.Height);
                    break;

                case "BOTTOM":
                    CurrentLevel = LoadedLevels[4];

                    LoadedLevels[0] = CurrentLevel;
                    LoadedLevels[1] = LoadLevel(new Vector2(CurrentLevel.LevelID.X + 1, CurrentLevel.LevelID.Y));
                    LoadedLevels[2] = LoadLevel(new Vector2(CurrentLevel.LevelID.X - 1, CurrentLevel.LevelID.Y));
                    LoadedLevels[3] = prev;
                    LoadedLevels[4] = LoadLevel(new Vector2(CurrentLevel.LevelID.X, CurrentLevel.LevelID.Y - 1));

                    p.Position = new Vector2(p.Position.X, 10);
                    break;
            }
        }
    }
}