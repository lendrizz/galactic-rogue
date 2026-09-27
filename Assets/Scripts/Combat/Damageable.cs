using System;
using UnityEngine;

namespace GalacticRogue.Combat
{
    public enum CombatFaction { Player, Enemy }

    public sealed class Damageable : MonoBehaviour
    {
        [SerializeField, Min(1)] private int maximumHull = 60;
        [SerializeField] private CombatFaction faction = CombatFaction.Enemy;

        private int currentHull;
        private GalacticRogue.Player.ExperimentalAbility experimentalAbility;

        public CombatFaction Faction => faction;
        public int CurrentHull => currentHull;
        public int MaximumHull => maximumHull;
        public event Action<Damageable> Died;

        private void Awake()
        {
            currentHull = maximumHull;
            if (faction == CombatFaction.Enemy)
                HealthBar.Create(gameObject, this);
            
            // Try to get ExperimentalAbility if player
            if (faction == CombatFaction.Player)
                experimentalAbility = GetComponent<GalacticRogue.Player.ExperimentalAbility>();
        }

        public void Configure(int hull, CombatFaction ownerFaction = CombatFaction.Enemy)
        {
            maximumHull = Mathf.Max(1, hull);
            currentHull = maximumHull;
            faction = ownerFaction;
        }

        public void ApplyDamage(int amount)
        {
            // Block damage if Experimental is shifting
            if (experimentalAbility != null && !experimentalAbility.CanTakeDamage())
                return;

            currentHull -= Mathf.Max(0, amount);
            if (currentHull <= 0)
            {
                Died?.Invoke(this);
                Destroy(gameObject);
            }
        }

        public void Heal(int amount)
        {
            currentHull = Mathf.Min(maximumHull, currentHull + Mathf.Max(0, amount));
        }

        public void IncreaseMaximumHull(int amount)
        {
            maximumHull += Mathf.Max(0, amount);
            currentHull = maximumHull;
        }
    }
}
