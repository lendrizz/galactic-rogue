# PROJECT_STATE.md – GALACTIC ROGUE

**Aktueller, überprüfter Projektstand. Gelesen vor jeder neuen Aufgabe.**

---

## 1. Letzte Aktualisierung

| Eigenschaft | Wert |
|---|---|
| **Datum** | 19. September 2026 |
| **Quelle** | Projekt-Analyse & Code-Review |
| **Grund** | MVP-Dokumentation, Vorbereitung Multi-AI-Entwicklung |
| **Nächste Überprüfung** | Nach Phase 1 Abschluss (oder bei größeren Änderungen) |

---

## 2. Projektstatus (Gesamt-Übersicht)

| Eigenschaft | Wert |
|---|---|
| **Phase** | **Phase 1 (MVP): ✓ Abgeschlossen** |
| **Unity-Version** | 6000.6.0f1 (Unity 6 LTS) |
| **Programmiersprache** | C# |
| **Plattform** | iOS (iPhone/iPad, aktuell nur Editor-Testing) |
| **Spielbar?** | ✓ Ja, ~5–15 min pro Run |
| **Stabil?** | ✓ Ja, keine bekannten Crashes |
| **Build-Status** | ⚠ Nicht getestet (iOS-Build, nur Editor) |

---

## 3. Implementierte Systeme (✓ Funktional)

| System | Status | Dateien | Kurzbeschreibung |
|--------|--------|---------|---|
| **Player Movement** | ✓ IMPL | PlayerShipController.cs | Drag-basierte Touch-Steuerung, Boundary-Clamp |
| **Auto-Fire** | ✓ IMPL | AutoFireController.cs, TargetRegistry.cs | Automatisches Schießen auf nächstes Ziel |
| **Gegner Waves** | ✓ IMPL | EnemySpawner.cs, EnemyShip.cs | 5 Gegnertypen, zeitbasierte Eskalation |
| **Boss-Kampf** | ✓ IMPL | BossShip.cs | 1 Boss (Maw of Kharon) mit 3 Phasen |
| **XP & Level-Up** | ✓ IMPL | ExperienceController.cs, ExperienceOrb.cs | Gegner → XP-Drops → Level-Up |
| **Upgrades** | ✓ IMPL | UpgradeSelectionController.cs | 15 Upgrades, 3-Option-Auswahl |
| **Combat & Damage** | ✓ IMPL | Damageable.cs, Projectile.cs, DamageNumber.cs | HP-System, Schaden, Feedback |
| **Hangar** | ✓ IMPL | HangarController.cs | Schiff-Übersicht, 2 Shop-Items |
| **Save/Load** | ✓ IMPL | PlayerProfile.cs | PlayerPrefs (lokal), 3 Daten-Keys |
| **Visuals** | ✓ IMPL | Starfield.cs, HealthBar.cs, PrototypeSpriteFactory | Runtime-Sprites, Hintergrund, Feedback |
| **Bootstrap** | ✓ IMPL | PrototypeBootstrap.cs | Auto-Init, Szenen-Aufbau |

**Zusammenfassung**: 11 Systeme vollständig implementiert, alle getestet im Editor.

---

## 4. Teilweise implementierte Systeme (⚠ Nicht komplett)

| System | Umfang | Fehlend | Priorität |
|--------|--------|---------|-----------|
| **Hangar** | 2 von X Upgrades | Waffen-Slots, Schiff-Auswahl, weitere Items | Phase 2 |
| **UI** | OnGUI Prototyp | Canvas-basierte polierte UI | Phase 3 |
| **Visuals** | Basis-Grafiken | Polierte Sprites, Animationen, Effekte | Phase 3 |

---

## 5. Noch nicht implementierte Systeme (❌ Geplant)

