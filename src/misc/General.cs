using System.IO;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GYARTE.gameObjects.entity;
using GYARTE.gameObjects;
using GYARTE.main.gameComponents;


namespace GYARTE.misc
{
    public delegate void Execution();
    
    public class Shot : GameObject
    {
        public readonly int Damage;
        private Vector2 _vector;
        private readonly LivingEntity _target;
        public double TTL;
        public double TimeOfBirth;
        
        public double Age(GameTime gameTime) => gameTime.TotalGameTime.TotalMilliseconds - TimeOfBirth;

        public Shot(Texture2D sprite, Vector2 startPosition, int damage, double ttlms, GameTime gameTime, LivingEntity target, Vector2? spriteSize = null) : base(sprite, startPosition, spriteSize)
        {
            Damage = damage;
            _target = target;
            TTL = ttlms;
            TimeOfBirth = gameTime.TotalGameTime.TotalMilliseconds;
            
            double alpha = Math.Atan2(Position.X - target.Position.X, Position.Y - target.Position.Y) + Math.PI;
            _vector = new Vector2((float) Math.Sin(alpha), (float) Math.Cos(alpha));
        }
        
        public void Update(GameTime gameTime)
        {
            if (Vector2.Distance(Position, _target.Position) < 300)
            {
                double alpha = Math.Atan2(Position.X - _target.Position.X, Position.Y - _target.Position.Y) + Math.PI;
                _vector = (49 * _vector + new Vector2((float) Math.Sin(alpha), (float) Math.Cos(alpha))) / 50;
            }
            
            Position += _vector * 1000 *(float) gameTime.ElapsedGameTime.TotalSeconds;
            
            base.Update();
        }

        public bool HitScan()
        {
            if (!Hitbox.Intersects(_target.Hitbox)) return false;
            return true;
        } 
        
    }
    public struct EnemyTemplate
    {
        public string Name;
        public string Symbol;
        public Vector2 SpriteSize;
        public float Speed;
        public int HP;
        public bool Animator;
        public bool IsAffectedByGravity;
        public bool IsAffectedByGravityDirection;
        public Texture2D Texture;

        public EnemyTemplate(string manifestCode, ContentManager contentManager)
        {
            string[] args = manifestCode.Split('.');

            Name = args[0];
            Symbol = args[1];
            string file_path = args[2];
            Texture = contentManager.Load<Texture2D>($@"./assets/img/{file_path}");
            int x = Int32.Parse(args[3]);
            int y = Int32.Parse(args[4]);
            SpriteSize = new Vector2(x == 0? Texture.Width : x, y == 0? Texture.Height : y);
            Speed = Single.Parse(args[5]);
            HP = Int32.Parse(args[6]);
            Animator = args[7] == "1";

            if (Animator && (x == 0 || y == 0))
                throw new ArgumentException("Sprite dimensions must be defined if Animator is enabled 🤬");

            IsAffectedByGravity = args[8] == "1";
            IsAffectedByGravityDirection = args[9] == "1";
        }

        public static List<EnemyTemplate> LoadManifest(ContentManager contentManager, string manifestFilePath = @"./enemy.manifest")
        {
            StreamReader SR = new StreamReader(manifestFilePath);

            List<EnemyTemplate> list = new List<EnemyTemplate>();
 
            string line;

            while ((line = SR.ReadLine()) != null)
            {
                if (line.StartsWith('#')) continue;
                list.Add(new EnemyTemplate(line, contentManager));
            }
            SR.Close();
            return list;
        }
    }
}