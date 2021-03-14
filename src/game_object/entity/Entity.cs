#nullable enable
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.gameObjects.platform;
using GYARTE.main.gameComponents;
using GYARTE.manager;

namespace GYARTE.gameObjects.entity
{
    public class Entity : GameObject
    {
        public Vector2 Velocity;
        public bool IsAffectedByGravity;
        public bool IsAffectedByGravityDirection;
        public Vector2 Direction;
        public int WalkCycle;
        public float Speed;

        protected Entity(Texture2D sprite,
            Vector2 startPosition,
            float speed,
            bool isAffectedByGravity = true,
            bool isAffectedByGravityDirection = false, Vector2? spriteSize = null)
            : base(sprite, startPosition, spriteSize)
        {
            Speed = speed;
            IsAffectedByGravity = isAffectedByGravity;
            IsAffectedByGravityDirection = isAffectedByGravityDirection;
            Direction = new Vector2(1, 1);
            WalkCycle = 0;
        }

        protected Rectangle GetSubSpriteRect()
        {
            int x = (int)Direction.X == 1 ? 0 : Sprite.Width - (int)SpriteSize.X;
            int y = (int)Direction.Y == 1 ? 0 : Sprite.Height / 2;
            int w = (int)SpriteSize.X;
            int h = (int)SpriteSize.Y;

            if (Velocity.X != 0 && Velocity.Y == 0)
            {
                WalkCycle += 1;

                if (WalkCycle < 5)
                {
                    x += (int)SpriteSize.X * (int)Direction.X;
                }
                else if (WalkCycle < 10)
                {
                    x += 2 * (int)SpriteSize.X * (int)Direction.X;
                }
                else
                {
                    WalkCycle = 0;
                }
            }

            return new Rectangle(x, y, w, h);
        }


        public virtual void Update(float g, int gDirection, GameTime gameTime, IEnumerable<Platform> platforms)
        {
            Direction.Y = IsAffectedByGravityDirection ? gDirection : 1;

            MoveAnCheckPlatfroms(g, gDirection, gameTime, platforms);

            base.Update();
        }

        public virtual void Update(float g, int gDirection, GameTime gameTime, IEnumerable<Platform> platforms, GameObject target)
        {
            Direction.Y = IsAffectedByGravityDirection ? gDirection : 1;
            //Move(g, gDirection, gameTime, target);

            //if (platforms != null)
            //CheckPlatforms(platforms);

            Velocity.Y = (IsAffectedByGravityDirection ? gDirection : 1) * (IsAffectedByGravity ? g : 0);

            Position.X += (Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
            /*foreach(var p in platforms)
            {
                if (!Rect.Intersects(p.Rect)) return;
                if (Position.X < p.Rect.Center.X)
                {
                    Position.X = p.Rect.X - Width;
                    Velocity.X = 0;
                }
                else if (Position.X > p.Rect.Center.X)
                {
                    Position.X = p.Rect.Right;
                    Velocity.X = 0;
                }
            }*/
            
            Position.Y += (Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
            foreach (var p in platforms)
            {
                if (Rect.Intersects(p.Rect))
                {
                    if (Position.Y < p.Rect.Center.Y)
                    {
                        Position.Y = p.Rect.Y - Height;
                        Velocity.Y = 0;
                    }
                    else if (Position.Y > p.Rect.Center.Y)
                    {
                        Position.Y = p.Rect.Bottom;
                        Velocity.Y = 0;
                    }
                }

            }

            base.Update();
        }

        private void MoveAnCheckPlatfroms(float g, int gDirection, GameTime gameTime, IEnumerable<Platform> platforms, GameObject? target = null)
        {
            if (target != null)
                Velocity.X = Speed * (!(new Rectangle((int)Position.X, 0, Rect.Width, GameComponents.WindowConfig.WindowHeight)
                    .Intersects(target.Rect)) ?
                    target.Position.X < Position.X ?
                        -1
                        : 1
                    : 0);

            Velocity.Y = (IsAffectedByGravityDirection ? gDirection : 1) * (IsAffectedByGravity ? g : 0);

            Position.X += (Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
            foreach (var p in platforms.Where((p) => { return (Math.Abs(p.Position.X) - Math.Abs(Position.X) < 256);}))
            {
                CheckStandPlatformX(p);
            }
             


            Position.Y += (Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
            foreach (var p in platforms.Where((p) => { return (Math.Abs(p.Position.X) - Math.Abs(Position.X) < 256);}
            ))
            {
                CheckStandPlatformY(p);
            }



        }



        protected virtual void CheckStandPlatformX(Platform platform)
        {
            if (Rect.Intersects(platform.Rect))
            {
                if (Position.X < platform.Rect.Center.X)
                {
                    Position.X = platform.Rect.X - Width;
                    Velocity.X = 0;
                }
                else if (Position.X > platform.Rect.Center.X)
                {
                    Position.X = platform.Rect.Right;
                    Velocity.X = 0;
                }
            }

        }
        protected virtual void CheckStandPlatformY(Platform platform)
        {
            if (Rect.Intersects(platform.Rect))
            {
                if (Position.Y < platform.Rect.Center.Y)
                {
                    Position.Y = platform.Rect.Y - Height;
                    Velocity.Y = 0;
                }
                else if (Position.Y > platform.Rect.Center.Y)
                {
                    Position.Y = platform.Rect.Bottom;
                    Velocity.Y = 0;
                }
            }
        }
    }


    public class LivingEntity : Entity
    {
        public float Hp;

        protected LivingEntity(Texture2D sprite, Vector2 startPosition, float startHp, float speed, bool isAffectedByGravity = true, bool isAffectedByGravityDirection = false, Vector2? spriteSize = null) : base(sprite, startPosition, speed, isAffectedByGravity, isAffectedByGravityDirection, spriteSize)
        {
            Hp = startHp;
        }

        public virtual void Update(float g, int gDirection, GameTime gameTime, IEnumerable<Platform> platforms, LivingEntity target)
        {
            base.Update(g, gDirection, gameTime, platforms, target);
            Attack(target, gameTime);
        }

        protected virtual void Attack(LivingEntity target, GameTime? gameTime = null)
        {
            throw new NotImplementedException();
        }
    }
}