# AI_RULES.md – GALACTIC ROGUE

Dieses Dokument ist das verbindliche Regelwerk für alle KI-Entwickler (Claude, ChatGPT, etc.) die am Projekt GALACTIC ROGUE arbeiten.

**Version**: 1.0  
**Stand**: September 2026  
**Autor**: Projektanalyse  

---

## 1. Projektidentität

| Eigenschaft | Wert |
|---|---|
| **Projektname** | GALACTIC ROGUE |
| **Genre** | 2D Top-Down Space Shooter / Roguelite / Arcade / Action |
| **Plattform** | iOS (iPhone primär, iPad sekundär) |
| **Steuerung** | Touch (Portrait-Modus) |
| **Game Engine** | Unity 6 LTS (6000.6.0f1) |
| **Programmiersprache** | C# |
| **Namespace-Convention** | `GalacticRogue.[Modul]` |
| **Aktueller Stand** | **Funktionsfähiger, spielbarer MVP / Vertikaler Slice** |
| **Spiellänge pro Run** | ~5–15 Minuten |
| **Zielgruppe** | Mobile Casual Players, Arcade-Fans, Roguelite-Enthusiasten |

### Status der Systeme

| System | Status | Beschreibung |
|--------|--------|---|
| Touch-Steuerung | ✓ Stabil | Drag-basierte Bewegung, präzise Input-Handling |
| Automatisches Feuer | ✓ Stabil | Zielsuche, Mehrfach-Laser, Durchschlag, Homing, Chain-Jump |
| Gegner-Waves | ✓ Stabil | 5 Gegnertypen, zeitbasierte Eskalation (0–5 min) |
| XP & Level-Up | ✓ Stabil | Upgrade-Pool, 3-Option-Auswahl, Pause-Logik |
| Hangar | ✓ Funktional | 2 Upgrade-Pfade (Laser, Hull), Lokal gespeichert |
| Boss-Kampf | ✓ Funktional | 3-Phasen-Angriffsmuster, Telegraphie |
| Speichern | ✓ Funktional | PlayerPrefs-basiert, lokal nur |
| UI | ⚠ Prototyp | OnGUI code-drawn, nicht Canvas-basiert |
| Audio | ❌ Nicht implementiert | Kein Sound/Musik |
| Visuals | ⚠ Minimal | Runtime-generierte Sprites, keine Asset-Grafiken |

### Aktueller MVP-Umfang

**Implementiert:**
- 1 Schiff (Interceptor Kestrel)
- 1 Mission (Asteroidengürtel)
- 5 Gegnertypen (Skitter, Spire, Orbital, Apex, Maw)
- 15 Upgrades
- Hangar mit permanenten Boni

**Nicht implementiert (geplant):**
- 3 Schiffs-Klassen (Scout, Destroyer, Experimental)
- 2+ Zonen/Missionen
- 3 Fraktionen mit Gameplay-Auswirkung
- 30+ Upgrades gesamt
- Schiffs-Skins / Kosmetika
- Supabase-Backend
- Leaderboards
- Dynamisches Balancing

---

## 2. Game Vision

GALACTIC ROGUE ist ein schnelles, zugängliches und hochgradig motivierendes iOS-Arcade-Game mit Roguelite-Struktur.

### Kern-Gameplay-Loop

1. Spieler öffnet das Spiel → befindet sich im **Hangar**
2. Spieler wählt Schiff → Wählt Ausrüstung → Startet **Mission**
3. Spieler **bewegt** das Schiff per Touch
4. Das Schiff greift **automatisch** an
5. Gegner erscheinen in **Wellen** und werden stärker
6. Gegner lassen XP und Ressourcen fallen
7. Bei **Level-Up** wählt Spieler eines von 3 zufälligen **Upgrades**
8. Jede Run erzeugt einen individuellen **Build**
9. Nach Elite-Gegner und **Boss** → Mission endet
10. Spieler kehrt in den **Hangar** zurück
11. Schiff und Ausrüstung werden mit Ressourcen **verbessert**
12. Nächster Run

