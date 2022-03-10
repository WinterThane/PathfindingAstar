using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace PathfindingAstar
{
    public class AStarStep
    {
        private static Node start;
        private static Node goal;
        private static List<Node> openList = new List<Node>();
        private static List<Node> path;
        public static bool InProgress = false;
        private static Comparison<Node> FScoreComparison = new Comparison<Node>(CompareNodesByFScore);

        private static int CompareNodesByFScore(Node x, Node y)
        {
            if (x.FScore > y.FScore)
            {
                return 1;
            }
            if (x.FScore < y.FScore)
            {
                return -1;
            }

            return 0;
        }

        private static float GetHScore(Vector2 vectorA, Vector2 vectorB)
        {
            return Vector2.Distance(vectorA, vectorB);
        }

        public static void Begin(Node start, Node goal)
        {
            foreach (var node in Actor.Actors.OfType<Node>())
            {
                node.Reset();
            }

            AStarStep.start = start;
            AStarStep.goal = goal;

            openList = new List<Node>();
            openList.Add(start);

            InProgress = true;
        }

        public static void Continue()
        {
            if (!InProgress)
            {
                return;
            }

            if (openList.Count == 0)
            {
                return;
            }

            openList.Sort(FScoreComparison);
            Node current = openList[0];

            openList.Remove(current);
            current.Closed = true;

            if (current == goal)
            {
                InProgress = false;
                path = BuildPath(goal);
                return;
            }

            foreach (var neighbor in current.Connected)
            {
                if (neighbor.Closed)
                {
                    continue;
                }

                float neighborGScore = current.GScore + Vector2.Distance(current.Position, neighbor.Position);

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                    neighbor.Parent = current;
                    neighbor.GScore = neighborGScore;
                    neighbor.HScore = GetHScore(neighbor.Position, goal.Position);
                    neighbor.FScore = neighbor.GScore + neighbor.HScore;
                }
                else if (neighborGScore < neighbor.GScore)
                {
                    neighbor.Parent = current;
                    neighbor.GScore = neighborGScore;
                    neighbor.FScore = neighbor.GScore + neighbor.HScore;
                }
            }
        }

        private static List<Node> BuildPath(Node goal)
        {
            List<Node> path = new List<Node>();
            Node node = goal;

            while (node != null)
            {
                path.Add(node);
                node.InPath = true;
                node = node.Parent;
            }

            path.Reverse();

            return path;
        }
    }
}
