using UnityEngine;

namespace GalacticRogue.Progression
{
    /// <summary>Owns run XP. Meta-progression deliberately remains outside this MVP system.</summary>
    public sealed class ExperienceController : MonoBehaviour
    {
        [SerializeField, Min(1)] private int experienceToNextLevel = 20;

        private int currentExperience;
        private int level = 1;
        private UpgradeSelectionController upgradeSelection;

        public int Level => level;
        public int CurrentExperience => currentExperience;
        public int ExperienceToNextLevel => experienceToNextLevel;

        private void Awake()
        {
            upgradeSelection = GetComponent<UpgradeSelectionController>();
        }

        public void AddExperience(int amount)
        {
            currentExperience += Mathf.Max(0, amount);
            if (currentExperience < experienceToNextLevel) return;

            currentExperience -= experienceToNextLevel;
            level++;
            experienceToNextLevel = Mathf.CeilToInt(experienceToNextLevel * 1.35f);
            upgradeSelection.Open(level);
        }
    }
}
