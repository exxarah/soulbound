﻿# SOULBOUND

- [X] v0.1.1 - Controller Support
  - Partially done already
- [X] v0.1.2 - Boss Enemy
  - Spawns as part of the final wave, more difficult
  - Grim Reaper
  - [X] Fix boss animations being too fast
  - [X] Display health bar (over enemy's heads) if health is down
  - [X] Test out charm requiring enemy to have low health
  - [X] Improve enemy player seeking (avoid trees, allies, etc)
    - https://www.youtube.com/watch?v=6BrZryMz-ac
  - [X] Balance Pass
    - Note that this update aims to present a full example of arcade mode (functionality-wise), therefore this balancing pass should be super thorough and long ;_; We need to set the stage now, so that it's clearer and easier to expand in future
    - [X] Enemies
    - [X] Player Abilities
    - [X] Enemy waves
- [ ] v0.1.3 - Small Improvements
  - [X] Minions spawn in the place of the enemy they came from
  - [X] Fullscreen option in settings
  - [X] Localisation Ready
    - Move all strings into loc
    - Fix issue with loc in boot scene
  - [ ] Balancing structure rework
    - Entities currently have some data (eg, health) defined in prefabs? Seems like a hacky solution from the game jam. pls fix
    - Reconsider how enemy definitions work
      - The data is not enemy specific, but wave specific. Can we change that database to be generic entity information, and move the wave information into the waves database
    - Reconsider how abilities work
      - Reify conditions, so there's less hacky workarounds
      - Change damage, etc,  to be an effect?
    - Note that part of v0.2 includes a huge ability expansion, consider whether some of the balancing rework can go into that, or into this, depending on the vibes
  - [ ] Balancing pass
- [ ] v0.1.4 - Tutorial
  - Tutorial level/mode, simple waves with instructions guiding the player through
  - On first boot, offer the player a popup to optionally skip the tutorial (which would otherwise launch on first boot), as well as modify their settings (particularly the controller display preference)

- [ ] v0.2.0 - Loadouts, more abilities
  - Add a bunch of new abilities
  - Players can choose which abilities to have for each ability slot
  - New ability mechanics/options (eg, dmg hula hoop, non-cone ranged, DoT, drop AoE at mouse position)
  - ANX hooks (so I can see which abilities are good/bad/op)

- [ ] v0.3.0 - Leaderboards and Endless Mode
  - Original mode reframed as "arcade mode"
  - Leaderboards for arcade mode, players can see their ranking, and others, as well as which loadouts they used
  - New endless mode, waves get more difficult over time procedurally, ends when player dies
  - New scoring for endless mode, more time = good
  - Leaderboard for endless mode

- [ ] v0.4.0 - Aesthetics Rework
  - Basically, make the game look good
  - Shaders
  - Post processing
  - Model and animation fixes / reworks
  - Minion revamp
    - Give them an actual model/animations
  - Spawn animations
  - Audio rework
  - Win/Lose graphics
  - Pause menu revamp / improvements
  - Includes character customisation, in preparation for multiplayer
  - Itch.io page design/upgrade :grimace:

- [ ] v0.5.0 - More Fun Together
  - Co-op mode (local or online? maybe both?)
  - Arena mode (same question as both)
  - Bots for both modes, to account for when no one plays lmao
