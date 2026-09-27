using GalacticRogue.Combat;
using GalacticRogue.Player;
using GalacticRogue.Progression;
using GalacticRogue.Run;
using UnityEngine;

namespace GalacticRogue.Enemies
{
    /// <summary>First boss prototype. Each health band introduces a new, fully telegraphed bullet pattern.</summary>
    public sealed class BossShip : MonoBehaviour
    {
        private Transform player;
        private Damageable hull;
        private float nextAttackTime;

        private void Awake()
        {
            hull = GetComponent<Damageable>();
            hull.Died += HandleDestroyed;
        }

        private void Start()
        {
            PlayerShipController playerController = Object.FindAnyObjectByType<PlayerShipController>();
            player = playerController != null ? playerController.transform : null;
            nextAttackTime = Time.time + 1.3f;
        }

        private void OnDestroy()
        {
            if (hull != null) hull.Died -= HandleDestroyed;
        }

        private void Update()
        {
            if (player == null || Time.time < nextAttackTime) return;

            float healthRatio = (float)hull.CurrentHull / hull.MaximumHull;
            Vector2 aim = ((Vector2)player.position - (Vector2)transform.position).normalized;

            if (healthRatio > 0.65f)
            {
                FireFan(aim, 2, 13f);
                nextAttackTime = Time.time + 1.35f;
            }
            else if (healthRatio > 0.30f)
            {
                FireRadial(8);
                nextAttackTime = Time.time + 1.15f;
            }
            else
            {
                FireFan(aim, 3, 12f);
                nextAttackTime = Time.time + 0.85f;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null && damageable.Faction == CombatFaction.Player)
                damageable.ApplyDamage(40);
        }

        private void FireFan(Vector2 direction, int halfCount, float stepDegrees)
        {
            for (int i = -halfCount; i <= halfCount; i++)
            {
                Vector2 boltDirection = Quaternion.Euler(0f, 0f, i * stepDegrees) * direction;
                Projectile.CreateEnemyBolt(transform.position, boltDirection, 12);
            }
        }

        private void FireRadial(int count)
        {
            for (int i = 0; i < count; i++)
            {
                float angle = i * 360f / count;
                Vector2 direction = Quaternion.Euler(0f, 0f, angle) * Vector2.up;
                Projectile.CreateEnemyBolt(transform.position, direction, 12);
            }
        }

        private void HandleDestroyed(Damageable _)
        {
            ExperienceOrb.Create(transform.position, 150);
            Object.FindAnyObjectByType<RunManager>().CompleteRun();
        }
    }
}
