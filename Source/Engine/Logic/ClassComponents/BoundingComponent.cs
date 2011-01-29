#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Logic.Actors;
#endregion

namespace Engine.Logic.ClassComponents
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 9.07.2010
    /// Description: Bounding Box Component
    /// </summary>
    public class BoundingComponent : ClassComponent
    {
        #region Fields
#if DEBUG
        Texture2D boundingTexture;
#endif
        private Point boxDimension;
        private BoundingBox box;
        #endregion

        #region Properties
        /// <summary>The bounding box</summary>
        public BoundingBox Box { get { return this.box; } set { this.box = value; } }
        #endregion

        #region Constructors
        /// <summary>The component used to check collisions</summary>
        /// <param name="owner">The actor the object belongs to</param>
        /// <param name="actorId">The actor ID</param>
        /// <param name="boxDim">The bounding box</param>
        /// <param name="hitDim">The hit box</param>
        public BoundingComponent(Actor owner, Point boxDim)
            : base(owner)
        {
            this.boxDimension = boxDim;

#if DEBUG
            //Set Data needs an array, so we do this stupid dance
            Color[] final = new Color[1];
            final[0] = Color.White;
            boundingTexture = new Texture2D(DeviceManager.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            boundingTexture.SetData<Color>(final);
#endif
            Update(null);
        }
        #endregion

        #region Public Methods
        public override void Update(GameTime gameTime)
        {
            Vector2 offset = Vector2.Zero;
            if (this.Owner.GetSprite() != null)
                offset = this.Owner.GetSprite().Offset;
            Vector2 pos = this.Owner.GetPosition().Position + offset;
            pos.X -= this.boxDimension.X / 2.0f;
            pos.Y -= this.boxDimension.Y;
            this.box = new BoundingBox(new Vector3(pos.X, pos.Y, 0), new Vector3(pos.X + this.boxDimension.X, pos.Y + this.boxDimension.Y, 0));
        }

        public void CheckCollisions()
        {
           //foreach (Actor actor in ...)
           //if (this.Owner.GetBounding().DoesCollid(actor))
           //   ...
        }

        public bool DoesCollid(Vector2 worldPos)
        {
            if (this.Box.Contains(new Vector3(worldPos.X, worldPos.Y, 0)) == ContainmentType.Contains)
                return true;
            else
                return false;
        }

        public bool DoesCollid(Actor actor)
        {
            if (actor == this.Owner)
                return false;

            bool collided = false;

            if (actor.GetBounding() != null)
            {
                BoundingBox b = actor.GetBounding().Box;
                if (actor.GetBounding().Box.Contains(this.Box) == ContainmentType.Contains)
                    collided = true;
                else if (actor.GetBounding().Box.Contains(this.Box) == ContainmentType.Intersects)
                    collided = true;
                else if (actor.GetBounding().Box.Intersects(this.Box))
                    collided = true;

                if (collided)
                    HandleCollision(actor);
            }

            return collided;
        }

#if DEBUG
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //top
            spriteBatch.Draw(this.boundingTexture, new Rectangle((int)this.box.Min.X, (int)this.box.Min.Y, this.boxDimension.X, 1), Color.Yellow);
            //Right
            spriteBatch.Draw(this.boundingTexture, new Rectangle((int)this.box.Max.X, (int)this.box.Min.Y, 1, this.boxDimension.Y), Color.Yellow);
            //Bottom
            spriteBatch.Draw(this.boundingTexture, new Rectangle((int)this.box.Min.X, (int)this.box.Max.Y, this.boxDimension.X, 1), Color.Yellow);
            //Left
            spriteBatch.Draw(this.boundingTexture, new Rectangle((int)this.box.Min.X, (int)this.box.Min.Y, 1, this.boxDimension.Y), Color.Yellow);
        }
#endif
        #endregion

        #region Private Methods
        /// <summary>
        /// The passed in actor has collided with the Owner
        /// </summary>
        /// <param name="actor">Colliding actor</param>
        private void HandleCollision(Actor actor)
        {
            
        }
        #endregion
    }
}
