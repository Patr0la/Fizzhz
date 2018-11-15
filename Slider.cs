using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fizzhz
{
    public class Slider
    {
		public Vector2 Position;
        public float MaxValue, MinValue, CurrentValue, XtoValueRatio, Length;
        public Rectangle Slide;
        public string Name;
        public bool IntsOnly, UpdatedFormula = true;

        public float[] ClapValues;

        public Slider(Vector2 position, float[] clapValues, int currentValue, float length, string name)
        {
            Position = position;
            ClapValues = clapValues;
            Length = length;
            Name = name;
            MinValue = 0;
            MaxValue = ClapValues.Length - 1;
            XtoValueRatio = (ClapValues.Length - 1)/ length;
            CurrentValue = ClapValues[currentValue];
            Slide = new Rectangle((int)(currentValue / XtoValueRatio + Position.X), (int)position.Y - 8, 10, 20);
        }

        public Slider(Vector2 position, float maxValue, float minValue, float currentValue, float length, string name, bool intsOnly)
        {
            Position = position;
            MaxValue = maxValue;
            MinValue = minValue;
            CurrentValue = currentValue;
            Length = length;
            XtoValueRatio = (maxValue - minValue) / length ;
            Slide = new Rectangle((int)((CurrentValue - MinValue) / XtoValueRatio + Position.X), (int)position.Y - 8, 10, 20);
            Name = name;
            IntsOnly = intsOnly;
        }

        internal bool moving = false;
        internal Point LastMousePos;


        public void Update(){
            if(Game1.MouseState.LeftButton == ButtonState.Pressed && Slide.Contains(Game1.MousePosition)){
                moving = true;
                LastMousePos = Game1.MousePosition.ToPoint();
            }
            if (moving && Game1.MouseState.LeftButton == ButtonState.Pressed)
            {
                if (ClapValues == null)
                {
                    UpdatedFormula = true;
                    Slide.X += -LastMousePos.X + Game1.MousePosition.ToPoint().X;


                    if (Slide.X > (int)Length + Position.X)
                    {
                        Slide.X = (int)Length + (int)Position.X;
                        CurrentValue = MaxValue;
                    }
                    if (Slide.X < (int)Position.X)
                    {
                        Slide.X = (int)Position.X;
                        CurrentValue = MinValue;
                    }
                    CurrentValue = XtoValueRatio * (Slide.X - Position.X) + MinValue;
                    //SlideX = (CurrentValue - MinValue) / XtoValueRatio + posX

                    if (IntsOnly)
                        CurrentValue = (float)Math.Round(CurrentValue);
                    if (CurrentValue > MaxValue) CurrentValue = MaxValue;
                    if (CurrentValue < MinValue) CurrentValue = MinValue;
                    LastMousePos = Game1.MousePosition.ToPoint();
                }else{
                    UpdatedFormula = true;
                    Slide.X += -LastMousePos.X + Game1.MousePosition.ToPoint().X;
                    if (Slide.X > (int)Length + Position.X)
                    {
                        Slide.X = (int)Length + (int)Position.X;
                        CurrentValue = ClapValues[ClapValues.Length - 1];
                    }
                    if (Slide.X < (int)Position.X)
                    {
                        Slide.X = (int)Position.X;
                        CurrentValue = ClapValues[0];
                    }
                    CurrentValue = ClapValues[(int)Math.Round(XtoValueRatio * (Slide.X - Position.X))];
                }
            }
            else{
                moving = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch){
            //new Vector2(Global.Global.CurrentResolution.X / Global.Textures.LoadingBackground.Width,
            //Global.Global.CurrentResolution.Y / Global.Textures.LoadingBackground.Height);
            spriteBatch.DrawString(Game1.MainFont, String.Format("{0} = {1}", Name, CurrentValue), Position - new Vector2(70, 5), Color.White);
            spriteBatch.Draw(Game1.EmptyTexture, new Rectangle((int)Position.X, (int)Position.Y, (int)Length, 4), Color.White);
            spriteBatch.Draw(Game1.EmptyTexture, Slide, Color.White);
        }
    }
}