### Inspirationsquellen (abstrahiert)

- **Archero**: Auto-Fire, Bewegung/Ausweichen, Zufällige Upgrades, Build-Synergien
- **DarkOrbit**: Galaktische Atmosphäre, Raumschiffe, Weltraum-Setting, Fraktionen

**Wichtig**: Das Spiel orientiert sich an abstrakten Konzepten. Keine Kopie von konkreten Grafiken, Namen, UI oder Inhalten.

### Zielvision (langfristig)

- Hohes Replayability-Potential durch Build-Synergien
- Tiefe Progression durch permanente Schiff-Upgrades
- Eigenständige visuelle Identität (Neon-Sci-Fi)
- Eigenständige Fraktionen und Weltbau
- Balance zwischen Skill und Zufall
- Mobile-first Design für iPhones

---

## 3. Verbindliche technische Regeln

### 3.1 Unity & C# Standards

**Unity-Version**: Keine Änderung ohne Rücksprache. Aktuell: **Unity 6 LTS (6000.6.0f1)**

**C# Standards**:
- Namespace: `GalacticRogue.[Modul]` für alle neuen Dateien
- Sealed classes wenn nicht geerbt
- Explizite public/private modifier
- `[SerializeField]` für Private Inspector-Variablen
- `null`-Prüfungen vor Methodenaufrufen (defensive)
- **Keine Unity API-Calls in structs oder statischen Kontexten ohne Nullcheck**

**Code-Style**:
```csharp
// ✓ Richtig
public sealed class PlayerShipController : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float moveSpeed = 8f;
    
    public bool IsMoving => hasMovementTarget;
    
    private void Awake() { }
}

// ❌ Falsch
public class PlayerShipController : MonoBehaviour
{
    public float moveSpeed = 8f; // Sollte [SerializeField]
}
```

### 3.2 Bestehende Architektur respektieren

**Komponenten-Struktur** (nicht verändern ohne Genehmigung):
- **PlayerShipController** – Input, Bewegung, Boundary-Clamp
- **AutoFireController** – Zielsuche, Schuss-Logik, Upgrade-Mechaniken
- **Damageable** – HP-System, Destruktion
- **TargetRegistry** – Ziel-Datenbank für Auto-Fire
- **ExperienceController** – XP-Sammlung, Level-Up-Trigger
- **UpgradeSelectionController** – Upgrade-UI und -Anwendung
- **RunManager** – Missionslogik, Belohnungen, Game-Over
- **EnemySpawner** – Wave-Eskalation, Gegner-Erzeuging
- **EnemyShip / BossShip** – Gegnerverhalten
- **PlayerProfile** – Persistente Spielerdaten (lokal)

**Abhängigkeiten** (nicht brechen):
- `AutoFireController` → `PlayerShipController`, `TargetRegistry`
- `UpgradeSelectionController` → `AutoFireController`, `PlayerShipController`, `Damageable`
- `RunManager` → `PlayerProfile`, `HangarController`
- `EnemySpawner` → `RunManager`, `PlayerShipController`

Wenn ein System refaktoriert wird, müssen alle abhängigen Systeme getestet werden.

### 3.3 Dateiorganisation

```
Assets/
├── Scripts/
│   ├── Bootstrap/         ← Szenen-Init (keine Gameplay-Logik)
│   ├── Combat/            ← Schaden, Projektile, Ziele
│   ├── Enemies/           ← Gegner-Verhalten, Spawning
│   ├── Hangar/            ← UI und Shop-Logik
│   ├── Meta/              ← Profile, Sicherheit, Meta-Systeme
│   ├── Player/            ← Spieler-Kontroller
│   ├── Progression/       ← XP, Level-Ups, Upgrades
│   ├── Run/               ← Missionslogik, HUD
│   └── Visuals/           ← Grafik-Effekte
├── Prefabs/
│   ├── Player/
│   ├── Enemies/
│   ├── Projectiles/
│   └── UI/
├── Scenes/
├── Sprites/
├── Audio/
└── Settings/
```

