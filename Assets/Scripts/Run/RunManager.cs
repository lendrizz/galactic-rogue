using GalacticRogue.Combat;
using GalacticRogue.Enemies;
using GalacticRogue.Player;
using GalacticRogue.Meta;
using GalacticRogue.Hangar;
using UnityEngine;

namespace GalacticRogue.Run
{
    /// <summary>Manages run state. Configures ship and zone on start. Unlocks next zone on completion. Applies hangar upgrades.</summary>
    public sealed class RunManager : MonoBehaviour
    {
        private bool isGameOver;
        private bool isComplete;
        private int earnedCredits;
        private float elapsedSeconds;

        public bool IsGameOver => isGameOver;
        public bool IsComplete => isComplete;
        public float ElapsedSeconds => elapsedSeconds;

        private void Start()
        {
            PlayerShipController player = Object.FindAnyObjectByType<PlayerShipController>();
            if (player == null) return;

            // Configure ship
            ShipDefinition ship = ShipDefinition.Get(PlayerProfile.SelectedShip);
            player.ConfigureShip(ship);

            var autoFire = player.GetComponent<AutoFireController>();
            if (autoFire != null)
            {
                autoFire.ConfigureShip(ship);
                // Apply hangar laser upgrades
                autoFire.AddDamage(PlayerProfile.LaserDamageBonus);
            }

            var damageable = player.GetComponent<Damageable>();
            if (damageable != null)
            {
                int baseHull = 60;
                int shipHull = Mathf.FloorToInt(baseHull * ship.HullMultiplier) + PlayerProfile.HullBonus;
                damageable.Configure(shipHull, CombatFaction.Player);
            }

            // Initialize Experimental Ability if selected
            if (PlayerProfile.SelectedShip == ShipType.Experimental)
            {
                var experimentalAbility = player.GetComponent<ExperimentalAbility>();
                if (experimentalAbility == null)
                {
                    experimentalAbility = player.gameObject.AddComponent<ExperimentalAbility>();
                }
            }

            // Configure zone
            EnemySpawner spawner = Object.FindAnyObjectByType<EnemySpawner>();
            if (spawner != null)
            {
                spawner.SetZone(PlayerProfile.SelectedZone);
            }

            damageable.Died += HandlePlayerDeath;
        }

        private void Update()
        {
            if (!isGameOver && !isComplete)
                elapsedSeconds += Time.deltaTime;
        }

        private void HandlePlayerDeath(Damageable _)
        {
            isGameOver = true;
            AwardCredits(false);
            Time.timeScale = 0f;
        }

        public void CompleteRun()
        {
            if (isGameOver) return;
            isComplete = true;
            AwardCredits(true);
            UnlockNextZone(PlayerProfile.SelectedZone);
            Time.timeScale = 0f;
        }

        private void OnGUI()
        {
            if (isGameOver)
            {
                DrawEndCard("SCHIFF ZERSTÖRT", "Dein Run endet hier.");
            }
            else if (isComplete)
            {
                DrawEndCard("MISSION ERFÜLLT", "Zone erfolgreich abgeschlossen.");
            }
        }

        private void AwardCredits(bool bossDefeated)
        {
            if (earnedCredits > 0) return;

            earnedCredits = Mathf.FloorToInt(ElapsedSeconds / 60f) * 15;
            if (bossDefeated) earnedCredits += 50;
            PlayerProfile.AddCredits(earnedCredits);
        }

        /// <summary>Unlock the next zone in sequence.</summary>
        private void UnlockNextZone(ZoneType currentZone)
        {
            ZoneType nextZone = currentZone switch
            {
                ZoneType.AsteroidRun => ZoneType.SpaceStation,
                ZoneType.SpaceStation => ZoneType.NebulaMist,
                ZoneType.NebulaMist => ZoneType.AlienRuins,
                _ => ZoneType.AsteroidRun
            };

            PlayerProfile.UnlockZone(nextZone);
        }

        private void DrawEndCard(string title, string message)
        {
            const float width = 300f;
            const float height = 160f;
            float left = (Screen.width - width) * 0.5f;
            float top = (Screen.height - height) * 0.5f;

            GUIStyle titleStyle = new GUIStyle(GUI.skin.box);
            titleStyle.fontSize = 28;
            titleStyle.fontStyle = FontStyle.Bold;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 20;

            GUI.Box(new Rect(left, top, width, height), title, titleStyle);
            GUI.Label(new Rect(left + 25f, top + 42f, width - 50f, 35f), message, CenteredLabel());
            GUI.Label(new Rect(left + 25f, top + 67f, width - 50f, 28f), $"Credits +{earnedCredits}    Gesamt: {PlayerProfile.Credits}", CenteredLabel());
            if (GUI.Button(new Rect(left + 25f, top + 96f, width - 50f, 38f), "Zurück zum Hangar", buttonStyle))
            {
                Time.timeScale = 1f;
                isGameOver = false;
                isComplete = false;
                earnedCredits = 0;
                elapsedSeconds = 0f;
                
                HangarController hangar = Object.FindAnyObjectByType<HangarController>();
                if (hangar != null)
                {
                    hangar.OpenHangar();
                }
            }
        }

        private static GUIStyle CenteredLabel()
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            return style;
        }
    }
}
