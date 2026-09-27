# ARCHITECTURE.md – GALACTIC ROGUE

**Technische Referenz für alle Entwickler und KIs.**

Dies ist die verbindliche Dokumentation der **aktuellen** Architektur. Geplante Systeme sind klar gekennzeichnet.

---

## 1. Architekturübersicht

### Status-Legende

| Status | Bedeutung |
|--------|-----------|
| **IMPLEMENTED** | Tatsächlich vorhanden, getestet, funktionsfähig |
| **PARTIAL** | Teilweise implementiert, Erweiterung geplant |
| **PLANNED** | Geplant, noch nicht implementiert |
| **UNKNOWN** | Nicht sicher überprüfbar |

---

### Kern-Systeme (Übersicht)

**IMPLEMENTED (10 Systeme)**:
1. Player (Steuerung, Movement)
2. Combat (Schaden, Projektile, Zielsuche)
3. Enemies (5 Gegnertypen + Boss)
4. Weapons/Auto-Fire (Zielsuche, Schuss)
5. XP & Progression (Level-Ups, Upgrades)
6. Run Management (Missions-Flow)
7. Hangar (Shop, Rückkehr)
8. Save System (PlayerPrefs, lokal)
9. Visuals (Starfield, Feedback)
10. Bootstrap (Runtime-Init)

**PARTIAL (1 System)**:
- Hangar (nur 2 Shop-Items)

**PLANNED (7 Systeme)**:
- Backend (Supabase)
- Fraktionen
- Mehrere Schiffe
- Mehrere Missionen
- Audio
- Canvas-UI
- Ressourcen-System

---

## 2. Script-Verantwortlichkeiten (Kurz-Format)

| Script | Klasse | Verantwortlichkeit | Abhängigkeiten | Status |
|--------|--------|---|---|---|
| **PlayerShipController.cs** | `sealed` | Input, Movement, Boundary-Clamp | Camera | ✓ |
| **AutoFireController.cs** | `sealed` | Auto-Fire, Zielsuche, Waffen-Stats | TargetRegistry, Projectile | ✓ |
| **Damageable.cs** | `sealed` | HP, Destruktion, Healing | HealthBar | ✓ |
| **Projectile.cs** | `sealed` | Bewegung, Collision, Effekte | TargetRegistry, DamageNumber | ✓ |
| **TargetRegistry.cs** | `static` | Ziel-Datenbank für Auto-Fire | Camera | ✓ |
| **DamageNumber.cs** | `sealed` | Floating-Text Feedback | – | ✓ |
| **HealthBar.cs** | `sealed` | Visuelle HP-Anzeige | Damageable | ✓ |
| **EnemyShip.cs** | `sealed` | 5 Gegnertypen-Verhalten | PlayerShipController | ✓ |
| **EnemySpawner.cs** | `sealed` | Wave-Timing, Eskalation | EnemyShip, RunManager | ✓ |
| **BossShip.cs** | `sealed` | Boss-Verhalten, Phases | Projectile, RunManager | ✓ |
| **ExperienceController.cs** | `sealed` | XP-Logik, Level-Up-Trigger | UpgradeSelectionController | ✓ |
| **UpgradeSelectionController.cs** | `sealed` | Upgrade-Pool, UI, Anwendung | AutoFireController, PlayerShipController, Damageable | ✓ |
| **ExperienceOrb.cs** | `sealed` | XP-Pickup, Homing | ExperienceController | ✓ |
| **RunManager.cs** | `sealed` | Missions-Timer, Game-Over, Belohnungen | PlayerProfile, HangarController | ✓ |
| **RunHud.cs** | `sealed` | Missions-HUD (OnGUI) | – | ✓ |
| **HangarController.cs** | `sealed` | Hangar-UI, Shop | PlayerProfile | ⚠ PARTIAL |
| **PlayerProfile.cs** | `static` | Save/Load (PlayerPrefs) | – | ✓ |
| **PrototypeBootstrap.cs** | `static` | Runtime-Szenen-Init | – | ✓ |
| **Starfield.cs** | `sealed` | Sternenhintergrund | – | ✓ |

