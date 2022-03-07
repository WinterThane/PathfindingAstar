using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using PathfindingAstar.Editor;

namespace PathfindingAstar
{
    public class SceneNavigation : Scene
    {
        SceneEditor sceneEditor;

        public override void BuildScene(GraphicsDevice graphicsDevice, ContentManager contant)
        {
            sceneEditor = new SceneEditor();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardInput.Update();
            MouseInput.Update();

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

            sceneEditor.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
