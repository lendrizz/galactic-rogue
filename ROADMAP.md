# ROADMAP.md – GALACTIC ROGUE

Phasenweise Entwicklungs-Planung für GALACTIC ROGUE vom aktuellen MVP bis zum iOS-Release.

---

## Phase 1: MVP (✓ COMPLETED)

**Status**: DONE – Stabil, spielbar, dokumentiert  
**Abgeschlossen**: 2026-09-18

### Inhalt
- 1 Schiff (Interceptor Kestrel)
- 1 Mission (Asteroidengürtel, ~5 Minuten)
- 5 Gegnertypen: Skitter, Spire, Orbital, Apex (Elite), Maw of Kharon (Boss)
- 15 Upgrades im Pool
- 2 permanente Upgrades (Laser, Hull) im Hangar
- PlayerPrefs-Speicher (3 Keys)
- OnGUI Prototyp-UI
- Runtime-generierte Sprites
- Keine externen Packages

### Test-Kriterien erfüllt
- [x] Run spielbar, ~5 min Dauer
- [x] Gegner-Verhalten unterschiedlich
- [x] Upgrades wirken sich aus
- [x] Boss-Kampf balanciert
- [x] Speichersystem funktioniert
- [x] Mobil-ready (Portrait)

---

## Phase 2: Content Expansion (nächste 6–8 Wochen)

**Ziel**: Spieltiefe und Replayability durch Varianz (Schiffe, Zonen, Gegner, Upgrades)

### Phase 2a: Schiff-Varianz (Woche 1–2)

Implementiere 2 neue Schiff-Klassen neben dem Interceptor.

#### GR-P2-001: Scout-Klasse
- **Stats**: +20% Speed, –20% HP, +15% Fire Rate
- **Spielstil**: Hit-and-run, schnell ausweichen
- **Balancing**: Gegen Interceptor fair
- **Aufwand**: 20–30 Stunden

#### GR-P2-002: Destroyer-Klasse
- **Stats**: –20% Speed, +30% HP, +25% Damage
- **Spielstil**: Tank, hohe Feuerkraft
- **Balancing**: Gegen Scout & Interceptor fair
- **Aufwand**: 20–30 Stunden

#### GR-P2-003: Experimental-Klasse
- **Unique Ability**: Temporal Shift, Holografischer Klon, oder ähnlich
- **Spielstil**: High-risk/high-reward
- **Balancing**: Nicht overpowered
- **Aufwand**: 25–35 Stunden

#### GR-P2-004: Hangar-Überhaul
- Schiff-Auswahl vor Mission
- Stats-Anzeige
- Speicherung des Schiff-Choice
- **Aufwand**: 15–20 Stunden

**Phase 2a Summe**: ~80–115 Stunden  
**Meilenstein**: 3 spielbare Schiffe mit Hangar-Integration

---

### Phase 2b: Zonen-Varianz (Woche 3–4)

Implementiere 2 neue Zonen neben Asteroidengürtel.

#### GR-P2-005: Raumstation-Zone
- Neue Gegner-Typen: Drohnen, Schild-Einheiten (min. 2)
- Neuer Boss (anders als Maw of Kharon)
- Run-Dauer: 10–15 Minuten
- **Aufwand**: 30–40 Stunden

#### GR-P2-006: Nebel-Zone
- Neue Gegner-Typen: Stealth-Gegner, Sniper (min. 2)
- Neuer Boss
- Run-Dauer: 10–15 Minuten
- **Aufwand**: 30–40 Stunden

#### GR-P2-007: Alien-Ruinen-Zone
- Neue Gegner-Typen: Alien-Kreaturen (min. 3)
- Komplexer Boss
- Run-Dauer: 10–15 Minuten
- **Aufwand**: 30–40 Stunden

#### GR-P2-008: Mission-Selector UI
- Hangar zeigt 3 Zonen
- Spieler wählt vor Mission
- Zone-Infos angezeigt
- **Aufwand**: 15–20 Stunden