**Regel**: Neue Systeme gehören in einen neuen Ordner mit entsprechendem Namespace. Keine Wildwuchs in bestehenden Ordnern.

### 3.4 Namensgebung

**Variablen**:
- `camelCase` für lokale Variablen und Properties
- `PascalCase` für Konstanten und enums
- Präfixe nur für Eingabehandler (`nextShotTime`, `currentHull`)

**Methoden**:
- `PascalCase` für public und protected
- Verben: `Create()`, `Spawn()`, `Apply()`, `Configure()`
- Prädikate: `IsMoving`, `HasTarget`, `CanFire()`

**Dateien**:
- `PascalCase.cs` (z.B., `PlayerShipController.cs`)
- Ein öffentliches Klasse pro Datei (mit Ausnahme von tiny helpers/enums)

### 3.5 Modularität

**Prinzip**: Ein Script = eine Verantwortlichkeit.

❌ **Nicht machen**:
```csharp
// Zu viel in einer Klasse
public class EnemyShip : MonoBehaviour
{
    // Movement
    // Pathfinding
    // Attacks
    // Loot-Drops
    // Visuals
    // Audio
}
```

✓ **Richtig**:
```csharp
public sealed class EnemyShip : MonoBehaviour { /* Bewegung + Angriffslogik */ }
public sealed class LootDropper : MonoBehaviour { /* Drops spawnen */ }
public sealed class EnemyVisuals : MonoBehaviour { /* Animationen, Farben */ }
```

### 3.6 Wiederverwendung

Wenn sich Code zwischen zwei Systemen ähnelt:
1. **Zuerst prüfen**: Gibt es bereits eine Utility-Klasse?
2. **Dann erstellen**: `Assets/Scripts/Utilities/` mit generischem Code
3. **Dann verwenden**: Beide Systeme nutzen denselben Helper

**Keine Copy-Paste von Logik** zwischen Dateien.

### 3.7 Performance

**Für iOS wichtig**:

- **Allocation-Overhead**: Keine `new` in Update(). Pools verwenden.
  ```csharp
  // ❌ Falsch
  private void Update()
  {
      var enemies = new List<Enemy>();  // Jedes Frame!
  }
  
  // ✓ Richtig
  private List<Enemy> enemies = new List<Enemy>();
  private void Update()
  {
      enemies.Clear();  // Wiederverwendung
  }
  ```

- **FindAnyObjectByType ist teuer**: Ergebnisse cachen.
  ```csharp
  private PlayerShipController player;  // Cache
  private void Awake() { player = FindAnyObjectByType<PlayerShipController>(); }
  ```

- **Physics-Queries**: Nutzen Sie `Physics2D.OverlapCircle()` mit Cache, nicht in jedem Frame mehrfach.

- **GC-Pressure reduzieren**: LINQ wo möglich vermeiden, foreach statt LINQ in Hot-Loops.

**Metriken**:
- Target: 60 FPS auf iPhone 12 mit 50+ Gegnern + Projektilen
- Speicher: < 200 MB RAM

### 3.8 Mobile-Optimierung

**Input**:
- Touch-Inputs müssen < 16ms Latenz haben
- Kein Input-Buffering über Frames
- Polling statt Event-basiert für Performance

**UI**:
- OnGUI ist nur für Prototypen. Finale UI muss Canvas sein.
- Screen-Size-aware: Alle HUD-Positionen müssen auf verschiedene iPhones skalieren
- Portrait-Modus ist Primary. Landscape optional, aber darf nicht brechen.

**Visuals**:
- Particle-Effekte sparsam nutzen (< 500 pro Frame)
- Sprite-Atlasing später, jetzt ok Runtime-Generated
- Keine 4K-Texturen

### 3.9 iOS-Kompatibilität

**Target**: iOS 14+

- **Notch & Safe Area**: Wird später relevant
- **Orientierung**: Portrait standard, Landscape optional
- **Device-Variation**: Testen auf iPhone SE, iPhone 12, iPhone 14 Pro

