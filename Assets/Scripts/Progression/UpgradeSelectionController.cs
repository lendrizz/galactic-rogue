using System.Collections.Generic;
using GalacticRogue.Combat;
using GalacticRogue.Player;
using UnityEngine;

namespace GalacticRogue.Progression
{
    /// <summary>Upgrade selection UI. Draws 3 random choices, tracks unique upgrades and run-acquired upgrades.</summary>
    public sealed class UpgradeSelectionController : MonoBehaviour
    {
        private const int ChoiceCount = 3;
        private readonly UpgradeChoice[] choices = new UpgradeChoice[ChoiceCount];
        private readonly HashSet<string> acquiredUniqueUpgrades = new HashSet<string>();
        private readonly List<UpgradeDefinition> acquiredRunUpgrades = new List<UpgradeDefinition>();
        private bool isOpen;
        private AutoFireController autoFire;
        private PlayerShipController movement;
        private Damageable hull;
        private UpgradeDefinition[] upgradePool;

        public List<UpgradeDefinition> AcquiredRunUpgrades => acquiredRunUpgrades;

        private void Awake()
        {
            autoFire = GetComponent<AutoFireController>();
            movement = GetComponent<PlayerShipController>();
            hull = GetComponent<Damageable>();
            upgradePool = UpgradePool.GetAllUpgrades();
        }

        public void Open(int level)
        {
            // Select 3 random unique upgrades from pool
            for (int i = 0; i < ChoiceCount; i++)
            {
                UpgradeDefinition upgrade;
                do
                {
                    int index = Random.Range(0, upgradePool.Length);
                    upgrade = upgradePool[index];
                } while (ContainsChoice(upgrade.Name, i) || 
                         (upgrade.IsUnique && acquiredUniqueUpgrades.Contains(upgrade.Name)));

                choices[i] = new UpgradeChoice(upgrade);
            }

            isOpen = true;
            Time.timeScale = 0f;
        }

        private bool ContainsChoice(string name, int count)
        {
            for (int i = 0; i < count; i++)
                if (choices[i].Definition.Name == name) return true;
            return false;
        }

        private void OnGUI()
        {
            if (!isOpen) return;

            GUIStyle titleStyle = new GUIStyle(GUI.skin.box);
            titleStyle.fontSize = 32;
            titleStyle.fontStyle = FontStyle.Bold;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 24;
            buttonStyle.fontStyle = FontStyle.Bold;

            float cardWidth = Mathf.Min(660f, Screen.width * 0.86f);
            float cardHeight = 164f;
            float left = (Screen.width - cardWidth) * 0.5f;
            float top = (Screen.height - (cardHeight * ChoiceCount + 180f)) * 0.5f;
            GUI.Box(new Rect(left - 36f, top - 108f, cardWidth + 72f, cardHeight * ChoiceCount + 144f), "LEVEL UP — Wähle ein Upgrade", titleStyle);

            for (int i = 0; i < ChoiceCount; i++)
            {
                UpgradeChoice choice = choices[i];
                string buttonText = $"{choice.Definition.Name}\n[{choice.Definition.Category}] {choice.Definition.Description}";
                
                if (GUI.Button(new Rect(left, top + i * cardHeight + 40f, cardWidth, cardHeight - 20f), buttonText, buttonStyle))
                {
                    ApplyUpgrade(choice);
                    isOpen = false;
                    Time.timeScale = 1f;
                }
            }
        }

        private void ApplyUpgrade(UpgradeChoice choice)
        {
            choice.Definition.ApplyAction?.Invoke(autoFire, movement, hull);
            acquiredRunUpgrades.Add(choice.Definition);
            
            if (choice.Definition.IsUnique)
                acquiredUniqueUpgrades.Add(choice.Definition.Name);
        }

        private readonly struct UpgradeChoice
        {
            public readonly UpgradeDefinition Definition;

            public UpgradeChoice(UpgradeDefinition definition)
            {
                Definition = definition;
            }
        }
    }
}
