#nullable enable
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.gameObjects.platform;
using GYARTE.main.gameComponents;
using GYARTE.manager;

namespace GYARTE.gameObjects.entity
{
    public delegate void ItemExecution(LivingEntity target);

    public interface IItem
    {
        public bool IsConsumable { get; }
        public bool IsConsumed { get; set; }
        public ItemExecution? TouchAction { get; }
        
    }
    

    public class StaticItem : GameObject, IItem
    {
        public bool IsConsumable { get; } 
        public bool IsConsumed { get; set; }
        public ItemExecution? TouchAction { get; }

        public StaticItem(Texture2D sprite, Vector2 startPosition, bool isConsumeable, ItemExecution? touchAction = null, Vector2? spriteSize = null) : base(sprite, startPosition, spriteSize)
        {
            IsConsumable = isConsumeable;
            IsConsumed = false;
            TouchAction = touchAction;
        }

        public void Update(LivingEntity target)
        {
            if (Rect.Intersects(target.Rect) && IsConsumable && !IsConsumed)
            {
                IsConsumed = true;
                TouchAction?.Invoke(target);
            }
            
            
            base.Update();
        }
        
        public override void Draw()
        {
            GameComponents.DrawManager.NewDrawCall.Sprite(Position, Sprite);
        }
    }

    public class DynamicItem : Entity, IItem
    {
        public bool IsConsumable { get; }
        public bool IsConsumed { get; set; }
        public ItemExecution? TouchAction { get; }
        

        public DynamicItem(Texture2D sprite, Vector2 startPosition, bool isConsumable, ItemExecution? touchAction = null) : base(sprite, startPosition, 0)
        {
            IsConsumable = isConsumable;
            IsConsumed = false;
            TouchAction = touchAction;
        }
        
        public void Update(float g, int gDirection, GameTime gameTime, IEnumerable<Platform> platforms, LivingEntity target)
        {

            if (Rect.Intersects(target.Rect) && IsConsumable && !IsConsumed)
            {
                IsConsumed = true;
                TouchAction?.Invoke(target);
            }

            base.Update(g, gDirection, gameTime, platforms);
        }
    }
}