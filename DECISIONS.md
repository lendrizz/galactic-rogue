# DECISIONS.md – GALACTIC ROGUE

Dokumentation aller wesentlichen technischen und fachlichen Entscheidungen.

---

## DEC-001 – Unity-Version fixieren auf 6000.6.0f1 (LTS)

- **Status**: ACCEPTED
- **Datum**: 2026-09-18
- **Entscheidung**:
  Unity 6 LTS (6000.6.0f1) als Standard-Zielversion für alle Entwicklung. Fallback: Unity 2022.3 LTS.

- **Begründung**:
  - Langzeitunterstützung (LTS) sichert stabile API über mehrere Jahre
  - 2D-Features in Unity 6 optimiert für Mobile
  - Keine Breaking Changes in Patch-Versionen innerhalb LTS
  - Reduziert Migrations-Risiken für Team-Zusammenarbeit

- **Alternativen**:
  - Unity 2022.3 LTS (älter, aber bekannt stabil)
  - Aktuelle Unity-Version (instabile API, häufige Updates)
  - Unreal Engine (C++, anderes Ökosystem)

- **Auswirkungen**:
  - Alle neuen Features müssen mit 6000.6.0f1 kompatibel sein
  - Keine Verwendung von Experimental-APIs
  - iOS/Android-Build-Ziele gegen diese Version testen

- **Betroffene Dateien/Systeme**:
  - ProjectSettings/ProjectVersion.txt
  - Manifest.json (Package-Versioning)
  - Alle .cs-Scripts (API-Kompatibilität)

---

## DEC-002 – Render Pipeline: Built-in 2D (keine URP/HDRP)

- **Status**: ACCEPTED
- **Datum**: 2026-09-18
- **Entscheidung**:
  Built-in Renderer für 2D-Rendering. Keine Universal Render Pipeline (URP) oder HDRP.

- **Begründung**:
  - Built-in 2D hat beste Kompatibilität mit Mobile
  - Keine zusätzliche Komplexität durch Shader-Graphs
  - Einfacheres Profiling für Performance-Optimierung
  - Runtime-generierte Sprites funktionieren ohne spezielle Konfiguration

- **Alternativen**:
  - URP 2D (zusätzliche Layer, nicht nötig für 2D)
  - HDRP (overkill für Mobile, zu speicherhungrig)

- **Auswirkungen**:
  - Keine URP Post-Processing Effekte
  - Shaders müssen für Built-in geschrieben sein
  - Einfacheres Deployment auf iOS/Android

- **Betroffene Dateien/Systeme**:
  - ProjectSettings/GraphicsSettings.asset
  - Alle Material & Shader Dateien

---

## DEC-003 – Input System: Unity Input System (nicht Legacy Input Manager)

- **Status**: ACCEPTED
- **Datum**: 2026-09-18
- **Entscheidung**:
  Verwende Unity Input System Package. Touch (Mobile) und Maus (Editor) über gleichen Eingabepfad.

- **Begründung**:
  - Input System hat bessere Multi-Touch-Unterstützung
  - Konsistente API für Editor und Device
  - Drag-Steuerung (Position-basiert) einfacher zu implementieren
  - Vorbereitung auf zukünftige Gamepad-Unterstützung

- **Alternativen**:
  - Legacy Input Manager (älter, weniger flexibel)
  - Raw Touch-Input API (zu low-level)

- **Auswirkungen**:
  - InputActionAsset erforderlich
  - Alle Input-Handler müssen Input System verwenden
  - Tests für Touch/Maus wichtig

- **Betroffene Dateien/Systeme**:
  - PlayerShipController.cs (Input-Abfrage)
  - Assets/Input/PlayerInputActions.inputactions (neu oder vorhanden)

---

## DEC-004 – Portrait-Modus als Standard-Orientierung

- **Status**: ACCEPTED
- **Datum**: 2026-09-18
- **Entscheidung**:
  Portrait-Orientierung (Hochformat) als einziger unterstützter Modus. Keine Landscape-Rotation.

- **Begründung**:
  - Mobile Standard (iOS/Android nativer Modus)
  - Drag-Steuerung im Portrait intuitiver
  - Einfacher zu layouten (schmale Viewport)
  - Konsistent mit Space-Shooter-Tradition (senkrechte Achse)

- **Alternativen**:
  - Landscape-Modus (weniger intuitiv für Drag)
  - Responsive Layout (zu komplex für MVP)

- **Auswirkungen**:
  - UILayout muss Portrait-optimiert sein
  - Camera Aspect Ratio fix auf ~9:16
  - Kein Landscape-UI notwendig

- **Betroffene Dateien/Systeme**:
  - ProjectSettings/PlayerSettings.asset (Orientierung)
  - Canvas Layout (wenn Canvas eingeführt)
  - Starfield.cs (Portrait-angepasste Dimensionen)

---

## DEC-005 – UI: OnGUI bis Phase 3 (später Canvas)

