# GT2_Custom Game
This repository contains the practical part of the Game Technologies 2 (GT2) course

# NECROPIA: Master of the Dead - Unity Game Repository

## Project Overview
NECROPIA: Master of the Dead is a strategic base-building game developed in Unity, emphasizing resource management and automation. The project was created by Juri Wiechmann, Tim Markmann, and Robin Jaspers as part of their MSc International Media & Computing program at Fachbereich 4, Berlin.

## Game Concept
In NECROPIA, players control a necromancer who commands undead minions to build an automated undead city. The game's core mechanics include resource gathering, refinement, and automation through a unique replay system inspired by titles like Anno and Factorio.

## Core Mechanics
- **Replay System:** Control undead units by recording and looping their actions.
- **Tile Map System:** Resource collection and interaction based on a grid system.
- **Building Kernel:** Construct buildings using predefined recipes.

### Characters and Tools
- **Necromancer:** The primary character who commands undead units.
- **Skeletons:** Undead workers for resource gathering and construction.
- **Curses:** Magical abilities for resource influence and minion control.

## Storyline
In a dystopian future where death has been conquered, players assume the role of a necromancer tasked with restoring balance by reintroducing the natural cycle of life and death. The necromancer must rebuild the underworld and restore the soul flow by commanding an army of skeletons.

## Technologies Used
- **Engine:** Unity
- **Codebase Management:** Feature-oriented folder structure
- **External Asset:** DOTween for advanced animation lerping

## Project Structure
The Unity project is structured as follows:
- **Replay System:** Manages recording and replaying unit actions.
- **Tile System:** Handles grid-based resource and interaction management.
- **Character:** Manages player and minion behavior.
- **Building Kernel:** Implements the construction mechanics using a state machine pattern.
- **Items:** Item management and crafting system.

## Installation and Setup
1. Clone the repository.
2. Open the project in Unity (recommended version 2023 or later).
3. Ensure DOTween is installed.
4. Playtest using the `MainScene`.

## Controls
- **WASD:** Movement
- **Q:** Interact
- **E:** Magic (summon skeletons, cast spells, build structures)

## How to Play
1. **Summon Skeletons:** Collect bones and souls to raise skeletons.
2. **Build Structures:** Use resources on a tile grid to build structures.
3. **Automate Production:** Use the replay system to loop skeleton actions.

## Development Credits
- **Authors:** Juri Wiechmann, Tim Markmann, Robin Jaspers
- **Supervision:** Alexander Fialski, Martin Steinicke
