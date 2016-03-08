using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary> 
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Matrix projection;
        public Matrix view;
        private SpriteFont mFont;
        car car;
        //Texture2D mSpriteTexture;
        road road;
        Hazard hazard;
         float Hv = 40f;
        float roaddepth0 = 0.0f;
        float roaddepth1 = 100.0f;
        private float RoadSpeed=20.0f;
        double xpositionc = 0;
        public bool hazardvisible = true;
        private Texture2D mBackground;
        public int hazardnumber = 1;
       
       
        private enum State
        {
            TitleScreen,      // ≥ı º∆¨Õ∑
            Running,
           // GameOver,
            Success
        }
        State mcurrentstate = State.TitleScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height, 1.0f, 1000.0f);
            view = Matrix.CreateLookAt(new Vector3(0.0f, 9.5f, -17.0f), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            car = new car();
            car.view = view;
            car.projection = projection;
            road = new road();
            road.view = view;
            road.projection = projection;
            hazard = new Hazard();
            hazard.view = view;
            hazard.projection = projection;
            hazard.HazardModel = Content.Load<Model>("Models//hazard");
            hazard.addHazard();
            hazard.hazardsphere.Center.X = hazard.haloc;
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
             car.CarModel = Content.Load<Model>("Models//car");
            //mSpriteTexture = this.Content.Load<Texture2D>("SpeedWin");
             road.RoadModel = Content.Load<Model>("Models//road");
             mFont = Content.Load<SpriteFont>("Models//MyFont");
             mBackground = Content.Load<Texture2D>("Models//Background");
   
             hazard.HazardModel = Content.Load<Model>("Models//hazard");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            switch (mcurrentstate)
            {
                case State.TitleScreen:
                    {
                        if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Space))
                        {
                            mcurrentstate = State.Running;
                        }
                        break;
                    }
                //case State.Success:
               case State.Success:
                    {
                        if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Space))
                        {
                            mcurrentstate = State.TitleScreen;
                        }
                        break;
                    }
                case State.Running:
                    {
                        if(car.carboundingsphere.Intersects(hazard.hazardsphere))
                        {
                            this.Exit();
                        }
                        else if(hazardnumber>=20)
                        {
                            mcurrentstate = State.Success;
                        }

                        else{
                        double elapsed = gameTime.ElapsedGameTime.TotalSeconds;
                        roaddepth0 -= (float)(RoadSpeed * elapsed);
                        roaddepth1 -= (float)(RoadSpeed * elapsed);
                        if (roaddepth0 < -75.0f)
                        {
                            roaddepth0 = roaddepth1 + 100.0f;
                        }
                        if (roaddepth1 < -75.0f)
                        {
                            roaddepth1 = roaddepth0 + 100.0f;
                        }
                        if (!hazardvisible)
                        {
                            hazard = new Hazard();
                            hazard.HazardModel = Content.Load<Model>("Models//hazard");
                            hazard.view = view;
                            hazard.projection = projection;
                            hazard.addHazard(gameTime);
                          
                            hazard.hazardsphere.Center.Z = 60f;
                            hazard.hazardsphere.Center.X = hazard.haloc;
                            hazardvisible = true;
                            hazardnumber++;
                        }

                        xpositionc += Hv * elapsed;
                        hazard.hazardsphere.Center.Z = (float)(60-xpositionc);
                        if (xpositionc > 70)
                        {
                            xpositionc = 0;
                            hazardvisible = false;

                        }
                        checkinput();
                        car.update(gameTime);
                        base.Update(gameTime);
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

           // spriteBatch.Begin();

            /*switch (mcurrentstate) {
                case State.TitleScreen:
                        {
                            spriteBatch.Draw(mBackground, new Rectangle(graphics.GraphicsDevice.Viewport.X, graphics.GraphicsDevice.Viewport.Y, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);


                            DrawTextCentered("Drive Fast And Avoid the Oncoming Obstacles", 200);
                            DrawTextCentered("Press 'Space' to begin", 260);
                            

                            break;
                        }
               case State.Running:{*/
                
                        road.RoadDraw(gameTime, 0, 0, roaddepth0);
                        road.RoadDraw(gameTime, 0, 0, roaddepth1);
                        car.CarDraw(gameTime);
            
                        hazard.hazardmove(gameTime,xpositionc);
                       // break;
            //  }
              //  default:{

              /*   if (mcurrentstate == State.GameOver)
                {
                    DrawTextDisplayArea();

                    DrawTextCentered("Game Over.", 200);
                    DrawTextCentered("Press 'Space' to try again.", 260);
             

                }
            
                break;
                }*/
                   

   
       // }
            //spriteBatch.End();

            base.Draw(gameTime);
        }
        public void checkinput()
        {
            KeyboardState newstate = Keyboard.GetState();
            if(newstate.IsKeyDown(Keys.Left))
            {
                car.movingleft = true;
                car.movingright = false;
              //  car.carlocation = 2.5f;
            }
            if (newstate.IsKeyDown(Keys.Right))
            {
                car.movingleft  = false;
                car.movingright = true;
            //  car.carlocation =- 2.5f;
            }
        }
        private void DrawTextDisplayArea()
        {
            int aPositionX = (int)((graphics.GraphicsDevice.Viewport.Width / 2) - (450 / 2));
            spriteBatch.Draw(mBackground, new Rectangle(aPositionX, 75, 450, 400), Color.White);
        }

        private void DrawTextCentered(string theDisplayText, int thePositionY)
        {
            Vector2 aSize = mFont.MeasureString(theDisplayText);
            int aPositionX = (int)((graphics.GraphicsDevice.Viewport.Width / 2) - (aSize.X / 2));

            spriteBatch.DrawString(mFont, theDisplayText, new Vector2(aPositionX, thePositionY), Color.Beige, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
            spriteBatch.DrawString(mFont, theDisplayText, new Vector2(aPositionX + 1, thePositionY + 1), Color.Brown, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
        }
    }
}