---

## 3. Kern-Datenflüsse

### 3.1 Gegner → Schaden → XP → Upgrades

```
EnemySpawner spawnt Enemy × N (alle 1-2 sec)
  ↓
EnemyShip.Update() bewegt sich, greift an
  ↓
Projectile.OnTriggerEnter2D(enemy)
  ↓
Damageable.ApplyDamage()
  ↓
Damageable.Died Event
  ↓
ExperienceOrb.Create() + DamageNumber
  ↓
Player pickupt Orb (Homing ab 1,4 dist)
  ↓
ExperienceController.AddExperience()
  ↓
Wenn XP >= Threshold: Level++
  ↓
UpgradeSelectionController.Open() (UI + Pause)
  ↓
Player wählt 1 von 3 Upgrades
  ↓
Upgrade.Apply() (Bonus)
  ↓
Time.timeScale = 1, Run weiter
```

---

### 3.2 Spieler nimmt Schaden

```
Enemy.OnTriggerEnter2D(player)
  ↓
Damageable.ApplyDamage(12-40 dmg)
  ↓
currentHull -= damage
  ↓
Wenn currentHull <= 0:
  ↓
Damageable.Died Event
  ↓
RunManager.HandlePlayerDeath()
  ↓
Time.timeScale = 0 (Pause)
  ↓
Zeige Game-Over UI
  ↓
PlayerProfile.AddCredits(earned)
  ↓
Spieler klickt "Hangar"
  ↓
HangarController.OpenHangar()
```

---

### 3.3 Hangar-Shop

```
PlayerProfile.Credits anzeigen
  ↓
Spieler klickt "Laser verbessern" / "Hull verbessern"
  ↓
PlayerProfile.TryBuyLaserUpgrade() / TryBuyHullUpgrade()
  ↓
Wenn Credits >= Cost:
  ↓
Credits -= Cost
Level++
PlayerProfile.Save() (PlayerPrefs)
  ↓
AutoFireController.AddDamage() / Damageable.IncreaseMaximumHull()
  ↓
Hangar-UI aktualisiert
  ↓
Spieler klickt "Mission starten"
  ↓
HangarController.isOpen = false
Time.timeScale = 1
  ↓
Neue Run startet (EnemySpawner reset)
```

---

## 4. Abhängigkeits-Diagramm

```
┌──────────────────────────────────────────────────────────┐
│ Input System (Unity Input System, External)              │
└─────────────────────┬──────────────────────────────────┘
                      ↓
         ┌────────────────────────────┐
         │ PlayerShipController       │
         │ (Movement, Input Handling) │
         └────────────┬───────────────┘
                      ↓
      ┌───────────────────────────────────────┐
      │ AutoFireController                    │
      │ (Zielsuche, Schuss-Logik, Stats)     │
      └───────────────┬───────────────────┬──┘
                      ↓                   ↓
      ┌──────────────────────┐   ┌──────────────────┐
      │ TargetRegistry       │   │ Projectile       │
      │ (Ziel-Datenbank)     │   │ (Movement, Dmg)  │
      └──────────────────────┘   └────────┬─────────┘
                      ↑                   ↓
      ┌──────────────────────────────────────────┐
      │ EnemyShip / BossShip (has Targetable)    │
      │ (Gegner-Verhalten)                       │
      └──────────────┬──────────────────┬────────┘
                     ↓                  ↓
      ┌──────────────────────┐ ┌──────────────────┐
      │ Damageable           │ │ ExperienceOrb    │
      │ (HP-System, Events)  │ │ (XP-Drops)       │
      └────────┬─────────────┘ └────────┬─────────┘
               ↓                        ↓
      ┌────────────────────────────────────────┐
      │ ExperienceController (XP-Logik)        │
      │        ↓                               │
      │ UpgradeSelectionController (UI)        │
      │        ↓                               │
      │ (Anwendung von Bonusses)               │
      └────────────┬──────────────────────────┘
                   ↓
      ┌────────────────────────────────────────┐
      │ RunManager (Missions-Flow)              │
      │ RunHud (HUD Rendering, OnGUI)           │
      │        ↓                               │
      │ HangarController (Shop, Return)        │
      │        ↓                               │
      │ PlayerProfile (Save/Load)              │
      └────────────────────────────────────────┘
```