- **Status**: ACCEPTED
- **Datum**: 2026-09-18
- **Entscheidung**:
  Verwende OnGUI für MVP-Phase (Phase 1–2). Canvas-basierte UI erst in Phase 3 (Polish).

- **Begründung**:
  - OnGUI schnell zum Prototypisieren
  - Keine Canvas-Overhead in frühen Phasen
  - Erlaubt Fokus auf Gameplay und Content
  - Canvas-Umstellung ist Breaking-Free Task in Phase 3

- **Alternativen**:
  - Canvas von Anfang an (overhead, aber professioneller)
  - TextMesh Pro nur (zu begrenzt)

- **Auswirkungen**:
  - UI-Code in OnGUI() in den Gameplay-Scripts
  - Keine fancy Canvas-Layouts
  - Kein TextMesh Pro (falls OnGUI)
  - Phase 3 muss vollständige UI-Migration beinhalten

- **Betroffene Dateien/Systeme**:
  - RunHud.cs (OnGUI)
  - UpgradeSelectionController.cs (OnGUI)
  - HangarController.cs (OnGUI)

---

## DEC-006 – Save-System: PlayerPrefs (lokal, keine Cloud)

- **Status**: ACCEPTED
- **Datum**: 2026-09-18
- **Entscheidung**:
  PlayerPrefs für lokale Persistierung (Credits, permanente Upgrades). Keine Cloud-Speicherung bis Phase 4.

- **Begründung**:
  - PlayerPrefs ist einfach und zuverlässig
  - Keine Backend-Abhängigkeit in MVP
  - Schnell zum Implementieren
  - Supabase-Integration erst in Phase 4

- **Alternativen**:
  - Supabase sofort (über-engineered für MVP)
  - Lokale JSON-Dateien (mehr Fehlerquellen)
  - Verschlüsselter Speicher (kompliziert, nicht nötig MVP)

- **Auswirkungen**:
  - 3 PlayerPrefs-Keys aktuell: credits, laser_level, hull_level
  - Phase 2 wird mehr Keys hinzufügen (für neue Schiffe)
  - Phase 4 muss PlayerPrefs → Supabase migrieren

- **Betroffene Dateien/Systeme**:
  - PlayerProfile.cs (Lese/Schreib-Logik)
  - HangarController.cs (Credits-Transaktion)

---

## DEC-007 – Asset-Management: Runtime-generierte Sprites (kein Ressourcen-Paket)

- **Status**: ACCEPTED
- **Datum**: 2026-09-18
- **Entscheidung**:
  Sprites zur Laufzeit erzeugen (simple geometrische Formen: Kreis, Quad). Keine vorgefertigten Sprite-Assets bis Phase 3.

- **Begründung**:
  - Zero Asset-Abhängigkeiten in MVP
  - Schnell und flexibel zum Prototypisieren
  - Keine Art-Pipeline notwendig
  - Reduziert Build-Größe für Tests

- **Alternativen**:
  - Vorgefertigte PNG-Sprites (benötigt Grafiker)
  - TextMesh Pro Glyphen (zu begrenzt)

- **Auswirkungen**:
  - Alle Gegner, Projektile sind farbige Primitive
  - Starfield.cs nutzt runtime CreateTexture2D()
  - Phase 3 wird Grafik-Assets einführen und Sprites ersetzen

- **Betroffene Dateien/Systeme**:
  - Starfield.cs (Runtime-Textures)
  - EnemyShip.cs (Gegner-Primitive)
  - Projectile.cs (Projektil-Primitive)
  - PlayerShipController.cs (Schiff-Primitive)

---

## DEC-008 – Keine externen Packages in MVP (nur Unity Standard)

- **Status**: ACCEPTED
- **Datum**: 2026-09-18
- **Entscheidung**:
  MVP verwendet nur Unity-Standard-Packages (Input System, Burst optional). Keine Drittanbieter-Libs (DoTween, Newtonsoft, etc.).

- **Begründung**:
  - Reduziert Abhängigkeiten und Konflikte
  - Einfacherer Build und Deployment
  - Klare Kontrollierbarkeit von Code
  - Phase 2–3 kann gezielt Assets evaluieren

- **Alternativen**:
  - DoTween für Animationen (aber Gameplay prioritär)
  - Newtonsoft JSON (aber PlayerPrefs ausreichend)
  - OdinInspector (nice-to-have, nicht notwendig)

- **Auswirkungen**:
  - Alle Animationen müssen mit Coroutines sein
  - Kein JSON-Parsing nötig (PlayerPrefs reicht)
  - Einfaches manuelles Tweening möglich
  - Phase 2 kann Packages evaluieren und hinzufügen

- **Betroffene Dateien/Systeme**:
  - Manifest.json (nur offizielle Packages)
  - Alle Animations-Scripts (Coroutine-basiert)

---

## Offene Entscheidungen (PROPOSED)

### DEC-OPEN-001 – Spatial Partitioning für TargetRegistry

