using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PathfindingAstar
{
    public static class NodeBuilder
    {
        public static void BuildGrid(Vector2 position, int numX, int numY, int spacing)
        {
            List<Node> nodes = new List<Node>();
            int count = numX * numY;

            for (int i = 0; i < count; i++)
            {
                int x = i % numX;
                int y = i / numX;

                Node node = new Node();
                nodes.Add(node);
                node.Position = new Vector2(x * spacing, y * spacing) + position;

                if (x > 0)
                {
                    node.DualConnection(nodes[i - 1]); // left connection
                }

                if (y > 0)
                {
                    node.DualConnection(nodes[i - numX]); // up connection
                }

                if (x > 0 && y > 0)
                {
                    node.DualConnection(nodes[i - numX - 1]); // diagonal left up
                }

                if (x < numX - 1 && y > 0)
                {
                    node.DualConnection(nodes[i - numX + 1]); // diagonal right up
                }
            }
        }

        public static void BuildCircle(Vector2 position, float radius, int count)
        {
            List<Node> nodes = new List<Node>();

            for (int i = 0; i < count; i++)
            {
                Node node = new Node();
                nodes.Add(node);

                float rotation = ((float)i / count) * MathHelper.TwoPi;
                node.Position = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * radius + position;

                if (i > 0)
                {
                    node.DualConnection(nodes[i - 1]);
                }
            }

            nodes[0].DualConnection(nodes[nodes.Count - 1]);
        }
    }
}
