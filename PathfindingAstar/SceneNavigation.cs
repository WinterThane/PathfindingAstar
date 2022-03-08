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

            for (int i = 0; i < 3; i++)
            {
                Actor actor = new Actor(Style.ArrowTexture, Color.White);
                actor.Position = new Vector2(i * 100 + 500, 300);
            }

            //Actor.Actors[0].Select();
            //Actor.Actors[2].Select();
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
