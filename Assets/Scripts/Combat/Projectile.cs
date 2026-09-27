using UnityEngine;

namespace GalacticRogue.Combat
{
    public sealed class Projectile : MonoBehaviour
    {
        private const float Lifetime = 1.3f;

        private Vector2 direction;
        private int damage;
        private float destroyTime;
        private float speed;
        private CombatFaction targetFaction;
        private int remainingPierce;
        private bool homesOnEnemies;
        private int remainingChainJumps;
        private static Sprite laserSprite;

        public static void CreatePlayerLaser(Vector3 position, Vector2 direction, int damage, int pierceCount = 0, bool homesOnEnemies = false, int chainJumps = 0, float speed = 15f)
        {
            Create(position, direction, damage, speed, CombatFaction.Enemy, new Color(0.25f, 1f, 1f), "Laser Projectile", pierceCount, homesOnEnemies, chainJumps);
        }

        public static void CreateEnemyBolt(Vector3 position, Vector2 direction, int damage)
        {
            Create(position, direction, damage, 7f, CombatFaction.Player, new Color(1f, 0.2f, 0.65f), "Enemy Bolt", 0, false, 0);
        }

        private static void Create(Vector3 position, Vector2 direction, int damage, float speed, CombatFaction targetFaction, Color color, string projectileName, int pierceCount, bool homesOnEnemies, int chainJumps)
        {
            GameObject projectileObject = new GameObject(projectileName);
            projectileObject.transform.position = position;
            projectileObject.transform.up = direction;

            SpriteRenderer renderer = projectileObject.AddComponent<SpriteRenderer>();
            renderer.sprite = CreateLaserSprite();
            renderer.color = color;

            BoxCollider2D collider = projectileObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = new Vector2(0.12f, 0.5f);

            Rigidbody2D rigidbody = projectileObject.AddComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            rigidbody.gravityScale = 0f;

            Projectile projectile = projectileObject.AddComponent<Projectile>();
            projectile.direction = direction;
            projectile.damage = damage;
            projectile.speed = speed;
            projectile.targetFaction = targetFaction;
            projectile.remainingPierce = pierceCount;
            projectile.homesOnEnemies = homesOnEnemies;
            projectile.remainingChainJumps = chainJumps;
            projectile.destroyTime = Time.time + Lifetime;
        }

        private void Update()
        {
            if (homesOnEnemies)
            {
                Targetable target = TargetRegistry.FindNearest(transform.position, 8f);
                if (target != null)
                {
                    Vector2 desiredDirection = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
                    direction = Vector2.Lerp(direction, desiredDirection, 9f * Time.deltaTime).normalized;
                    transform.up = direction;
                }
            }

            transform.position += (Vector3)(direction * speed * Time.deltaTime);
            if (Time.time >= destroyTime)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable == null || damageable.Faction != targetFaction || (targetFaction == CombatFaction.Enemy && !TargetRegistry.IsVisible(other.transform.position)))
            {
                return;
            }

            DamageNumber.Create(transform.position, damage);
            damageable.ApplyDamage(damage);
            TryChainToNextTarget(other.transform);
            if (remainingPierce-- <= 0)
                Destroy(gameObject);
        }

        private void TryChainToNextTarget(Transform hitTarget)
        {
            if (targetFaction != CombatFaction.Enemy || remainingChainJumps <= 0) return;

            Targetable nextTarget = TargetRegistry.FindNearest(transform.position, 8f, true, hitTarget);
            if (nextTarget == null) return;

            Vector2 jumpDirection = ((Vector2)nextTarget.transform.position - (Vector2)transform.position).normalized;
            CreatePlayerLaser(transform.position + (Vector3)(jumpDirection * 0.45f), jumpDirection, damage, remainingPierce, homesOnEnemies, remainingChainJumps - 1, speed);
        }

        private static Sprite CreateLaserSprite()
        {
            if (laserSprite != null) return laserSprite;

            Texture2D texture = new Texture2D(8, 32, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[8 * 32];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Color.white;
            }

            texture.SetPixels(pixels);
            texture.Apply();
            laserSprite = Sprite.Create(texture, new Rect(0, 0, 8, 32), new Vector2(0.5f, 0.5f), 32f);
            return laserSprite;
        }
    }
}
