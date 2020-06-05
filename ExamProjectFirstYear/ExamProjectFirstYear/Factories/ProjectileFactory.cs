using ExamProjectFirstYear.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
    class ProjectileFactory : IFactory
    {
        private Projectile bossProjectile;
        private SpriteRenderer bossProjectileRenderer;

        private Projectile playerMeleeObject;
        private SpriteRenderer playerMeleeObjectRenderer;

        private Projectile meleeObject;
        private SpriteRenderer meleeObjectRenderer;

        private Projectile playerProjectile;
        private SpriteRenderer playerProjectileRenderer;

        private Projectile enemyProjectile;
        private SpriteRenderer enemyProjectileRenderer;

        private Projectile projectile;
        private SpriteRenderer projectileRenderer;

        private static ProjectileFactory instance;

        public static ProjectileFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProjectileFactory();
                }

                return instance;
            }
        }

        private ProjectileFactory()
        {
            CreateProjectilePrototype(ref bossProjectile, ref bossProjectileRenderer, "OopPlayerSprite2");
            CreateProjectilePrototype(ref playerProjectile, ref playerProjectileRenderer, "FlyingEnemy");
            CreateProjectilePrototype(ref playerMeleeObject, ref playerMeleeObjectRenderer, "MeleeObject2");
            CreateProjectilePrototype(ref meleeObject, ref meleeObjectRenderer, "MeleeObject2");
            CreateProjectilePrototype(ref projectile, ref enemyProjectileRenderer, "OopPlayerSprite2");
            CreateProjectilePrototype(ref projectile, ref projectileRenderer, "OopPlayerSprite2");
            CreateProjectilePrototype(ref enemyProjectile, ref enemyProjectileRenderer, "OopPlayerSprite2");
        }

        private void CreateProjectilePrototype(ref Projectile projectile, ref SpriteRenderer renderer, string spriteName)
        {
            projectile = new Projectile();
            renderer = new SpriteRenderer(spriteName);
        }

        public GameObject Create(Tag type, Tag sender)
        {
            GameObject gameObject = new GameObject();
            gameObject.Tag = sender;

            switch (type)
            {
                case Tag.BOSSPROJECTILE:
                    gameObject.AddComponent(bossProjectile.Clone());
                    gameObject.AddComponent(bossProjectileRenderer.Clone());
                    gameObject.AddComponent(new Collider(bossProjectileRenderer, (Projectile)gameObject.GetComponent(Tag.PROJECTILE)));
                    break;
                case Tag.PLAYERPROJECTILE:
                    gameObject.AddComponent(playerProjectile.Clone());
                    gameObject.AddComponent(playerProjectileRenderer.Clone());
                    gameObject.AddComponent(new Collider(playerProjectileRenderer, (Projectile)gameObject.GetComponent(Tag.PROJECTILE)));
                    gameObject.AddComponent(new Movement(false, 1000));
                    break;
                case Tag.PLAYERMELEEATTACK:
                    gameObject.AddComponent(playerMeleeObject.Clone());
                    gameObject.AddComponent(playerMeleeObjectRenderer.Clone());
                    gameObject.AddComponent(new Collider(playerMeleeObjectRenderer, (Projectile)gameObject.GetComponent(Tag.PROJECTILE)));
                    //gameObject.AddComponent(new Movement(false, 0, 0));
                    break;
                case Tag.MELEEATTACK:
                    gameObject.AddComponent(meleeObject.Clone());
                    gameObject.AddComponent(meleeObjectRenderer.Clone());
                    gameObject.AddComponent(new Collider(meleeObjectRenderer, (Projectile)gameObject.GetComponent(Tag.PROJECTILE)));
                    //gameObject.AddComponent(new Movement(false, 0, 0));
                    break;
                case Tag.PROJECTILE:
                    gameObject.AddComponent(projectile.Clone());
                    gameObject.AddComponent(projectileRenderer.Clone());
                    gameObject.AddComponent(new Collider(projectileRenderer, (Projectile)gameObject.GetComponent(Tag.PROJECTILE)));
                    gameObject.AddComponent(new Movement(false, 1000));
                    break;
                case Tag.ENEMYPROJECTILE:
                    gameObject.AddComponent(enemyProjectile.Clone());
                    gameObject.AddComponent(enemyProjectileRenderer.Clone());
                    gameObject.AddComponent(new Collider(enemyProjectileRenderer, (Projectile)gameObject.GetComponent(Tag.PROJECTILE)));
                    break;
            }

            return gameObject;
        }
    }
}
