using GalacticRogue.Combat;
using GalacticRogue.Meta;
using GalacticRogue.Player;
using UnityEngine;

namespace GalacticRogue.Hangar
{
    /// <summary>Hangar with ship and zone selection. Shows 4 ships, 4 zones, and upgrades. Respects unlock status.</summary>
    public sealed class HangarController : MonoBehaviour
    {
        private bool isOpen = true;
        private ShipType selectedShip;
        private ZoneType selectedZone;

        public void OpenHangar()
        {
            isOpen = true;
            selectedShip = PlayerProfile.SelectedShip;
            selectedZone = PlayerProfile.SelectedZone;
            Time.timeScale = 0f;
        }

        private void Start()
        {
            selectedShip = PlayerProfile.SelectedShip;
            selectedZone = PlayerProfile.SelectedZone;
            Time.timeScale = 0f;
        }

        private void OnGUI()
        {
            if (!isOpen) return;

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = 20;
            labelStyle.alignment = TextAnchor.MiddleCenter;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 16;

            GUIStyle selectedButtonStyle = new GUIStyle(buttonStyle);
            selectedButtonStyle.normal.background = Texture2D.whiteTexture;
            selectedButtonStyle.normal.textColor = Color.black;

            GUIStyle lockedButtonStyle = new GUIStyle(buttonStyle);
            lockedButtonStyle.normal.textColor = Color.gray;

            const float panelWidth = 900f;
            const float panelHeight = 1100f;
            float panelLeft = (Screen.width - panelWidth) * 0.5f;
            float panelTop = (Screen.height - panelHeight) * 0.5f;

            GUI.Box(new Rect(panelLeft, panelTop, panelWidth, panelHeight), "GALACTIC ROGUE — HANGAR");

            // Ship section
            float shipTop = panelTop + 30f;
            GUI.Label(new Rect(panelLeft + 60f, shipTop, panelWidth - 120f, 40f), "SCHIFF", new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold });
            
            float shipButtonTop = shipTop + 45f;
            DrawShipButton(panelLeft, shipButtonTop, panelWidth, ShipType.Interceptor, selectedButtonStyle, buttonStyle, lockedButtonStyle);
            DrawShipButton(panelLeft, shipButtonTop + 90f, panelWidth, ShipType.Scout, selectedButtonStyle, buttonStyle, lockedButtonStyle);
            DrawShipButton(panelLeft, shipButtonTop + 180f, panelWidth, ShipType.Destroyer, selectedButtonStyle, buttonStyle, lockedButtonStyle);
            DrawShipButton(panelLeft, shipButtonTop + 270f, panelWidth, ShipType.Experimental, selectedButtonStyle, buttonStyle, lockedButtonStyle);

            // Zone section
            float zoneTop = shipButtonTop + 360f;
            GUI.Label(new Rect(panelLeft + 60f, zoneTop, panelWidth - 120f, 40f), "MISSION", new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold });

            float zoneButtonTop = zoneTop + 45f;
            DrawZoneButton(panelLeft, zoneButtonTop, panelWidth, ZoneType.AsteroidRun, selectedButtonStyle, buttonStyle, lockedButtonStyle);
            DrawZoneButton(panelLeft, zoneButtonTop + 85f, panelWidth, ZoneType.SpaceStation, selectedButtonStyle, buttonStyle, lockedButtonStyle);
            DrawZoneButton(panelLeft, zoneButtonTop + 170f, panelWidth, ZoneType.NebulaMist, selectedButtonStyle, buttonStyle, lockedButtonStyle);
            DrawZoneButton(panelLeft, zoneButtonTop + 255f, panelWidth, ZoneType.AlienRuins, selectedButtonStyle, buttonStyle, lockedButtonStyle);

