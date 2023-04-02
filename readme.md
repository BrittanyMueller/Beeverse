# Beeverse

Beeverse is a survival city builder to grow and sustain a bee colony.  The main objective is to keep your bees alive by collecting resources and expanding the colony.  The game starts with a single honeycomb inhabited by your Queen Bee which you must expand from by building new Honeycomb structures.  Honeycombs increase your bee population capacity and allow your Worker Bees to perform new tasks such as the construction of new Honeycomb types, resource conversion, and raising new Baby Bees.  Bees have a lifespan and will passively die over time.  The game will end if your Queen Bee dies without a replacement or your colony dies out from old age, so keep on building! 

## Authors
Brittany Mueller

Lawrence Milne

## Playing the Game

A mouse (not trackpad) is recommended for best experience with controls. Headphones should also be worn to experiences the amazing music and sound effects!

#### Running with the Editor
- Open the Main Scene: Assets/Scenes/MainMenu
- Screen size in the game tab should be set to full HD (1920x1080)

#### Official Builds
Official builds can be obtained off of our ci/cd pipeline. We offer Linux, Windows, and Unofficial Mac support(Never tested) This is the best way to play our game as it will be more optimized and allow for a better experience.
https://gitlab.larrycloud.ca/cis4820/Beeverse

#### Packaged Builds
Package builds can be found under Builds/[linux/windows/macos]

## Controls

#### Camera Movement
  - [W] : forward
  - [S] : backwards
  - [A] : left
  - [D] : right
  - [Q] : rotate left
  - [E] : rotate right
  - [Q] + Right-Click : rotate down
  - [E] + Right-Click : rotate up
  - [SPACE] : reset camera
  - Middle Scroll : zoom in/out

### Game Interactions
- To build new Structures, Left-Click the desired structure from the Honeycombs Menu on the right of the screen
  - The valid buildable areas will be highlighted
  - Left-Click in the desired location to create the structure
  - If you cannot afford a structure, the option will be greyed out.
- To open Structure menus, Left-Click on the structure (Honeycombs, Flowers)
  - To assign/remove Bees from the structure, Left-Click on a work slots bee portrait, and select the Bee from the pop-up
  - NOTE: The Queen Honeycomb is intentionally not interactable
- To see information about a Bee, Left-Click on the desired Bee to open their profile
- After an Egg has been laid in the Brood Nest, Left-Click it to open an Egg Type selection menu
  - To grow the Egg into a Worker Bee, select the option for 10 Royal Jelly + 40 Honey
  - To grow the Egg into a Queen Bee, select the option for 50 Royal Jelly
    - NOTE: You may only grow one Queen Bee Egg at a time, so the option will be greyed out if you already have one.
  - If you cannot afford an option, the option will be greyed out.


## Game Assets

A combination of free assets sourced online and our own assets created in Blender were used to build the game.

#### Created Assets
- All songs are originally created and played by Lawrence Milne
- Sound effects for Bee death and building created by Lawrence Milne
- Bee and Honeycomb models created in Blender in collaboration between Brittany Mueller and Lawrence Milne
- Bee Animations created in Blender in collaborations between Brittany Mueller and Lawrence Milne
- All Blender models (and their animations) can be found in /BlenderModels 

#### Third Party Assets
- Resource Icons - https://www.flaticon.com/
- UI Buttons/Backgrounds - https://assetstore.unity.com/packages/2d/gui/fantasy-wooden-gui-free-103811
- Wood Textures - https://assetstore.unity.com/packages/2d/textures-materials/wood/hand-painted-seamless-wood-texture-vol-6-162145
- Flower Assets - https://assetstore.unity.com/packages/3d/environments/landscapes/free-low-poly-nature-forest-205742
- Sky Asset - https://assetstore.unity.com/packages/3d/simple-sky-cartoon-assets-42373
- Scroll/Progress Bar and Settings Icon - https://assetstore.unity.com/packages/2d/gui/sweet-land-gui-208285
- Ambient Bee Noises https://pixabay.com/sound-effects/id-24412/

## Disclaimer
Save and Load functionality do not currently exist but the buttons were left in the game to preserve the UI. Save/Load was not part of our original plan, we plan to expand this as a personal project after submission.