| System | Geplante Phase | Grund |
|--------|---|---|
| **Supabase-Backend** | Phase 4 | Cloud-Save, Auth, RLS |
| **Fraktionen** | Phase 2 | 3 Fraktionen mit Gameplay-Auswirkung |
| **Mehrere Schiffe** | Phase 2 | Scout, Destroyer, Experimental (3 Klassen) |
| **Mehrere Missionen** | Phase 2 | 3+ Zonen statt 1 Asteroidengürtel |
| **Audio** | Phase 3 | Musik, SFX, Sound-System |
| **Canvas-UI** | Phase 3 | Polierte, responsive Mobile-UI |
| **Ressourcen-System** | Phase 4 | Credits, Iridium, Tech Parts, Void Shards |
| **Leaderboards** | Phase 4 | Optional, über Supabase |

---

## 6. Bekannte Fehler

### 6.1 Performance-Kandidaten (Keine Bugs, aber Optimize-Chancen)

| ID | Beschreibung | Dateien | Priorität | Status |
|----|---|---|---|---|
| **PERF-001** | TargetRegistry.FindNearest() ist O(n) Linear Search | TargetRegistry.cs | Low | ⏳ Offen – Nach Performance-Audit |
| **PERF-002** | Projectile-Erzeuging ohne Pooling | Projectile.cs | Low | ⏳ Offen – Nach Performance-Audit |
| **PERF-003** | `FindAnyObjectByType()` Aufrufe nicht alle gecacht | Mehrere Scripts | Low | ⏳ Offen – Phase 2 Refactoring |

### 6.2 Bekannte Limitationen (nicht Bugs, absichtlich MVP)

| ID | Limitation | Dateien | Phase für Fix |
|----|---|---|---|
| **LIM-001** | Nur 1 Schiff (Interceptor Kestrel) | PrototypeBootstrap.cs | Phase 2 |
| **LIM-002** | Nur 1 Mission (Asteroidengürtel) | EnemySpawner.cs | Phase 2 |
| **LIM-003** | Nur 15 Upgrades (begrenzte Synergien) | UpgradeSelectionController.cs | Phase 2 |
| **LIM-004** | Nur 2 Hangar-Upgrade-Pfade | HangarController.cs | Phase 2 |
| **LIM-005** | Keine Audio-Implementierung | – | Phase 3 |
| **LIM-006** | OnGUI UI statt Canvas | HangarController.cs, RunManager.cs | Phase 3 |

### 6.3 Nicht überprüfte Punkte

| Punkt | Grund | Nächster Schritt |
|-------|-------|---|
| **iOS-Build** | Kein iPhone vorhanden, nur Editor-Test | Erst Phase 5 |
| **Memory-Leaks** | Kein Profiler-Audit durchgeführt | Phase 5 Performance-Audit |
| **Extreme Load** | Nicht mit 1000+ Gegnern getestet | Später, wenn Performance-Audit zeigt es ist nötig |
| **Prefab-Abhängigkeiten** | Nur Code-Fallback überprüft | Manuelle Prefab-Inspection nötig |

---

## 7. Aktuelle Aufgaben

### 7.1 Abgeschlossen (Phase 1)

| Task | Beschreibung | Status | Datum |
|------|---|---|---|
| **TASK-001-ANALYSIS** | Projekt-Analyse durchführen | ✓ Done | 19.9.2026 |
| **TASK-002-AI-RULES** | AI_RULES.md erstellen | ✓ Done | 19.9.2026 |
| **TASK-003-README** | README.md überarbeiten | ✓ Done | 19.9.2026 |
| **TASK-004-ARCHITECTURE** | ARCHITECTURE.md erstellen | ✓ Done | 19.9.2026 |
| **TASK-005-PROJECT-STATE** | PROJECT_STATE.md erstellen | ✓ Done | 19.9.2026 |

### 7.2 Nächste Aufgaben (Phase 1 → Phase 2)

| Task-ID | Beschreibung | Verantwortung | Abhängigkeiten | Priorität |
|---------|---|---|---|---|
| **TASK-006-ROADMAP** | ROADMAP.md erstellen (Phasen 2–5) | TBD | PROJECT_STATE.md | ✓ Hoch |
| **TASK-007-TASKS** | TASKS.md erstellen (Aufgabenliste) | TBD | ROADMAP.md | ✓ Hoch |
| **TASK-008-CHANGELOG** | CHANGELOG.md erstellen | TBD | PROJECT_STATE.md | ⚠ Mittel |
| **TASK-009-DECISIONS** | DECISIONS.md erstellen | TBD | ARCHITECTURE.md | ⚠ Mittel |

