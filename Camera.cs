using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Fizzhz
{
    public class Camera
    {
        public float Zoom;
        public Vector2 Position;
        public Rectangle Bounds;
        public Rectangle VisibleArea;
        public Matrix Transform;

//        public int MiniX, MaxX, MiniY, MaxY, ChunkX, ChunkY;

        private float currentMouseWheelValue, previousMouseWheelValue, zoom, previousZoom;

        public Camera(Viewport viewport)
        {
            Bounds = viewport.Bounds;
            Zoom = 1f;
            Position = Vector2.Zero;
        }


        private void UpdateVisibleArea()
        {
            var inverseViewMatrix = Matrix.Invert(Transform);

            var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            var tr = Vector2.Transform(new Vector2(Bounds.X, 0), inverseViewMatrix);
            var bl = Vector2.Transform(new Vector2(0, Bounds.Y), inverseViewMatrix);
            var br = Vector2.Transform(new Vector2(Bounds.Width, Bounds.Height), inverseViewMatrix);

            var min = new Vector2(
                MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
            var max = new Vector2(
                MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
            VisibleArea = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));

            /*
            MiniX = VisibleArea.X / 32 - 1 + Global.CenterOfMapX * 128;
            MaxX = MiniX + VisibleArea.Width / 32 + 2;
            MiniY = VisibleArea.Y / 32 - 1 + Global.CenterOfMapY * 128;
            MaxY = MiniY + VisibleArea.Height / 32 + 2;


            ChunkX = ((int)Position.X) / 4096 + Global.CenterOfMapX;
            ChunkY = ((int)Position.Y) / 4096 + Global.CenterOfMapY;

            if (Position.X < 0) ChunkX -= 1;
            if (Position.Y < 0) ChunkY -= 1;
            */
        }

        private void UpdateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            UpdateVisibleArea();
        }

        public void MoveCamera(Vector2 movePosition)
        {
            Vector2 newPosition = Position + movePosition;
            Position = newPosition;
        }

        public void AdjustZoom(float zoomAmount)
        {
            Zoom += zoomAmount;
            if (Zoom < .25f)
                Zoom = .25f;
            if (Zoom > 2f)
                Zoom = 2f;
        }

        public Point LastMousePosition;
        public bool MovingCamera = false;
        public void UpdateCamera(Viewport bounds)
        {
            Bounds = bounds.Bounds;
            UpdateMatrix();
   
            float moveSpeed;

            if (Zoom > 0.8f)
                moveSpeed = 1f;
            else if (zoom > 0.6f)
                moveSpeed = 1.335f;
            else if (zoom > 0.4f)
                moveSpeed = 2f;
            else
                moveSpeed = 4;
            

            if (Game1.MouseState.MiddleButton == ButtonState.Pressed)
            {
                if (MovingCamera)
                    MoveCamera(new Vector2(LastMousePosition.X - Game1.MouseState.X, LastMousePosition.Y - Game1.MouseState.Y) * new Vector2(moveSpeed, moveSpeed));
                MovingCamera = true;
                LastMousePosition = Game1.MouseState.Position;
            }
            else
                MovingCamera = false;

            previousMouseWheelValue = currentMouseWheelValue;
            currentMouseWheelValue = Mouse.GetState().ScrollWheelValue;

            if (currentMouseWheelValue > previousMouseWheelValue)
                AdjustZoom(.25f);

            if (currentMouseWheelValue < previousMouseWheelValue)
                AdjustZoom(-.25f);

            previousZoom = zoom;
            zoom = Zoom;
        }
    }
}