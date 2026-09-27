using UnityEngine;

namespace GalacticRogue.Meta
{
    /// <summary>Defines ship types and their stat multipliers.</summary>
    public enum ShipType
    {
        Interceptor,
        Scout,
        Destroyer,
        Experimental
    }

    /// <summary>Immutable ship configuration. Multipliers are applied to base stats.</summary>
    public readonly struct ShipDefinition
    {
        public readonly ShipType Type;
        public readonly float SpeedMultiplier;
        public readonly float HullMultiplier;
        public readonly float FireRateMultiplier;
        public readonly int BaseDamage;
        public readonly string DisplayName;
        public readonly string Description;

        private ShipDefinition(
            ShipType type,
            float speedMult,
            float hullMult,
            float fireRateMult,
            int baseDamage,
            string displayName,
            string description)
        {
            Type = type;
            SpeedMultiplier = Mathf.Max(0.1f, speedMult);
            HullMultiplier = Mathf.Max(0.1f, hullMult);
            FireRateMultiplier = Mathf.Max(0.1f, fireRateMult);
            BaseDamage = Mathf.Max(1, baseDamage);
            DisplayName = displayName;
            Description = description;
        }

        /// <summary>Get definition by ship type.</summary>
        public static ShipDefinition Get(ShipType type) => type switch
        {
            ShipType.Interceptor => new ShipDefinition(
                ShipType.Interceptor,
                speedMult: 1.0f,
                hullMult: 1.0f,
                fireRateMult: 1.0f,
                baseDamage: 12,
                displayName: "INTERCEPTOR KESTREL",
                description: "Ausgewogen · Vielseitiger Allrounder"),

            ShipType.Scout => new ShipDefinition(
                ShipType.Scout,
                speedMult: 1.2f,
                hullMult: 0.8f,
                fireRateMult: 1.15f,
                baseDamage: 12,
                displayName: "SCOUT EAGLE",
                description: "Schnell · Weniger Hull · Hohe Feuerrate"),

            ShipType.Destroyer => new ShipDefinition(
                ShipType.Destroyer,
                speedMult: 0.8f,
                hullMult: 1.3f,
                fireRateMult: 0.85f,
                baseDamage: 15,
                displayName: "DESTROYER LEVIATHAN",
                description: "Langsam · Viel Hull · Starker Schaden"),

            ShipType.Experimental => new ShipDefinition(
                ShipType.Experimental,
                speedMult: 0.9f,
                hullMult: 0.9f,
                fireRateMult: 1.0f,
                baseDamage: 12,
                displayName: "EXPERIMENTAL PHANTOM",
                description: "Balanced · Unique Ability"),

            _ => Get(ShipType.Interceptor)
        };
    }
}