### 3.10 Abhängigkeiten

**Externe Pakete**: Keine ohne Rücksprache. MVP funktioniert mit Vanilla Unity.

**Planned Dependencies**:
- Supabase (Phase 5, Backend)
- Networking: Später für Multiplayer

### 3.11 Sicherheit

**Für MVP (lokal)**:
- PlayerProfile speichert nur in PlayerPrefs (nicht sensitiv in MVP)
- Keine Secrets im Code
- Keine Hardcoded Server-URLs

**Für Backend (später)**:
- Client ist nicht vertrauenswürdig
- Wertvolle Ressourcen werden serverseitig validiert
- RLS-Policies für alle Spieler-Daten
- API-Keys über Umgebungsvariablen, nicht in Code

### 3.12 Datenhaltung

**MVP (aktuell)**:
- `PlayerProfile` → `PlayerPrefs` (lokal)
- Keine Cloud-Anbindung

**Struktur** (keine Änderung ohne Genehmigung):
```csharp
public static class PlayerProfile
{
    public static int Credits { get; }
    public static int LaserLevel { get; }
    public static int HullLevel { get; }
    public static void AddCredits(int amount) { }
    public static bool TryBuyLaserUpgrade() { }
}
```

**Später**: Cloud-Save wird diese Boundary respektieren (Dependency-Injection ohne API-Änderung).

---

## 4. Arbeitsprozess jeder KI

Jede KI muss diese Schritte befolgen. **Abweichungen dokumentieren.**

### 4.1 Vorbereitung (Vor jeder Aufgabe)

1. **AI_RULES.md lesen** (diese Datei)
2. **README.md lesen** (Projekt-Overview)
3. **PROJECT_STATE.md lesen** (aktueller Stand, sofern existent)
4. **ROADMAP.md lesen** (nächste Schritte, sofern existent)
5. **TASKS.md lesen** (definierte Aufgaben, sofern existent)
6. **Relevante Script-Dateien analysieren** (die betroffenen Komponenten)
7. **Abhängigkeiten prüfen** (Welche anderen Systeme sind betroffen?)
8. **Existierende Tests prüfen** (Falls vorhanden)

### 4.2 Plan erstellen

Vor dem Schreiben von Code:

1. **Aufgabe zusammenfassen**: Was ist zu tun?
2. **Abhängigkeiten auflisten**: Welche Systeme sind betroffen?
3. **Änderungen skizzieren**: Welche Dateien, welche Funktionen?
4. **Risiken identifizieren**: Könnte das brechen?
5. **Rückfragen stellen** (Falls unklar)

Beispiel:
```
Task: "Neuen Gegnertyp 'Swarm' implementieren"

Abhängigkeiten:
- EnemySpawner (Spawn-Logik)
- EnemyShip (Verhalten)
- TargetRegistry (Zielsuche muss für Swarms funktionieren)
- ExperienceController (XP-Drops)

Änderungen:
- Enum EnemyType um 'Swarm' erweitern
- EnemyShip.Configure() case Swarm hinzufügen
- EnemySpawner.GetEnemyType() Spawn-Zeitpunkte definieren

Risiken:
- Könnte TargetRegistry overlasten wenn zu viele Swarms?
- Wave-Balance könnte kaputt gehen
```

### 4.3 Implementierung

1. **Nur beauftragte Aufgabe umsetzen**. Keine "Verbesserungen" ohne Rücksprache.
2. **Tests im Editor laufen lassen**. Jede neue Funktion validieren.
3. **Keine bestehenden Systeme brechen**. Vor Commit: Alte Features testen.
4. **Code nach AI_RULES.md Stil formatieren**.
5. **Dokumentation aktualisieren** (README, Inhalte-Kommentare, etc.)

### 4.4 Testing

Für jede neue Feature:

- **Editor-Test**: Play-Button drücken, funktioniert es?
- **Integrations-Test**: Brechen andere Systeme?
- **Extremfall-Test**: Was mit 0 oder 100 Gegnern?
- **Regressions-Test**: Alte Features noch okay?

