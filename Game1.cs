using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fizzhz
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Vector2 CurrentResolution;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            CurrentResolution = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

        }
        public static Camera Camera;
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Camera = new Camera(GraphicsDevice.Viewport);


            base.Initialize();
        }

        public static BasicEffect LineBasicEffect;
        public static Texture2D EmptyTexture, SpringTexture;

        public static SpriteFont MainFont;
        public static Graf Graf;

        public static Button New;
        public static Button ClearAll, ResetAll;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            LineBasicEffect = new BasicEffect(GraphicsDevice);
            LineBasicEffect.VertexColorEnabled = true;
            LineBasicEffect.Projection = Matrix.CreateOrthographicOffCenter
            (0, GraphicsDevice.Viewport.Width,     // left, right
            GraphicsDevice.Viewport.Height, 0,    // bottom, top
            0, 1);


            MainFont = Content.Load<SpriteFont>("MainFont");

            SpringTexture = Content.Load<Texture2D>("Spring");

            EmptyTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Int32[] pixel = { 0xFFFFFF }; // White. 0xFF is Red, 0xFF0000 is Blue
            EmptyTexture.SetData<Int32>(pixel, 0, EmptyTexture.Width * EmptyTexture.Height);

            WeightSistems.Add(new WeightSistem(Color.Green, new Vector2(0, 0)));
            /*WeightSistems.Add(new WeightSistem(Color.Blue, new Vector2(300, 0)));
            WeightSistems.Add(new WeightSistem(Color.Red, new Vector2(600, 0)));
            WeightSistems.Add(new WeightSistem(Color.Yellow, new Vector2(900, 0)));*/

            Graf = new Graf(new Vector2(0, 350));

            New = new Button(new Vector2(-100, -250), delegate {
                Game1.WeightSistems.Add(new WeightSistem(Program.Colors[Game1.WeightSistems.Count], new Vector2(Game1.WeightSistems.Count * 300, 0)));
            }, "Add");

            ClearAll = new Button(new Vector2(-100, -200), delegate {
                Game1.WeightSistems = new List<WeightSistem>();
                Game1.WeightSistems.Add(new WeightSistem(Program.Colors[Game1.WeightSistems.Count], new Vector2(Game1.WeightSistems.Count * 300, 0)));
            }, "Clear all");

            ResetAll = new Button(new Vector2(-200, -200), delegate {
                for (int i = 0; i < WeightSistems.Count; i++)
                    if (WeightSistems[i] != null)
                        WeightSistems[i].TotalTime = 0;
            }, "Reset all");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public static MouseState MouseState;
        public static KeyboardState KeyboardState;
        public static Vector2 MousePosition;
        public static bool GoneFullScreen = false;
        public static List<WeightSistem> WeightSistems = new List<WeightSistem>();
        double TimeSinceLastUpdate = 0;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState = Mouse.GetState();
            KeyboardState = Keyboard.GetState();

            New.Update(gameTime);
            ClearAll.Update(gameTime);
            ResetAll.Update(gameTime);

            if (!GoneFullScreen && KeyboardState.IsKeyDown(Keys.F12))
            {
                GoneFullScreen = true;

                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

                graphics.ToggleFullScreen();
                graphics.ApplyChanges();
            }

            Camera.UpdateCamera(GraphicsDevice.Viewport);
            MousePosition = Vector2.Transform(MouseState.Position.ToVector2(), Matrix.Invert(Camera.Transform));

            for (int i = 0; i < Program.Executing.Count; i++)
            {
                if (Program.Executing[i] != null)
                {
                    Program.Executing[i]();
                }
            }

            TimeSinceLastUpdate += gameTime.ElapsedGameTime.TotalMilliseconds;
            /*if (TimeSinceLastUpdate > 100 || true)
            {*/
                for (int i = 0; i < WeightSistems.Count; i++)
                    if (WeightSistems[i] != null)
                        WeightSistems[i].Update(gameTime);
            //}

            Graf.Update(gameTime);
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Camera.Transform);

            for (int i = 0; i < WeightSistems.Count; i++)
                if (WeightSistems[i] != null)
                    WeightSistems[i].Draw(spriteBatch, gameTime);

            Graf.Draw(spriteBatch, gameTime, GraphicsDevice);

            New.Draw(spriteBatch,gameTime);
            ClearAll.Draw(spriteBatch, gameTime);
            ResetAll.Draw(spriteBatch, gameTime);

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
