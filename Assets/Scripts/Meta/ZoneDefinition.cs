using UnityEngine;

namespace GalacticRogue.Meta
{
    /// <summary>Zone types available in GALACTIC ROGUE.</summary>
    public enum ZoneType
    {
        AsteroidRun,
        SpaceStation,
        NebulaMist,
        AlienRuins
    }

    /// <summary>Immutable zone configuration. Defines enemies, waves, and boss.</summary>
    public readonly struct ZoneDefinition
    {
        public readonly ZoneType Type;
        public readonly string DisplayName;
        public readonly string Description;
        public readonly int BaseRunDurationSeconds;
        public readonly float DifficultyMultiplier;

        private ZoneDefinition(
            ZoneType type,
            string displayName,
            string description,
            int baseRunDuration,
            float difficultyMult)
        {
            Type = type;
            DisplayName = displayName;
            Description = description;
            BaseRunDurationSeconds = Mathf.Max(120, baseRunDuration);
            DifficultyMultiplier = Mathf.Max(0.5f, difficultyMult);
        }

        /// <summary>Get definition by zone type.</summary>
        public static ZoneDefinition Get(ZoneType type) => type switch
        {
            ZoneType.AsteroidRun => new ZoneDefinition(
                ZoneType.AsteroidRun,
                displayName: "ASTEROID RUN",
                description: "Klassische Mission · Maw of Kharon",
                baseRunDuration: 300,
                difficultyMult: 1.0f),

            ZoneType.SpaceStation => new ZoneDefinition(
                ZoneType.SpaceStation,
                displayName: "SPACE STATION",
                description: "Außenstation · Technische Gegner",
                baseRunDuration: 300,
                difficultyMult: 1.15f),

            ZoneType.NebulaMist => new ZoneDefinition(
                ZoneType.NebulaMist,
                displayName: "NEBULA MIST",
                description: "Stealth-Zone · Versteckte Gegner",
                baseRunDuration: 300,
                difficultyMult: 1.1f),

            ZoneType.AlienRuins => new ZoneDefinition(
                ZoneType.AlienRuins,
                displayName: "ALIEN RUINS",
                description: "Antike Struktur · Komplexer Boss",
                baseRunDuration: 300,
                difficultyMult: 1.25f),

            _ => Get(ZoneType.AsteroidRun)
        };
    }
}
