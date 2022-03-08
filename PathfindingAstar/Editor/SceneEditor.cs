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

            KeyboardInput.KeyRelease += KeyboardInput_KeyRelease;

            MouseInput.MouseMove += MouseInput_MouseMove;
            MouseInput.MouseDown += MouseInput_MouseDown;
            MouseInput.MouseUp += MouseInput_MouseUp;
        }

        private void KeyboardInput_KeyRelease(Keys key, KeyboardState keyState)
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

        private void MouseInput_MouseMove(Vector2 Position, Vector2 movement)
        {
            if (editMode == EditMode.Move && MouseInput.IsLeftButtonDown)
            {
                foreach (var actor in Actor.Selection)
                {
                    actor.Position += movement;
                }
            }
        }

        private void MouseInput_MouseDown(Vector2 position)
        {
            Actor actor = GetActorAt(position);
            if (actor != null)
            {
                if(!actor.IsSelected && !KeyboardInput.IsControlDown)
                {
                    Actor.Selection.Clear();
                }

                if (KeyboardInput.IsControlDown)
                {
                    actor.ToggleSelect();
                }
                else
                {
                    actor.Select();
                }
            }
            else if (!KeyboardInput.IsControlDown)
            {
                Actor.Selection.Clear();
            }
        }

        private void MouseInput_MouseUp(Vector2 position)
        {
            if (Actor.Selection.Count > 0 && !KeyboardInput.IsControlDown)
            {
                Actor.Selection.Clear();

                Actor actor = GetActorAt(position);
                if (actor != null)
                {
                    actor.Select();
                }
            }
        }

        private Actor GetActorAt(Vector2 position)
        {
            for (int i = Actor.Actors.Count - 1; i >= 0; i--)
            {
                Actor actor = Actor.Actors[i];
                if (Vector2.Distance(actor.Position, position) < actor.Radius)
                {
                    return actor;
                }
            }

            return null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            string text = string.Format("Mode: {0}", editMode);
            spriteBatch.DrawString(Style.FontLarge, "Q:Select mode, W:Move mode", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(Style.FontLarge, text, new Vector2(10, 40), Color.White);
        }
    }
}
