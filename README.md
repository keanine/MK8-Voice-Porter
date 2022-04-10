# MK8 Voice Porter
Currently only for Mario Kart 8 on WiiU. Port BARS/BFWAV/~~WAV~~* files to work with other characters!

When using BFWAV or WAV files as your input, make sure to name your input files using the User-Friendly filenames.
MK8-Voice-Porter can also detect the filenames from any character on the WiiU version of the game 
which is useful for quick porting, however it may be more useful in that situation to just use 
the BARS conversion.

You can also use this tool to change the names of your files to the User-Friendly version for easier editing.

# How to Use
### First Time Setup
- Download the latest version of MK8 Voice Porter
- Click "Extract Params..."
- In the first textbox, search for your content folder in your MK8 dump. For example, mine is "F:\AMKP\MARIO KART 8\content\"
- In the second textbox, search for your content folder in your MK8 update dump. For example, mine is "F:\AMKP\MARIO KART 8 UPDATE v64\content\". This can be skipped if you're not going to be porting to or from DLC characters.
- Click "Extract Params". The program will freeze for a moment. When it's unfrozen, close the Extract Params window.

### Porting one character's voice to another (BARS to BARS)
- Click "Input Folder", empty it if there's anything inside it, then put the .bars file of the characters voice you want to use inside the folder. I will use "SNDG_Daisy.bars" as the character I want to port from.
- In the "Output Format" dropdown select format you want, generally this will be .bars.
- In the "Output Driver" dropdown select the driver you would like to port over, I will select Mario.
- Click Port.
- Click OK.
- Click "Output Folder" to see the completed file, in my case I see "SNDG_Mario.bars". You can patch this in with whatever method you normally use to mod MK8 on console and on emulator.

### Creating a bars file from sound files. (BFWAV to BARS)
- Create or find your .bfwav files. If you don't know how to make these I can't help you. To name these files a list of User-Friendly names can be found below, however any vanilla file name can also be used if that is your preference.
- Click "Input Folder", empty it if there's anything inside it, then put your correctly named .bfwav/~~wav~~* files in the folder. 
- In the "Output Format" dropdown select format you want, generally this will be .bars.
- In the "Output Driver" dropdown select the driver you would like to port over.
- Click Port.
- Click OK.
- Click "Output Folder" to see the completed file. You can patch this in with whatever method you normally use to mod MK8 on console and on emulator.

\* Porting from WAV files has been removed until further notice.

# List of User-Friendly Filenames
```
SFX_HORN_OFF
SFX_HORN_ON
SFX_ITEM_HORN
SFX_BIKE_TRICK_SA
SFX_BIKE_TRICK_SB
SFX_KART_TRICK
SFX_KART_TRICK_SA
SFX_KART_TRICK_SB
RETRIEVED_FROM_LAVA_1
RETRIEVED_FROM_LAVA_2
RETRIEVED_FROM_WATER_1
RETRIEVED_FROM_WATER_2
DAMAGE_BOUND_1
DAMAGE_BOUND_2
DAMAGE_CRASH_1
DAMAGE_CRASH_2
DAMAGE_S_1
DAMAGE_S_2
DAMAGE_SPIN_1
DAMAGE_SPIN_2
DASH_1
DASH_2
DASH_3
DASH_4
DASH_S_1
DASH_S_2
DASH_S_3
DASH_S_4
FALL_LAVA_1
FALL_LAVA_2
FALL_SHORT_1
FALL_SHORT_2
FALL_WATER_1
FALL_WATER_2
FINISH_BAD_1
FINISH_BAD_2
FINISH_BAD_3
FINISH_GOOD_1
FINISH_GOOD_2
FINISH_GOOD_3
FINISH_FIRST_1
FINISH_FIRST_2
FINISH_FIRST_3
ANTI-GRAVITY_SPIN_1
ANTI-GRAVITY_SPIN_2
ANTI-GRAVITY_SPIN_3
ANTI-GRAVITY_DASH_1
ANTI-GRAVITY_DASH_2
ANTI-GRAVITY_DASH_3
ITEM_BLOOPER_1
ITEM_BLOOPER_2
ITEM_PLACE_BEHIND_1
ITEM_PLACE_BEHIND_2
ITEM_HIT_SUCCESS_1
ITEM_HIT_SUCCESS_2
ITEM_HIT_SUCCESS_3
ITEM_STAR_1
ITEM_STAR_2
ITEM_THROW_FORWARD_1
ITEM_THROW_FORWARD_2
TRICK_1
TRICK_2
TRICK_3
TRICK_SA_1
TRICK_SA_2
TRICK_SA_3
TRICK_SB_1
TRICK_SB_2
TRICK_SB_3
OVERTAKE_1
OVERTAKE_2
OVERTAKE_3
OVERTAKE_4
OVERTAKE_5
START_GLIDE_1
START_GLIDE_2
START_GLIDE_3
START_ANTI-GRAVITY_1
START_ANTI-GRAVITY_2
START_ANTI-GRAVITY_3
SLIPSTREAM_1
SLIPSTREAM_2
SLIPSTREAM_3
STALL_1
STALL_2
SELECT_DRIVER
UNLOCK_DRIVER
```
