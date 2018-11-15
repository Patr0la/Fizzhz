using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fizzhz
{
    public class Graf
    {
		public Vector2 Position;
        public Vector2[][] Vertices;

        internal Button DrawButton;
        internal bool Drawing = false;

        public Graf(Vector2 position)
        {
            DrawButton = new Button(new Vector2((int)position.X - 150, (int)position.Y - 15), delegate {
                Drawing = !Drawing;
            }, "Display graf");

            Position = position;

            Vertices = new Vector2[Game1.WeightSistems.Count][];
        }

        public void Update(GameTime gameTime)
        {
            DrawButton.Update(gameTime);
            if (!Drawing) return;

            for (int i = 0; i < Game1.WeightSistems.Count; i++)
                if (Game1.WeightSistems[i].a.UpdatedFormula || Game1.WeightSistems[i].k.UpdatedFormula || Game1.WeightSistems[i].m.UpdatedFormula)
                    Vertices = new Vector2[Game1.WeightSistems.Count][];


            for (int i = 0; i < Game1.WeightSistems.Count; i++)
            {
                List<Vector2> temp = new List<Vector2>();
                for (float x = 0; x < 10; x += 0.01f)
                    temp.Add(new Vector2(x * 100 + Position.X, Position.Y + Game1.WeightSistems[i].a.CurrentValue * 6 * (float)Math.Sin(Math.PI / 2 + x / Math.Sqrt(Game1.WeightSistems[i].k.CurrentValue / Game1.WeightSistems[i].m.CurrentValue))));

                Game1.WeightSistems[i].a.UpdatedFormula = false;
                Game1.WeightSistems[i].k.UpdatedFormula = false;
                Game1.WeightSistems[i].m.UpdatedFormula = false;

                Vertices[i] = temp.ToArray();
            }


            //Time += gameTime.ElapsedGameTime.TotalMilliseconds / 10;
            /*  T = 2pi/o
             *  T = 2pi * s(k/m)
             *  o = 1 / s(k/m)
             * */
            if (Time > 1000) Time = 0;
        }

        public double Time = 0;
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            DrawButton.Draw(spriteBatch, gameTime);
            if (!Drawing) return;

            for (int i = 0; i < Game1.WeightSistems.Count; i++)
            {
                if (Vertices[i] == null) continue;

                for (int j = 0; j < Vertices[i].Length - 1; j++){
                    float dx = Vertices[i][j].X - Vertices[i][j + 1].X;
                    float dy = Vertices[i][j].Y - Vertices[i][j + 1].Y;
                    double angle = Math.PI / 2 - Math.Atan2(dx, dy);

                    spriteBatch.Draw(
                        Game1.EmptyTexture,
                        new Rectangle((int)Vertices[i][j].X,  (int)Vertices[i][j].Y, Math.Max(5, (int)(Math.Sqrt(dx * dx + dy * dy))), 5),
                        null,
                        Game1.WeightSistems[i].Color,
                        (float)angle,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0f
                    );
                }



                //GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, new VertexPositionColor[] { new VertexPositionColor(new Vector3(0, 200, 0), Color.Blue), new VertexPositionColor(new Vector3(1000, 200, 0), Color.Blue) }, 0, 1);
            }

            spriteBatch.Draw(
                Game1.EmptyTexture,
                new Rectangle((int)Position.X, (int)Position.Y - 5, 1000, 5),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0f
            );
        }
    }
}