- **Status**: PROPOSED
- **Priorität**: Nach Performance-Audit (Phase 2–3)
- **Frage**: Soll TargetRegistry.FindNearest() von O(n) auf Quadtree/Grid optimiert werden?

**Pro Quadtree**:
- Skaliert auf 100+ Gegner besser
- Reduziert CPU bei Zielsuche

**Contra**:
- Komplexität für MVP nicht nötig (<20 Gegner)
- Profiler muss zeigen, ob nötig

**Empfehlung**: Nach Profiler-Audit in Phase 2c entscheiden.

---

### DEC-OPEN-002 – Upgrade-Pool als ScriptableObject

- **Status**: PROPOSED
- **Priorität**: Phase 2c
- **Frage**: Sollen Upgrades in einem SO definiert werden statt hardcoded im Code?

**Pro SO**:
- Vereinfacht Balancing und Content
- Designer können Pool ändern ohne Code
- Versionskontrolle einfacher

**Contra**:
- Zusätzliche Komplexität für MVP
- Hardcoding reicht für 15 Upgrades

**Empfehlung**: Phase 2c oder 2d implementieren wenn 30+ Upgrades da sind.

---

### DEC-OPEN-003 – Event-System (Observer Pattern)

- **Status**: PROPOSED
- **Priorität**: Phase 2d (optional)
- **Frage**: Sollen alle System-Interaktionen über Events laufen statt direkte Refs?

**Pro Events**:
- Entkopplung von Systemen
- Leichter testbar
- Skalierbarer Code

**Contra**:
- Overhead für MVP
- Direkte Refs reichen aktuell
- Debugging komplizierter

**Empfehlung**: Phase 2d bei Refactoring evaluieren.

---

### DEC-OPEN-004 – Waffen-Slots & Loadouts

- **Status**: PROPOSED
- **Priorität**: Phase 2b
- **Frage**: Sollen Schiffe mehrere Waffen-Slots haben (z.B., Primary + Secondary)?

**Pro**:
- Strategische Tiefe
- Unique Gameplay pro Schiff
- Upgrade-Vielfalt

**Contra**:
- Komplexität für MVP
- AutoFireController müsste refactored

**Empfehlung**: Phase 2b Design-Decision treffen.

---

### DEC-OPEN-005 – Backend: Supabase vs Alternative

- **Status**: PROPOSED
- **Priorität**: Phase 4 (Backend)
- **Frage**: Welches Backend für Cloud-Save, Leaderboards, Auth?

**Optionen**:
1. Supabase (PostgreSQL + Auth + RLS)
2. Firebase (Google, aber Vendor-Lock)
3. Custom Node.js (volle Kontrolle, mehr Arbeit)

**Empfehlung**: Supabase geplant wegen PostgreSQL-Power und RLS.

---

### DEC-OPEN-006 – Monetarisierungsmodell

- **Status**: PROPOSED
- **Priorität**: Phase 5 (Release)
- **Frage**: Free-to-Play mit IAP oder Premium-Kauf?

**Optionen**:
1. Free-to-Play (Cosmetics IAP)
2. Einmalig Premium (2.99–4.99€)
3. Hybrid (Free mit Premium-Modus)

**Empfehlung**: Später mit Publisher oder Community entscheiden.

---

### DEC-OPEN-007 – Cross-Plattform Priorität

- **Status**: PROPOSED
- **Priorität**: Phase 5
- **Frage**: iOS zuerst oder parallel iOS+Android?

**Pro iOS-First**:
- Höhere Monetisierung
- Smaller Player-Base = weniger Server-Druck

**Pro Parallel**:
- Größere Audience sofort
- Länger für beide Plattformen debuggen

**Empfehlung**: iOS zuerst (Phase 5), Android Q1 2027.

---

### DEC-OPEN-008 – Multiplayer / PvP

- **Status**: PROPOSED, zunächst NICHT GEPLANT
- **Priorität**: Phase 6+ (nie geplant)
- **Entscheidung**: PvP, Koop und Clan-Systeme bewusst zurückgestellt.

**Begründung**:
- Erhebliche Backend-Komplexität
- Balancing-Herausforderungen
- Scope-Explosion
- Solo-Gameplay fokussiert erst

**Alternativ**: Community-Feedback Phase 5–6 entscheiden.

---

## Zusammenfassung

| Bereich | Entscheidung |
|---------|--------------|
| Unity-Version | 6000.6.0f1 LTS |
| Render Pipeline | Built-in 2D |
| Input | Input System |
| Orientierung | Portrait |
| UI | OnGUI (MVP), Canvas (Phase 3) |
| Save | PlayerPrefs |
| Assets | Runtime-generiert |
| Packages | Nur Unity Standard |
| Monetarisierung | Offen (Phase 5) |
| Backend | Supabase geplant (Phase 4) |

---

## Änderungslog

| Datum | Entscheidung | Typ |
|-------|--------------|-----|
| 2026-09-18 | 8 Akzeptiert, 8 Proposed | Initial |

Gültig ab: 19. September 2026