**Keine Annahmen**: "Das sollte funktionieren" ist nicht ausreichend. Testen.

### 4.5 Dokumentation

Nach bedeutenden Änderungen:

1. **Script-Kommentare aktualisieren** (Summary, Public Methods)
2. **README.md updaten** (Falls neue Features)
3. **PROJECT_STATE.md updaten** (Aktueller Stand)
4. **Abschlussbericht schreiben** (siehe Sektion 9)

---

## 5. Prioritäten

Diese Reihenfolge ist **verbindlich** und darf nicht ohne Genehmigung des Projekt-Leads geändert werden.

### Phase 1: Foundation & Core Loop (MVP)
1. ✓ **Spielbarkeit** – Game läuft, ist nicht crashed
2. ✓ **Stabilität** – Keine Crashes, keine Speicherlecks
3. ✓ **Core Gameplay Loop** – Bewegung, Auto-Fire, Gegner-Waves, XP, Upgrades funktionieren
4. ✓ **Touch-Steuerung** – Input ist präzise und responsive

**Status Phase 1**: ✓ Abgeschlossen

### Phase 2: Content & Balancing
5. **Gegner-Varianz** – 10+ Gegnertypen (statt 5)
6. **Schiffs-Varianz** – 3 Klassen (Scout, Interceptor, Destroyer)
7. **Zonen-Varianz** – 3+ Missionen mit unterschiedlichen Gegnern
8. **Upgrade-Varianz** – 30+ Upgrades mit echter Synergietiefe
9. **Balancing** – Run-Länge, Schwierigkeit, Spawn-Timing konsistent

**Status Phase 2**: ⏳ In Planung

### Phase 3: Polish & UI
10. **Canvas-basierte UI** – statt OnGUI
11. **Visuelle Kohärenz** – Einheitlicher Stil für Schiffe, Gegner, Effekte
12. **Audio** – Musik, SFX, Feedback
13. **Hangar-Überhaul** – Schiff-Auswahl, Skins, Waffen-Management

**Status Phase 3**: ⏳ Nicht gestartet

### Phase 4: Backend & Meta-Progression
14. **Fraktionen-Gameplay** – Fraktionen beeinflussen Gegner/Missionen
15. **Ressourcen-System** – Mehrere Währungen mit klaren Funktionen
16. **Supabase-Integration** – Auth, Cloud-Save, RLS
17. **Leaderboards** – Optional für MVP+

**Status Phase 4**: ⏳ Nicht gestartet

### Phase 5: Release & Monetarisierung
18. **Performance-Audit** – Ziel: 60 FPS auf iPhone 12
19. **Gerätekompatibilität** – Testen auf SE, 12, 14 Pro
20. **App-Store-Assets** – Icons, Screenshots, Description
21. **TestFlight & Review** – Beta-Testing, App-Store-Submission
22. **Monetarisierung** – Kosmetische Items, Battle Pass (optional)

**Status Phase 5**: ⏳ Nicht gestartet

### Regeln für Prioritäten-Änderungen

- **Keine KI darf Prioritäten eigenständig verschieben**
- Wenn eine neue Aufgabe kommt, die nicht in dieser Liste ist: **Rückfrage stellen**
- Wenn eine Aufgabe mehrere Phasen überspringt: **Risk-Assessment schreiben**

---

## 6. Regeln für mehrere KIs

Wenn mehrere KIs am selben Projekt arbeiten, gelten folgende Regeln.

### 6.1 Keine ungeplanten Änderungen

Jede KI darf nur die **explizit zugewiesene Aufgabe** umsetzen.

❌ **Nicht machen**:
- "Ich seh da noch ein Bug, fix ich schnell mit"
- "Ich refaktoriere das mal nebenbei"
- "Die UI könnte besser sein"

✓ **Richtig**:
- Nur zugewiesene Aufgabe umsetzen
- Entdeckte Probleme dokumentieren und melden
- Rückfragen stellen bei Unsicherheit

