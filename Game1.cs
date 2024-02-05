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

        // create player object
        Texture2D playerSprite = Content.Load<Texture2D>("player");
        Vector2 playerPosition = new(50, 50);
        player = new Player(
            playerPosition,     // player initial position
            4f,                 // player speed
            playerSprite        // player's sprite
        );
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

        // ~~~ draw debug information ~~~
        if (enableDebugDrawing) {
            // draw player's hitbot
            DebugLib.DrawRectOutline(
                _spriteBatch,
                player.Hitbox,
                5f,
                Color.Red
            );

            // draw player's direction
            Vector2 playerCenterPos = player.Hitbox.Center.ToVector2();
            Vector2 directionPos = playerCenterPos + (player.Direction * 100f);
            DebugLib.DrawLine(
                _spriteBatch,
                playerCenterPos,
                directionPos,
                5f,
                Color.Green
            );
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
