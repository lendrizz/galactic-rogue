using GalacticRogue.Combat;
using GalacticRogue.Meta;
using UnityEngine;

namespace GalacticRogue.Player
{
    /// <summary>Experimental Phantom's unique ability: Temporal Shift.
    /// Brief invulnerability (0.6s) with visual feedback. Cooldown 8s.</summary>
    public sealed class ExperimentalAbility : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float shiftDuration = 0.6f;
        [SerializeField, Min(0.5f)] private float cooldownDuration = 8f;

        private Damageable damageable;
        private float shiftEndTime;
        private float cooldownEndTime;
        private bool isShifting;

        public bool IsShifting => isShifting;
        public float CooldownRemaining => Mathf.Max(0f, cooldownEndTime - Time.time);

        private void Awake()
        {
            damageable = GetComponent<Damageable>();
        }

        private void Update()
        {
            if (isShifting && Time.time >= shiftEndTime)
            {
                EndShift();
            }
        }

        /// <summary>Activate Temporal Shift if not on cooldown.</summary>
        public bool TryActivateShift()
        {
            if (Time.time < cooldownEndTime || isShifting)
                return false;

            isShifting = true;
            shiftEndTime = Time.time + shiftDuration;
            cooldownEndTime = Time.time + cooldownDuration;
            return true;
        }

        /// <summary>Called when shift ends. Return to normal state.</summary>
        private void EndShift()
        {
            isShifting = false;
        }

        /// <summary>Damage is blocked during shift. Called by Damageable.</summary>
        public bool CanTakeDamage() => !isShifting;
    }
}
