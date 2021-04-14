using System;
using GYARTE.main.gameComponents;
using System.Collections.Generic;
using GYARTE.manager;
using Microsoft.Xna.Framework.Input;

namespace GYARTE.menu
{
    public class MenuManager
    {
        private Menu[] _menus;
        int Selected = 0;

        private Menu _currentMenu => _menus[(int) GameComponents.GameState];
        public MenuManager()
        {
            _menus = new Menu[10];
        }

        public void AddMenu(Menu menu, int index)
        {
            _menus[index] = menu;
        }
        public void AddMenu(Menu menu, GameState index)
        {
            _menus[(int) index] = menu;
        }

        public void Update()
        {

            _currentMenu.Draw(Selected);
            InputManager.WhenPressedOnce(Keys.Down, () => 
            {
                if (Selected + 1 == _currentMenu.MenuItems.Count) 
                {
                    return;
                }   

                Selected++; 
            });
            
            InputManager.WhenPressedOnce(Keys.Up, () => 
            {
                if (Selected == 0)
                {
                    return;
                }

                Selected--;
            });

            InputManager.WhenPressedOnce(Keys.Enter, () =>
            {
                _currentMenu.MenuItems[Selected].Select();
            });
        }

        public void ResetSelected()
        {
            Selected = 0;
        }
    }
}

