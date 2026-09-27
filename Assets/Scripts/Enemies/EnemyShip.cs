using GalacticRogue.Combat;
using GalacticRogue.Player;
using UnityEngine;

namespace GalacticRogue.Enemies
{
    public enum EnemyType { Skitter, Spire, Orbital, Apex, Maw, SecurityBot, SentryTurret }

    /// <summary>
    /// Base enemy controller. Behavior varies by type:
    /// - Skitter: Approaches directly.
    /// - Spire: Keeps distance, fires readable bolts.
    /// - Orbital: Orbits player, fires when close.
    /// - Apex: Elite, fast, fires fan.
    /// - SecurityBot: Space Station, homing projectiles.
    /// - SentryTurret: Space Station, stationary, sustained fire.
    /// </summary>
    public sealed class EnemyShip : MonoBehaviour
    {
        private EnemyType type;
        private Transform player;
        private float moveSpeed;
        private float preferredDistance;
        private float nextShotTime;
        private float orbitAngle;

        private void Awake()
        {
            GetComponent<Damageable>().Died += HandleDestroyed;
        }

        private void OnDestroy()
        {
            Damageable damageable = GetComponent<Damageable>();
            if (damageable != null) damageable.Died -= HandleDestroyed;
        }

        public void Configure(EnemyType enemyType)
        {
            type = enemyType;
            moveSpeed = enemyType switch
            {
                EnemyType.Skitter => 2.3f,
                EnemyType.Spire => 1.1f,
                EnemyType.Orbital => 1.9f,
                EnemyType.Apex => 1.45f,
                EnemyType.SecurityBot => 2.0f,
                EnemyType.SentryTurret => 0f, // stationary
                _ => 1.5f
            };
            preferredDistance = enemyType switch
            {
                EnemyType.Skitter => 0.55f,
                EnemyType.Spire => 3.6f,
                EnemyType.Orbital => 2.4f,
                EnemyType.Apex => 3.1f,
                EnemyType.SecurityBot => 2.8f,
                EnemyType.SentryTurret => 4.5f,
                _ => 2.5f
            };
            orbitAngle = Random.Range(0f, 360f);
        }

        private void Start()
        {
            PlayerShipController playerController = Object.FindAnyObjectByType<PlayerShipController>();
            player = playerController != null ? playerController.transform : null;
            nextShotTime = Time.time + 1.1f;
        }

        private void Update()
        {
            if (player == null) return;

            Vector2 toPlayer = (Vector2)(player.position - transform.position);
            float distance = toPlayer.magnitude;

            switch (type)
            {
                case EnemyType.Orbital:
                    UpdateOrbital(toPlayer, distance);
                    break;
                case EnemyType.SecurityBot:
                    UpdateSecurityBot(toPlayer, distance);
                    break;
                case EnemyType.SentryTurret:
                    UpdateSentryTurret(toPlayer, distance);
                    break;
                case EnemyType.Apex:
                    UpdateApex(toPlayer, distance);
                    break;
                case EnemyType.Spire:
                    UpdateSpire(toPlayer, distance);
                    break;
                default: // Skitter, others
                    UpdateChaser(toPlayer, distance);
                    break;
            }

            if (transform.position.y < -6.5f) Destroy(gameObject);
        }

        private void UpdateChaser(Vector2 toPlayer, float distance)
        {
            if (distance > preferredDistance)
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }

        private void UpdateOrbital(Vector2 toPlayer, float distance)
        {
            orbitAngle += moveSpeed * 45f * Time.deltaTime;
            Vector2 orbitOffset = new Vector2(Mathf.Cos(orbitAngle * Mathf.Deg2Rad), Mathf.Sin(orbitAngle * Mathf.Deg2Rad)) * preferredDistance;
            transform.position = Vector2.Lerp(transform.position, (Vector2)player.position + orbitOffset, 2.5f * Time.deltaTime);
            
            if (distance <= 3.2f && Time.time >= nextShotTime)
            {
                Projectile.CreateEnemyBolt(transform.position, toPlayer.normalized, 9);
                nextShotTime = Time.time + 0.95f;
            }
        }

        private void UpdateSpire(Vector2 toPlayer, float distance)
        {
            if (distance > preferredDistance)
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            if (distance <= 5f && Time.time >= nextShotTime)
            {
                Projectile.CreateEnemyBolt(transform.position, toPlayer.normalized, 10);
                nextShotTime = Time.time + 1.65f;
            }
        }

        private void UpdateApex(Vector2 toPlayer, float distance)
        {
            if (distance > preferredDistance)
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            if (distance <= 6f && Time.time >= nextShotTime)
            {
                FireApexFan(toPlayer.normalized);
                nextShotTime = Time.time + 1.35f;
            }
        }

        private void UpdateSecurityBot(Vector2 toPlayer, float distance)
        {
            // Chase player, fire homing projectiles
            if (distance > preferredDistance)
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            if (distance <= 4.5f && Time.time >= nextShotTime)
            {
                // Homing projectile (TODO: implement homing in Projectile)
                Projectile.CreateEnemyBolt(transform.position, toPlayer.normalized, 11);
                nextShotTime = Time.time + 1.2f;
            }
        }

        private void UpdateSentryTurret(Vector2 toPlayer, float distance)
        {
            // Stationary, sustained fire
            if (distance <= preferredDistance && Time.time >= nextShotTime)
            {
                Projectile.CreateEnemyBolt(transform.position, toPlayer.normalized, 10);
                nextShotTime = Time.time + 0.5f;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable == null || damageable.Faction != CombatFaction.Player) return;

            int damage = type switch
            {
                EnemyType.Skitter => 12,
                EnemyType.Spire => 20,
                EnemyType.Orbital => 16,
                EnemyType.Apex => 30,
                EnemyType.SecurityBot => 14,
                EnemyType.SentryTurret => 18,
                _ => 10
            };

            damageable.ApplyDamage(damage);
            Destroy(gameObject);
        }

        private void HandleDestroyed(Damageable _)
        {
            int xp = type switch
            {
                EnemyType.Skitter => 5,
                EnemyType.Spire => 10,
                EnemyType.Orbital => 8,
                EnemyType.Apex => 40,
                EnemyType.SecurityBot => 12,
                EnemyType.SentryTurret => 15,
                _ => 5
            };

            GalacticRogue.Progression.ExperienceOrb.Create(transform.position, xp);
        }

        private void FireApexFan(Vector2 direction)
        {
            for (int i = -1; i <= 1; i++)
            {
                Vector2 boltDirection = Quaternion.Euler(0f, 0f, i * 18f) * direction;
                Projectile.CreateEnemyBolt(transform.position, boltDirection, 12);
            }
        }
    }
}
