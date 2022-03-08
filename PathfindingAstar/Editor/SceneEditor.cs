using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PathfindingAstar
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

            NodeBuilder.BuildGrid(new Vector2(600, 250), 9, 7, 80);
            (Actor.Actors[40] as Node).DeleteActor();
            (Actor.Actors[31] as Node).DeleteActor();
            (Actor.Actors[22] as Node).DeleteActor();
            //NodeBuilder.BuildCircle(new Vector2(500, 500), 300, 20);

            //NodeBuilder.BuildGrid(new Vector2(320, 220), 3, 3, 80);
            //NodeBuilder.BuildCircle(new Vector2(400, 300), 200, 8);0
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
            else if (key == Keys.Delete)
            {
                foreach (var actor in Actor.Selection)
                {
                    actor.DeleteActor();
                }

                Actor.Selection.Clear();
            }
            else if (key == Keys.C && Actor.Selection.Count >= 2)
            {
                for (int i = 1; i < Actor.Selection.Count; i++)
                {
                    Node nodeA = Actor.Selection[i - 1] as Node;
                    Node nodeB = Actor.Selection[i] as Node;

                    if (nodeA != null && nodeB != null)
                    {
                        nodeA.DualConnection(nodeB);
                    }
                }
            }
            else if (key == Keys.X && Actor.Selection.Count >= 2)
            {
                for (int i = 1; i < Actor.Selection.Count; i++)
                {
                    Node nodeA = Actor.Selection[i - 1] as Node;
                    Node nodeB = Actor.Selection[i] as Node;

                    if (nodeA != null && nodeB != null)
                    {
                        nodeA.DualDisconnect(nodeB);
                    }
                }
            }
            else if (key == Keys.P && Actor.Selection.Count >= 2)
            {
                Node start = Actor.Selection[0] as Node;
                Node goal = Actor.LastSelected as Node;

                if (start != null && goal != null)
                {
                    AStarStep.Begin(start, goal);
                    AStarStep.Continue();
                }
            }
            else if (key == Keys.Space)
            {
                if (AStarStep.InProgress)
                {
                    AStarStep.Continue();
                }
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
            if (KeyboardInput.IsShiftDown)
            {
                Node node = new Node();
                node.Position = position;
                
                Node lastNode = Actor.LastSelected as Node;
                if (lastNode != null)
                {
                    node.DualConnection(lastNode);
                }
                Actor.Selection.Clear();
                node.Select();

                return;
            }

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
            spriteBatch.DrawString(Style.FontLarge, "Q:Select mode, W:Move mode, Shift+Click:Create node, C:Create connection,\nX:Disconnect, Delete:Remove node, P:Start path, Space:Continue path", new Vector2(10, 10), Color.Red);
            spriteBatch.DrawString(Style.FontLarge, text, new Vector2(10, 55), Color.White);
        }
    }
}
