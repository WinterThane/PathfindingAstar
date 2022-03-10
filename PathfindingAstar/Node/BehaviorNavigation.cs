using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PathfindingAstar
{
    public class BehaviorNavigation : Behavior
    {
        int nodeIndex = 0;
        List<Node> path;

        public event EventHandler GoalReached = delegate (object sender, EventArgs e) { };

        public BehaviorNavigation(float weight) : base(weight) { }
        
        public void BeginNavigation(List<Node> path)
        {
            nodeIndex = 0;
            this.path = path;
        }

        public override void Update(Actor actor)
        {
            if (path != null && nodeIndex < path.Count)
            {
                Node nextNode = path[nodeIndex];
                if (Vector2.Distance(actor.Position, nextNode.Position) < nextNode.Radius)
                {
                    nodeIndex++;

                    if (nodeIndex == path.Count)
                    {
                        GoalReached(this, EventArgs.Empty);
                    }

                    return;
                }

                Vector2 targetDirection = nextNode.Position - actor.Position;
                if (targetDirection != Vector2.Zero)
                {
                    targetDirection.Normalize();
                }

                actor.Direction += targetDirection * Weight;
            }
        }
    }
}
