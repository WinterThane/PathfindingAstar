using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PathfindingAstar.Editor
{
    public static class Line
    {
        public static void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Color color, Actor actorA, Actor actorB, float layer)
        {
            Vector2 pointA = actorA.Position;
            Vector2 pointB = actorB.Position;

            Vector2 direction = pointA - pointB;
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }

            pointA -= direction * actorA.Radius;
            pointB += direction * actorB.Radius;

            float length = (pointA - pointB).Length();

            float rotation = (float)Math.Atan2(direction.Y, direction.X);

            Rectangle rect = new Rectangle((int)(pointA.X + pointB.X) / 2,
                                           (int)(pointA.Y + pointB.Y) / 2, 
                                           (int)length, 
                                           2);
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            spriteBatch.Draw(texture, rect, null, color, rotation, origin, SpriteEffects.None, layer);
        }
    }
}
