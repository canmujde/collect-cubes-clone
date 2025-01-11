# README #

This repository is clone of Collect Cubes by Alictus.

### Review ###

https://github.com/user-attachments/assets/318e122c-6433-4e0f-acea-5b876748c021

### What is this repository for? ###

* Case Study

### Please note that ###
This project contains **my own Library** that contains several Managers, Extensions, Utilities.


### Task List ###

**The marks on the left indicate whether the task has finished.**

✅ : DONE

🟧 : DONE BUT IMPROVEABLE

🚧 : IN PROGRESS

🔴 : NOT DONE

###CORE###

✅ Create a character controller almost identical to Collect Cubes controller in
terms of game experience. Character speed, response time, movement delay
etc.

✅ ★ BONUS: Use scriptable objects to contain data that can variate game
experience.

✅ Use object pooling to create & destroy cubes in the game.

🟧 Create a level editor system.

✅ ★ BONUS: Your level editor reads an image pixel, places cubes and
attains their properties (colors, scales etc) based on this information.

✅ ★ BONUS: Create an input system inherited by UnityEngine’s EventSystem
interfaces. (for example IPointerClickHandler etc.)

✅ ★ BONUS: Apply relevant physics calculation tricks to increase performance of
the game and please do describe your approach in detail.

###TIMER FEATURE###

✅ Add a timer to your game scene that is connected to your level manager
system so that the given amount of time can be parameterized.

✅ Add a scoreboard to your game scene which is counting the amount of cubes
that are collected.

✅ Using your object pooling system that you have implemented in Part I, spawn
cubes continuously in the middle of the play area. (as shown with red arrow in
the image below)

✅ Again parametrize your spawn system with the points that you think that
might be important considering the game experience.

###AI FEATURE###

✅ Add a basic AI behavior that challenges the player during the gameplay.

🟧 The AI player and the user’s collector object cannot interact with each
other. Also, it is important that if a cube is currently being scoped, that
cube cannot be interacted until the carrier leaves it to the playground.

✅ The AI behavior should be simplified, and can be parameterized in order
to modify the challenging factor.

🔴 ★ BONUS: Add obstacles to the game area, which makes the collector
object explode and respawn at the start position. You should find a
proper solution to improve your AI to make it dodge the obstacles.



