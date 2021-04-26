using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.gameObjects.platform;
using GYARTE.gameObjects.entity.enemy;
using GYARTE.misc;
namespace GYARTE
{
    public struct Level
    {
        public Vector2 LevelID;
        public List<Platform> Platforms;
        public List<Enemy> Enemies;
        public Level(string layoutCode, Texture2D spriteSheet, Vector2 lvlID, List<Enemy> enemies)
        {
            if (layoutCode.Length != 100)
                throw new ArgumentOutOfRangeException();

            if (!layoutCode.All(bit => (bit == '0' || bit == '1')))
                throw new ArgumentException();

            LevelID = lvlID;
            Platforms = new List<Platform>();
            Enemies = new List<Enemy>(enemies);

            for (int i = 0; i < 100; i++)
            {
                char bit = layoutCode[i];
                
                if (bit == '0') 
                    continue;    
                
                int[] surr = new int[4]; //top, höger, bott, vänster
                
                string pos = i.ToString();
                if (i < 10)
                {
                    pos = "0" + pos;
                    surr[0] = 1;
                }

                surr[0] = i < 10 || layoutCode[i - 10] == '1'? 1 : 0;
                surr[1] = pos[1] == '9' || layoutCode[i + 1] == '1' ? 1 : 0;
                surr[2] = i > 89 || layoutCode[i + 10] == '1'? 1 : 0;
                surr[3] = pos[1] == '0' || layoutCode[i - 1] == '1' ? 1 : 0;

                int yOffset = 0;
                
                switch (surr.Sum()) //TODO: OPTIMZE
                {
                    case 1:
                        switch (Array.IndexOf(surr, 1))
                        {
                            case 0:
                                yOffset = 3;
                                break;

                            case 1:
                                yOffset = 1;
                                break;

                            case 2:
                                yOffset = 2;
                                break;

                            case 3:
                                yOffset = 0;
                                break;
                        }
                        break;
                    
                    case 2:
                        string switchArg = "";
                        foreach (int dir in surr)
                        {
                            switchArg += dir.ToString();
                        }

                        switch (switchArg)
                        {
                            case "1100":
                                yOffset = 2;
                                break;
                            
                            case "1010":
                                yOffset = 5;
                                break;
                            
                            case "1001":
                                yOffset = 1;
                                break;
                            
                            case "0110":
                                yOffset = 3;
                                break;
                            
                            case "0101":
                                yOffset = 4;
                                break;
                            
                            case "0011":
                                yOffset = 0;
                                break;
                        }
                        
                        
                        break;
                    
                    case 3:
                        switch (Array.IndexOf(surr, 0))
                        {
                            case 0:
                                yOffset = 0;
                                break;
                            
                            case 1:
                                yOffset = 3;
                                break;
                            
                            case 2:
                                yOffset = 1;
                                break;
                            
                            case 3:
                                yOffset = 2;
                                break;
                        }
                        break;
                }
                
                Platforms.Add(new Platform(spriteSheet, new Vector2(128 * Int16.Parse(pos[1].ToString()), 72 *  Int16.Parse(pos[0].ToString())), new Vector2(128 * surr.Sum(), 72 * yOffset),  spriteSize: new Vector2(128, 72)));
            }
        }

        public void UpdateLevel()
        {
            foreach (var i in Platforms)
            {
                i.Update();
            }
        }

        public void DrawLevel()
        {
            foreach (var i in Platforms)
            {
                i.Draw();
            }
        }
    }
}