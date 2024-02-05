using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DebugLibDemo;

public class Game1 : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private KeyboardState kb;
    private KeyboardState kbPrev;

    private Player player;
    private SpriteFont arial25;
    private bool enableDebugDrawing = true;

    public Game1() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // load texture and initialize player
        Texture2D playerSprite = Content.Load<Texture2D>("player");
        Vector2 playerPosition = new(200, 200);
        player = new Player(
            playerPosition,     // player initial position
            4f,                 // player speed
            playerSprite        // player's sprite
        );

        // load font
        arial25 = Content.Load<SpriteFont>("arial25");
    }

    protected override void Update(GameTime gameTime) {
        // update keyboard state
        kbPrev = kb;
        kb = Keyboard.GetState();

        // exit game when escape pressed
        if (kb.IsKeyDown(Keys.Escape)) {
            Exit();
        }

        // toggle debug drawing with space key
        if (kb.IsKeyDown(Keys.Space) && kbPrev.IsKeyUp(Keys.Space)) {
            enableDebugDrawing = !enableDebugDrawing;
        }

        // update player (get input and move)
        player.Update(kb);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        // ~~~ draw player ~~~
        player.Draw(_spriteBatch);

        // ~~~ draw instruction text ~~
        _spriteBatch.DrawString(
            arial25,
            "Press space to toggle debug info\nand arrow keys to move",
            new Vector2(20, 20),
            Color.Black
        );

        // ~~~ draw debug information ~~~
        if (enableDebugDrawing) {
            // draw player's hitbox
            DebugLib.DrawRectOutline(
                _spriteBatch,
                player.Hitbox,
                5f,
                Color.Red
            );

            // draw player's direction
            Vector2 directionPos = player.CenterPosition + (player.Direction * 100f);
            DebugLib.DrawLine(
                _spriteBatch,
                player.CenterPosition,
                directionPos,
                5f,
                Color.Green
            );
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
