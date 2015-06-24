using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlipFlopNinjaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

internal class Player
{
    public Texture2D Texture;
    public Vector2 Position;
    public Rectangle Boundaries;
    public float Rotation = 0f;
    public float RotationSpeed = 0f;

    public Vector2 Gravity = new Vector2(0, 9.81f);

    private Vector2 _velocity;
    private float _speed = 2;

    public Rectangle SourceRectangle
    {
        get { return new Rectangle(0, 0, this.Texture.Width, this.Texture.Height); }
    }

    public Player(Texture2D texture)
    {
        this.Texture = texture;

        //Boundaries = new Rectangle(this.Position.X, this.Position.Y, this.Texture.Width, this.Texture.Height);
    }

    public void Update()
    {
        // horizontal
        if (Keyboard.GetState().IsKeyDown(Keys.Left))
            _velocity.X -= _speed;
        else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            _velocity.X += _speed;

        // vertical
        if (Keyboard.GetState().IsKeyDown(Keys.Up))
            _velocity.Y -= _speed;
        else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            _velocity.Y += _speed;

        // rotation
        if (Keyboard.GetState().IsKeyDown(Keys.N))
            RotationSpeed = 1f;
        else if (Keyboard.GetState().IsKeyDown(Keys.M))
            RotationSpeed = -1;
        else
            RotationSpeed = 0;

        //_velocity += Gravity;

        Position += _velocity*Game1.DeltaTime;
        Rotation += RotationSpeed * Game1.DeltaTime;

        ScreenWrap();
    }

    public bool DrawScreenWrapClone;
    public Vector2 ClonePosition;
    void ScreenWrap()
    {
        ClonePosition = Position;
        #region Horizontal
        if (Position.X < 0) // left
        {
            DrawScreenWrapClone = true;
            ClonePosition.X = Game1.SCREEN_WIDTH+Position.X;

            if (Max.X < 0)
            {
                Position.X = ClonePosition.X;
                DrawScreenWrapClone = false;
            }
        }
        else if (Max.X > Game1.SCREEN_WIDTH) // right
        {
            DrawScreenWrapClone = true;
            ClonePosition.X = Max.X - Game1.SCREEN_WIDTH - this.Texture.Width;

            if (Position.X > Game1.SCREEN_WIDTH)
            {
                Position.X = ClonePosition.X;
                DrawScreenWrapClone = false;
            }
        }
        #endregion
        #region Vertical
        if (Position.Y < 0) // left
        {
            DrawScreenWrapClone = true;
            ClonePosition.Y = Game1.SCREEN_HEIGHT + Position.Y;

            if (Max.Y < 0)
            {
                Position.Y = ClonePosition.Y;
                DrawScreenWrapClone = false;
            }
        }
        else if (Max.Y > Game1.SCREEN_HEIGHT) // right
        {
            DrawScreenWrapClone = true;
            ClonePosition.Y = Max.Y - Game1.SCREEN_HEIGHT - this.Texture.Height;

            if (Position.Y > Game1.SCREEN_HEIGHT)
            {
                Position.Y = ClonePosition.Y;
                DrawScreenWrapClone = false;
            }
        }
        #endregion
        //else if ()
    }

    public Vector2 Origin
    {
        get { return new Vector2(Position.X + this.Texture.Width/2, Position.Y + this.Texture.Height/2); }
    }

    public Vector2 Min
    {
        get { return new Vector2(Origin.X - this.Texture.Width / 2, Origin.Y - this.Texture.Height / 2); }
    }

    public Vector2 Max
    {
        get { return new Vector2(Origin.X + this.Texture.Width / 2, Origin.Y + this.Texture.Height / 2); }        
    }

}
