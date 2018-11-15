using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fizzhz
{
    public class Button
    {
        public Rectangle Position;

        public Action Callback;

        public string Text;

        public Button(Vector2 position, Action callback, string text)
        {
            Position = new Rectangle((int)position.X, (int)position.Y, (int)(Game1.MainFont.MeasureString(text).X * 2), (int)(2 * Game1.MainFont.MeasureString(text).Y));
            Callback = callback;
            Text = text;
        }

        internal double TimeSinceLastCLick = 0;
        public void Update(GameTime gameTime){
            TimeSinceLastCLick += gameTime.ElapsedGameTime.TotalSeconds;
            if (Position.Contains(Game1.MousePosition) && Callback != null && Game1.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && TimeSinceLastCLick > 0.25f)
            {
                Callback();
                TimeSinceLastCLick = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime){
            spriteBatch.Draw(Game1.EmptyTexture, Position, Color.Lime);
            spriteBatch.DrawString(Game1.MainFont, Text, new Vector2(Position.X + 0.5f * (float)Game1.MainFont.MeasureString(Text).X, Position.Y + 0.5f * (float)Game1.MainFont.MeasureString(Text).Y), Color.White );
        }
    }
}
