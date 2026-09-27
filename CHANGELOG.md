# Changelog

Alle wichtigen Änderungen an GALACTIC ROGUE werden in dieser Datei dokumentiert.

Das Format basiert auf [Keep a Changelog](https://keepachangelog.com/en/1.0.0/) und folgt [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [0.1.0] – 2026-09-19

### MVP Release – Funktionsfähiger Vertikaler Slice

**Status**: ✓ Spielbar, stabil, vollständige Core Loop

#### Added

**Gameplay**
- Touch-basierte Steuerung (Drag-Bewegung, Portrait-Modus)
- Automatisches Feuer-System mit Zielsuche
- 5 Gegnertypen: Skitter, Spire, Orbital, Apex, Maw
- Wave-basiertes Spawning mit zeitlicher Eskalation (0–5 min)
- XP-System mit Level-Up-Mechanik
- 3-Option-Upgrade-Auswahl mit Synergien
- Boss-Kampf mit 3-Phasen-Angriffsmuster
- Game-Over und Rückkehr-Logik

**Schiffe & Loadout**
- 1 spielbares Schiff: InterceptorKestrel
- 2 permanente Upgrade-Pfade: Laser, Hull
- Hangar mit Shop-UI (OnGUI)

**Persistenz**
- PlayerPrefs-basiertes Speichern (lokal)
- Credits-System mit Hangar-Upgrades
- Schiff-Statistiken speichern

**Technisch**
- Unity 6 LTS (6000.6.0f1)
- C# mit Namespace-Konvention `GalacticRogue.[Modul]`
- Modular: PlayerShipController, AutoFireController, EnemySpawner, etc.
- Performance: 60 FPS auf iPhone 12 mit 50+ Gegnern + Projektilen

#### Systems

| System | Umfang | Status |
|--------|--------|--------|
| **Touch-Steuerung** | Drag-basiert, Boundary-Clamp | ✓ Stabil |
| **Auto-Fire** | Zielsuche, Multi-Laser, Durchschlag, Homing, Chain-Jump | ✓ Stabil |
| **Gegner-Waves** | 5 Typen, Eskalation, Zeitbasiert | ✓ Stabil |
| **XP & Level-Up** | Pool-basiert, 3er-Auswahl, Pause-Logik | ✓ Stabil |
| **Hangar** | Shop, 2 Upgrade-Pfade, Speichern | ✓ Funktional |
| **Boss-Kampf** | 3 Phasen, Telegraphie | ✓ Funktional |
| **Speicher** | PlayerPrefs, lokal | ✓ Funktional |

#### UI

- OnGUI code-drawn (Prototyp)
- HUD: Leben, Level, Upgrade-Panel, Boss-Indicator
- Menü: Hangar, Game-Over Screen

#### Assets

- Runtime-generierte Sprites (keine externe Grafiken)
- Keine Audio (kein Sound/Musik)
- Keine animierten Gegner

---

## [0.2.0] – TBD (Phase 2a: Schiff-Varianz)

### Geplant: Multi-Klassen-System

#### Added

**Schiffe**
- [ ] Scout-Klasse: +20% Speed, –20% HP, +15% Fire Rate
- [ ] Destroyer-Klasse: –20% Speed, +30% HP, +25% Schaden
- [ ] Experimental-Klasse: Unique Ability (z.B., Temporal Shift, Klon)

**Hangar-Überhaul**
- [ ] Schiff-Auswahl UI (statt nur Upgrades)
- [ ] Stats-Anzeige pro Schiff
- [ ] Persistent Schiff-Auswahl speichern

#### Changed

- PlayerShipController: Multi-Klassen-Support
- AutoFireController: Klassen-spezifische Waffensysteme
- HangarController: Erweiterbar für 3+ Schiffe

---

## [0.3.0] – TBD (Phase 2b: Zonen-Varianz)

### Geplant: Mehrere Missionen

#### Added

**Zonen**
- [ ] Raumstation-Zone (neue Gegner, Boss)
- [ ] Nebel-Zone (Stealth-Gegner)
- [ ] Alien-Ruinen-Zone (komplexer Boss)

**Mission-Selector**
- [ ] Hangar zeigt Zone-Auswahl
- [ ] Zone-spezifische Gegner-Pools
- [ ] Zone-spezifische Musik/Visuals (Phase 3)

#### Changed

- EnemySpawner: Zone-agnostisch machen
- RunManager: Zone-Kontext

---

## [0.4.0] – TBD (Phase 2c: Content Expansion)

### Geplant: Gegner & Upgrade Expansion

#### Added

**Gegner**
- [ ] 10+ neue Gegnertypen (total 15+)

**Upgrades**
- [ ] 30+ Total-Upgrades (statt 15)
- [ ] 5+ Synergien dokumentiert
- [ ] Fraktionen-System (optional)

**Balancing**
- [ ] Run-Länge: 10–15 Min Standard
- [ ] Wave-Eskalation justiert
- [ ] Upgrade-Synergien balanciert

---

## [0.5.0] – TBD (Phase 3: Polish)

### Geplant: Visual & Audio Polish

#### Added

**UI**
- [ ] Canvas-basierte UI (statt OnGUI)
- [ ] Responsive auf iPhone SE bis 14 Pro

**Visuals**
- [ ] Schiff-Skins / Varianten
- [ ] Gegner-Grafiken
- [ ] Effekte & Particles

**Audio**
- [ ] Musik (Ambient, Boss)
- [ ] SFX (Schuss, Explosion, Upgrade)
- [ ] Audio Manager

#### Changed

- Visuelle Identität: Neon-Sci-Fi

---

## [0.6.0] – TBD (Phase 4: Backend)

### Geplant: Cloud & Meta

#### Added

**Backend**
- [ ] Supabase-Integration (Auth)
- [ ] Cloud-Save
- [ ] Leaderboards
- [ ] RLS-Policies für Spielerdaten

**Meta**
- [ ] Fraktions-Gameplay (fortgeschritten)
- [ ] Battle Pass (optional)
- [ ] Kosmetik-Items

---

## [0.7.0] – TBD (Phase 5: Release)

### Geplant: App Store Ready

#### Added

- [ ] Performance-Audit (60 FPS auf iPhone 12)
- [ ] Geräte-Kompatibilität (SE, 12, 14 Pro getestet)
- [ ] App Store Assets (Icons, Screenshots, Description)
- [ ] TestFlight & Review

#### Changed

- iOS Build-Settings finalizen

---

## Versionshistorie

| Version | Datum | Typ | Beschreibung |
|---------|-------|------|--------------|
| 0.1.0 | 2026-09-19 | MVP | Erste spielbare Version |
| 0.2.0 | TBD | Feature | Multi-Klassen |
| 0.3.0 | TBD | Feature | Mehrere Zonen |
| 0.4.0 | TBD | Feature | Content Expansion |
| 0.5.0 | TBD | Polish | UI, Visuals, Audio |
| 0.6.0 | TBD | Backend | Cloud & Leaderboards |
| 0.7.0 | TBD | Release | App Store Ready |

---

## Semantik

- **Added**: Neue Funktionen
- **Changed**: Existierende Features verändert
- **Deprecated**: Wird in Zukunft entfernt
- **Removed**: Entfernte Features
- **Fixed**: Bugfixes
- **Security**: Sicherheits-Fixes

---

**Gültig ab**: 20. September 2026  
**Letztes Update**: Durch Abschlussbericht aktualisieren
