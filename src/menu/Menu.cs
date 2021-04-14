using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GYARTE.misc;
using GYARTE.main.gameComponents;
using GYARTE.manager;
using System.Linq;
namespace GYARTE.menu
{
    public class Menu
    {
        public List<MenuItem> MenuItems;

        public string Name;

        public Menu(string name, List<MenuItem> items)
        {
            Name = name;
            MenuItems = new List<MenuItem>(items);
        }
        
        public void Draw(int sel)
        {
            GameComponents.DrawManager.NewDrawCall.Text(GameComponents.FontTable["Arial16Bold"], Name, new Vector2(10, 50),priority: -10, alignment: TextDrawCall.Alignment.TopLeft);
            foreach(MenuItem m in MenuItems.Where((x) => x != null))
            {
                GameComponents.DrawManager.NewDrawCall.Text(
                    GameComponents.FontTable["Arial16Bold"], 
                    m.Name, new Vector2(10, 70 + MenuItems.IndexOf(m) * 20), 
                    color: MenuItems.IndexOf(m) == sel? Color.Red : Color.White,
                    priority: -10, 
                    alignment: TextDrawCall.Alignment.TopLeft);
            }
        }
    }

    public class MenuItem
    {
        public Execution OnClick;

        public int Place;
        public string Name;

        public MenuItem(string name, Execution onClick, int place = 0)
        {
            Name = name;
            OnClick = onClick;
            Place = place;
        }

        public void Select()
        {
            GameComponents.MenuManager.ResetSelected();
            OnClick.Invoke();
        }
    }
}
