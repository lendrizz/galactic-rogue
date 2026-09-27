using System;
using UnityEngine;

namespace GalacticRogue.Progression
{
    /// <summary>Upgrade categories for filtering and synergies.</summary>
    public enum UpgradeCategory
    {
        Damage,
        Firepower,
        Movement,
        Defense,
        Special,
        Utility
    }

    /// <summary>Immutable upgrade definition. Contains name, description, and apply action.</summary>
    public readonly struct UpgradeDefinition
    {
        public readonly string Name;
        public readonly string Description;
        public readonly UpgradeCategory Category;
        public readonly bool IsUnique;
        public readonly Action<GalacticRogue.Player.AutoFireController, GalacticRogue.Player.PlayerShipController, GalacticRogue.Combat.Damageable> ApplyAction;

        public UpgradeDefinition(
            string name,
            string description,
            UpgradeCategory category,
            Action<GalacticRogue.Player.AutoFireController, GalacticRogue.Player.PlayerShipController, GalacticRogue.Combat.Damageable> applyAction,
            bool isUnique = false)
        {
            Name = name;
            Description = description;
            Category = category;
            ApplyAction = applyAction;
            IsUnique = isUnique;
        }
    }

    /// <summary>Upgrade pool with 30+ upgrades. Organized by category.</summary>
    public static class UpgradePool
    {
        public static UpgradeDefinition[] GetAllUpgrades() => new[]
        {
            // Damage (5)
            new UpgradeDefinition(
                "Fokussierte Linsen", "+3 Laserschaden",
                UpgradeCategory.Damage,
                (af, _, __) => af.AddDamage(3)),

            new UpgradeDefinition(
                "Prismatische Linse", "+2 Laserschaden",
                UpgradeCategory.Damage,
                (af, _, __) => af.AddDamage(2)),

            new UpgradeDefinition(
                "Hochleistungs-Emitter", "+4 Laserschaden",
                UpgradeCategory.Damage,
                (af, _, __) => af.AddDamage(4),
                isUnique: true),

            new UpgradeDefinition(
                "Plasma-Kern", "+5 Laserschaden",
                UpgradeCategory.Damage,
                (af, _, __) => af.AddDamage(5),
                isUnique: true),

            new UpgradeDefinition(
                "Energievervielfachung", "+1 Laserschaden pro Upgrade",
                UpgradeCategory.Damage,
                (af, _, __) => af.AddDamage(1)),

            // Firepower (8)
            new UpgradeDefinition(
                "Overclock", "+20% Feuerrate",
                UpgradeCategory.Firepower,
                (af, _, __) => af.MultiplyFireRate(1.2f)),

            new UpgradeDefinition(
                "Split Beam", "+1 Laserprojektil",
                UpgradeCategory.Firepower,
                (af, _, __) => af.AddProjectile()),

            new UpgradeDefinition(
                "Durchschlag", "Laser trifft +1 Gegner",
                UpgradeCategory.Firepower,
                (af, _, __) => af.AddPierce()),

            new UpgradeDefinition(
                "High Velocity", "Laser +25% schneller",
                UpgradeCategory.Firepower,
                (af, _, __) => af.MultiplyProjectileSpeed(1.25f)),

            new UpgradeDefinition(
                "Rapid Barrage", "+35% Feuerrate",
                UpgradeCategory.Firepower,
                (af, _, __) => af.MultiplyFireRate(1.35f)),

            new UpgradeDefinition(
                "Plasma-Beschleuniger", "Laser +40% schneller",
                UpgradeCategory.Firepower,
                (af, _, __) => af.MultiplyProjectileSpeed(1.4f),
                isUnique: true),

            new UpgradeDefinition(
                "Doppel-Emitter", "+2 Laserprojektile",
                UpgradeCategory.Firepower,
                (af, _, __) => { af.AddProjectile(); af.AddProjectile(); },
                isUnique: true),

            new UpgradeDefinition(
                "Zielcomputer", "+3 Angriffsreichweite",
                UpgradeCategory.Firepower,
                (af, _, __) => af.AddRange(3f)),

            // Movement (5)
            new UpgradeDefinition(
                "Drift Thrusters", "+15% Bewegung",
                UpgradeCategory.Movement,
                (_, pm, __) => pm.MultiplyMoveSpeed(1.15f)),

            new UpgradeDefinition(
                "Boost Modul", "+25% Bewegung",
                UpgradeCategory.Movement,
                (_, pm, __) => pm.MultiplyMoveSpeed(1.25f),
                isUnique: true),

            new UpgradeDefinition(
                "Überladeantrieb", "+35% Bewegung",
                UpgradeCategory.Movement,
                (_, pm, __) => pm.MultiplyMoveSpeed(1.35f),
                isUnique: true),

            new UpgradeDefinition(
                "Leichte Rüstung", "+10% Bewegung",
                UpgradeCategory.Movement,
                (_, pm, __) => pm.MultiplyMoveSpeed(1.1f)),

            new UpgradeDefinition(
                "Aerodynamische Hull", "+20% Bewegung",
                UpgradeCategory.Movement,
                (_, pm, __) => pm.MultiplyMoveSpeed(1.2f)),

            // Defense (6)
            new UpgradeDefinition(
                "Nanoreparatur", "Repariert 25 Hull",
                UpgradeCategory.Defense,
                (_, __, dm) => dm.Heal(25)),

            new UpgradeDefinition(
                "Verstärkte Schildgenerator", "+30 Max Hull",
                UpgradeCategory.Defense,
                (_, __, dm) => dm.IncreaseMaximumHull(30)),

            new UpgradeDefinition(
                "Titanium Panzerung", "+50 Max Hull",
                UpgradeCategory.Defense,
                (_, __, dm) => dm.IncreaseMaximumHull(50),
                isUnique: true),

            new UpgradeDefinition(
                "Regenerative Zellen", "+20 Hull Reparatur",
                UpgradeCategory.Defense,
                (_, __, dm) => dm.Heal(20)),

            new UpgradeDefinition(
                "Energiespeicher", "+40 Max Hull",
                UpgradeCategory.Defense,
                (_, __, dm) => dm.IncreaseMaximumHull(40)),

            new UpgradeDefinition(
                "Notfall-Reparatur", "+35 Hull Reparatur",
                UpgradeCategory.Defense,
                (_, __, dm) => dm.Heal(35)),

            // Special (4 Unique-Upgrades)
            new UpgradeDefinition(
                "Kampfprotokoll", "Schießt auch während Bewegung",
                UpgradeCategory.Special,
                (af, _, __) => af.EnableFireWhileMoving(),
                isUnique: true),

            new UpgradeDefinition(
                "Jäger-Leitsystem", "Laser verfolgen Gegner",
                UpgradeCategory.Special,
                (af, _, __) => af.EnableHomingLasers(),
                isUnique: true),

            new UpgradeDefinition(
                "Lichtbogen-Relais", "Laser springt zu 1 weiterem Ziel",
                UpgradeCategory.Special,
                (af, _, __) => af.AddChainJump(),
                isUnique: true),

            new UpgradeDefinition(
                "Seeking Burst", "+30% Feuer + +1 Schuss",
                UpgradeCategory.Special,
                (af, _, __) => { af.MultiplyFireRate(1.3f); af.AddProjectile(); },
                isUnique: true),

            // Utility/Hybrid (2+)
            new UpgradeDefinition(
                "Ausgewogenes Paket", "+10% Schaden, +10% Reichweite",
                UpgradeCategory.Utility,
                (af, _, __) => { af.AddDamage(1); af.AddRange(1f); }),

            new UpgradeDefinition(
                "Offensive Suite", "+2 Schaden, +15% Feuerrate",
                UpgradeCategory.Utility,
                (af, _, __) => { af.AddDamage(2); af.MultiplyFireRate(1.15f); },
                isUnique: true)
        };
    }
}