**Phase 2b Summe**: ~105–140 Stunden  
**Meilenstein**: 3 spielbare Zonen mit Gegner-Varietät

---

### Phase 2c: Gegner & Upgrades Expansion (Woche 5–6)

#### GR-P2-009: 10+ neue Gegnertypen
- Insgesamt ~15 Gegnertypen (aktuell 5)
- Unterschiedliche Verhaltensmuster pro Typ
- Balanciert in Wave-Mix
- **Aufwand**: 40–60 Stunden

#### GR-P2-010: 15+ neue Upgrades
- Insgesamt ~30 Upgrades (aktuell 15)
- Neue Synergien: min. 5 funktionierende Combos
- Balanciert für 3-Option-Auswahl
- **Aufwand**: 30–40 Stunden

#### GR-P2-011: Upgrade-Synergien
- Dokumentation aller Synergien
- Balancing-Pass
- Testing: Alle Combos spielbar
- **Aufwand**: 10–15 Stunden

#### GR-P2-012: Run-Balancing
- Iterativ 20+ Runs spielen
- Durchschnittliche Run-Dauer: 10–15 Minuten
- Schwierigkeits-Kurve fair
- Gegner-Mix balanciert
- **Aufwand**: 10–20 Stunden (iterativ)

#### GR-P2-013: Fraktionen-System (optional)
- 3 Fraktionen: Terranische Allianz, Voidborn, Helix Syndicate
- Gegner gehören Fraktion
- Boss je Fraktion unterschiedlich
- Bonus beim Besiegen
- **Aufwand**: 25–35 Stunden
- **Status**: Optional, kann zu Phase 3 verschoben

**Phase 2c Summe**: ~115–170 Stunden  
**Meilenstein**: Tiefes Upgrade-System, Gegner-Varietät, Fraktionen optional

---

### Phase 2 Gesamtaufwand
**~300–425 Stunden** (6–8 Wochen bei 50 Stunden/Woche)

### Phase 2 Exit-Kriterien
- [ ] 3 Schiffe spielbar & balanciert
- [ ] 3 Zonen spielbar & unterschiedlich
- [ ] 15+ Gegnertypen
- [ ] 30+ Upgrades mit Synergien
- [ ] Mission-Selector funktioniert
- [ ] Runs dauern konsistent 10–15 Minuten
- [ ] Alle neuen Features in HangarController integriert

---

## Phase 3: Polish & Audio (6–8 Wochen)

**Ziel**: Professionelle Präsentation, Audio, Visuals

### GR-P3-001: Canvas-basierte UI Migration
- Ersetze OnGUI mit Canvas
- Responsive auf iPhone SE bis 14 Pro
- TextMesh Pro für Text
- UI-Übergang ohne Gameplay-Änderung
- **Aufwand**: 60–80 Stunden

### GR-P3-002: Grafik-Assets & Skins
- Visuelle Unterscheidung der Schiffe
- Gegner-Varianten (visuell)
- Schiff-Skins (kosmetisch, IAP-ready)
- Particle-Effekte für Waffen/Explosionen
- **Aufwand**: 80–120 Stunden (Art Team)

### GR-P3-003: Audio-System
- Musik (3–5 Tracks: Menu, Zones, Boss)
- SFX: Schüsse, Explosionen, Upgrades, UI
- Audio Manager für Mixing
- **Aufwand**: 30–50 Stunden (Audio Designer)

### GR-P3-004: Effekte & Screenspace
- Screenspace-Effekte: Screen Shake, Flash
- Gegner-Effekte: Hover, Damage-Feedback
- Boss-Effekte: Unique Visuellen
- **Aufwand**: 20–40 Stunden

### GR-P3-005: Performance Audit & Optimization
- Profiling: CPU, GPU, Memory
- Optimierungen nach Audit
- Target: 60 FPS auf iPhone 11+
- **Aufwand**: 20–40 Stunden

