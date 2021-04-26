using GYARTE.misc;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.gameObjects.entity;
using GYARTE.gameObjects.entity.enemy;
using System.Linq;
using GYARTE.main.gameComponents;
namespace GYARTE.manager
{
      public class LevelManager
    {
        public List<Level> LoadedLevels; //C; R; L; T; B;
        public Level CurrentLevel;
        private readonly Texture2D _spriteSheet;
        private List<EnemyTemplate> _enemyTemplates;

        public LevelManager(Texture2D spriteSheet, List<EnemyTemplate> enemyTemplates, Vector2 curLevelID)
        {
            _spriteSheet = spriteSheet;
            _enemyTemplates = enemyTemplates;
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
            MapManager.Draw(CurrentLevel.LevelID);
            if (p.Hitbox.Center.X < 0)
            {
                StageNewLevel("LEFT", p);
            } 
            else if (p.Hitbox.Center.X > 1280)
            {
                StageNewLevel("RIGHT", p);
            }
            else if (p.Hitbox.Center.Y < 0)
            {
                StageNewLevel("TOP", p);
            }
            else if (p.Hitbox.Center.Y > 720)
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
            using (StreamWriter sw = new StreamWriter(@$"./levels/.{lvlID.X}_{lvlID.Y}.room"))
            {
                string ECbin = "";

                if (lvlID == Vector2.Zero)
                {
                    ECbin = "0000";
                }
                else 
                {
                    ECbin += File.Exists(@$"./levels/.{lvlID.X + 1}_{lvlID.Y}.room")    //PONTUS HAR STORA BEN
                        ? GetEC(new Vector2(lvlID.X + 1, lvlID.Y), "LEFT")
                        : null 
                    ?? rand.Next(0, 2).ToString();

                    ECbin += File.Exists(@$"./levels/.{lvlID.X - 1}_{lvlID.Y}.room") 
                        ? GetEC(new Vector2(lvlID.X - 1, lvlID.Y), "RIGHT")
                        : null 
                    ?? rand.Next(0, 2).ToString();

                    ECbin += File.Exists(@$"./levels/.{lvlID.X}_{lvlID.Y + 1}.room") 
                        ? GetEC(new Vector2(lvlID.X, lvlID.Y + 1), "BOTTOM") 
                        : null
                    ?? rand.Next(0, 2).ToString();

                    ECbin += File.Exists(@$"./levels/.{lvlID.X}_{lvlID.Y - 1}.room") 
                        ? GetEC(new Vector2(lvlID.X, lvlID.Y - 1), "TOP")
                        : null 
                    ?? rand.Next(0, 2).ToString();
                }
                string EChex = Convert.ToString(Convert.ToInt16(ECbin.ToString(), 2), 16);

                sw.Write($"{lvlID.X}.{lvlID.Y}.{EChex}.1111{(ECbin[2] == '0'? "00" : "11")}"
                +$"1111{ECbin[1]}00000000{ECbin[0]}{ECbin[1]}00000000{ECbin[0]}{ECbin[1]}00000000"
                +$"{ECbin[0]}{ECbin[1]}00000000{ECbin[0]}{ECbin[1]}00000000{ECbin[0]}{ECbin[1]}00000000"
                +$"{ECbin[0]}{ECbin[1]}00000000{ECbin[0]}{ECbin[1]}00000000{ECbin[0]}1111{(ECbin[3] == '0'? "00" : "11")}1111");

                
                if (lvlID != Vector2.Zero)
                { 
                    int max = 1;
                    for (int rnd = rand.Next(0, max); 
                        rnd < Math.Sqrt(max); 
                        rnd = rand.Next(0, max))
                    {
                        sw.Write(
                        $".{_enemyTemplates[rand.Next(_enemyTemplates.Count)].Symbol}" + 
                        $".1.{rand.Next(1, 10)}.{rand.Next(1, 10)}");
                        max *= 2;
                    }
                }
            
            }
        }

        private string GetEC(Vector2 lvlID, string direction)
        {
            if (direction.ToUpper() != "TOP" && direction.ToUpper() != "BOTTOM" && direction.ToUpper() != "LEFT" && direction.ToUpper() != "RIGHT")
            {
                throw new ArgumentException("Direction must be TOP, BOTTOM, LEFT or RIGHT");
            }

            StreamReader sr = new StreamReader(@$"./levels/.{lvlID.X}_{lvlID.Y}.room");
            string room = sr.ReadLine();
            string EChex = room.Split('.')[2];
            string ECbin = Convert.ToString(Convert.ToInt16(EChex, 16), 2).PadLeft(4, '0');

            switch(direction.ToUpper())
            {
                case "RIGHT":
                    return ECbin[0].ToString();

                 case "LEFT":
                    return ECbin[1].ToString();

                case "TOP":
                    return ECbin[2].ToString();
                
                case "BOTTOM":
                    return ECbin[3].ToString();
                
                default:
                    throw new Exception();
            }
        }
        private Level ReadNewLevel(Vector2 lvlID)
        {
            StreamReader sr = new StreamReader(@$"./levels/.{lvlID.X}_{lvlID.Y}.room");
            string[] room = sr.ReadLine().Split('.');
            System.Diagnostics.Debug.WriteLine(Convert.ToString(Convert.ToInt32(room[2], 16), 2).PadLeft(4, '0'));
            List<string> enemiesStr = new List<string>();
            for (int i=0; i < room.Length - 4; i+=4)
            {
                enemiesStr.Add($"{room[4 + i]}.{room[4 + i + 1]}.{room[4 + i + 2]}.{room[4 + i + 3]}");
            }

            List<Enemy> enemies = new List<Enemy>();

            foreach(string e in enemiesStr)
            {
                string[] args = e.Split('.');
                if (args[1] == "0") continue;
                EnemyTemplate template = _enemyTemplates.First((x) => { return x.Symbol == args[0]; });

                switch(template.Symbol.ToUpper())
                {
                    case "J":
                    case "A":
                        enemies.Add(new Goblin(template, new Vector2(Int32.Parse(args[2]) * 128, Int32.Parse(args[3]) * 72)));
                        break;
                    
                    case "F":
                        enemies.Add(
                            new Floater(template.Texture, 
                            new Vector2(
                                Int32.Parse(args[2]) * 128, 
                                Int32.Parse(args[3]) * 72), 
                            template.Speed, 
                            100, 
                            GameComponents.SpriteTable["ShotSprite"], 
                            template.IsAffectedByGravity,
                            template.IsAffectedByGravityDirection));
                        break;


                }
            }
            
            Level lvl = new Level(room[3], _spriteSheet, lvlID, enemies);
            sr.Close();

            try {MapManager.LoadedRooms.Add($"{lvlID.X}.{lvlID.Y}", room[2]);} catch (ArgumentException){}
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

                    p.Position = new Vector2(1280 - p.Hitbox.Width - 10, p.Position.Y);
                    break;

                case "TOP":
                    CurrentLevel = LoadedLevels[3];

                    LoadedLevels[0] = CurrentLevel;
                    LoadedLevels[1] = LoadLevel(new Vector2(CurrentLevel.LevelID.X + 1, CurrentLevel.LevelID.Y));
                    LoadedLevels[2] = LoadLevel(new Vector2(CurrentLevel.LevelID.X - 1, CurrentLevel.LevelID.Y));
                    LoadedLevels[3] = LoadLevel(new Vector2(CurrentLevel.LevelID.X, CurrentLevel.LevelID.Y + 1));
                    LoadedLevels[4] = prev;

                    p.Position = new Vector2(p.Position.X, 720 - 10 - p.Hitbox.Height);
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