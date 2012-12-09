﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameJamTest.Assets;
using GameJamTest.GameObjects.Zombie;
using GameJamTest.Screens;

namespace GameJamTest.GameObjects.Player
{
    public class PlayerShip : GameJamComponent
    {
        long score;
        private GameKeyboard keyboard;

        public PlayerShip(Game game, GameScreen screen)
            : base(game, screen, new Vector2(100, 100))
        {
            this.keyboard = new GameKeyboard();
            this.Layer = Layer.PLAYER;
        }

        public override void Destroy()
        {
            // invulnerability!
        }

        private void Fire()
        {
            Vector2 bulletPos = Vector2.Add(this.Position, new Vector2(12, 3));
            Screen.AddComponent(new Bullet(Game, Screen, Team.PLAYER, bulletPos, new Vector2(15, 0)));
        }

        public override void Initialize()
        {
            this.Sprite = Sprites.Ship;
        }

        public override void Update(GameTime gameTime)
        {
            keyboard.Update(gameTime);

            Vector2 velocity = new Vector2(0, 0);

            if (keyboard.Up.IsHeld())
            {
                velocity = Vector2.Add(velocity, new Vector2(0, -5));
            }

            if (keyboard.Left.IsHeld())
            {
                velocity = Vector2.Add(velocity, new Vector2(-5, 0));
            }

            if (keyboard.Down.IsHeld())
            {
                velocity = Vector2.Add(velocity, new Vector2(0, 5));
            }

            if (keyboard.Right.IsHeld())
            {
                velocity = Vector2.Add(velocity, new Vector2(5, 0));
            }

            this.Velocity = velocity;

            base.Update(gameTime);

            this.Position = new Vector2(
                MathHelper.Clamp(this.Position.X, 0, Game1.SCREEN_WIDTH - this.Sprite.Width),
                MathHelper.Clamp(this.Position.Y, 0, Game1.SCREEN_HEIGHT - this.Sprite.Height)
            );

            foreach (GameComponent component in this.Screen.Components)
            {
                GameJamComponent drawable = component as GameJamComponent;
                if (drawable != null && this.Collide(drawable))
                {
                    Bullet bullet = drawable as Bullet;
                    if (bullet != null && bullet.Team == Team.ZOMBIE)
                    {
                        this.Explode();
                        bullet.Destroy();
                    }

                    if (drawable is Asteroid)
                    {
                        this.Explode();
                    }

                    if (drawable is ZombieShip)
                    {
                        this.Explode();
                        drawable.Explode();
                    }
                }
            }

            if (keyboard.Fire.IsPressed())
            {
                this.Fire();
            }
        }

        public void ScorePoints(long points)
        {
            this.score += points;
        }

        public long Score
        {
            get { return this.score; }
        }
    }

}
