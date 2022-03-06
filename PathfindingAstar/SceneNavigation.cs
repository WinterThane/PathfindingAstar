using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PathfindingAstar
{
    public class SceneNavigation : Scene
    {
        public override void BuildScene(GraphicsDevice graphicsDevice, ContentManager contant)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (Actor actor in Actor.Actors)
            {
                actor.Update();
            }
        }

        public override void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            graphicsDevice.Clear(new Color(0, 0, 0));

            spriteBatch.Begin();

            foreach (var actor in Actor.Actors)
            {
                actor.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
