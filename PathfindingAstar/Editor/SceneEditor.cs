using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PathfindingAstar.Editor
{
    enum EditMode
    {
        Select,
        Move,
    }

    public class SceneEditor
    {
        EditMode editMode;

        public SceneEditor()
        {
            //KeyboardInput.AddKey(Keys.Q);
            //KeyboardInput.AddKey(Keys.W);

            Keys[] allKeys = (Keys[])Enum.GetValues(typeof(Keys)); // listen to all possible keys
            foreach (var key in allKeys)
            {
                KeyboardInput.AddKey(key);
            }

            KeyboardInput.KeyPress += KeyboardInput_KeyPress;
        }

        private void KeyboardInput_KeyPress(Keys key, KeyboardState keyState)
        {
            if (key == Keys.Q)
            {
                editMode = EditMode.Select;
            }
            else if (key == Keys.W)
            {
                editMode = EditMode.Move;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            string text = string.Format("Mode: {0}", editMode);
            spriteBatch.DrawString(Style.FontLarge, text, new Vector2(10, 10), Color.White);
        }
    }
}
