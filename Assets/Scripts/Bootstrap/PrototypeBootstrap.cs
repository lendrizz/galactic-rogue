using UnityEngine;
using GalacticRogue.Meta;

namespace GalacticRogue.Bootstrap
{
    public static class PrototypeBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreatePrototypeScene()
        {
            if (Object.FindAnyObjectByType<GalacticRogue.Player.PlayerShipController>() != null)
                return;

            CreateCamera();
            CreateStarfield();
            CreateShip();
            CreateEnemySpawner();
            CreateRunUi();
        }

        private static void CreateCamera()
        {
            if (Camera.main != null) return;
            GameObject camObj = new GameObject("Main Camera");
            Camera cam = camObj.AddComponent<Camera>();
            camObj.tag = "MainCamera";
            cam.orthographic = true;
            cam.orthographicSize = 5f;
            cam.backgroundColor = new Color(0.015f, 0.025f, 0.07f);
            camObj.transform.position = new Vector3(0f, 0f, -10f);
        }

        private static void CreateStarfield()
        {
            new GameObject("Starfield").AddComponent<GalacticRogue.Visuals.Starfield>();
        }

        private static void CreateShip()
        {
            // Get the selected ship type, or default to Interceptor
            ShipType selectedShip = PlayerProfile.SelectedShip;
            ShipDefinition shipDef = ShipDefinition.Get(selectedShip);

            GameObject prefab = Resources.Load<GameObject>("Prefabs/Player/InterceptorKestrel");
            if (prefab != null)
            {
                GameObject ship = Object.Instantiate(prefab, new Vector3(0f, -3.5f, 0f), Quaternion.identity);
                ConfigureSpawnedShip(ship, shipDef);
                return;
            }

            // Fallback to code spawn
            CreateShipCode(shipDef);
        }

        private static void ConfigureSpawnedShip(GameObject ship, ShipDefinition shipDef)
        {
            var autoFire = ship.GetComponent<GalacticRogue.Player.AutoFireController>();
            if (autoFire != null)
            {
                autoFire.ConfigureShip(shipDef);
                autoFire.AddDamage(PlayerProfile.LaserDamageBonus);
            }

            var damageable = ship.GetComponent<GalacticRogue.Combat.Damageable>();
            if (damageable != null)
            {
                int baseHull = 60;
                int shipHull = Mathf.FloorToInt(baseHull * shipDef.HullMultiplier) + PlayerProfile.HullBonus;
                damageable.Configure(shipHull, GalacticRogue.Combat.CombatFaction.Player);
            }
        }

        private static void CreateShipCode(ShipDefinition shipDef)
        {
            GameObject ship = new GameObject(shipDef.Type.ToString());
            ship.transform.position = new Vector3(0f, -3.5f, 0f);
            
            SpriteRenderer renderer = ship.AddComponent<SpriteRenderer>();
            renderer.sprite = PrototypeSpriteFactory.CreateDiamondSprite();
            renderer.color = new Color(0.2f, 0.95f, 1f);
            ship.transform.localScale = new Vector3(0.55f, 0.8f, 1f);
            
            CircleCollider2D collider = ship.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = 0.7f;
            
            GalacticRogue.Combat.Damageable damageable = ship.AddComponent<GalacticRogue.Combat.Damageable>();
            int baseHull = 60;
            int shipHull = Mathf.FloorToInt(baseHull * shipDef.HullMultiplier) + PlayerProfile.HullBonus;
            damageable.Configure(shipHull, GalacticRogue.Combat.CombatFaction.Player);
            
            ship.AddComponent<GalacticRogue.Player.PlayerShipController>();
            
            GalacticRogue.Player.AutoFireController autoFire = ship.AddComponent<GalacticRogue.Player.AutoFireController>();
            autoFire.ConfigureShip(shipDef);
            autoFire.AddDamage(PlayerProfile.LaserDamageBonus);
            
            ship.AddComponent<GalacticRogue.Progression.UpgradeSelectionController>();
            ship.AddComponent<GalacticRogue.Progression.ExperienceController>();
        }

        private static void CreateEnemySpawner()
        {
            new GameObject("Wave Spawner").AddComponent<GalacticRogue.Enemies.EnemySpawner>();
        }

        private static void CreateRunUi()
        {
            GameObject ui = new GameObject("Run UI");
            ui.AddComponent<GalacticRogue.Run.RunManager>();
            ui.AddComponent<GalacticRogue.Run.RunHud>();
            ui.AddComponent<GalacticRogue.Hangar.HangarController>();
        }
    }

    internal static class PrototypeSpriteFactory
    {
        private const int TextureSize = 64;
        private static Sprite circleSprite;
        private static Sprite diamondSprite;

        public static Sprite CreateCircleSprite()
        {
            if (circleSprite != null) return circleSprite;
            Texture2D texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            Color[] pixels = new Color[TextureSize * TextureSize];
            float radius = TextureSize * 0.46f;
            Vector2 center = new Vector2((TextureSize - 1) * 0.5f, (TextureSize - 1) * 0.5f);
            for (int y = 0; y < TextureSize; y++)
                for (int x = 0; x < TextureSize; x++)
                    pixels[y * TextureSize + x] = Vector2.Distance(new Vector2(x, y), center) <= radius ? Color.white : Color.clear;
            texture.SetPixels(pixels);
            texture.Apply();
            circleSprite = Sprite.Create(texture, new Rect(0, 0, TextureSize, TextureSize), new Vector2(0.5f, 0.5f), TextureSize);
            return circleSprite;
        }

        public static Sprite CreateDiamondSprite()
        {
            if (diamondSprite != null) return diamondSprite;
            Texture2D texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            Color[] pixels = new Color[TextureSize * TextureSize];
            float center = (TextureSize - 1) * 0.5f;
            for (int y = 0; y < TextureSize; y++)
                for (int x = 0; x < TextureSize; x++)
                    pixels[y * TextureSize + x] = (Mathf.Abs(x - center) / 20f + Mathf.Abs(y - center) / 31f) <= 1f ? Color.white : Color.clear;
            texture.SetPixels(pixels);
            texture.Apply();
            diamondSprite = Sprite.Create(texture, new Rect(0, 0, TextureSize, TextureSize), new Vector2(0.5f, 0.5f), TextureSize);
            return diamondSprite;
        }
    }
}
