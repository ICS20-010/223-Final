# 223-Final
223-Final Project in Unity

#### Author
Alexander Q Bos

## Description
Format: 3D Third Person

Simple game of get to the goal in the fastest time possible!
Ofcourse there are catches, enemies will follow three behaviours of
Offensive, Defensive, and Obstructive behaviour to slow the player
down. There are several paths that lead to the exit, but the shortest
isn't always the best. There is a point system that will calculate a
number of seconds off your total time, while dying will reset the room,
but not the run.

### Mechanics
There are several mechanics within the game for the player and the enemies.
The player will have fairly high mobility with wall running and sliding, but
only a single jump to use. Enemies will have a variety of mechanics that
depend on their preset behaviour, Offensive will attack from range or meele,
Defensive will block exits or hold a key for an exit, and Obstructive will
try to bind, blind, or confuse the player.

### Objectives
The Player has two objectives that will give the best time to complete:
1. Reach the exit as fast as possible.
2. Earn as amany points as possible.

---

### Branches
- main - main branch
- alpha - Major milestone implementation in working game
- prototype - Work area, branch for feature integration

### Project Phases
Phases use the semantic versioning (Major.Minor.Patch).
Each bullet would be a patch.

#### 1.0.4a
- Player movement (walk, run, jump)
- Room Template (entrance, exit, pillar)
- Event System
- Main menu scene transition to Game

#### 1.2.8a
- Player movement (Wall Running)
- Link rooms together
- Implemented Timer, start, and stop
- Save top time per run, on game exit

#### 1.3.11a
- Add Player Stamin, Health
- Draft User HUD
- Create arrow pointing to exit

#### To be expanded