using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PathfindingAstar
{
    public enum EnemyState
    {
        Wander,
        SeekPlayer,
        SeekHealth,
    }

    public class Enemy : Actor
    {
        public EnemyState State = EnemyState.Wander;
        public float SightRadius = 200;

        public Enemy() : base (Style.EnemyTexture, Color.White) { }

        public void ChangeState(EnemyState state)
        {
            State = state;
        }

        private void DrawCircle(SpriteBatch spriteBatch, float radius)
        {
            Color color = new Color(0, 0, 0, 100);
            Texture2D texture = Style.NodeTexture;
            int textureRadius = texture.Width / 2;

            float scale = radius / textureRadius;
            Vector2 origin = new Vector2(textureRadius, textureRadius);

            spriteBatch.Draw(texture, Position, null, color, 0f, origin, scale, SpriteEffects.None, 1f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            DrawCircle(spriteBatch, SightRadius);
        }
    }
}
