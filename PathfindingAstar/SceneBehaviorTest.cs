using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PathfindingAstar
{
    public class SceneBehaviorTest : Scene
    {
        private readonly int screenWidth = 1920;
        private readonly int screenHeight = 1080;

        public override void BuildScene(GraphicsDevice graphicDevice, ContentManager content)
        {
            Texture2D arrowTexture = content.Load<Texture2D>("arrow2");

            List<Actor> obsticles = new List<Actor>();
            Color obsticleColor = new Color(255, 200, 96);

            //for (int i = 0; i < 4; i++)
            //{
            //    obsticles.Add(new Actor(arrowTexture, obsticleColor));
            //    obsticles[i].Position = new Vector2(500 * i + 200, 500);
            //}

            int hCount = screenHeight / 150;
            int vCount = screenWidth / 300;

            for (int i = 0; i < hCount; i++) // top
            {
                Actor obsticle = new Actor(arrowTexture, obsticleColor)
                {
                    Position = new Vector2((float)i / hCount * screenWidth + 100, 0),
                    Direction = new Vector2(0, 1)
                };
                obsticles.Add(obsticle);
            }

            for (int i = 0; i < hCount; i++) // bottom
            {
                Actor obsticle = new Actor(arrowTexture, obsticleColor)
                {
                    Position = new Vector2((float)i / hCount * screenWidth + 100, screenHeight),
                    Direction = new Vector2(0, -1)
                };
                obsticles.Add(obsticle);
            }

            for (int i = 0; i < vCount; i++) // right
            {
                Actor obsticle = new Actor(arrowTexture, obsticleColor)
                {
                    Position = new Vector2(screenWidth, (float)i / vCount * screenHeight + 100),
                    Direction = new Vector2(-1, 0)
                };
                obsticles.Add(obsticle);
            }

            for (int i = 0; i < vCount; i++) // left
            {
                Actor obsticle = new Actor(arrowTexture, obsticleColor)
                {
                    Position = new Vector2(0, (float)i / vCount * screenHeight + 100),
                    Direction = new Vector2(1, 0)
                };
                obsticles.Add(obsticle);
            }

            Actor leader = new Actor(arrowTexture, new Color(64, 255, 64))
            {
                Speed = 4,
                Direction = Actor.GetRandomDirection(),
                Position = Actor.GetRandomPosition(screenWidth, screenHeight)
            };
            //leader.BehaviorList.Add(new BehaviorConstant(0.1f, new Vector2(1, 0)));
            leader.BehaviorList.Add(new BehaviorMovement(0.5f));
            leader.BehaviorList.Add(new BehaviorWander(0.1f, 60));

            Actor enemy = new Actor(arrowTexture, new Color(255, 120, 120))
            {
                Speed = 2,
                Position = new Vector2(200, 150)
            };
            enemy.BehaviorList.Add(new BehaviorWander(0.1f, 30));
            enemy.BehaviorList.Add(new BehaviorSeek(0.1f, leader));

            Behavior seek = new BehaviorSeek(0.05f, leader);
            Behavior avoidEnemy = new BehaviorAvoid(0.1f, enemy, 150);

            for (int i = 0; i < 10; i++)
            {
                Actor drone = new Actor(arrowTexture, Color.White)
                {
                    Speed = 3,
                    Direction = Actor.GetRandomDirection(),
                    Position = Actor.GetRandomPosition(screenWidth, screenHeight)
                };
                drone.BehaviorList.Add(seek);
                drone.BehaviorList.Add(new BehaviorWander(0.03f, 15));
                drone.BehaviorList.Add(avoidEnemy);
                foreach (var obsticle in obsticles)
                {
                    drone.BehaviorList.Add(new BehaviorAvoid(0.2f, obsticle, 200));
                }
            }

            foreach (var obsticle in obsticles)
            {
                leader.BehaviorList.Add(new BehaviorAvoid(0.2f, obsticle, 150));
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var actor in Actor.Actors)
            {
                actor.Update();

                if (actor.Position.X > screenWidth)
                {
                    actor.Position.X = 0;
                }
                else if (actor.Position.X < 0)
                {
                    actor.Position.X = screenWidth;
                }

                if (actor.Position.Y > screenHeight)
                {
                    actor.Position.Y = 0;
                }
                else if (actor.Position.Y < 0)
                {
                    actor.Position.Y = screenHeight;
                }
            }
        }

        public override void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            graphicsDevice.Clear(new Color(96, 96, 111));

            spriteBatch.Begin();

            foreach (var actor in Actor.Actors)
            {
                actor.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
