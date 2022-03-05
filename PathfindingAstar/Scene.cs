using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PathfindingAstar
{
    public class Scene
    {
        public virtual void BuildScene(GraphicsDevice graphicsDevice, ContentManager contant) { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch) { }
    }
}