### 6.2 Keine gleichzeitige Bearbeitung

Wenn zwei KIs **dieselbe Datei** bearbeiten (z.B., `PlayerShipController.cs`):
- **Merge-Konflikte entstehen**
- **Letzte Änderung überschreibt Erste**
- **Tests schlagen fehl**

**Regel**: TASKS.md muss definieren, wer was bearbeitet. **Keine parallele Bearbeitung derselben Datei.**

Wenn das nicht vermeidbar ist:
1. KI A schreibt Abschlussbericht
2. KI B **liest Abschlussbericht** bevor sie startet
3. KI B mergt Änderungen manuell

### 6.3 Keine Löschung fremder Arbeit

❌ **Nicht machen**:
- Dateien löschen (auch "alte" Dateien)
- Scripts entfernen
- Prefabs überwrite
- Szenen überwrite

✓ **Richtig**:
- Bei Konflikten: Rückfrage stellen
- Alte Dateien umbenennen (z.B., `_OLD_PlayerController.cs`)
- Immer vor Git-Push testen, dass nichts kaputt geht

### 6.4 Keine ungeplanten Architektur-Änderungen

**Architektur-Entscheidungen** sind entscheidend. Keine KI darf diese eigenständig ändern.

❌ **Nicht machen**:
- `AutoFireController` in `WeaponSystem` umbenennen
- Abhängigkeiten umstrukturieren
- Event-System einführen statt `FindAnyObjectByType`
- Neue Base-Klassen für bestehende Scripts

✓ **Richtig**:
- Architektur-Vorschlag schreiben
- Auswirkungen erklären
- Rückfrage stellen vor Änderung

### 6.5 Keine Änderung der Unity-Version

**Regel**: Unity-Version bleibt **6000.6.0f1**

❌ **Nicht machen**:
- Unity updaten
- Neue Pakete installieren (ohne Genehmigung)
- Packages.json ändern

Wenn ein Paket notwendig ist:
1. Rückfrage stellen (Warum? Welche Version? Abhängigkeiten?)
2. Warten auf Genehmigung
3. Dokumentieren in TASK-Abschlussbericht

### 6.6 Keine Änderung des MVP-Umfangs

MVP ist bewusst klein:
- 1 Schiff
- 1 Mission
- 5 Gegnertypen
- 15 Upgrades

❌ **Nicht machen**:
- 3 Schiffe implementieren "weil es einfach ist"
- Fraktionen-System bauen "weil Masterprompt sagt so"
- 30 Upgrades createn

✓ **Richtig**:
- MVP bleibt MVP
- Content-Expansion ist Phase 2
- Vorschläge für Phase 2 dokumentieren

### 6.7 Keine Behauptung über nicht getestete Funktionen

❌ **Nicht machen**:
- "Das sollte funktionieren"
- "Ich hab es nicht getestet, aber es sollte ok sein"
- "In meinen Augen das System funktioniert"

✓ **Richtig**:
- "Ich hab das im Editor getestet (Screenshot/Log)"
- "Das System funktioniert in folgenden Szenarien: X, Y, Z"
- "Ich konnte Z nicht testen weil..."

**Abschlussbericht muss ausdrücklich sagen: Was wurde getestet? Was nicht?**

### 6.8 Dokumentationspflicht

Nach **jeder relevanten Änderung**:
1. Script-Kommentare aktualisieren
2. README.md updaten (falls neue Features)
3. Abschlussbericht schreiben
4. TASKS.md → Status setzen zu ✓ Abgeschlossen

**Dokumentation ist Grundbedingung für Abnahme.**

---

## 7. Umgang mit Unsicherheit

Anforderungen sind manchmal unklar oder widersprechen sich. Folge diesem Prozess:

### 7.1 Problem erkennen

**Szenarien**:
- "Die Anforderung sagt X, aber das Masterprompt sagt Y"
- "Ich weiß nicht, wie ich das implementieren soll"
- "Das würde ein anderes System brechen"
- "Die Anforderung ist mehrdeutig"

### 7.2 Nicht raten

