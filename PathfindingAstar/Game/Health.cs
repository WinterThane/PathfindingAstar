using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PathfindingAstar
{
    public class Health : Actor
    {
        public bool Active;

        public Health() : base (Style.HealthTexture, Color.White) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                base.Draw(spriteBatch);
            }            
        }
    }
}
