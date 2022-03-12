using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PathfindingAstar
{
    public class Player : Actor
    {
        float playerSpeed;

        public Player(float speed) : base (Style.PlayerTexture, Color.White) 
        {
            playerSpeed = speed;
            BehaviorList.Add(new BehaviorMovement(0.5f));
        }

        public override void Update()
        {
            Speed = playerSpeed; 

            base.Update();
        }
    }
}
