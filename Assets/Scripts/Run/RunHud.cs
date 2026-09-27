using GalacticRogue.Combat;
using GalacticRogue.Player;
using GalacticRogue.Progression;
using GalacticRogue.Enemies;
using UnityEngine;

namespace GalacticRogue.Run
{
    /// <summary>Combat HUD with pause functionality. Shows stats, timer, pause button, and acquired upgrades.</summary>
    public sealed class RunHud : MonoBehaviour
    {
        private Damageable playerHull;
        private ExperienceController experience;
        private RunManager runManager;
        private Damageable bossHull;
        private UpgradeSelectionController upgradeSelection;
        private bool isPaused;

        private void Start()
        {
            PlayerShipController player = Object.FindAnyObjectByType<PlayerShipController>();
            if (player == null) return;

            playerHull = player.GetComponent<Damageable>();
            experience = player.GetComponent<ExperienceController>();
            upgradeSelection = player.GetComponent<UpgradeSelectionController>();
            runManager = GetComponent<RunManager>();
        }

        private void OnGUI()
        {
            if (playerHull == null || experience == null) return;

            // Only show HUD during active run (not Game Over, not Complete)
            bool isActiveRun = !runManager.IsGameOver && !runManager.IsComplete;

            if (isActiveRun)
            {
                DrawRunHud();
                DrawPauseButton();
            }

            if (isPaused && isActiveRun)
            {
                DrawPauseOverlay();
            }

            // Update boss HUD
            if (bossHull == null)
            {
                BossShip boss = Object.FindAnyObjectByType<BossShip>();
                bossHull = boss != null ? boss.GetComponent<Damageable>() : null;
            }

            if (bossHull != null && isActiveRun)
            {
                DrawBossHud();
            }
        }

        private void DrawRunHud()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = 28;
            labelStyle.fontStyle = FontStyle.Bold;

            GUI.Box(new Rect(36f, 36f, 410f, 196f), GUIContent.none);
            GUI.Label(new Rect(60f, 50f, 360f, 44f), "INTERCEPTOR KESTREL", labelStyle);
            GUI.Label(new Rect(60f, 96f, 360f, 44f), $"HULL  {playerHull.CurrentHull}/{playerHull.MaximumHull}", labelStyle);
            GUI.Label(new Rect(60f, 140f, 360f, 44f), $"LV {experience.Level}   XP {experience.CurrentExperience}/{experience.ExperienceToNextLevel}", labelStyle);
            int wholeSeconds = Mathf.FloorToInt(runManager.ElapsedSeconds);
            GUI.Label(new Rect(60f, 184f, 360f, 44f), $"RUN  {wholeSeconds / 60:00}:{wholeSeconds % 60:00}", labelStyle);
        }

        private void DrawBossHud()
        {
            GUIStyle bossStyle = new GUIStyle(GUI.skin.box);
            bossStyle.fontSize = 28;
            bossStyle.fontStyle = FontStyle.Bold;

            float ratio = (float)bossHull.CurrentHull / bossHull.MaximumHull;
            float width = Mathf.Min(840f, Screen.width - 96f);
            float left = (Screen.width - width) * 0.5f;
            GUI.Box(new Rect(left, 36f, width, 84f), "MAW OF KHARON", bossStyle);
            GUI.Box(new Rect(left + 24f, 86f, (width - 48f) * ratio, 18f), GUIContent.none);
        }

        private void DrawPauseButton()
        {
            GUIStyle pauseButtonStyle = new GUIStyle(GUI.skin.button);
            pauseButtonStyle.fontSize = 16;
            pauseButtonStyle.fontStyle = FontStyle.Bold;

            float buttonWidth = 100f;
            float buttonHeight = 40f;
            float right = Screen.width - 36f - buttonWidth;
            float top = 36f;

            if (GUI.Button(new Rect(right, top, buttonWidth, buttonHeight), "⏸ PAUSE", pauseButtonStyle))
            {
                isPaused = true;
                Time.timeScale = 0f;
            }
        }

        private void DrawPauseOverlay()
        {
            const float overlayWidth = 500f;
            const float maxHeight = 600f;
            float overlayLeft = (Screen.width - overlayWidth) * 0.5f;

            GUIStyle titleStyle = new GUIStyle(GUI.skin.box);
            titleStyle.fontSize = 32;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.alignment = TextAnchor.MiddleCenter;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 18;
            buttonStyle.fontStyle = FontStyle.Bold;

            GUIStyle upgradeLabelStyle = new GUIStyle(GUI.skin.label);
            upgradeLabelStyle.fontSize = 14;
            upgradeLabelStyle.alignment = TextAnchor.MiddleLeft;

            // Calculate overlay height based on number of acquired upgrades
            int upgradeCount = upgradeSelection?.AcquiredRunUpgrades.Count ?? 0;
            float upgradesHeight = Mathf.Min(upgradeCount * 50f + 60f, 300f);
            float totalHeight = 120f + upgradesHeight + 80f; // title + upgrades + buttons
            float overlayTop = (Screen.height - totalHeight) * 0.5f;

            // Semi-transparent background
            GUI.color = new Color(0, 0, 0, 0.5f);
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), GUIContent.none);
            GUI.color = Color.white;

            // Pause panel
            GUI.Box(new Rect(overlayLeft, overlayTop, overlayWidth, totalHeight), "");
            GUI.Label(new Rect(overlayLeft, overlayTop + 20f, overlayWidth, 50f), "PAUSE", titleStyle);

            // Acquired upgrades
            float upgradesTop = overlayTop + 80f;
            GUI.Label(new Rect(overlayLeft + 20f, upgradesTop, overlayWidth - 40f, 30f), "Upgrades in this run:", upgradeLabelStyle);

            if (upgradeSelection?.AcquiredRunUpgrades.Count > 0)
            {
                float upgradeItemTop = upgradesTop + 35f;
                for (int i = 0; i < upgradeSelection.AcquiredRunUpgrades.Count; i++)
                {
                    var upgrade = upgradeSelection.AcquiredRunUpgrades[i];
                    string upgradeText = $"• {upgrade.Name}";
                    GUI.Label(new Rect(overlayLeft + 40f, upgradeItemTop + i * 30f, overlayWidth - 80f, 25f), upgradeText, upgradeLabelStyle);
                }
            }
            else
            {
                GUI.Label(new Rect(overlayLeft + 20f, upgradesTop + 35f, overlayWidth - 40f, 25f), "(None yet)", upgradeLabelStyle);
            }

            // Resume button
            float buttonTop = overlayTop + totalHeight - 50f;
            if (GUI.Button(new Rect(overlayLeft + 50f, buttonTop, overlayWidth - 100f, 40f), "⏯ RESUME", buttonStyle))
            {
                isPaused = false;
                Time.timeScale = 1f;
            }
        }
    }
}
