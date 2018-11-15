using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fizzhz
{
    public class WeightSistem
    {
        public Vector2 Position;

        public Slider a;
        public Slider k;
        public Slider m;

        public Color Color;

        public Button PlayStop;
        public Button Reset;

        public WeightSistem(Color color, Vector2 position)
        {
            Color = color;
            Position = position;

            a = new Slider(position - new Vector2(150, 100), 20, 0, 0, 100, "y0", true);
            k = new Slider(position - new Vector2(150, 75), new float[] { 50, 100, 200}, 1, 100, "k");
            m = new Slider(position - new Vector2(150, 50), new float[] { 50, 100, 200 }, 1, 100, "m");

            PlayStop = new Button(new Vector2((int)position.X - 150, (int)position.Y), delegate {
                Playing = !Playing;
            }, "Play");
            Reset = new Button(new Vector2((int)position.X - 150, (int)position.Y + 50), delegate {
                TotalTime = 0;
            }, "Reset");
        }

        public double TotalTime = 0;
        public int Y = 0;

        public bool Playing = false;

        public void Update(GameTime gameTime){
            PlayStop.Update(gameTime);
            Reset.Update(gameTime);


            a.Update();
            k.Update();
            m.Update();

            if (!Playing)
                return;

            TotalTime += gameTime.ElapsedGameTime.TotalSeconds;
            Y = (int)(a.CurrentValue * 3 * (float)Math.Sin(TotalTime / Math.Sqrt(k.CurrentValue / m.CurrentValue) + Math.PI/2));

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime){
            a.Draw(spriteBatch);
            k.Draw(spriteBatch);
            m.Draw(spriteBatch);

            PlayStop.Draw(spriteBatch, gameTime);
            Reset.Draw(spriteBatch, gameTime);
            //spriteBatch.DrawString(Game1.MainFont, String.Format("y = {0} * sin( pi * {1} * t + pi * {2})", a.CurrentValue, 2 / b.CurrentValue, c.CurrentValue), Position - new Vector2(150, 130), Color.White);

            spriteBatch.Draw(
                Game1.SpringTexture,
                new Rectangle((int)Position.X, -100, 20 , (int)Position.Y + Y + 100),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0f
            );

            spriteBatch.Draw(
                Game1.EmptyTexture,
                new Rectangle((int)Position.X, (int)Position.Y + Y, 20, 20),
                null,
                Color,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0f
            );

            spriteBatch.DrawString(Game1.MainFont, Math.Round(TotalTime, 3).ToString(), Position - new Vector2(150, -100), Color.White);
        }
    }
}