---

## 8. Letzte Änderungen

### 8.1 Diese Sitzung (19. September 2026)

**Neue Dateien erstellt:**
- `AI_RULES.md` – Verbindliche Regeln für alle KI-Entwickler
- `ARCHITECTURE.md` – Technische Referenz
- `PROJECT_STATE.md` – Diese Datei

**Aktualisierte Dateien:**
- `README.md` – Komplette Überarbeitung für Einstieg und Multi-AI-Struktur

**Was nicht geändert wurde:**
- ✓ Keine Unity-Projektdateien
- ✓ Keine Scripts
- ✓ Keine Szenen

---

### 8.2 Frühere Änderungen

**Nicht dokumentiert** (erst ab heute wird CHANGELOG.md geführt).

Siehe später: `CHANGELOG.md` für vollständige Historie.

---

## 9. Nächster sinnvoller Schritt

### Sofort (Diese Woche)

1. **`ROADMAP.md` erstellen** (Phase 2–5 planen)
   - Detaillierte Task-Zerlegung
   - Timeline-Schätzung
   - Abhängigkeiten auflösen
   - **Effort**: ~2–3 Stunden

2. **`TASKS.md` erstellen** (Aufgabenliste für KIs)
   - Konkrete Tasks aus ROADMAP
   - Zuweisung (wer, wann, was)
   - Abhängigkeiten verlinken
   - **Effort**: ~1–2 Stunden

### Diese Woche

3. **`DECISIONS.md` erstellen** (Offene Entscheidungen)
   - 5 offene Architektur-Fragen (aus ARCHITECTURE.md)
   - Pro/Contra für Optionen
   - Gewählte Lösung dokumentieren
   - **Effort**: ~1 Stunde

4. **`CHANGELOG.md` erstellen** (Version-Tracking)
   - Vorlage für zukünftige Einträge
   - Phase-basierte Strukturierung
   - **Effort**: ~30 Min

### Nächste Phase (Phase 2 Vorbereitung)

5. **Performance-Audit** (nicht jetzt, aber planen)
   - TargetRegistry unter Last testen
   - Memory-Leak-Check
   - Ziel: Baseline für Phase 2 Optimierungen

---

## 10. Offene Fragen

### 10.1 Technische Fragen

| Frage | Kontext | Betroffen | Priorität |
|-------|---------|-----------|-----------|
| **Q-ARCH-001** | Spatial Partitioning für TargetRegistry? | PERF-001 | Low |
| **Q-ARCH-002** | Upgrade-Pool als ScriptableObject? | Phase 2 Content | Medium |
| **Q-ARCH-003** | Event-System vollständig einführen? | Phase 2 Refactoring | Low |
| **Q-ARCH-004** | Waffen-Slots implementieren? | Phase 2 Gameplay | High |
| **Q-ARCH-005** | Cloud-Save: Supabase oder Alternative? | Phase 4 Backend | Medium |

### 10.2 Design-Fragen

| Frage | Kontext | Betroffen | Priorität |
|-------|---------|-----------|-----------|
| **Q-DESIGN-001** | Schiff-Asymmetrien (Scout schneller, Destroyer kräftiger)? | Phase 2 Schiffe | High |
| **Q-DESIGN-002** | Fraktionen: 3 oder 5? | Phase 2 Content | Medium |
| **Q-DESIGN-003** | Zonen-Theme: Asteroid, Station, Nebel, Alien, Wormhole? | Phase 2 Missionen | Medium |
| **Q-DESIGN-004** | Ressourcen-Progression: Exponentiell oder Linear? | Phase 4 | Low |
| **Q-DESIGN-005** | Battle-Pass oder Seasonal-Content? | Phase 5 Monetarisierung | Low |

### 10.3 Geschäftliche Fragen

