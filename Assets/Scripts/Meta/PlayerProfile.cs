using UnityEngine;
using System.Collections.Generic;

namespace GalacticRogue.Meta
{
    /// <summary>Local-only MVP profile. Cloud Save replaces this boundary later without touching combat code.</summary>
    public static class PlayerProfile
    {
        private const string CreditsKey = "galactic_rogue.credits";
        private const string LaserLevelKey = "galactic_rogue.laser_level";
        private const string HullLevelKey = "galactic_rogue.hull_level";
        private const string SelectedShipKey = "galactic_rogue.selected_ship";
        private const string SelectedZoneKey = "galactic_rogue.selected_zone";
        private const string UnlockedShipsKey = "galactic_rogue.unlocked_ships";
        private const string UnlockedZonesKey = "galactic_rogue.unlocked_zones";

        public static int Credits => PlayerPrefs.GetInt(CreditsKey, 0);
        public static int LaserLevel => PlayerPrefs.GetInt(LaserLevelKey, 0);
        public static int HullLevel => PlayerPrefs.GetInt(HullLevelKey, 0);
        
        public static ShipType SelectedShip
        {
            get
            {
                int shipIndex = PlayerPrefs.GetInt(SelectedShipKey, 0);
                return (ShipType)shipIndex;
            }
            set
            {
                PlayerPrefs.SetInt(SelectedShipKey, (int)value);
                PlayerPrefs.Save();
            }
        }

        public static ZoneType SelectedZone
        {
            get
            {
                int zoneIndex = PlayerPrefs.GetInt(SelectedZoneKey, 0);
                return (ZoneType)zoneIndex;
            }
            set
            {
                PlayerPrefs.SetInt(SelectedZoneKey, (int)value);
                PlayerPrefs.Save();
            }
        }

        public static int LaserDamageBonus => LaserLevel * 2;
        public static int HullBonus => HullLevel * 15;

        public static void AddCredits(int amount)
        {
            PlayerPrefs.SetInt(CreditsKey, Credits + Mathf.Max(0, amount));
            PlayerPrefs.Save();
        }

        public static int GetLaserUpgradeCost() => 50 + LaserLevel * 50;
        public static int GetHullUpgradeCost() => 50 + HullLevel * 50;

        public static bool TryBuyLaserUpgrade() => TryBuyUpgrade(LaserLevelKey, GetLaserUpgradeCost());
        public static bool TryBuyHullUpgrade() => TryBuyUpgrade(HullLevelKey, GetHullUpgradeCost());

        private static bool TryBuyUpgrade(string levelKey, int cost)
        {
            if (Credits < cost) return false;

            PlayerPrefs.SetInt(CreditsKey, Credits - cost);
            PlayerPrefs.SetInt(levelKey, PlayerPrefs.GetInt(levelKey, 0) + 1);
            PlayerPrefs.Save();
            return true;
        }

        /// <summary>Check if a ship is unlocked. Interceptor is always unlocked.</summary>
        public static bool IsShipUnlocked(ShipType ship)
        {
            if (ship == ShipType.Interceptor) return true;
            return GetUnlockedShipsList().Contains(ship);
        }

        /// <summary>Unlock a specific ship.</summary>
        public static void UnlockShip(ShipType ship)
        {
            if (ship == ShipType.Interceptor) return;
            var unlockedList = GetUnlockedShipsList();
            if (!unlockedList.Contains(ship))
            {
                unlockedList.Add(ship);
                SaveUnlockedShips(unlockedList);
            }
        }

        /// <summary>Check if a zone is unlocked. AsteroidRun is always unlocked.</summary>
        public static bool IsZoneUnlocked(ZoneType zone)
        {
            if (zone == ZoneType.AsteroidRun) return true;
            return GetUnlockedZonesList().Contains(zone);
        }

        /// <summary>Unlock a specific zone.</summary>
        public static void UnlockZone(ZoneType zone)
        {
            if (zone == ZoneType.AsteroidRun) return;
            var unlockedList = GetUnlockedZonesList();
            if (!unlockedList.Contains(zone))
            {
                unlockedList.Add(zone);
                SaveUnlockedZones(unlockedList);
            }
        }

        /// <summary>Get all unlocked ships (internal use).</summary>
        private static List<ShipType> GetUnlockedShipsList()
        {
            var list = new List<ShipType> { ShipType.Interceptor };
            string saved = PlayerPrefs.GetString(UnlockedShipsKey, string.Empty);
            if (!string.IsNullOrEmpty(saved))
            {
                string[] parts = saved.Split(',');
                foreach (string part in parts)
                {
                    if (int.TryParse(part, out int shipIndex) && shipIndex >= 0 && shipIndex <= 3)
                    {
                        ShipType ship = (ShipType)shipIndex;
                        if (!list.Contains(ship)) list.Add(ship);
                    }
                }
            }
            return list;
        }

        /// <summary>Get all unlocked zones (internal use).</summary>
        private static List<ZoneType> GetUnlockedZonesList()
        {
            var list = new List<ZoneType> { ZoneType.AsteroidRun };
            string saved = PlayerPrefs.GetString(UnlockedZonesKey, string.Empty);
            if (!string.IsNullOrEmpty(saved))
            {
                string[] parts = saved.Split(',');
                foreach (string part in parts)
                {
                    if (int.TryParse(part, out int zoneIndex) && zoneIndex >= 0 && zoneIndex <= 3)
                    {
                        ZoneType zone = (ZoneType)zoneIndex;
                        if (!list.Contains(zone)) list.Add(zone);
                    }
                }
            }
            return list;
        }

        private static void SaveUnlockedShips(List<ShipType> ships)
        {
            List<string> indices = new List<string>();
            foreach (ShipType ship in ships) indices.Add(((int)ship).ToString());
            PlayerPrefs.SetString(UnlockedShipsKey, string.Join(",", indices));
            PlayerPrefs.Save();
        }

        private static void SaveUnlockedZones(List<ZoneType> zones)
        {
            List<string> indices = new List<string>();
            foreach (ZoneType zone in zones) indices.Add(((int)zone).ToString());
            PlayerPrefs.SetString(UnlockedZonesKey, string.Join(",", indices));
            PlayerPrefs.Save();
        }
    }
}
