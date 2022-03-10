using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;

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

        Actor bot;
        BehaviorNavigation nav;

        Random random = new Random();

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
            //NodeBuilder.BuildCircle(new Vector2(400, 300), 200, 8);

            nav = new BehaviorNavigation(0.2f);
            nav.GoalReached += Nav_GoalReached;

            bot = new Actor(Style.ArrowTexture, Color.LightGreen);
            bot.Position = new Vector2(925, 490);
            bot.BehaviorList.Add(nav);
        }

        private void Nav_GoalReached(object sender, EventArgs e)
        {
            //bot.Speed = 0f;
            Node start = GetClosestNode(bot.Position);
            Node goal = GetRandomNode();
            NavigateToNode(start, goal);
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
            else if (key == Keys.Enter)
            {
                Node start = GetClosestNode(bot.Position);
                Node goal = Actor.LastSelected as Node;
                NavigateToNode(start, goal);
            }
        }

        private void MouseInput_MouseMove(Vector2 position, Vector2 movement)
        {
            if (KeyboardInput.IsKeyDown(Keys.Z) && Actor.Selection.Count > 0)
            {
                Node start = Actor.Selection[0] as Node;
                Node goal = GetClosestNode(position);

                if (start != null && goal != null)
                {
                    AStar.FindPath(start, goal);
                }
            }

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

        private Node GetClosestNode(Vector2 position)
        {
            Node result = null;
            float shortestDistance = float.PositiveInfinity;

            foreach (var node in Actor.Actors.OfType<Node>())
            {
                float distance = Vector2.Distance(node.Position, position);
                if (distance < shortestDistance)
                {
                    result = node;
                    shortestDistance = distance;
                }
            }

            return result;
        }

        private void NavigateToNode(Node start, Node goal)
        {
            if (start != null && goal != null)
            {
                List<Node> path = AStar.FindPath(start, goal);

                if (path != null)
                {
                    nav.BeginNavigation(path);
                    bot.Speed = 2f;
                }
            }
        }

        private Node GetRandomNode()
        {
            IEnumerable<Node> nodes = Actor.Actors.OfType<Node>();
            return nodes.ElementAt(random.Next(nodes.Count()));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            string text = string.Format("Mode: {0}", editMode);
            spriteBatch.DrawString(Style.FontLarge, text, new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(Style.FontLarge, "Q:Select mode,\n" +
                "W:Move mode,\n" +
                "Shift+Click:Create node,\n" +
                "Ctrl+Click:Multi-select,\n" +
                "C:Create connection,\n" +
                "X:Disconnect,\n" +
                "Delete:Remove node,\n" +
                "P:Start path,\n" +
                "Space:Continue path,\n" +
                "Z:Hold for A*,\n" +
                "Enter:Start navigation", new Vector2(10, 35), Color.Red);
            
        }
    }
}
