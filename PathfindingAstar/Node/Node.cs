using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using PathfindingAstar.Editor;
using System;

namespace PathfindingAstar
{
    public class Node : Actor
    {
        public List<Node> Connected = new List<Node>();
        public bool DebugPaths = false;

        // A* state info
        public Node Parent;
        public bool Closed;
        public bool InPath;
        public float GScore;
        public float HScore;
        public float FScore;

        public Node() : base(Style.NodeTexture, Style.NodeColor) { }

        public void Reset()
        {
            Parent = null;
            Closed = false;
            InPath = false;
            GScore = 0;
            HScore = 0;
            FScore = 0;
        }

        public void ConnectTo(Node node)
        {
            if (!Connected.Contains(node))
            {
                Connected.Add(node);
            }
        }

        public void DisconnectFrom(Node node)
        {
            Connected.Remove(node);
        }

        public void DualConnection(Node node)
        {
            ConnectTo(node);
            node.ConnectTo(this);
        }

        public void DualDisconnect(Node node)
        {
            DisconnectFrom(node);
            node.DisconnectFrom(this);
        }

        public override void DeleteActor()
        {
            foreach (var node in Actors.OfType<Node>())
            {
                node.DisconnectFrom(this);
            }

            base.DeleteActor();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!DebugPaths)
            {
                return;
            }

            base.Draw(spriteBatch);

            // draw lines
            foreach (var node in Connected)
            {
                Color lineColor = Style.LineColor;
                float lineLayer = Style.LineLayer;

                if (InPath && node.InPath && node == Parent)
                {
                    lineColor = Style.PathColor;
                    lineLayer = Style.MarkerLayer;
                }

                Line.DrawLine(spriteBatch, Style.FillTexture, lineColor, this, node, lineLayer);
            }

            // draw parents
            Node parent = Parent;
            if (parent != null && !InPath)
            {
                Vector2 direction = parent.Position - Position;
                float rotation = (float)Math.Atan2(direction.Y, direction.X);

                spriteBatch.Draw(Style.TailTexture, Position, null, Color.Red, rotation, Style.TailOrigin, 1f, SpriteEffects.None, Style.ParentLayer);
            }

            // draw markers
            if (Closed || InPath)
            {
                Color markerColor = Style.ClosedColor;
                if (InPath)
                {
                    markerColor = Style.PathColor;
                }

                spriteBatch.Draw(Style.MarkerTexture, Position, null, markerColor, 0f, Style.MarkerOrigin, 1f, SpriteEffects.None, Style.MarkerLayer);
            }

            // draw text
            if (Parent != null)
            {
                string text;
                Vector2 textDimensions;
                Vector2 textPosition;

                text = string.Format("{0:0}", GScore);
                textDimensions = Style.FontSmall.MeasureString(text);
                textPosition = Position + new Vector2(-Origin.X - textDimensions.X, Origin.Y);
                spriteBatch.DrawString(Style.FontSmall, text, textPosition, Style.DarkTextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, Style.TextLayer);

                text = string.Format("{0:0}", HScore);
                textDimensions = Style.FontSmall.MeasureString(text);
                textPosition = Position + new Vector2(Origin.X, Origin.Y);
                spriteBatch.DrawString(Style.FontSmall, text, textPosition, Style.DarkTextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, Style.TextLayer);

                text = string.Format("{0:0}", FScore);
                textDimensions = Style.FontSmall.MeasureString(text);
                textPosition = Position + new Vector2(-Origin.X - textDimensions.X, -Origin.Y - textDimensions.Y);
                spriteBatch.DrawString(Style.FontSmall, text, textPosition, Style.BrightTextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, Style.TextLayer);
            }
        }

        public static Node GetClosestNode(Vector2 position)
        {
            Node result = null;
            float shortestDistance = float.PositiveInfinity;

            foreach (var node in Actors.OfType<Node>())
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

        public static Node GetRandomNode(Random random)
        {
            IEnumerable<Node> nodes = Actors.OfType<Node>();
            return nodes.ElementAt(random.Next(nodes.Count()));
        }
    }
}