❌ **Nicht machen**:
- Eigene Interpretation ohne Rückfrage implementieren
- "Wahrscheinlich gemeint" umsetzen
- Hoffen, dass es richtig ist

✓ **Richtig**:
- Ambiguität dokumentieren
- Mögliche Interpretationen auflisten
- Konkrete Rückfrage stellen

### 7.3 Widerspruch dokumentieren

Beispiel:
```
UNSICHERHEIT:
Anforderung: "Gegner spawnen alle 1 Sekunde"
Konflikt: "Mit 300 DPI Touch-Input + 50 Gegner + Physics = 10 FPS"

Interpretationen:
1. Spawn-Rate reduzieren je nach Performance
2. Gegner-Pool nutzen statt neue Instanzierung
3. "Alle 1 Sekunde" nur bis Gegneranzahl = X

Frage: Welche Interpretation ist gewünscht?
```

### 7.4 Auswirkungen erklären

Niemals nur "Das geht nicht". Erklären warum und mit welchen Folgen.

```
❌ "Das Upgrade kann nicht im Hangar gekauft werden"
✓ "Das Upgrade kann nicht im Hangar gekauft werden, weil:
   - UpgradeSelectionController ist nur in Run aktiv
   - PlayerProfile hat kein Upgrade-Inventar
   - HangarController kennt nur Laser/Hull-Upgrades
   
   Optionen:
   1. Upgrade-Inventar in PlayerProfile hinzufügen (1-2 Stunden)
   2. HangarController erweiterbar machen (3-4 Stunden)
   3. Upgrades nur in Run, nicht im Hangar (0 Stunden)
   
   Welche Lösung ist gewünscht?"
```

### 7.5 Keine kritischen Entscheidungen eigenständig

KIs dürfen **taktische** Entscheidungen treffen (wie Code strukturieren, Performance-Optimierungen), aber **keine strategischen** (Gameplay-Balance, Feature-Scope, Architektur-Umstrukturierung).

**Strategisch** = Rückfrage stellen  
**Taktisch** = Selbst entscheiden

---

## 8. Qualitätsanforderungen

Eine Aufgabe ist nur fertig, wenn **alle** dieser Punkte erfüllt sind.

### 8.1 Code-Qualität

- [ ] **Compiles ohne Fehler** – Keine Warnings ignorieren
- [ ] **AI_RULES.md Stil** – Namensgebung, Formatierung, Namespace
- [ ] **Kein Code-Duplikat** – Keine Copy-Paste-Logik
- [ ] **Null-Sicherheit** – `?.` oder explizite null-Checks
- [ ] **Readonly wo möglich** – `private readonly` für unveränderliche Daten
- [ ] **Kommentare für komplexe Logik** – Warum, nicht Was

### 8.2 Unity-Editor Check

- [ ] **Keine Inspector-Fehler** – Keine fehlenden Referenzen, roten Punkte
- [ ] **Szenen laden** – PrototypeScene öffnet, kein Crash
- [ ] **Play-Mode** – Game läuft, keine Exceptions

### 8.3 Laufzeit-Validierung

- [ ] **Funktionalität getestet** – Mindestens 5 Minuten Play-Test
- [ ] **Edge-Cases geprüft** – Null-Gegner, zu viele Gegner, schnelle Upgrades
- [ ] **Memory-Leak Check** – Profiler 5 min auf, keine steigenden Zahlen
- [ ] **Performance ok** – 60 FPS im Editor auf MacBook Air

### 8.4 Mobile-Steuerung

- [ ] **Touch-Input funktioniert** – Falls Feature Touch betrifft
- [ ] **Kein UI-Overlap** – Buttons erreichbar, lesbar
- [ ] **Portrait-Modus OK** – Standard-Orientation

### 8.5 Regressions-Test

- [ ] **Alte Features noch funktional** – Bewegung, Auto-Fire, etc.
- [ ] **Keine neuen Bugs** – Gegner-Spawning, Upgrades, Speichern
- [ ] **Abhängige Systeme OK** – Wenn Datei betroffen, deren Konsumenten testen

