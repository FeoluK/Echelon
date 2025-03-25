# Echelon-Project

# Watch the Video

[![Watch Video](https://img.shields.io/badge/YouTube-Watch-red?logo=youtube&style=for-the-badge)](https://youtu.be/4PR8-mKllMo)

# Echelon

**Echelon** is a first-person virtual reality action RPG that places you in the shoes of a lone ascender navigating a mysterious dungeon system. Inspired by the world of *Solo Leveling*, the game centers around dynamic combat, immersive world-building, and portal-based exploration. You begin as a seemingly ordinary individual in a central chamber — the gateway to everything. From this room, you enter portals to dungeons where every enemy, every strike, and every decision could determine your survival.

Designed natively for VR headsets using Unity and the XR Interaction Toolkit, *Echelon* combines cinematic combat with interactive physical mechanics like swordplay, telekinesis, and environmental traversal.

---

## Core Gameplay

The experience begins in a dark, ambient **portal chamber**, where the only way forward is through. Each portal transports you to a procedurally placed dungeon environment, where:

- You **summon a sword** at will using XR controller input.
- You **engage enemies** using physics-based melee combat.
- You **wield telekinesis**, grabbing and launching objects in your environment.
- You **face bosses** that dynamically react to player distance, play randomized voice lines, and execute one of several animation-driven attack types.

Enemies and bosses are not simple NPCs. They detect you, track your movement via Unity’s NavMesh system, and react in real time. Bosses include:

- Five randomized attack animations, each with distinct behavior.
- Nine randomized voice lines played during combat.
- Custom death triggers that halt all logic, play final audio, and remove the boss after a delay.

The game also features floating damage indicators, dynamic health tracking for both the player and enemies, and diegetic audio feedback to enhance immersion.

---

## Technical Breakdown

This project was built in **Unity 2022.3 LTS** with **Universal Render Pipeline (URP)** for visual polish and lighting control.

### Combat System

- Real-time sword collisions using physics colliders and tags.
- Attack animations managed through triggers, cooldowns, and conditional logic.
- Damage randomized per strike, with floating 3D text displaying impact value.
- Enemies and bosses implement local health systems and handle despawning.

### AI & Animation

- NavMesh Agents navigate environment dynamically.
- Animation states include running, attacking, idle, and death.
- Bosses pick attacks randomly and play synchronized voice lines.

### Audio & Visual Feedback

- AudioSources are instantiated and controlled via script.
- Death and attack voice lines selected randomly, with cooldowns.
- VR-optimized glowing materials applied to enemies and damage text.
- Scene-based background music system for immersive environments.

### Telekinesis

- All objects tagged “Interactable” can be grabbed and thrown using ray-based selection.
- Uses Unity physics and VR input to create a natural feeling throw mechanic.

### Portals & Scene Transitions

- Tagged triggers detect player collision and load new scenes.
- Scene transitions maintain VR state and are fully immersive.

---

## Folder Structure

- `Scripts`: Core systems for combat, AI, health, portals, and interaction.
- `Prefabs`: Modular prefabs including sword, enemy, boss, and environment assets.
- `Audio`: All voice lines, music loops, and combat sounds.
- `Materials`: URP-emissive and transparent shaders.
- `Scenes`: Includes `PortalRoom`, `Dungeon1`, and `BossArena`.

---

## Getting Started

1. Clone the repository.
2. Open the project in Unity 2022.3 or higher.
3. Ensure XR Plug-in Management is installed and headset integration is enabled.
4. Load the `PortalRoom` scene and enter Play mode with your XR headset.

This project is tested on both **Meta Quest 2** and **Apple Vision Pro** using Unity's XR Plugin architecture.

---

## Future Plans

Echelon is built with modularity in mind. Planned future expansions include:

- Procedural dungeon generation
- Enemy variety and elemental types
- Weapon loadouts and upgrade system
- Persistent skill tree and progression
- Voice-reactive boss logic
- Networked multiplayer raids