| Frage | Kontext | Betroffen | Priorität |
|-------|---------|-----------|-----------|
| **Q-BUSINESS-001** | Free-to-Play oder Premium? | Phase 5 Release | High |
| **Q-BUSINESS-002** | Zielgruppe: Core oder Casual? | Phase 2 Design | High |
| **Q-BUSINESS-003** | Supportverpflichtung post-Launch? | Phase 6+ | Low |

---

## 11. Metriken & Fortschritt

### 11.1 Implementierungs-Status

```
Phase 1 MVP:           ███████████████████ 100% ✓
├─ Core Gameplay      ███████████████████ 100% ✓
├─ Player System      ███████████████████ 100% ✓
├─ Combat System      ███████████████████ 100% ✓
├─ Progression        ███████████████████ 100% ✓
├─ Save System        ███████████████████ 100% ✓
└─ Bootstrap          ███████████████████ 100% ✓

Phase 2 Content:       ░░░░░░░░░░░░░░░░░░░   0% (Geplant)
├─ Mehrere Schiffe    ░░░░░░░░░░░░░░░░░░░   0%
├─ Mehrere Missionen  ░░░░░░░░░░░░░░░░░░░   0%
├─ Fraktionen         ░░░░░░░░░░░░░░░░░░░   0%
└─ Balancing          ░░░░░░░░░░░░░░░░░░░   0%

Phase 3 Polish:        ░░░░░░░░░░░░░░░░░░░   0% (Geplant)
Phase 4 Backend:       ░░░░░░░░░░░░░░░░░░░   0% (Geplant)
Phase 5 Release:       ░░░░░░░░░░░░░░░░░░░   0% (Geplant)
```

### 11.2 Code-Statistiken

| Metrik | Wert |
|--------|------|
| **C# Script-Dateien** | 19 |
| **Zeilen Code** | ~2000 (geschätzt) |
| **Namespaces** | 8 (GalacticRogue.*) |
| **Szenen** | 2 (1 aktiv) |
| **Externe Packages** | 0 |
| **Abhängige Systeme** | ~80% gut strukturiert, ~20% Refactoring-Kandidaten |

---

## 12. Prüflistenstand

### 12.1 MVP-Kriterien

- [x] Spielbar ohne Crash
- [x] Core-Loop funktioniert (Gegner → Schaden → XP → Upgrades)
- [x] Touch-Steuerung präzise
- [x] Automatisches Feuer funktioniert
- [x] Gegner verhalten sich unterschiedlich
- [x] XP-System funktioniert
- [x] Level-Up funktioniert
- [x] Upgrades werden angewendet
- [x] Boss-Kampf funktioniert
- [x] Game-Over funktioniert
- [x] Hangar funktioniert
- [x] Speichern funktioniert
- [x] Laden funktioniert

### 12.2 Multi-AI-Readiness

- [x] AI_RULES.md existiert
- [x] README.md für Einstieg
- [x] ARCHITECTURE.md existiert
- [ ] ROADMAP.md existiert (nächste Task)
- [ ] TASKS.md existiert (nächste Task)
- [ ] DECISIONS.md existiert (nächste Task)
- [ ] CHANGELOG.md existiert (nächste Task)

---

## 13. Support & Kontakt

### Wenn du ein Problem findest:

1. **Bekannter Fehler?** → Siehe Sektion 6
2. **Architektur-Frage?** → Siehe Sektion 10
3. **Task unklar?** → Siehe TASKS.md (in Entwicklung)
4. **Neue Information?** → Dokumentiere in DECISIONS.md

### Wenn du änderungen machst:

1. **AI_RULES.md lesen** (Prozess)
2. **Abschlussbericht schreiben** (Vorlage in AI_RULES.md Sektion 9)
3. **PROJECT_STATE.md aktualisieren** (nach größeren Änderungen)

---

## 14. Versionsverlauf dieser Datei

| Version | Datum | Autor | Änderung |
|---------|-------|-------|----------|
| 1.0 | 19.9.2026 | Projekt-Analyse | Initial |

---

**Status**: ✓ MVP ist stabil, spielbar und dokumentiert.  
**Nächster Review**: Nach ROADMAP.md & TASKS.md Abschluss.  
**Gültig für**: Phase 1 Abschluss + Phase 2 Vorbereitung