### 8.6 Dokumentation

- [ ] **Script-Kommentare aktualisiert** – Public Methods, nicht-triviale Logik
- [ ] **README.md aktualisiert** – Falls neue Features / Änderungen relevant
- [ ] **Abschlussbericht geschrieben** – Format siehe Sektion 9

**Keine Abnahme ohne Dokumentation.**

---

## 9. Abschlussbericht (Standard-Format)

Nach jeder Aufgabe **muss** ein Abschlussbericht geschrieben werden. Format:

```
# ABSCHLUSSBERICHT – [AUFGABEN-NAME]

## Task-Information
- **Task-ID**: [Falls vorhanden]
- **KI**: [Modell, z.B. Claude Haiku 4.5]
- **Datum**: [Aktuelles Datum]
- **Status**: ✓ Abgeschlossen / ⚠ Mit Einschränkungen / ❌ Nicht abgeschlossen

## Zusammenfassung
[1-2 Sätze: Was wurde getan?]

## Geänderte Dateien
```
- Assets/Scripts/Player/PlayerShipController.cs
- Assets/Scripts/Combat/TargetRegistry.cs
- README.md
```

## Implementierung
[Technische Details: Was wurde wie implementiert?]

### Neue Funktionen
- `Method1()`: [Beschreibung]
- `Method2()`: [Beschreibung]

### Geänderte Funktionen
- `OldMethod()`: [Alte Signatur] → [Neue Signatur] – [Grund]

## Tests
- [x] Editor-Szene lädt
- [x] Play-Mode: 5 Minuten getestet
- [x] Regressions-Test: Auto-Fire, Gegner-Spawning OK
- [ ] Performance-Test: Profiler nicht gemessen (LOW PRIORITY)

## Bekannte Limitationen
- [Falls Feature nicht vollständig, was fehlt?]
- [Keine neuen bekannten Bugs]

## Nächste Schritte
- [Wer sollte das als nächstes machen?]
- [Task-ID für Folgaufgabe, falls relevant]

## Fragen / Ambiguität
- [Falls während Entwicklung Unsicherheiten entstanden sind]
```

**Minimal-Version** (Falls simple Bugfix):
```
# ABSCHLUSSBERICHT – [AUFGABEN-NAME]

- **Status**: ✓ Abgeschlossen
- **Änderungen**: [Datei] – [Was geändert]
- **Tests**: [Kurz getestet im Editor, funktioniert]
```

**Abschlussbericht ist nicht optional. Gehört zu jeder Aufgabe.**

---

## 10. Häufige Fragen

### Was wenn der Masterprompt veraltet ist?
Folge dem aktuellen Projektstand (AI_RULES.md, PROJECT_STATE.md), nicht dem Masterprompt. Der Masterprompt ist die ursprüngliche Vision, aber implementierte Realität siegt.

### Was wenn zwei KIs unterschiedliche Entscheidungen treffen?
**Der Mensch entscheidet.** Dokumentiere beide Optionen mit Vor-/Nachteilen, stelle die Frage.

### Was wenn ein Fehler im MVP auftaucht?
**Sofort dokumentieren.** Nicht "auf später verschieben". Abschlussbericht schreiben, mit "Status: ⚠ Mit Einschränkungen".

### Darf ich neue Scripts hinzufügen?
Ja, wenn es in den MVP-Scope passt und in der TASKS.md definiert ist. Wenn nicht, vorher fragen.

### Was wenn das Projekt mehr Zeit braucht als erwartet?
**Früh kommunizieren.** Abschlussbericht mit "Status: ⚠ Mit Einschränkungen" + detaillierte Erklärung.

---

## Changelog

| Version | Datum | Änderung |
|---------|-------|----------|
| 1.0 | Sept 2026 | Initial |

---

**Gültig ab**: September 2026  
**Letzte Überprüfung**: [Aktualisiert vom nächsten Lead]

Diese Datei ist verbindlich für alle KI-Entwickler.
