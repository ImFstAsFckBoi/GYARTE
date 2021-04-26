using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GYARTE.misc;

namespace GYARTE.manager
{
    public enum Buttons
    {
        LeftButton,
        MiddleButton,
        RightBUtton
    }
    public interface IKeyboard
    {
        void WhenPressed(Keys key, Execution execution, Execution executionIfNotPressed = null);

        void WhenPressedOnce(Keys key, Execution execution);
    }

    public interface IMouse
    {
        Point Position { get; }
        void WhenPressed(Buttons button, Execution execution, Execution executionIfNotPressed = null);

        void WhenPressedOnce(Buttons button, Execution execution);
    }

    public class InputManager : IKeyboard, IMouse
    {
        public IKeyboard KeyboardIO => this;
        public IMouse MouseIO => this;

        private KeyboardState _keyboardState = Keyboard.GetState();
        private MouseState _mouseState = Mouse.GetState();

        private static Dictionary<Keys, bool> _K_hasBeenPressed = new Dictionary<Keys, bool>();
        private static Dictionary<Buttons, bool> _M_hasBeenPressed = new Dictionary<Buttons, bool>();
        
        public InputManager()
        {

        }

        void IKeyboard.WhenPressed(Keys key, Execution execution, Execution executionIfNotPressed)
        {
            if (_keyboardState.IsKeyDown(key))
                execution.Invoke();
            else if(executionIfNotPressed != null)
                executionIfNotPressed.Invoke();
        }   
        
        void IKeyboard.WhenPressedOnce(Keys key, Execution execution)
        {
            _K_hasBeenPressed[key] = IfPressedOnce(key, execution);
        }

        void IMouse.WhenPressed(Buttons button, Execution execution, Execution executionIfNotPressed)
        {
            bool btnPrsed = false;
            switch(button)
            {
                case Buttons.LeftButton:
                    btnPrsed = _mouseState.LeftButton == ButtonState.Pressed;
                    break;
                case Buttons.MiddleButton:
                    btnPrsed = _mouseState.MiddleButton == ButtonState.Pressed;
                    break;
                case Buttons.RightBUtton:
                    btnPrsed = _mouseState.RightButton == ButtonState.Pressed;
                    break;
            }

            if(btnPrsed)
                execution.Invoke();
            else if(executionIfNotPressed != null)
                executionIfNotPressed.Invoke();
        }
        
        void IMouse.WhenPressedOnce(Buttons button, Execution execution)
        {
            _M_hasBeenPressed[button] = IfPressedOnce(button, execution);
        }

        Point IMouse.Position => _mouseState.Position;
        public void Update()
        {
            _keyboardState = Keyboard.GetState();
            _mouseState = Mouse.GetState();
        }

        private bool IfPressedOnce(Keys key, Execution execution)
        {
            if (_keyboardState.IsKeyDown(key) && !_K_hasBeenPressed[key])
            {
                execution.Invoke();
                
                return true;
            }
            
            return _keyboardState.IsKeyDown(key) && _K_hasBeenPressed[key];
        }

        private bool IfPressedOnce(Buttons button, Execution execution)
        {
            bool btnPrsed = false;
            switch(button)
            {
                case Buttons.LeftButton:
                    btnPrsed = _mouseState.LeftButton == ButtonState.Pressed;
                    break;
                case Buttons.MiddleButton:
                    btnPrsed = _mouseState.MiddleButton == ButtonState.Pressed;
                    break;
                case Buttons.RightBUtton:
                    btnPrsed = _mouseState.RightButton == ButtonState.Pressed;
                    break;
            }

            if (btnPrsed && !_M_hasBeenPressed[button])
            {
                execution.Invoke();
                
                return true;
            }
            
            return btnPrsed && _M_hasBeenPressed[button];
        }
    }
}