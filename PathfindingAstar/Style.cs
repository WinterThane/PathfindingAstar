using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PathfindingAstar
{
    public static class Style
    {
        public static Color NodeColor = new Color(200, 200, 200);
        public static Color SelectionColor = new Color(144, 238, 144);
        public static Color LineColor = new Color(8, 8, 8);
        public static Color ClosedColor = new Color(255, 200, 160);
        public static Color PathColor = new Color(180, 180, 255);

        public static Color BrightTextColor = new Color(255, 255, 255);
        public static Color DarkTextColor = new Color(150, 150, 150);

        public static float LineLayer = 0.1f;
        public static float ParentLayer = 0.1f;
        public static float ActorLayer = 0.9f;
        public static float MarkerLayer = 0.1f;
        public static float TextLayer = 1f;

        public static SpriteFont FontLarge;
        public static SpriteFont FontNormal;
        public static SpriteFont FontSmall;

        public static Texture2D FillTexture;
        public static Texture2D ArrowTexture;
        public static Texture2D NodeTexture;
        public static Texture2D HealthTexture;
        public static Texture2D BackgroundTexture;
        public static Texture2D EnemyTexture;
        public static Texture2D PlayerTexture;

        public static Texture2D MarkerTexture;
        public static Vector2 MarkerOrigin;

        public static Texture2D TailTexture;
        public static Vector2 TailOrigin;

        public static void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            FontLarge = contentManager.Load<SpriteFont>("Fonts/fontLarge");
            FontNormal = contentManager.Load<SpriteFont>("Fonts/fontNormal");
            FontSmall = contentManager.Load<SpriteFont>("Fonts/fontSmall");

            FillTexture = new Texture2D(graphicsDevice, 1, 1);
            FillTexture.SetData(new Color[] { Color.White });

            ArrowTexture = contentManager.Load<Texture2D>("arrow");
            NodeTexture = contentManager.Load<Texture2D>("node");
            HealthTexture = contentManager.Load<Texture2D>("health");
            BackgroundTexture = contentManager.Load<Texture2D>("background");
            EnemyTexture = contentManager.Load<Texture2D>("enemy");
            PlayerTexture = contentManager.Load<Texture2D>("player");

            MarkerTexture = contentManager.Load<Texture2D>("circle");
            MarkerOrigin = new Vector2(MarkerTexture.Width / 2, MarkerTexture.Height / 2);

            TailTexture = contentManager.Load<Texture2D>("line");
            TailOrigin = new Vector2(0, TailTexture.Height / 2);
        }
    }
}
