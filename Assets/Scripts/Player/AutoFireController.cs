using GalacticRogue.Combat;
using GalacticRogue.Meta;
using UnityEngine;

namespace GalacticRogue.Player
{
    /// <summary>
    /// Fires at the nearest registered target. Applies ship-specific fire rate multiplier.
    /// </summary>
    public sealed class AutoFireController : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float baseShotsPerSecond = 4f;
        [SerializeField, Min(1)] private int laserDamage = 12;
        [SerializeField, Min(0.1f)] private float range = 12f;
        [SerializeField, Min(0.1f)] private float projectileSpeed = 18f;
        [SerializeField, Min(1)] private int projectilesPerShot = 1;
        [SerializeField, Min(0)] private int pierceCount;
        
        private PlayerShipController movement;
        private bool firesWhileMoving;
        private bool homingLasers;
        private int chainJumpCount;
        private float shotsPerSecond;

        private float nextShotTime;

        public void AddDamage(int amount) => laserDamage += Mathf.Max(0, amount);
        public void MultiplyFireRate(float multiplier) => shotsPerSecond *= Mathf.Max(1f, multiplier);
        public void AddRange(float amount) => range += Mathf.Max(0f, amount);
        public void MultiplyProjectileSpeed(float multiplier) => projectileSpeed *= Mathf.Max(0.5f, multiplier);
        public void AddProjectile() => projectilesPerShot++;
        public void AddPierce() => pierceCount++;
        public void EnableFireWhileMoving() => firesWhileMoving = true;
        public void EnableHomingLasers() => homingLasers = true;
        public void AddChainJump() => chainJumpCount++;

        private void Awake()
        {
            movement = GetComponent<PlayerShipController>();
            ConfigureShip(ShipDefinition.Get(PlayerProfile.SelectedShip));
        }

        /// <summary>Apply ship's fire rate multiplier to base fire rate.</summary>
        public void ConfigureShip(ShipDefinition ship)
        {
            shotsPerSecond = baseShotsPerSecond * ship.FireRateMultiplier;
        }

        private void Update()
        {
            if (!firesWhileMoving && movement.IsMoving)
                return;

            if (Time.time < nextShotTime)
            {
                return;
            }

            Targetable target = TargetRegistry.FindNearest(transform.position, range);
            if (target == null)
            {
                return;
            }

            FireAt(target.transform.position);
            nextShotTime = Time.time + 1f / shotsPerSecond;
        }

        private void FireAt(Vector3 targetPosition)
        {
            Vector2 direction = (targetPosition - transform.position).normalized;
            float firstAngle = -(projectilesPerShot - 1) * 6f;
            for (int i = 0; i < projectilesPerShot; i++)
            {
                Vector2 shotDirection = Quaternion.Euler(0f, 0f, firstAngle + i * 12f) * direction;
                Projectile.CreatePlayerLaser(transform.position + (Vector3)(shotDirection * 0.45f), shotDirection, laserDamage, pierceCount, homingLasers, chainJumpCount, projectileSpeed);
            }
        }
    }
}
