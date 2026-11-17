# Metro Rush India

A playable, stylized endless runner built in Unity URP with C#.

## What is included

- Three-lane endless runner movement
- Jump, slide, turn lane changes
- Train/barrier obstacles and track segments
- Coin collection, score and high-score saving
- Power-ups: magnet, hoverboard, jetpack and shield
- Responsive chase camera and simple UI
- Runtime bootstrap that creates the gameplay scene automatically

## Recommended Unity version

- Unity 2022.3 LTS or newer
- URP enabled in the project

## Setup in Unity

1. Open a new Unity 3D project.
2. Copy this repository into the project root.
3. In the Unity Editor, open the package manager and install URP if needed.
4. Create an empty scene.
5. Add a new empty GameObject named `Bootstrap`.
6. Attach the `GameBootstrap` script from `Assets/Scripts/Bootstrap/GameBootstrap.cs`.
7. Press Play.

The bootstrap script will generate the player, tracks, camera, HUD, and gameplay objects automatically.

## Project structure

- `Assets/Scripts/Bootstrap/GameBootstrap.cs` - creates the full game runtime in an empty scene
- `Assets/Scripts/Core/GameManager.cs` - game flow, score, coins, speed, state machine
- `Assets/Scripts/Core/UIManager.cs` - in-game HUD and game over flow
- `Assets/Scripts/Player/RunnerController.cs` - player movement, jump, slide, lane switching, power-up application
- `Assets/Scripts/World/WorldGenerator.cs` - endless track generation and obstacle spawning
- `Assets/Scripts/World/Collectible.cs` - coin and power-up logic
- `Assets/Scripts/Camera/FollowCamera.cs` - smooth chase camera

## Controls

- Swipe or A/D keys: move left/right
- Swipe up / W / Space: jump
- Swipe down / S / Left Ctrl: slide
- Esc: pause (optional)

## Notes

This is a clean, reusable Unity prototype designed for extension. The project intentionally avoids copyrighted Subway Surfers assets and instead uses original runtime-generated geometry, colors, and gameplay systems.
