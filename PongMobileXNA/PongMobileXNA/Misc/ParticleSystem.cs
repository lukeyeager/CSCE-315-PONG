// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// ParticleSystem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PONG
{
    /// <summary>
    /// A relatively simple particle system.  We recycle particles instead of creating
    /// and destroying them as we need more.  "Effects" are created via factory methods
    /// on ParticleSystem, rather than a data driven model due to the relatively low
    /// number of effects.
    /// </summary>
    public class ParticleSystem
    {
        Random random;

        Texture2D defaultCollisionEffect;

        SpriteBatch spriteBatch;

        List<Particle> particles;

        public ParticleSystem(ContentManager content, SpriteBatch spriteBatch)
        {
            random = new Random();

            particles = new List<Particle>();

            this.spriteBatch = spriteBatch;

            defaultCollisionEffect = content.Load<Texture2D>("Images/defaultCollisionEffect");
        }

        /// <summary>
        /// Update all active particles.
        /// </summary>
        /// <param name="elapsed">The amount of time elapsed since last Update.</param>
        public void Update(float elapsed)
        {
            for (int i = 0; i < particles.Count; ++i)
            {
                particles[i].Life -= elapsed;
                if (particles[i].Life <= 0.0f)
                {
                    continue;
                }
                particles[i].Position += particles[i].Velocity * elapsed;
                particles[i].Rotation += particles[i].RotationRate * elapsed;
                particles[i].Alpha += particles[i].AlphaRate * elapsed;
                particles[i].Scale += particles[i].ScaleRate * elapsed;

                if (particles[i].Alpha <= 0.0f)
                    particles[i].Alpha = 0.0f;
            }
        }

        /// <summary>
        /// Draws the particles.
        /// </summary>
        public void Draw()
        {
            for (int i = 0; i < particles.Count; ++i)
            {
                Particle p = particles[i];
                if (p.Life <= 0.0f)
                    continue;

                float alphaF = 255.0f * p.Alpha;
                if (alphaF < 0.0f)
                    alphaF = 0.0f;
                if (alphaF > 255.0f)
                    alphaF = 255.0f;

                spriteBatch.Draw(p.Texture, p.Position, null, new Color(p.Color.R, p.Color.G, p.Color.B, (byte)alphaF), p.Rotation, new Vector2(p.Texture.Width / 2, p.Texture.Height / 2), p.Scale, SpriteEffects.None, 0.0f);
            }
        }

        /// <summary>
        /// Creates a particle, preferring to reuse a dead one in the particles list 
        /// before creating a new one.
        /// </summary>
        /// <returns></returns>
        Particle CreateParticle()
        {
            Particle p = null;

            for (int i = 0; i < particles.Count; ++i)
            {
                if (particles[i].Life <= 0.0f)
                {
                    p = particles[i];
                    break;
                }
            }

            if (p == null)
            {
                p = new Particle();
                particles.Add(p);
            }

            p.Color = Color.White;

            return p;
        }

        /// <summary>
        /// Creates the default effect for a ball collision
        /// </summary>
        /// <param name="position">Where on the screen to create the effect.</param>
        /// <param name="texture">What texture to display.</param>
        public void CreateDefaultCollisionEffect(Vector2 position)
        {
            Particle p = null;

            for (int i = 0; i < 1; ++i)
            {
                p = CreateParticle();
                p.Position = position;
                p.RotationRate = 0f;
                p.Scale = 0.1f;
                p.ScaleRate = 5.0f;// *(float)random.NextDouble();
                p.Alpha = 1f;
                p.AlphaRate = 0f;
                p.Velocity.X = 0f;// -32.0f + 64.0f * (float)random.NextDouble();
                p.Velocity.Y = 0f;//-32.0f + 64.0f * (float)random.NextDouble();
                p.Texture = defaultCollisionEffect;
                p.Life = 0.2f;
            }
        }
    }

    /// <summary>
    /// A basic particle.  Since this is strictly a data class, I decided to not go
    /// the full property route and used public fields instead.
    /// </summary>
    public class Particle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Texture;
        public float RotationRate;
        public float Rotation;
        public float Life;
        public float AlphaRate;
        public float Alpha;
        public float ScaleRate;
        public float Scale;
        public Color Color = Color.White;
    }
}
