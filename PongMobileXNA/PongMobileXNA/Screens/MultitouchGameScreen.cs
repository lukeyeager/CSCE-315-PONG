using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using PongClasses;

namespace PONG
{
    public class MultitouchGameScreen : PongGameScreen
    {
        #region Fields

        //Input related
        public TouchLocation topPaddleTouch;
        public Int32 topPaddleTouchId;

        #endregion

        #region Initialization

        public MultitouchGameScreen()
            :base()
        {
            topPaddleTouch = new TouchLocation();
            topPaddleTouchId = -1;
        }

        public override void Start()
        {
            base.Start();

            topPaddle.Position.Y = paddleTouchBufferSize;
        }

        #endregion

        #region Finalization

        public override void finishCurrentGame()
        {
            foreach (GameScreen screen in ScreenManager.GetScreens())
                screen.ExitScreen();
            ScreenManager.AddScreen(new BackgroundScreen());
            ScreenManager.AddScreen(new MainMenuScreen());
        }

        #endregion

        #region Update

        public override void UpdateTopPaddle(float elapsed)
        {
            //Update from touch input
            if (topPaddleTouchId != -1)
            {
                var newX = topPaddleTouch.Position.X + 5 - topPaddle.Width / 2;
                topPaddle.Velocity.X = (newX - topPaddle.Position.X) / (128.0f * elapsed);
            }
            //Update from key input
            else if (lastKeyInput.Count > 0)
            {
                if (lastKeyInput.Contains(Keys.D) && !lastKeyInput.Contains(Keys.F))
                {
                    topPaddle.Velocity.X += -0.3f * topPaddle.MaxSpeed;
                }
                else if (lastKeyInput.Contains(Keys.F) && !lastKeyInput.Contains(Keys.D))
                {
                    topPaddle.Velocity.X += 0.3f * topPaddle.MaxSpeed;
                }
            }

            base.UpdateTopPaddle(elapsed);
        }

        #endregion

        #region Input
        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            /// <summary>
            /// Read Touchscreen input
            /// </summary>
            /// <remarks>
            /// The base HandleInput has already populated lastTouchInput
            /// </remarks>
            if (lastTouchInput.Count > 0)
            {
                foreach (var touch in lastTouchInput)
                {
                    switch (touch.State)
                    {
                        case TouchLocationState.Pressed:
                            if (topPaddleTouchId == -1 &&
                                touch.Position.Y < topPaddle.Position.Y + topPaddle.Height &&
                                touch.Position.X > topPaddle.Position.X - 20 &&
                                touch.Position.X < topPaddle.Position.X + topPaddle.Width + 20)
                            {
                                topPaddleTouch = touch;
                                topPaddleTouchId = touch.Id;
                                //TODO: remove touch from lastTouchInput?
                            }
                            break;
                        case TouchLocationState.Moved:
                            if (touch.Id == topPaddleTouchId)
                            {
                                topPaddleTouch = touch;
                            }
                            break;
                        case TouchLocationState.Released:
                            if (touch.Id == topPaddleTouchId)
                            {
                                topPaddleTouchId = -1;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

        }

        #endregion
    }
}
