# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Popping Bubble** is a physics-based puzzle game (Tetris-like) built in Unity/C#. Players drop colored bubbles that bounce and roll with realistic physics. Same-colored adjacent bubbles form groups, which are destroyed only when the group connects to **both the left AND right walls simultaneously**. Consecutive eliminations trigger combo multipliers.

- **Development Period**: Feb 2024 - Jun 2024
- **Platforms**: PC (500×900 window), Android
- **Main Scene**: `00_Scenes/Main.unity`

## Architecture

### Core Game Loop

```
BallController (spawning) → Ball (physics & connections) → GameManager (scoring & combos)
```

### Key Scripts

| Script | Location | Purpose |
|--------|----------|---------|
| `GameManager.cs` | `01_Scripts/BaseCode/` | Singleton master controller: scoring, combos, game state, audio |
| `BallController.cs` | `01_Scripts/GameContorol/` | Ball spawning, drop intervals, preview system, input handling |
| `Ball.cs` | `01_Scripts/GameContorol/` | Physics simulation, color-group detection via collisions, wall tracking, removal logic |
| `WaitingBall.cs` | `01_Scripts/GameContorol/` | Mouse X-axis tracking for positioning balls before drop |
| `EraseMode.cs` | `01_Scripts/GameContorol/` | Last-chance revival mechanic (one free ball removal on first game-over) |
| `UIManager.cs` | `01_Scripts/UI/` | UI orchestration singleton |

### Critical Algorithms

**Ball Connection Detection** (`Ball.cs`):
- Uses Collider2D contacts (not distance) to detect adjacency
- `RecursiveFindConnectedBalls()` performs graph traversal to find all connected same-color balls
- Removal condition: connected group must touch both left AND right walls

**Scoring Formula** (`GameManager.cs`):
```csharp
finalScore = baseScore × 10^(combo - 1)
// combo=1: ×1, combo=2: ×10, combo=3: ×100
```

### Game Over Conditions
1. Ball reaches `EndLine` (top boundary trigger)
2. Player declines Last-Chance revival

## Project Structure

```
Assets/
├── 00_Scenes/           # Main.unity (primary scene)
├── 01_Scripts/          # All C# game logic
│   ├── BaseCode/        # Core managers (GameManager, ResolutionManager)
│   ├── GameContorol/    # Main mechanics (Ball, BallController, WaitingBall)
│   ├── UI/              # UI components
│   ├── Admob/           # Google Mobile Ads
│   └── Setting/         # Audio/settings
├── 02_Sprites/          # Ball textures, UI assets
├── 03_Prefabs/          # Reusable game objects
├── 05_Sound/            # Audio files (BGM/SFX)
└── 06_Physics Material 2D/  # Ball physics materials
```

Note: Folder name `GameContorol/` contains a typo (should be `GameControl`).

## External Dependencies

- **MasterAudio** (DarkTonic): Audio system - BGM/SFX via `GameManager` and `Settings.cs`
- **DOTween**: Animation tweening
- **Firebase SDK**: Auth & Realtime Database (partially implemented/disabled)
- **Google Play Games**: Leaderboards (partially implemented)
- **Google Mobile Ads**: Ad integration
- **TextMesh Pro**: UI text rendering

## Data Persistence

Uses `PlayerPrefs`:
- `"HighScore"`: Best score
- `"BGM"`: Background music toggle (0/1)
- `"SFX"`: Sound effects toggle (0/1)

## Development Notes

- **EraseMode uses `Time.timeScale = 0`** during revival choice - be aware of edge cases with paused physics
- **Firebase/GPGS integration is incomplete** - large sections are commented out
- Physics Material 2D settings in `06_Physics Material 2D/` control bounce/friction
- `ResolutionManager.cs` handles PC window sizing (500×900) and wall positioning relative to camera viewport
