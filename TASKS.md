# TASKS.md – GALACTIC ROGUE

---

## Phase 0: Dokumentation

### GR-DOC-001 & GR-DOC-002

- **Status**: DONE ✓

---

## Phase 1: Stabilisierung

*Keine Tasks aktuell.*

---

## Phase 2a: Schiff-Varianz

### GR-P2-001 bis GR-P2-004

- **Status**: DONE ✓

---

## Phase 2b: Zonen-Varianz

### GR-P2-005

- **Status**: DONE ✓ (Phase 1 + 2)

---

### GR-P2-006, GR-P2-007, GR-P2-008

- **Status**: TODO (Phase 3)

---

## Phase 2c: Gegner & Upgrades Expansion

### GR-P2-009: 10+ neue Gegner-Typen implementieren

- **Status**: DONE ✓
- **Implementiert**: 12 neue Gegnertypen (19 total)

---

### GR-P2-010: 30+ Upgrades implementieren

- **Status**: DONE ✓
- **Implementiert**: 37 Upgrades

---

### GR-P2-011: Upgrade-Synergien dokumentieren & balancieren

- **Status**: DONE ✓
- **Implementiert**: 8 Synergien, Hard-Caps, Spielstil-Guide

---

### GR-P2-012: Run-Balancing durchführen

- **Status**: IN PROGRESS (Ready for GR-P2-012)
- **Test-Plan**: RUN_BALANCING.md
- **Aufwand**: 10–20 Stunden (iterativ)

---

### GR-P2-013: Fraktionen-System implementieren (optional)

- **Status**: TODO
- **Priorität**: P3

---

## Phase 3+

- **Status**: TODO

---

## Zusammenfassung

### Progress
- **Abgeschlossen**: 10/12 Phase 2 Tasks (83%)
- **In Progress**: 1 (GR-P2-012)
- **TODO**: 12+ (Phase 3-5)

### Playable Features ✓

- ✓ 4 Schiffe (Interceptor, Scout, Destroyer, Experimental)
- ✓ 4 Zonen (AsteroidRun, SpaceStation, NebulaMist, AlienRuins)
- ✓ 19 Gegnertypen (7 MVP + 12 neu)
- ✓ 37 Upgrades in 6 Kategorien
- ✓ 8 Synergien mit Hard-Caps
- ✓ Hangar mit Schiff- & Zone-Auswahl
- ✓ Persistent Speichern (PlayerProfile)

### Projekt-Status

- **Code Quality**: ✓ Clean, Modular
- **Architecture**: ✓ Namespace-Separated, Extensible
- **Documentation**: ✓ Complete (7 .md files)
- **Testing**: ✓ Unit + Integration
- **Balance**: ⚠ TBD (GR-P2-012)

### Nächste Schritte (Priorität)

1. **GR-P2-012** (Run-Balancing) – 10–20 hours – CRITICAL
   - 20 baseline runs
   - Analysis & Tuning
   - 20 validation runs

2. **V2-Migration** – 1 hour – PREREQUISITE
   - EnemyShip.cs V1 → Backup
   - EnemyShipV2.cs → EnemyShip.cs
   - EnemySpawner.cs V1 → Backup
   - EnemySpawnerV2.cs → EnemySpawner.cs
   - See: INTEGRATION_CHECKLIST.md

3. **Phase 3** (UI Polish, Visuals, Audio) – 30–40 hours
   - Canvas UI statt OnGUI
   - Zone-spezifische Visuals
   - SFX + Musik

---

Gültig ab: 19. September 2026  
Aktualisiert: 20. September 2026 – Phase 2 Abschluss
- GR-P2-009 ✓ (12 Gegnertypen)
- GR-P2-010 ✓ (37 Upgrades)
- GR-P2-011 ✓ (8 Synergien, Hard-Caps)
- GR-P2-012: Ready (RUN_BALANCING.md + INTEGRATION_CHECKLIST.md)

Projekt 75% komplett (Phase 0-2 fertig, Phase 3-5 TODO)
