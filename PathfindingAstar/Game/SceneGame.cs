using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace PathfindingAstar
{
    public class SceneGame : Scene
    {
        float playerSpeed = 3f;

        Player player;
        Enemy enemy;
        Health health;

        BehaviorNavigation enemyNavigation;
        Random random = new Random();

        public override void BuildScene(GraphicsDevice graphicsDevice, ContentManager contant)
        {
            NodeIO.LoadNodes("nodes.txt");

            player = new Player(playerSpeed);
            player.Position = new Vector2(910, 540);

            enemyNavigation = new BehaviorNavigation(0.2f);
            enemyNavigation.NodeReached += EnemyNavigation_NodeReached;
            enemyNavigation.GoalReached += EnemyNavigation_GoalReached;

            enemy = new Enemy
            {
                Position = new Vector2(100, 500),
                Speed = 2f
            };
            enemy.BehaviorList.Add(enemyNavigation);

            NavigateToActor(Node.GetRandomNode(random));

            health = new Health();
            health.Position = new Vector2(150, 735);
        }

        private void EnemyNavigation_NodeReached(object sender, EventArgs e)
        {
            if (enemy.State == EnemyState.SeekPlayer)
            {
                NavigateToActor(player);
            }
        }

        private void EnemyNavigation_GoalReached(object sender, EventArgs e)
        {
            if (enemy.State == EnemyState.Wander)
            {
                NavigateToActor(Node.GetRandomNode(random));
            }
            else if (enemy.State == EnemyState.SeekPlayer)
            {
                NavigateToActor(player);
            }
            else if (enemy.State == EnemyState.SeekHealth)
            {
                NavigateToActor(health);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var actor in Actor.Actors)
            {
                actor.Update();
            }

            if (Vector2.Distance(player.Position, enemy.Position) < player.Radius + enemy.Radius)
            {
                enemy.ChangeState(EnemyState.SeekHealth);
                health.Active = true;
            }
            else if (Vector2.Distance(player.Position, enemy.Position) < player.Radius + enemy.SightRadius)
            {
                if (enemy.State != EnemyState.SeekHealth)
                {
                    enemy.ChangeState(EnemyState.SeekPlayer);
                }
            }

            if (Vector2.Distance(health.Position, enemy.Position) < health.Radius + enemy.Radius)
            {
                if (enemy.State == EnemyState.SeekHealth)
                {
                    health.Active = false;
                    enemy.ChangeState(EnemyState.Wander);
                }
            }
        }

        public override void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            graphicsDevice.Clear(new Color(0, 0, 0));

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            spriteBatch.Draw(Style.BackgroundTexture, Vector2.Zero, Color.White);

            string text = string.Format("Enemy state: {0}", enemy.State);
            spriteBatch.DrawString(Style.FontLarge, text, new Vector2(10, 20), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            foreach (var actor in Actor.Actors)
            {
                actor.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        private void NavigateToActor(Actor actor)
        {
            Node start = Node.GetClosestNode(enemy.Position);
            Node goal = Node.GetClosestNode(actor.Position);

            if (start != null & goal != null)
            {
                List<Node> path = AStar.FindPath(start, goal);
                if (path != null)
                {
                    if (path.Count > 1)
                    {
                        path[0].InPath = false;
                        path.RemoveAt(0);
                    }

                    enemyNavigation.BeginNavigation(path);
                }
            }
        }
    }
}
