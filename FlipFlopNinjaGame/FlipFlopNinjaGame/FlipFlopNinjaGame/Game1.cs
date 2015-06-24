using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlipFlopNinjaGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int SCREEN_WIDTH = 1024;
        public static int SCREEN_HEIGHT = 768;

        private SpriteFont font;

        private Player player;
        private Texture2D arrowSprite;

        public static float DeltaTime;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;

            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>(@"Fonts/Tahoma");

            arrowSprite = Content.Load<Texture2D>(@"Textures/sprite");

            player = new Player(Content.Load<Texture2D>(@"Textures/gf"));

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            player.Update();

            //if (Keyboard.GetState().IsKeyDown(Keys.A))
              //  player.RotationSpeed -= 0.1f*DeltaTime;
            //else if (Keyboard.GetState().IsKeyDown(Keys.S))
              //  player.RotationSpeed += 0.1f * DeltaTime;

           

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            // original
            spriteBatch.Draw(player.Texture, player.Position, player.SourceRectangle, Color.White,
                player.Rotation, Vector2.Zero, 1f, SpriteEffects.None, 1);

            // clone
            if (player.DrawScreenWrapClone)
                spriteBatch.Draw(player.Texture, player.ClonePosition, player.SourceRectangle, Color.Red,
                    player.Rotation, Vector2.Zero, 1f, SpriteEffects.None, 1);

            spriteBatch.Draw(arrowSprite, player.Origin, Color.White);


            string t = player.ClonePosition.ToString();
            spriteBatch.DrawString(font, t, new Vector2(10, 10), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
