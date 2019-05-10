# Rice Games Unity

Example Project for Rice Games

+ Script 1: PlayerMovement.cs
	- This script is simple, it controls the players actions in the "overworld", a top-down perspective 3D space.
	- The keys 'w', 'a', 's', 'd' and UP, DOWN, LEFT, RIGHT can be used to move.
	- The script also has control for the player engaging with the dialouge system, it sends a RayCast out of the character when MOUSE1 (Left-Click) is pressed.
	- The last feature in this script is the NPC cooldown, which prevents a player who is repeatedly pressing the interaction button from getting caught in a loop of speaking to the same character forever.
	
+ Script 2: DialougeManager.cs
	- As mentioned in the previous script, this project includes a dialouge system, which is controlled by this script.
	- The system is built so that it can be applied to any NPC, and takes strings from the NPC's data to print onto the screen. With this system, it is simple to give any NPC unique dialouge in the Unity Editor.
	- The dialouge system has scrolling text, this is done by displaying a substring of the dialouge string to the screen and gradually increasing the substring every few frames.
	- The dialouge system also has a prompt system. This prompt can be synced with the battle script to allow any NPC to use the prompt to request a battle.
	
+ Script 3: BattleManager.cs
	- The nature of this script means it will continously be developed as more cards with new effects are added to the game. As such, it is **INCOMPLETE** and has bugs, many of which are specified in the comments.
	- This script is built to control an entire turn based card game system. It cycles through the player and then enemy turns.
	- The script cycles through phases and checks all cards in play for an effect that is activatable. If so, it allows the player to activate. The SPACE key is used to continue the phases. A cooldown is implemented so it cannot be spammed.
	- The enemy AI is not fully implemented, so occasionally they will freeze and make no moves.
