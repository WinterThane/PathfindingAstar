using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PathfindingAstar
{
    public static class NodeIO
    {
        private static OpenFileDialog dialogOpen = new OpenFileDialog();
        private static SaveFileDialog dialogSave = new SaveFileDialog();

        public static void ShowSaveDialog()
        {
            if (dialogSave.ShowDialog() == DialogResult.OK)
            {
                SaveNodes(dialogSave.FileName);
            }
        }

        public static void ShowLoadDialog()
        {
            if (dialogOpen.ShowDialog() == DialogResult.OK)
            {
                for (int i = Actor.Actors.Count - 1; i >= 0; i--)
                {
                    if (Actor.Actors[i] is Node)
                    {
                        Actor.Actors[i].DeleteActor();
                    }
                }

                LoadNodes(dialogOpen.FileName);
            }
        }

        public static void SaveNodes(string fileName)
        {
            using StreamWriter writer = new StreamWriter(fileName);
            Dictionary<Node, int> nodeIdMap = new Dictionary<Node, int>();

            IEnumerable<Node> nodes = Actor.Actors.OfType<Node>();
            foreach (var node in nodes)
            {
                int nodeId = nodeIdMap.Count + 1;
                nodeIdMap[node] = nodeId;
                writer.WriteLine("n,{0},{1},{2}", nodeId, node.Position.X, node.Position.Y);
            }

            foreach (var node in nodes)
            {
                foreach (var neighbor in node.Connected)
                {
                    writer.WriteLine("c,{0},{1}", nodeIdMap[node], nodeIdMap[neighbor]);
                }
            }
        }

        public static void LoadNodes(string fileName)
        {
            using StreamReader reader = new StreamReader(fileName);
            Dictionary<int, Node> nodeRefMap = new Dictionary<int, Node>();

            while (!reader.EndOfStream)
            {
                string[] fields = reader.ReadLine().Split(',');

                if (fields[0] == "n")
                {
                    int nodeId = Convert.ToInt32(fields[1]);
                    float x = Convert.ToSingle(fields[2]);
                    float y = Convert.ToSingle(fields[3]);

                    Node node = new Node();
                    node.Position = new Vector2(x, y);
                    nodeRefMap[nodeId] = node;
                }
                else if (fields[0] == "c")
                {
                    int nodeId = Convert.ToInt32(fields[1]);
                    int neighborId = Convert.ToInt32(fields[2]);

                    Node node = nodeRefMap[nodeId];
                    Node neighbor = nodeRefMap[neighborId];

                    node.ConnectTo(neighbor);
                }
            }
        }
    }
}
