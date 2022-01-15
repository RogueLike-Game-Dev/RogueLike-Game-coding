# Rogue World

## Introduction

The game is a 2D roguelike platformer, aiming to defeat enemies and collect as much coins as possible in order to buy various upgrades (extra HP, movement speed, damage and others) or consumable items, such as revive, invisibility and invulnerability.

The main source of inspiration was [Rogue Legacy 2](https://www.youtube.com/watch?v=2yi1pXd9AyY).

## Description

### **Main menu**

At the beginning of the game, a menu appears, in which you can change the settings related to the volume of the background music, as well as keybindings for character control. In addition to these settings, the player can choose to start a new game or continue an older run, collecting more gold (the game is saving both the settings for background music, the time played so far, the gold collected and the various purchased things).

### **The Characters**

After the game starts, the player can choose the character to play with. There are 4 different characters, which differ in the special attack:

<p>
    <b>Zhax</b> can throw various objects, such as stones, foul-smelling fish or chicken legs.
    <image alt="Zhax" align="center" src="Assets/Characters/Satyrs/PNG/Satyr_02/PNG Sequences/Idle Blink/Satyr_02_Idle Blinking_000.png" width="150"/>
</p>

<p>
    <image alt="Demetria" align="center" src="Assets/Characters/Fallen_Angels_1/PNG/PNG Sequences/Idle/0_Fallen_Angels_Idle_000.png" width="150"/>
    <b>Demetria</b> has as a special attack a throwing fireball, which deals more damage to the enemy it hits.
</p>

<p>
    <b>Esteros</b> becomes invulnerable, destroying objects that cause damage (excluding enemies).
    <image alt="Esteros" align="center" src="Assets/Characters/Fallen_Angels_2/PNG/PNG Sequences/Idle/0_Fallen_Angels_Idle_000.png" width="150"/>
</p>

<p>
    <image alt="Lyn" align="center" src="Assets/Characters/Fallen_Angels_3/PNG/PNG Sequences/Idle/0_Fallen_Angels_Idle_000.png" width="150"/>
    <b>Lyn</b> attacks using lightning strike if he is close enough to the enemy.
</p>

### **The Enemies**

There are 4 types of enemies, each of them attacking in its own way:

-   a **Minotaur**, a melee enemy that starts following you if you get in its range
-   a **Fire Golem**, also melee, which patrols and starts chasing you while you are in his range
-   an **Ice Golem**, a turret throwing either ice shards or heavy rocks, dealing damage on collision
-   two types of **Wraith**, one of which follows you everywhere and can even fly through walls, dealing damage on collision, and the other one throwing deadly knives at you

### **The Shop**

The game also has a shop, where you can upgrade your skills in the Skill Mountain or buy consumable items.

### The Skills

-   **Armor** - when you take damage, the armor is damaged first, stopping the extra damage that you take when the armor is broken
-   **Max HP** - increases the maximum HP
-   **HP Regen** - if you take damage, in time, your health is regenerated
-   **Damage** - increases the damage amount dealt by the melee or the special attack
-   **Movement speed** - increases the movement speed of the character
-   **Triple jump** - instead of only 2 consecutive jumps, the player can jump 3 times if this skill is unlocked

### The Items

-   **Revive** - can be used once per run, reviving you if you fall or die in fight
-   **Immunity** - the enemies can attack you, but you take no damage from their hit
-   **Invisibility** - the enemies cannot see you while you are invisible, but you can still take damage from another dangerous items, such as Spikes or Lava

### **The Levels**

The game has 4 main levels and one boss fight level. Each of the main levels comes with its challenges, such as fighting with enemies, collecting items for an NPC, evading dangerous tunnels or finding the right path to the exit.

Each run is made up by multiple levels, chosen randomly. Once the player finishes the level, he may go to another one, through a door. With each completed level, the higher the chance to enter the final level in which the player has to defeat an intelligent boss (trained with AI).

## Gameplay

<image src="Screenshots/Menu.png" alt="Main Menu">
<image src="Screenshots/SkillMountain.png" alt="Skill Mountain">
<image src="Screenshots/ChooseCharacter.png" alt="Choose Character">
<image src="Screenshots/puzzle.png" alt="Dangerous Puzzle">

<p align="center">
<image src="Screenshots/Demetria.png" alt="Demetria" width="40%">
<image src="Screenshots/Zhax.png" alt="Zhax" width="50%">
</p>
<p align="center">
<image src="Screenshots/Esteros.png" alt="Esteros" width="40%">
<image src="Screenshots/Lyn.png" alt="Lyn" width="40%" margin="">
</p>
<br/>

## Resources

-   [Market Cartoon Tileset](https://craftpix.net/freebies/free-market-cartoon-2d-game-tileset/)
-   [Medieval Ruins Cartoon Tileset](https://craftpix.net/freebies/free-medieval-ruins-cartoon-2d-tileset/)
-   [Fallen Angels](https://craftpix.net/freebies/free-fallen-angel-chibi-2d-game-sprites/)
-   [Satyr](https://craftpix.net/freebies/free-satyr-tiny-style-2d-sprites/)
-   [Minotaurs](https://craftpix.net/freebies/free-minotaur-tiny-style-2d-sprites/)
-   [Golems](https://craftpix.net/freebies/free-golems-chibi-2d-game-sprites/)
-   [Wraiths](https://craftpix.net/freebies/free-wraith-tiny-style-2d-sprites/)
-   [Other Backgrounds](https://craftpix.net/freebies/free-horizontal-2d-game-backgrounds/)
