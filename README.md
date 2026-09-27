# GALACTIC ROGUE – README

**Ein iOS Space Shooter mit Roguelite-Gameplay in Entwicklung.**

---

## 📱 Projektübersicht

| Eigenschaft | Details |
|---|---|
| **Genre** | 2D Top-Down Space Shooter / Roguelite / Arcade / Action |
| **Plattform** | iOS (iPhone primär, iPad sekundär) |
| **Steuerung** | Touch-basiert, Portrait-Modus |
| **Game Engine** | Unity 6 LTS (6000.6.0f1) |
| **Programmiersprache** | C# |
| **Status** | ✓ MVP spielbar, stabil, unter Entwicklung |
| **Spiellänge** | ~5–15 Minuten pro Run |

---

## 🎮 Spielidee

GALACTIC ROGUE ist ein schnelles, zugängliches Arcade-Game mit Roguelite-Struktur. Der Spieler steuert ein experimentelles Raumschiff per Touch, während es automatisch auf Gegner schießt. Der Fokus liegt auf **Bewegung und Positionierung**, nicht auf manuelles Zielen.

Kernmechaniken:
- **Automatisches Feuer**: Das Schiff schießt auf nächste Gegner
- **Gegnerwellen**: Progressive Eskalation über 5+ Minuten
- **Zufällige Upgrades**: Bei jedem Level-Up wählt der Spieler aus 3 Optionen
- **Build-Synergien**: Upgrades kombinieren sich zu mächtigen Builds
- **Run-basierte Progression**: Jeder Run ist anders, dauerhafte Verbesserungen im Hangar

---

## 📊 Aktueller Entwicklungsstand

**MVP ist implementiert und spielbar:**
- ✓ 1 spielbares Schiff (Interceptor Kestrel)
- ✓ 1 Mission (Asteroidengürtel mit Boss)
- ✓ 5 Gegnertypen + Elite/Boss
- ✓ 15 Upgrades im Pool
- ✓ Hangar mit permanenten Boni
- ✓ Lokal gespeicherte Progression

**Noch nicht implementiert:**
- ❌ Schiff-Variationen (Scout, Destroyer, Experimental)
- ❌ Mehrere Missionen/Zonen
- ❌ Fraktionen-System
- ❌ Canvas-basierte UI (aktuell: Code-UI)
- ❌ Audio (Musik, SFX)
- ❌ Supabase-Backend

**Für Details siehe:**
- [`PROJECT_STATE.md`](PROJECT_STATE.md) – Was ist implementiert? Was nicht?
- [`ROADMAP.md`](ROADMAP.md) – Phasen 1–5, nächste Schritte
- [`AI_RULES.md`](AI_RULES.md) – Verbindliche Regeln für alle Entwickler

---

## 🚀 Schnelleinstieg

### 1. Projekt öffnen

```bash
# Mit Unity Hub
1. "Open Project" → Ordner wählen: /Users/robinlenson/Galactic Rogue
2. Unity 6 LTS öffnen (6000.6.0f1)
3. Warten bis Editor bereit ist
```

### 2. Spiel starten

```
1. Szene öffnen: Assets/Scenes/PrototypeScene.unity
2. Play-Button drücken (oder Strg+P)
3. PrototypeBootstrap erzeugt die Szene automatisch
```

### 3. Spielen

- **Bewegung**: Finger/Maus gedrückt halten → Schiff folgt
- **Angriff**: Automatisch auf nächsten Gegner
- **Level-Up**: Beim Sammeln von XP → 3 Upgrade-Optionen
- **Mission Ende**: Boss besiegt → Credits erhalten → Hangar-Rückkehr
- **Hangar**: Laser/Hull mit Credits verbessern → Nächster Run

**Spieldauer**: ~5 Minuten bis Boss-Sieg, dann Hangar-Menü

---

## 📂 Projektstruktur

```
Assets/
├── Scripts/
│   ├── Bootstrap/         ← Szenen-Initialisierung (auto-spawn)
│   ├── Combat/            ← Schaden, Projektile, Zielsuche
│   ├── Enemies/           ← Gegner-Verhalten (5 Typen + Boss)
│   ├── Hangar/            ← Hangar-UI, Shop-Logik
│   ├── Meta/              ← PlayerProfile (Speichern)
│   ├── Player/            ← Steuerung, Auto-Fire
│   ├── Progression/       ← XP, Level-Ups, Upgrades
│   ├── Run/               ← Missions-Manager, HUD
│   └── Visuals/           ← Grafik-Effekte (Starfield)
├── Prefabs/
│   ├── Player/            ← Schiff-Prefab
│   ├── Enemies/
│   ├── Projectiles/
│   └── UI/
├── Scenes/
│   └── PrototypeScene.unity (aktiv)
├── Sprites/               ← (derzeit runtime-generiert)
├── Audio/                 ← (leer, für später)
└── Settings/              ← Unity Project Settings
```

**Wichtig**: Diese Struktur ist **verbindlich**. Neue Systeme in entsprechende Ordner + Namespaces (`GalacticRogue.[Modul]`).

