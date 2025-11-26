# ScriptableObject Architecture for BirdyFlap

This document describes the ScriptableObject-based architecture used for the Game Manager and UI systems in BirdyFlap.

## Overview

This architecture follows Unity best practices for decoupled, data-driven game design:

1. **Events** - ScriptableObject-based event system for decoupled communication
2. **Variables** - Shared data containers that can be observed
3. **Configuration** - Centralized, designer-friendly settings
4. **UI State** - Panel navigation and state management

## Architecture Benefits

- **Decoupling**: Components communicate through ScriptableObjects, not direct references
- **Testability**: Systems can be tested in isolation
- **Designer-Friendly**: Values are editable in the Inspector without code changes
- **Scene Independence**: Configuration persists across scenes
- **Live Editing**: Values can be tweaked during play mode

---

## 1. Event System

### GameEvent

ScriptableObject-based events for decoupled communication.

**Create**: `Assets > Create > BirdyFlap > Events > Game Event`

```csharp
// Raising an event (from any script)
[SerializeField] private GameEvent onPlayerDied;

void OnPlayerDeath()
{
    onPlayerDied.Raise();
}

// Listening via MonoBehaviour (using GameEventListener component)
// Or via C# subscription:
void OnEnable()
{
    onPlayerDied.Subscribe(HandlePlayerDeath);
}

void OnDisable()
{
    onPlayerDied.Unsubscribe(HandlePlayerDeath);
}
```

### IntEvent

Events that carry integer data (e.g., score changes).

**Create**: `Assets > Create > BirdyFlap > Events > Int Event`

```csharp
[SerializeField] private IntEvent onScoreChanged;

void AddScore(int points)
{
    onScoreChanged.Raise(points);
}
```

### GameEventListener

MonoBehaviour that listens for events and invokes UnityEvents.

Add this component to any GameObject, assign a GameEvent, and wire up responses in the Inspector.

---

## 2. Variables

### IntVariable

Shared integer values that can be observed.

**Create**: `Assets > Create > BirdyFlap > Variables > Int Variable`

```csharp
[SerializeField] private IntVariable score;

void Start()
{
    score.ResetToInitial();
    score.OnValueChanged += OnScoreChanged;
}

void AddPoint()
{
    score.Increment();
    // or: score.Add(10);
}

void OnScoreChanged(int oldValue, int newValue)
{
    Debug.Log($"Score: {oldValue} -> {newValue}");
}
```

### BoolVariable

Shared boolean values for game states.

**Create**: `Assets > Create > BirdyFlap > Variables > Bool Variable`

```csharp
[SerializeField] private BoolVariable isPaused;

void Update()
{
    if (isPaused.Value) return;
    // Game logic...
}
```

---

## 3. Configuration

### GameSettings

Central game configuration with all tunable parameters.

**Create**: `Assets > Create > BirdyFlap > Config > Game Settings`

Contains:
- Player settings (jump force, gravity)
- Obstacle settings (spawn intervals, gap sizes)
- Game speed settings

```csharp
[SerializeField] private GameSettings settings;

void Start()
{
    _rb.AddForce(Vector2.up * settings.JumpForce, ForceMode2D.Impulse);
}
```

### SceneReference

Type-safe scene references without hardcoded strings.

**Create**: `Assets > Create > BirdyFlap > Config > Scene Reference`

```csharp
[SerializeField] private SceneReference gameScene;

void LoadGame()
{
    if (gameScene.IsValid)
    {
        SceneManager.LoadScene(gameScene.SceneName);
    }
}
```

### AudioSettings

Audio configuration with runtime adjustment support.

**Create**: `Assets > Create > BirdyFlap > Config > Audio Settings`

---

## 4. UI System

### UIPanelState

Represents a UI panel's state and configuration.

**Create**: `Assets > Create > BirdyFlap > UI > Panel State`

Properties:
- Panel identity and navigation
- Back button support
- Pause game when active
- Animation triggers
- Events for show/hide

### UINavigationChannel

Central hub for UI navigation.

**Create**: `Assets > Create > BirdyFlap > UI > Navigation Channel`

Features:
- Navigation stack for back navigation
- Panel history
- Default panel on start

```csharp
[SerializeField] private UINavigationChannel navigation;
[SerializeField] private UIPanelState optionsPanel;

void ShowOptions()
{
    navigation.NavigateTo(optionsPanel);
}

void GoBack()
{
    navigation.NavigateBack();
}
```

### UIPanel

Component that syncs a GameObject with its UIPanelState.

```csharp
// Add UIPanel component to your panel GameObject
// Assign the corresponding UIPanelState
// The panel will automatically show/hide based on state
```

---

## Setup Guide

### Step 1: Create ScriptableObject Assets

1. Create the following folder structure:
   ```
   Assets/
   └── ScriptableObjects/
       ├── Events/
       ├── Config/
       ├── Variables/
       └── UI/
   ```

2. Create required assets:
   - `GameSettings.asset` (BirdyFlap > Config > Game Settings)
   - `MainMenuScene.asset` (BirdyFlap > Config > Scene Reference)
   - `GameScene.asset` (BirdyFlap > Config > Scene Reference)
   - `Score.asset` (BirdyFlap > Variables > Int Variable)
   - `HighScore.asset` (BirdyFlap > Variables > Int Variable)
   - `IsPaused.asset` (BirdyFlap > Variables > Bool Variable)
   - `IsGameOver.asset` (BirdyFlap > Variables > Bool Variable)

3. Create events:
   - `OnStartGameRequest.asset`
   - `OnPauseRequest.asset`
   - `OnResumeRequest.asset`
   - `OnRestartRequest.asset`
   - `OnQuitRequest.asset`
   - `OnPlayerDeath.asset`
   - `OnGameStarted.asset`
   - `OnGamePaused.asset`
   - `OnGameResumed.asset`
   - `OnGameOver.asset`

4. Create UI states:
   - `MainMenuPanel.asset`
   - `OptionsPanel.asset`
   - `PausePanel.asset`
   - `GameOverPanel.asset`

5. Create navigation channel:
   - `UINavigation.asset`

### Step 2: Configure GameManagerSO

1. Create a new GameObject with `GameManagerSO` component
2. Assign all ScriptableObject references
3. Make it a prefab and add to your initial scene

### Step 3: Configure UI

1. Add `UIPanel` component to each panel GameObject
2. Assign corresponding `UIPanelState` assets
3. Add `UINavigationController` to your UI canvas
4. Use `MainMenuUISO`, `OptionsMenuUISO`, etc. for panel-specific logic

---

## Best Practices

1. **One Source of Truth**: Keep configuration in ScriptableObjects, not in multiple scripts
2. **Event-Driven**: Use events for cross-system communication
3. **Observe, Don't Poll**: Subscribe to variable changes instead of checking every frame
4. **Naming Convention**: Use clear, descriptive names for SO assets
5. **Debug Mode**: Enable debug logging in events during development
6. **Reset on Play**: ScriptableObjects reset runtime values in OnEnable

---

## Example: Adding a New Feature

To add a "Lives" system:

1. Create `Lives.asset` (Int Variable, initial value: 3)
2. Create `OnPlayerLostLife.asset` (Game Event)
3. Create `OnGameOver.asset` (Game Event) - if not exists
4. Subscribe to `OnPlayerLostLife` in GameManagerSO
5. When lives reach 0, raise `OnGameOver`
6. Update HUD to display lives from the IntVariable

This approach requires no direct references between systems!
