using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PONG
{
    public class CampaignGameScreen : PongGameScreen
    {
        int errorAccumulate = 0;

        public override void UpdateTopPaddle(float elapsed)
        {
            HandleAI();
            base.UpdateTopPaddle(elapsed);
        }

        #region AI
        /// <summary>
        /// Handles movement of the top paddle in single player
        /// </summary>
        /// <param name="input">The state of the gamepads</param>       
        public void HandleAI()
        {
            int MAX_SPEED = 8;
            Random error = new Random();
            int errorNum = error.Next(-1, 1);

            float CenterOfPaddle = topPaddle.Position.X + (topPaddle.Texture.Width / 2);
            float CenterOfBall = balls[0].Position.X + (balls[0].Texture.Width / 2) + errorAccumulate;

            //I want the paddle to pursue the ball once it's gone past
            //the screen's halfway point, before that the paddle will
            //just move to the center
            if (balls[0].Position.Y < 600) //arbitrary, what's half the screen?
            {
                if (CenterOfPaddle > CenterOfBall) //is the ball on the right?
                {
                    //TODO : Fix bounds of field
                    /*if (topPaddle.Position.X + MAX_SPEED + topPaddle.Texture.Width > 480)
                        topPaddle.Position.X = 480 - topPaddle.Texture.Width;*/
                    if (CenterOfPaddle - CenterOfBall < MAX_SPEED)
                        topPaddle.Position.X -= CenterOfPaddle - CenterOfBall;
                    else
                        topPaddle.Position.X -= MAX_SPEED;
                }
                else if (CenterOfPaddle < CenterOfBall)//the ball must be on the left
                {
                    //TODO : Fix bounds of field
                    /*if (CenterOfPaddle - MAX_SPEED < 0)
                        CenterOfPaddle = 0;*/
                    if (CenterOfBall - CenterOfPaddle < MAX_SPEED)
                        topPaddle.Position.X += CenterOfBall - CenterOfPaddle;
                    else
                        topPaddle.Position.X += MAX_SPEED;
                }
            }
            else
            {
                if (topPaddle.Position.X > 240 - topPaddle.Texture.Width / 2)
                    topPaddle.Position.X -= 5;
                if (topPaddle.Position.X < 240 - topPaddle.Texture.Width / 2)
                    topPaddle.Position.X += 5;
                errorAccumulate += errorNum;
            }
        }
        #endregion
    }
}
