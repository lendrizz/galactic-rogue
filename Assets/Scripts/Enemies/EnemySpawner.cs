using GalacticRogue.Bootstrap;
using GalacticRogue.Combat;
using GalacticRogue.Meta;
using GalacticRogue.Run;
using UnityEngine;

namespace GalacticRogue.Enemies
{
    /// <summary>Zone-aware enemy spawner. Waves and enemies depend on current zone.</summary>
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField, Min(0.25f)] private float baseSpawnInterval = 1.1f;
        
        private float nextSpawnTime;
        private int spawnCount;
        private GameObject activeElite;
        private bool eliteWasSpawned;
        private GameObject activeBoss;
        private bool bossWasSpawned;
        private RunManager runManager;
        private ZoneType currentZone = ZoneType.AsteroidRun;

        public void SetZone(ZoneType zone)
        {
            currentZone = zone;
        }

        private void Start()
        {
            nextSpawnTime = Time.time + 0.6f;
            runManager = Object.FindAnyObjectByType<RunManager>();
        }

        private void Update()
        {
            float elapsedSeconds = runManager != null ? runManager.ElapsedSeconds : 0f;
            
            // Check for elite spawn (zone-dependent)
            if (!eliteWasSpawned && ShouldSpawnElite(elapsedSeconds))
            {
                activeElite = Spawn(GetEliteType(currentZone));
                eliteWasSpawned = true;
                return;
            }

            // Check for boss spawn (zone-dependent)
            if (eliteWasSpawned && activeElite == null && !bossWasSpawned && ShouldSpawnBoss(elapsedSeconds))
            {
                activeBoss = Spawn(GetBossType(currentZone));
                bossWasSpawned = true;
                return;
            }

            if (activeElite != null || activeBoss != null || bossWasSpawned || Time.time < nextSpawnTime) 
                return;

            float spawnInterval = GetSpawnInterval(elapsedSeconds);
            EnemyType enemyType = GetEnemyType(elapsedSeconds);
            Spawn(enemyType);
            spawnCount++;
            nextSpawnTime = Time.time + spawnInterval;
        }

        private bool ShouldSpawnElite(float elapsedSeconds) => elapsedSeconds >= GetEliteSpawnTime(currentZone);
        private bool ShouldSpawnBoss(float elapsedSeconds) => elapsedSeconds >= GetBossSpawnTime(currentZone);

        private float GetEliteSpawnTime(ZoneType zone) => zone switch
        {
            ZoneType.AsteroidRun => 210f,
            ZoneType.SpaceStation => 200f,
            ZoneType.NebulaMist => 220f,
            ZoneType.AlienRuins => 190f,
            _ => 210f
        };

        private float GetBossSpawnTime(ZoneType zone) => zone switch
        {
            ZoneType.AsteroidRun => 270f,
            ZoneType.SpaceStation => 260f,
            ZoneType.NebulaMist => 280f,
            ZoneType.AlienRuins => 250f,
            _ => 270f
        };

        private EnemyType GetEliteType(ZoneType zone) => zone switch
        {
            ZoneType.AsteroidRun => EnemyType.Apex,
            ZoneType.SpaceStation => EnemyType.SecurityBot,
            ZoneType.NebulaMist => EnemyType.Orbital,
            ZoneType.AlienRuins => EnemyType.Apex,
            _ => EnemyType.Apex
        };

        private EnemyType GetBossType(ZoneType zone) => zone switch
        {
            ZoneType.AsteroidRun => EnemyType.Maw,
            ZoneType.SpaceStation => EnemyType.Maw,
            ZoneType.NebulaMist => EnemyType.Maw,
            ZoneType.AlienRuins => EnemyType.Maw,
            _ => EnemyType.Maw
        };

        private EnemyType GetEnemyType(float elapsedSeconds)
        {
            return currentZone switch
            {
                ZoneType.SpaceStation => GetSpaceStationEnemyType(elapsedSeconds),
                ZoneType.NebulaMist => GetNebulaMistEnemyType(elapsedSeconds),
                ZoneType.AlienRuins => GetAlienRuinsEnemyType(elapsedSeconds),
                _ => GetAsteroidRunEnemyType(elapsedSeconds)
            };
        }

        private EnemyType GetAsteroidRunEnemyType(float elapsedSeconds)
        {
            if (elapsedSeconds < 60f) return EnemyType.Skitter;
            if (elapsedSeconds < 90f) return spawnCount % 3 == 2 ? EnemyType.Spire : EnemyType.Skitter;
            if (elapsedSeconds < 150f) return (spawnCount % 4 == 2) ? EnemyType.Orbital : (spawnCount % 2 == 1 ? EnemyType.Spire : EnemyType.Skitter);
            int choice = spawnCount % 5;
            return choice == 2 ? EnemyType.Orbital : choice == 3 ? EnemyType.Spire : EnemyType.Skitter;
        }

        private EnemyType GetSpaceStationEnemyType(float elapsedSeconds)
        {
            if (elapsedSeconds < 60f) return EnemyType.SecurityBot;
            if (elapsedSeconds < 120f) return spawnCount % 3 == 2 ? EnemyType.SentryTurret : EnemyType.SecurityBot;
            if (elapsedSeconds < 180f) return (spawnCount % 4 == 2) ? EnemyType.Orbital : (spawnCount % 2 == 1 ? EnemyType.SentryTurret : EnemyType.SecurityBot);
            int choice = spawnCount % 5;
            return choice == 2 ? EnemyType.Orbital : choice == 3 ? EnemyType.SentryTurret : EnemyType.SecurityBot;
        }

        private EnemyType GetNebulaMistEnemyType(float elapsedSeconds)
        {
            if (elapsedSeconds < 60f) return EnemyType.Orbital;
            if (elapsedSeconds < 90f) return spawnCount % 2 == 1 ? EnemyType.Spire : EnemyType.Orbital;
            int choice = spawnCount % 4;
            return choice == 0 ? EnemyType.Skitter : choice == 1 ? EnemyType.Orbital : choice == 2 ? EnemyType.Spire : EnemyType.Orbital;
        }

        private EnemyType GetAlienRuinsEnemyType(float elapsedSeconds)
        {
            if (elapsedSeconds < 60f) return spawnCount % 2 == 0 ? EnemyType.Spire : EnemyType.Orbital;
            int choice = spawnCount % 6;
            return choice switch
            {
                0 or 1 => EnemyType.Skitter,
                2 or 3 => EnemyType.Spire,
                4 => EnemyType.Orbital,
                _ => EnemyType.Apex
            };
        }

        private float GetSpawnInterval(float elapsedSeconds)
        {
            float baseInterval = currentZone switch
            {
                ZoneType.SpaceStation => 0.95f,
                ZoneType.NebulaMist => 1.2f,
                ZoneType.AlienRuins => 0.8f,
                _ => 1.1f
            };

            if (elapsedSeconds < 60f) return baseInterval;
            if (elapsedSeconds < 150f) return baseInterval * 0.77f;
            return baseInterval * 0.59f;
        }

        private static GameObject Spawn(EnemyType enemyType)
        {
            GameObject enemy = new GameObject(enemyType.ToString());
            Vector3 spawnPos = GetSpawnPosition(enemyType);
            enemy.transform.position = spawnPos;

            SpriteRenderer renderer = enemy.AddComponent<SpriteRenderer>();
            renderer.sprite = PrototypeSpriteFactory.CreateCircleSprite();
            renderer.color = GetEnemyColor(enemyType);
            enemy.transform.localScale = GetEnemyScale(enemyType);

            CircleCollider2D collider = enemy.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            Rigidbody2D rigidbody = enemy.AddComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            rigidbody.gravityScale = 0f;

            Damageable damageable = enemy.AddComponent<Damageable>();
            damageable.Configure(GetEnemyHull(enemyType), CombatFaction.Enemy);
            enemy.AddComponent<Targetable>();

            if (enemyType == EnemyType.Maw)
                enemy.AddComponent<BossShip>();
            else
            {
                EnemyShip controller = enemy.AddComponent<EnemyShip>();
                controller.Configure(enemyType);
            }
            return enemy;
        }

        private static Vector3 GetSpawnPosition(EnemyType type) => type switch
        {
            EnemyType.Maw => new Vector3(Random.Range(-2.2f, 2.2f), 3.5f, 0f),
            EnemyType.Apex => new Vector3(Random.Range(-2.2f, 2.2f), 4.4f, 0f),
            EnemyType.SentryTurret => new Vector3(Random.Range(-2.2f, 2.2f), 5.5f, 0f),
            _ => new Vector3(Random.Range(-2.2f, 2.2f), 6.1f, 0f)
        };

        private static Color GetEnemyColor(EnemyType type) => type switch
        {
            EnemyType.Skitter => new Color(1f, 0.28f, 0.66f),
            EnemyType.Spire => new Color(0.92f, 0.42f, 1f),
            EnemyType.Orbital => new Color(0.35f, 0.8f, 1f),
            EnemyType.Apex => new Color(1f, 0.45f, 0.08f),
            EnemyType.Maw => new Color(1f, 0.16f, 0.12f),
            EnemyType.SecurityBot => new Color(0.2f, 0.8f, 0.2f),
            EnemyType.SentryTurret => new Color(1f, 1f, 0.2f),
            _ => Color.white
        };

        private static Vector3 GetEnemyScale(EnemyType type) => type switch
        {
            EnemyType.Skitter => Vector3.one * 0.38f,
            EnemyType.Spire => Vector3.one * 0.58f,
            EnemyType.Orbital => Vector3.one * 0.5f,
            EnemyType.Apex => Vector3.one * 1.1f,
            EnemyType.Maw => Vector3.one * 1.7f,
            EnemyType.SecurityBot => Vector3.one * 0.6f,
            EnemyType.SentryTurret => Vector3.one * 0.75f,
            _ => Vector3.one * 0.5f
        };

        private static int GetEnemyHull(EnemyType type) => type switch
        {
            EnemyType.Skitter => 24,
            EnemyType.Spire => 38,
            EnemyType.Orbital => 32,
            EnemyType.Apex => 260,
            EnemyType.Maw => 1300,
            EnemyType.SecurityBot => 45,
            EnemyType.SentryTurret => 55,
            _ => 30
        };
    }
}
