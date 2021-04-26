using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GYARTE.gameObjects.entity;
using GYARTE.gameObjects.entity.enemy;

namespace GYARTE.manager
{
    public static class CullingManager
    { 
        public static void HpCulling(List<LivingEntity> entities)
        {
            foreach (var i in entities.ToList())
                if (i.HP <= 0)
                    entities.Remove(i); 
        }
        
        public static void HpCulling(LivingEntity entity) //TODO: -_-
        {
            if (entity.HP <= 0)
                if (entity is Player) 
                    Environment.Exit(0);
        }

        public static void ItemCulling(List<StaticItem> items)
        {
            foreach (var i in items.ToList())
            {
                if (i.IsConsumable && i.IsConsumed)
                {
                    items.Remove(i);
                }
            }
        }
        
        public static void ItemCulling(List<DynamicItem> items)
        {
            foreach (var i in items.ToList())
                if (i.IsConsumable && i.IsConsumed)
                    items.Remove(i);
        }
        
        public static void HpCulling(List<Enemy> entities)
        {
            foreach (var i in entities.ToList())
                if (i.HP <= 0)
                    entities.Remove(i);
        }
    }
}