Siehe: [`ARCHITECTURE.md`](ARCHITECTURE.md) – Detaillierte Abhängigkeiten und Komponenten

---

## 🛠 Technische Architektur (Übersicht)

```
PrototypeBootstrap (Init)
  ├── Camera + Starfield (Visuals)
  ├── PlayerShip + AutoFire + Progression
  │   └── TargetRegistry (Zielsuche)
  ├── EnemySpawner (Gegner-Waves)
  │   ├── EnemyShip × N (Verhalten)
  │   └── BossShip (Phase-Angriffe)
  ├── RunManager (Timer, Belohnungen)
  └── HangarController (UI, Progression)
```

**Daten-Flow**: Gegner → Schaden → Drops XP → Upgrade-UI → neuer Build → Boss

**Wichtige Komponenten**:
- `PlayerShipController` – Drag-Input, Movement
- `AutoFireController` – Auto-Targeting, Schüsse, Upgrades
- `ExperienceController` – XP-Logik
- `UpgradeSelectionController` – Level-Up-UI
- `PlayerProfile` – PlayerPrefs-Speichern (lokal)
- `RunManager` – Missions-Flow
- `TargetRegistry` – Ziel-Datenbank

**Kein Multiplayer, kein Backend (MVP).** Cloud-Save kommt in Phase 4.

Für Details: [`ARCHITECTURE.md`](ARCHITECTURE.md)

---

## 👥 Entwicklung mit mehreren KIs

Dieses Projekt soll **von Claude, ChatGPT und anderen KIs gemeinsam** entwickelt werden. Dafür gibt es verbindliche Regeln:

### Zentrale Dateien

Jede KI **muss** diese Dateien lesen, bevor sie Code schreibt:

1. **[`AI_RULES.md`](AI_RULES.md)** – Verbindliche Regeln, Code-Style, Prozess
2. **[`PROJECT_STATE.md`](PROJECT_STATE.md)** – Aktueller Stand (was ist implementiert?)
3. **[`ROADMAP.md`](ROADMAP.md)** – Phasen 1–5, nächste Schritte
4. **[`TASKS.md`](TASKS.md)** – Aufgaben-Liste, wer macht was?
5. **[`ARCHITECTURE.md`](ARCHITECTURE.md)** – Technische Details, Abhängigkeiten
6. **[`DECISIONS.md`](DECISIONS.md)** – Getroffene Entscheidungen, Begründungen

### Arbeitsweise

```
1. AI_RULES.md lesen (5 min)
   ↓
2. Projektstand prüfen (PROJECT_STATE.md, ROADMAP.md) (10 min)
   ↓
3. Task übernehmen (TASKS.md) (1 min)
   ↓
4. Plan erstellen (Abhängigkeiten, Risiken) (5 min)
   ↓
5. Implementieren + Testen (variabel)
   ↓
6. Abschlussbericht schreiben (siehe AI_RULES.md Sektion 9) (10 min)
   ↓
7. Dokumentation aktualisieren (README, PROJECT_STATE.md, etc.) (5 min)
```

**Wichtig**: Keine KI darf eigenständig Architektur-Entscheidungen treffen oder MVP-Scope ändern.

---

## 🔧 Entwicklungsumgebung

### Anforderungen

