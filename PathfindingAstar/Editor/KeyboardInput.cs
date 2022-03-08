using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace PathfindingAstar
{
    public delegate void KeyHandler(Keys key, KeyboardState keyState);

    public static class KeyboardInput
    {
        private static List<Keys> boundKeys = new List<Keys>();
        private static KeyboardState lastKeyState;
        public static event KeyHandler KeyPress = delegate(Keys key, KeyboardState keyState) { };
        public static event KeyHandler KeyRelease = delegate(Keys key, KeyboardState keyState) { };

        public static bool IsControlDown { get { return lastKeyState.IsKeyDown(Keys.LeftControl) || lastKeyState.IsKeyDown(Keys.RightControl); } }
        public static bool IsShiftDown { get { return lastKeyState.IsKeyDown(Keys.LeftShift) || lastKeyState.IsKeyDown(Keys.RightShift); } }
        public static bool IsAltDown { get { return lastKeyState.IsKeyDown(Keys.LeftAlt) || lastKeyState.IsKeyDown(Keys.LeftAlt); } }

        public static void AddKey(Keys key)
        {
            boundKeys.Add(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return lastKeyState.IsKeyDown(key);
        }

        public static void Update()
        {
            KeyboardState keyState = Keyboard.GetState();

            for (int i = 0; i < boundKeys.Count; i++)
            {
                if (keyState.IsKeyDown(boundKeys[i]) && lastKeyState.IsKeyUp(boundKeys[i]))
                {
                    KeyPress(boundKeys[i], keyState);
                }

                if (keyState.IsKeyUp(boundKeys[i]) && lastKeyState.IsKeyDown(boundKeys[i]))
                {
                    KeyRelease(boundKeys[i], keyState);
                }
            }

            lastKeyState = keyState;
        }
    }
}
