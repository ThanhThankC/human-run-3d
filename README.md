# HumanRun

A simple 3D mobile runner game built with Unity — collect coins and complete each level!

## Tech Stack

- **Engine:** Unity 2022.3.32f1 (LTS)
- **Platform:** Android / iOS
- **Language:** C#

## Gameplay

- Run from start to finish across handcrafted levels
- Collect coins scattered throughout each map
- Avoid obstacles along the way
- Complete all levels to win
- Simple joystick + button controls optimized for mobile

## Project Structure

```
HumanRun/
├── Assets/
│   ├── _Game/                        # Core game assets (tracked by Git)
│   │   ├── Scripts/
│   │   │   ├── Player/
│   │   │   ├── Camera/
│   │   │   ├── Collectibles/
│   │   │   ├── UI/
│   │   │   └── Managers/
│   │   ├── Prefabs/
│   │   │   ├── Player/
│   │   │   ├── Collectibles/
│   │   │   └── UI/
│   │   ├── Animations/
│   │   │   └── Player/
│   │   ├── Materials/
│   │   │   └── Player/
│   │   ├── Audio/
│   │   │   ├── SFX/
│   │   │   └── Music/
│   │   └── Textures/
│   │       └── UI/
│   │
│   ├── Map/                          
│   │   ├── Level_01/
│   │   │   ├── Scenes/
│   │   │   ├── Meshes/
│   │   │   ├── Textures/
│   │   │   ├── Materials/
│   │   │   └── Prefabs/
│   │   ├── Level_02/
│   │   │   └── ...
│   │   └── Shared/                   
│   │       ├── Textures/
│   │       └── Materials/
│   │
│   ├── Scenes/
│   │   ├── MainMenu.unity
│   │   ├── Level_01.unity
│   │   ├── Level_02.unity
│   │   └── ...
│   ├── Settings/
│   └── ThirdParty/
│       └── .gitkeep
│
├── Packages/
│   └── manifest.json
├── ProjectSettings/
├── .gitignore
└── README.md
```

## Level Design Convention

Each level lives in its own subfolder under `Assets/Map/Level_XX/` and has a matching scene at `Assets/Scenes/Level_XX.unity`. The scene file is tracked by Git; the heavy map assets (meshes, textures) are not.

## Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/HumanRun.git
   cd HumanRun
   ```

2. **Open in Unity Hub**
   - Add project → select the cloned folder
   - Use Unity **2022.3.32f1**

3. **Open a level scene**
   - `Assets/Scenes/Level_01.unity`

4. **Build for mobile**
   - File → Build Settings → Android / iOS
   - Switch Platform → Build

## Controls

| Platform | Move         | Jump            |
|----------|--------------|-----------------|
| Mobile   | Virtual joystick | Tap jump button |
| Editor   | WASD / Arrow keys | Space       |

## Notes

- Map assets (meshes, textures per level) are excluded from this repo due to file size
- Scene files (`.unity`) **are** tracked — only the raw art assets are ignored
- Requires Unity 2022.3.32f1 LTS or newer patch

## 📄 License

MIT License — see [LICENSE](LICENSE) for details.