            // Upgrades section
            float upgradesTop = zoneButtonTop + 340f;
            GUI.Label(new Rect(panelLeft + 60f, upgradesTop, panelWidth - 120f, 40f), "UPGRADES", new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold });
            GUI.Label(new Rect(panelLeft + 60f, upgradesTop + 48f, panelWidth - 120f, 44f), $"CREDITS  {PlayerProfile.Credits}", labelStyle);

            GUI.Label(new Rect(panelLeft + 60f, upgradesTop + 100f, panelWidth - 120f, 32f), $"Laser Mk. {PlayerProfile.LaserLevel}  (+{PlayerProfile.LaserDamageBonus})", new GUIStyle(GUI.skin.label) { fontSize = 14, alignment = TextAnchor.MiddleCenter });
            if (GUI.Button(new Rect(panelLeft + 60f, upgradesTop + 136f, panelWidth - 120f, 44f), $"Laser upgrade — {PlayerProfile.GetLaserUpgradeCost()} Credits", buttonStyle))
                BuyLaserUpgrade();

            GUI.Label(new Rect(panelLeft + 60f, upgradesTop + 188f, panelWidth - 120f, 32f), $"Hull Mk. {PlayerProfile.HullLevel}  (+{PlayerProfile.HullBonus})", new GUIStyle(GUI.skin.label) { fontSize = 14, alignment = TextAnchor.MiddleCenter });
            if (GUI.Button(new Rect(panelLeft + 60f, upgradesTop + 224f, panelWidth - 120f, 44f), $"Hull upgrade — {PlayerProfile.GetHullUpgradeCost()} Credits", buttonStyle))
                BuyHullUpgrade();

            // Launch button
            float launchTop = upgradesTop + 276f;
            if (GUI.Button(new Rect(panelLeft + 60f, launchTop, panelWidth - 120f, 60f), "Mission starten", new GUIStyle(buttonStyle) { fontSize = 18 }))
            {
                PlayerProfile.SelectedShip = selectedShip;
                PlayerProfile.SelectedZone = selectedZone;
                isOpen = false;
                Time.timeScale = 1f;
            }
        }

        private void DrawShipButton(float panelLeft, float top, float panelWidth, ShipType shipType, GUIStyle selectedStyle, GUIStyle buttonStyle, GUIStyle lockedStyle)
        {
            ShipDefinition ship = ShipDefinition.Get(shipType);
            bool isUnlocked = PlayerProfile.IsShipUnlocked(shipType);
            bool isSelected = selectedShip == shipType;
            
            GUIStyle style;
            if (!isUnlocked)
                style = lockedStyle;
            else if (isSelected)
                style = selectedStyle;
            else
                style = buttonStyle;

            GUI.Label(new Rect(panelLeft + 60f, top, panelWidth - 120f, 30f), ship.DisplayName, new GUIStyle(GUI.skin.label) { fontSize = 16, alignment = TextAnchor.MiddleCenter });
            
            string descriptionText = isUnlocked ? ship.Description : "[LOCKED]";
            GUI.Label(new Rect(panelLeft + 60f, top + 32f, panelWidth - 120f, 24f), descriptionText, new GUIStyle(GUI.skin.label) { fontSize = 12, alignment = TextAnchor.MiddleCenter });

            string buttonText = isSelected ? "✓ SELECTED" : (isUnlocked ? "Select" : "🔒 LOCKED");
            GUI.enabled = isUnlocked;
            if (GUI.Button(new Rect(panelLeft + 60f, top + 58f, panelWidth - 120f, 28f), buttonText, style))
            {
                if (isUnlocked) selectedShip = shipType;
            }
            GUI.enabled = true;
        }

        private void DrawZoneButton(float panelLeft, float top, float panelWidth, ZoneType zoneType, GUIStyle selectedStyle, GUIStyle buttonStyle, GUIStyle lockedStyle)
        {
            ZoneDefinition zone = ZoneDefinition.Get(zoneType);
            bool isUnlocked = PlayerProfile.IsZoneUnlocked(zoneType);
            bool isSelected = selectedZone == zoneType;
            
            GUIStyle style;
            if (!isUnlocked)
                style = lockedStyle;
            else if (isSelected)
                style = selectedStyle;
            else
                style = buttonStyle;

            GUI.Label(new Rect(panelLeft + 60f, top, panelWidth - 120f, 28f), zone.DisplayName, new GUIStyle(GUI.skin.label) { fontSize = 15, alignment = TextAnchor.MiddleCenter });
            
            string descriptionText = isUnlocked ? zone.Description : "[LOCKED]";
            GUI.Label(new Rect(panelLeft + 60f, top + 30f, panelWidth - 120f, 20f), descriptionText, new GUIStyle(GUI.skin.label) { fontSize = 11, alignment = TextAnchor.MiddleCenter });

            string buttonText = isSelected ? "✓ SELECTED" : (isUnlocked ? "Select" : "🔒 LOCKED");
            GUI.enabled = isUnlocked;
            if (GUI.Button(new Rect(panelLeft + 60f, top + 52f, panelWidth - 120f, 28f), buttonText, style))
            {
                if (isUnlocked) selectedZone = zoneType;
            }
            GUI.enabled = true;
        }

        private static void BuyLaserUpgrade()
        {
            if (!PlayerProfile.TryBuyLaserUpgrade()) return;
            var autoFire = Object.FindAnyObjectByType<AutoFireController>();
            if (autoFire != null)
                autoFire.AddDamage(2);
        }

        private static void BuyHullUpgrade()
        {
            if (!PlayerProfile.TryBuyHullUpgrade()) return;
            var damageable = Object.FindAnyObjectByType<PlayerShipController>()?.GetComponent<Damageable>();
            if (damageable != null)
                damageable.IncreaseMaximumHull(15);
        }
    }
}
