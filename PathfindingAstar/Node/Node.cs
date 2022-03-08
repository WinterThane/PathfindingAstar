using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using PathfindingAstar.Editor;

namespace PathfindingAstar
{
    public class Node : Actor
    {
        public List<Node> Connected = new List<Node>();

        public Node() : base(Style.NodeTexture, Style.NodeColor)
        {

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
            base.Draw(spriteBatch);

            foreach (var node in Connected)
            {
                Line.DrawLine(spriteBatch, Style.FillTexture, Style.LineColor, this, node, 0f);
            }
        }
    }
}