**Phase 3 Gesamtaufwand**: ~210–330 Stunden  
**Meilenstein**: Polished, Audio-complete, Performance-optimized

---

## Phase 4: Backend & Cloud (4–6 Wochen)

**Ziel**: Online-Features, Leaderboards, Cloud-Save

### GR-P4-001: Supabase Projekt Setup
- Projekt erstellen
- PostgreSQL-Schema: users, runs, leaderboards
- RLS-Policies
- **Aufwand**: 5–10 Stunden

### GR-P4-002: Auth-Integration
- Email/Password Authentication
- Social Login (optional: Google, Apple)
- Session Management
- **Aufwand**: 15–25 Stunden

### GR-P4-003: Cloud-Save Migration
- PlayerPrefs → Supabase
- Konflikt-Auflösung (local vs cloud)
- Offline-Queue
- **Aufwand**: 20–30 Stunden

### GR-P4-004: Leaderboards
- Global High-Score Board
- Friends Leaderboard
- Weekly/Monthly Rankings
- **Aufwand**: 20–30 Stunden

### GR-P4-005: Analytics & Telemetry
- Session Tracking
- Run-Daten (Duration, Score, Schiff, Zone)
- User Retention Metrics
- **Aufwand**: 15–25 Stunden

**Phase 4 Gesamtaufwand**: ~75–120 Stunden  
**Meilenstein**: Cloud-connected, Leaderboards Live

---

## Phase 5: Release & Launch (2–3 Wochen)

**Ziel**: iOS App Store Release

### GR-P5-001: iOS Build
- Build Settings konfigurieren
- Code Signing
- Provisioning Profiles
- **Aufwand**: 5–10 Stunden

### GR-P5-002: App Store Submission
- Privacy Policy
- Screenshots & Metadata
- Rating Approval
- **Aufwand**: 10–15 Stunden

### GR-P5-003: Launch Marketing
- Press Release
- Social Media Campaign
- Beta Testers (TestFlight)
- **Aufwand**: 20–40 Stunden (Marketing Team)

### GR-P5-004: Post-Launch Monitoring
- Crash Reporting
- User Feedback
- Server Monitoring
- Hotfix Pipeline
- **Aufwand**: 10–20 Stunden (ongoing)

**Phase 5 Gesamtaufwand**: ~45–85 Stunden  
**Meilenstein**: Live on App Store

---

## Zusammenfassung

| Phase | Inhalt | Dauer | Status |
|-------|--------|-------|--------|
| 1 | MVP | 4 Wochen | ✓ DONE |
| 2 | Content | 6–8 Wochen | TODO |
| 3 | Polish | 6–8 Wochen | TODO |
| 4 | Backend | 4–6 Wochen | TODO |
| 5 | Release | 2–3 Wochen | TODO |

**Gesamtprojekt**: ~22–33 Wochen (~6–8 Monate)  
**Start**: Jederzeit (Phase 2 kann sofort beginnen)

---

## Bewusst zurückgestellt

Diese Features sind nicht geplant oder erst später:

- **PvP/Multiplayer**: Übermäßige Komplexität, solo-fokussiert
- **Koop-Modus**: Backend-Herausforderung
- **Clan-System**: Post-launch
- **Seasonal Content**: Post-launch
- **Pay-to-Win**: Cosmetics-only, kein P2W
- **Android Release**: Phase 6 (Q1 2027)

---

## Entscheidungspunkte

Nach jeder Phase:

1. **Nach Phase 1**: Go/No-Go für Phase 2 (tatsächliche Player-Feedback)
2. **Nach Phase 2**: Balancing-Review, Community-Feedback
3. **Nach Phase 3**: Beta-Test Decision (Early Access vs Direct Release)
4. **Nach Phase 4**: Launch Readiness Check
5. **Phase 5+**: Post-launch Roadmap basierend auf Nutzerdaten

---

Gültig ab: 19. September 2026
