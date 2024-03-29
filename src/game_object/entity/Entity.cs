﻿#nullable enable
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
    public abstract class Entity : GameObject
    {
        public Vector2 Velocity;
        public bool IsAffectedByGravity;
        public bool IsAffectedByGravityDirection;
        public Vector2 Direction; // 1 | -1
        public float Speed;

        private Animator? _animator;

        protected Entity(Texture2D sprite,
            Vector2 startPosition,
            float speed,
            bool isAffectedByGravity = true,
            bool isAffectedByGravityDirection = false, Vector2? spriteSize = null, bool animator = false)
            : base(sprite, startPosition, spriteSize)
        {
            Speed = speed;
            IsAffectedByGravity = isAffectedByGravity;
            IsAffectedByGravityDirection = isAffectedByGravityDirection;
            Direction = new Vector2(1, 1);
            if (animator)
            {
                if (!spriteSize.HasValue)
                {
                    throw new ArgumentNullException("BTS");
                }
                else 
                {
                    _animator = new Animator(this, Sprite);
                }
            }
        }

        public virtual void Update(float g, int gDirection, GameTime gameTime, IEnumerable<Platform> platforms)
        {
            Direction.Y = IsAffectedByGravityDirection ? gDirection : 1;

            MoveAndCheckPlatforms(g, gDirection, gameTime, platforms);

            base.Update();
        }
        
        public virtual void Update(
            float g, int gDirection, 
            GameTime gameTime, 
            IEnumerable<Platform> platforms, 
            GameObject target)
        {
            Direction.Y = IsAffectedByGravityDirection ? gDirection : 1;

            MoveAndCheckPlatforms(g, gDirection, gameTime, platforms, target);

            base.Update();
        }
        
        private void MoveAndCheckPlatforms(
            float g, int gDirection, 
            GameTime gameTime, 
            IEnumerable<Platform> platforms, 
            GameObject? target = null)
        {
            if (target != null)
            {
                Velocity.X = Speed * (!(new Rectangle((int)Position.X, 0, Hitbox.Width, 1280)
                    .Intersects(target.Hitbox)) ?
                    target.Position.X < Position.X ?
                        -1
                        : 1
                    : 0);

                Direction.X = target.Position.X < Position.X ? -1 : 1;
            }
                

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
            if (Hitbox.Intersects(platform.Hitbox))
            {
                if (Position.X < platform.Hitbox.Center.X)
                {
                    Position.X = platform.Hitbox.X - Width;
                    Velocity.X = 0;
                }
                else if (Position.X > platform.Hitbox.Center.X)
                {
                    Position.X = platform.Hitbox.Right;
                    Velocity.X = 0;
                }
            }

        }
        
        protected virtual void CheckStandPlatformY(Platform platform)
        {
            if (Hitbox.Intersects(platform.Hitbox))
            {
                if (Position.Y < platform.Hitbox.Center.Y)
                {
                    Position.Y = platform.Hitbox.Y - Height;
                    Velocity.Y = 0;
                }
                else if (Position.Y > platform.Hitbox.Center.Y)
                {
                    Position.Y = platform.Hitbox.Bottom;
                    Velocity.Y = 0;
                }
            }
        }

        public override void Draw()
        {
            if (_animator == null)
                base.Draw();
            else 
                _animator.Draw();
        }
    }


    public abstract class LivingEntity : Entity
    {
        public float HP;

        protected LivingEntity(Texture2D sprite, Vector2 startPosition, float startHp, 
        float speed, bool isAffectedByGravity = true, bool isAffectedByGravityDirection = false, 
        Vector2? spriteSize = null, bool animator = false) 
        : base(sprite, startPosition, speed, isAffectedByGravity, isAffectedByGravityDirection, spriteSize, animator)
        {
            HP = startHp;
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