- **macOS** (getestet auf MacBook Air M1)
- **Unity 6 LTS** (6000.6.0f1) – [Download](https://unity.com/download)
- **Xcode** 14+ (für später iOS-Build)

### Setup

```bash
# 1. Projekt klonen / öffnen
cd /Users/robinlenson/Galactic\ Rogue

# 2. Unity 6.0.6 in Unity Hub hinzufügen und öffnen
# (Falls nicht vorhanden: Download → Add)
```

### Packages

**Keine externen Pakete im MVP.** Vanilla Unity 2D ist ausreichend:
- Unity Input System (eingebaut)
- 2D Physics (PhysicsCollider2D)
- Sprite Rendering

Falls ein Paket notwendig wird: **Vorher AI_RULES.md Sektion 3.10 lesen + Genehmigung einholen.**

### Build-Ziel (später)

```
Platform: iOS
Min iOS Version: 14.0
Orientation: Portrait (Primary)
Build Size Target: < 50 MB
Performance Target: 60 FPS iPhone 12+
```

Derzeit nur Editor-Testing, kein iOS-Build.

---

## ⚠️ Bekannte Einschränkungen

**MVP-bewusste Einschränkungen** (nicht Bugs):

- **Nur 1 Schiff** – Interceptor Kestrel, keine Variationen
- **Nur 1 Mission** – Asteroidengürtel, keine Zonen-Auswahl
- **Nur 1 Hangar-Upgrade-Pfad** – Nur Laser + Hull, keine Waffen-Slots
- **Keine Audio** – Kein Sound oder Musik implementiert
- **UI ist Prototyp** – OnGUI code-drawn, nicht poliert oder canvas-basiert
- **Keine Fraktionen** – Gegner optisch unterschiedlich, aber kein Impact auf Gameplay
- **Keine Leaderboards** – Lokal-only MVP
- **Keine Cloud-Anbindung** – PlayerPrefs statt Supabase

**Geplante Ergänzungen** (Phase 2–5 im ROADMAP.md):
- ✓ 3 Schiff-Klassen
- ✓ 3+ Zonen
- ✓ 30+ Upgrades
- ✓ Canvas-UI
- ✓ Audio
- ✓ Supabase-Backend

---

## 📖 Einstieg für neue KI-Entwickler

### Schritt 1: Regeln verstehen (15 min)

```
1. Diese README lesen (du bist hier) ✓
2. AI_RULES.md lesen (Prozess, Code-Style, Regeln)
3. PROJECT_STATE.md lesen (aktueller Stand)
4. ROADMAP.md lesen (nächste Schritte)
```

### Schritt 2: Projekt-Struktur erkunden (10 min)

```
1. Unity öffnen
2. Assets/Scripts/ durchblättern
3. PrototypeScene.unity öffnen
4. Spiel einmal durchspielen (5 min)
5. Komponenten im Hierarchy inspizieren
```

### Schritt 3: Task auswählen (1 min)

```
1. TASKS.md öffnen
2. Freie Task auswählen (nicht "In Progress")
3. Status auf "In Progress" setzen
```

### Schritt 4: Plan schreiben (5 min)

```
Bevor du Code schreibst:
- Abhängigkeiten auflisten
- Risiken identifizieren
- Rückfragen stellen (wenn unklar)
```

### Schritt 5: Implementieren (variabel)

```
- AI_RULES.md Code-Style befolgen
- Im Editor testen
- Keine Annahmen, nur getestete Features
```

### Schritt 6: Dokumentation (10 min)

```
- Script-Kommentare aktualisieren
- Abschlussbericht schreiben (Vorlage: AI_RULES.md Sektion 9)
- README.md updaten (falls relevant)
- PROJECT_STATE.md updaten
```

---

## 🎯 Prüfkriterien (Spielbarkeit)

Diese Tests sollten **vor jeden Commit** durchgeführt werden:

- [ ] **Editor-Start**: Szene öffnen → Play → Kein Crash
- [ ] **Bewegung**: Finger/Maus halten → Schiff folgt präzise
- [ ] **Auto-Fire**: Gegner spawnen → Schiff schießt automatisch
- [ ] **Gegner**: Skitter und Spire verhalten sich unterschiedlich
- [ ] **XP**: Gegner-Kills geben XP, Level-Up zeigt 3 Optionen
- [ ] **Upgrades**: Auswahl funktioniert, Bonus wird angewendet
- [ ] **Boss**: Bei ~4:30 spawnt Maw of Kharon
- [ ] **Hangar**: Nach Run-Ende öffnet sich Hangar-UI
- [ ] **Speichern**: Credits sind nach Restart noch da (PlayerPrefs)
- [ ] **Kein Crash**: 10 Minuten spielen ohne Exception

---

## 📞 Kontakt & Fragen

Fragen zu Aufgaben, Anforderungen oder Architektur?

1. **Task-Ambiguität**: Frage in [`TASKS.md`](TASKS.md) dokumentieren
2. **Code-Unsicherheit**: Siehe AI_RULES.md Sektion 7
3. **Neue Feature-Idee**: Vorschlag in [`DECISIONS.md`](DECISIONS.md)

Keine KI sollte raten. **Dokumentieren und fragen.**

---

## 📋 Checkliste für Dokumentations-Updates

Wenn diese README überholt ist:

- [ ] PROJECT_STATE.md existiert und ist aktuell?
- [ ] ROADMAP.md existiert und ist aktuell?
- [ ] ARCHITECTURE.md existiert?
- [ ] DECISIONS.md existiert?
- [ ] TASKS.md existiert und ist organisiert?
- [ ] Alle Links funktionieren?

Falls eine Datei fehlt, siehe nächster Schritt unten.

---

## 🔄 Nächste Schritte

Nach der MVP-Validierung (diese README + AI_RULES.md):

1. **[`PROJECT_STATE.md`](PROJECT_STATE.md) erstellen** – Was ist implementiert? Was nicht?
2. **[`ROADMAP.md`](ROADMAP.md) erstellen** – Phasen 2–5, Timeline, Prioritäten
3. **[`ARCHITECTURE.md`](ARCHITECTURE.md) erstellen** – Technische Tiefe, Abhängigkeits-Diagramme
4. **[`DECISIONS.md`](DECISIONS.md) erstellen** – Getroffene Entscheidungen + Begründung
5. **[`TASKS.md`](TASKS.md) erstellen** – Konkrete Aufgaben für KI-Entwickler

**Diese README soll dann aktualisiert werden**, wenn die oben genannten Dateien existieren.

---

**Versioniert**: September 2026  
**Gültig für**: MVP-Phase (Phase 1)  
**Nächste Review**: Nach Phase 1 Abschluss