**Wichtig**: Viele `FindAnyObjectByType<>()` Aufrufe. Für MVP ok, aber später gecacht.

---

## 5. Szenen und Prefabs

### Szenen

| Szene | Status | Inhalt |
|-------|--------|--------|
| **PrototypeScene.unity** | ✓ Aktiv | Leer. PrototypeBootstrap erzeugt alles. |
| **SampleScene.unity** | ⚠ Unused | Unity default, nicht verwendet |

---

### Prefabs

| Pfad | Status | Beschreibung |
|------|--------|---|
| **Player/InterceptorKestrel** | ✓ Loadbar | Schiff-Prefab. Fallback: Code-Erzeuging |
| **Enemies/\*** | ? UNKNOWN | Gegner-Prefabs? (Code erzeugt runtime) |
| **Projectiles/\*** | ? UNKNOWN | Projektil-Prefabs? (Code erzeugt runtime) |
| **UI/\*** | ? UNKNOWN | UI-Prefabs? (OnGUI, nicht Canvas) |

**Status UNKNOWN**: Nur Fallback-Code überprüft, nicht alle Prefabs gelesen.

---

## 6. ScriptableObjects und Datenstrukturen

**Status**: Keine ScriptableObjects implementiert.

**Geplant (Phase 2)**:
- `UpgradeDefinition` SO
- `EnemyWaveDefinition` SO
- `ShipDefinition` SO
- `WeaponDefinition` SO

**Aktuell**: Alle Daten hart kodiert (UpgradeSelectionController, EnemySpawner).

---

## 7. Architektur-Regeln (verbindlich)

### 7.1 Keine unnötige Kopplung
- Verwende Events statt direktem Zugriff (z.B., `Damageable.Died`)
- Wenn möglich: Register-Pattern (z.B., TargetRegistry)

### 7.2 Keine doppelten Systeme
- **1 TargetRegistry** (nicht mehrere Finder-Klassen)
- **1 ExperienceController** (nicht mehrere XP-Quellen)
- **1 PlayerProfile** (nicht mehrere Save-Systeme)
- **1 EnemySpawner** (nicht mehrere Wave-Manager)

### 7.3 Singleton-Nutzung minimieren
- `FindAnyObjectByType()` resultiert **cachen** bei Awake()
- Nicht in jedem Update() aufrufen
- Null-Checks immer

❌ Falsch:
```csharp
private void Update() {
    var player = FindAnyObjectByType<PlayerShipController>();  // Teuer!
}
```

✓ Richtig:
```csharp
private PlayerShipController player;
private void Awake() { player = FindAnyObjectByType<PlayerShipController>(); }
```

### 7.4 Abhängigkeiten dokumentieren
Alle öffentlichen Abhängigkeiten in Script-Kommentaren:
```csharp
/// <summary>
/// Upgrade Manager.
/// 
/// Dependencies:
/// - AutoFireController (apply damage bonus)
/// - PlayerShipController (apply speed bonus)
/// </summary>
```

### 7.5 Keine ungeplanten Architektur-Änderungen
- FindAnyObjectByType → Service-Locator: **Rückfrage zuerst**
- Event-System einführen: **Rückfrage zuerst**
- Komponenten zusammenführen: **Rückfrage zuerst**

---

## 8. Geplante Systeme (Phase 2–5)

### Phase 2: Content Expansion **PLANNED**
- Mehrere Schiffe (Scout, Destroyer, Experimental)
- 10+ Gegnertypen (statt 5)
- 3+ Missionen (statt 1)
- 30+ Upgrades (mit echten Synergien)
- Balancing-Iteration

