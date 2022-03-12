using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PathfindingAstar
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        readonly int ScreenWidth = 1920;
        readonly int ScreenHeight = 1080;

        private Scene scene;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = ScreenWidth,
                PreferredBackBufferHeight = ScreenHeight
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Style.LoadContent(GraphicsDevice, Content);

            //scene = new SceneBehaviorTest();
            //scene.BuildScene(GraphicsDevice, Content);

            // editor
            //scene = new SceneNavigation();
            //scene.BuildScene(GraphicsDevice, Content);

            scene = new SceneGame();
            scene.BuildScene(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            scene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            scene.Draw(GraphicsDevice, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}