### Phase 3: Polish **PLANNED**
- Canvas-basierte UI (statt OnGUI)
- Musik & SFX
- Particle-Effekte
- Schiff-Skins

### Phase 4: Backend **PLANNED**
- Supabase-Integration (Auth, Cloud-Save)
- Fraktionen-Gameplay (3 Fraktionen)
- Ressourcen-System (Credits, Iridium, Tech Parts, Void Shards)
- Leaderboards

### Phase 5: Release **PLANNED**
- iOS-Build
- Performance-Audit (60 FPS iPhone 12)
- App-Store-Assets
- Monetarisierung

---

## 9. Offene Architektur-Entscheidungen

| Frage | Optionen | Status |
|-------|----------|--------|
| **Performance unter Last?** | Spatial Partitioning vs Linear Search | Offen, nach MVP |
| **Upgrade-Pool als SO?** | Code (hart kodiert) vs ScriptableObject | Phase 2 |
| **Waffen-Architektur?** | Single Laser vs Weapon-Slots | Phase 2 |
| **Event-System?** | Events vs FindAnyObjectByType | Phase 2 Refactoring |
| **Cloud-Save?** | Supabase vs Firebase vs Custom | Phase 4 Supabase geplant |

---

## 10. Performance-Ziele

**Targets (MVP)**:
- 60 FPS iPhone 12+ mit 50+ Gegnern + Projektilen
- < 200 MB RAM
- Keine GC-Spikes > 50 ms

**Optimierungs-Kandidaten**:
- TargetRegistry.FindNearest() → O(n) Linear Search
- Projectile-Erzeuging → Keine Pooling
- FindAnyObjectByType → Nicht alle gecacht

**Status**: Nicht gemessen. Erst nach MVP-Stabilisierung.

---

## 11. Getestete vs. Ungetestete Systeme

### Getestet (manuell im Editor)
- ✓ Movement + Boundary-Clamp
- ✓ Auto-Fire + Zielsuche
- ✓ Gegner-Waves + Typ-Variation
- ✓ XP + Level-Up + Upgrades
- ✓ Boss-Kampf + 3 Phasen
- ✓ Game-Over + Hangar-Return
- ✓ Save/Load (PlayerPrefs)

### Nicht getestet
- ❌ iOS-Hardware (nur Editor)
- ❌ Extreme Last (1000+ Gegner)
- ❌ Memory-Leaks (Profiler-Audit fehlt)
- ❌ Netzwerk (kein Backend)

---

## Zusammenfassung

### ✓ Implementierte Systeme (10)
1. Player (Steuerung, Movement, Upgrades)
2. Combat (Schaden, Projektile, Zielsuche)
3. Enemies (5 Gegnertypen + Boss)
4. Weapons/Auto-Fire (Zielsuche, Schuss)
5. XP & Progression (Level-Up, Upgrade-Pool)
6. Run Management (Missions-Flow)
7. Hangar (Shop mit 2 Items)
8. Save System (PlayerPrefs, lokal)
9. Visuals (Starfield, Feedback)
10. Bootstrap (Runtime-Init)

### ❌ Fehlende Systeme (7)
1. Backend (Supabase, Cloud-Save)
2. Fraktionen (Gameplay-Auswirkung)
3. Mehrere Schiffe (Scout, Destroyer, Experimental)
4. Mehrere Missionen (Zonen-Auswahl)
5. Audio (Musik, SFX)
6. Canvas-UI (statt OnGUI)
7. Ressourcen-System (mehrere Währungen)

### ⚠ Offene Entscheidungen (5)
1. Performance unter Last (Spatial Partitioning?)
2. Upgrade-Pool (Code vs ScriptableObject?)
3. Waffen-Architektur (Single vs Slots?)
4. Event-System (vollständig einführen?)
5. Cloud-Save (Supabase-Integration?)

---

**Gültig ab**: September 2026  
**Nächste Review**: Nach Phase 1 Abschluss
