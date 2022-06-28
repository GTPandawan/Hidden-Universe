# Hidden Universe Web Client

Flyff Universe Web Client allows you to play Flyff Universe natively
with dedicated client, QoL Improvments and Automation.
**This WebClient is against Flyff Universe ToS. Use at your own risk!**
**Built and Shared for educational porpuses only.**
**Forked from Flawkee's Project. All rights for building the base for this program (the webclient integration) goes to him.**

- Better FPS & Latency
- Flexible for your needs
- Ease of use for multipe characters in parallel
- Additional Quality of Life features
- Bots

## Features

- Cache is saved per profile meaning different settings and auto login for multiple characters is possible with one click!
- Set Window Mode / Full Screen.
- Set custom resolution.
- Launch the web client anywhere in your screen you'd like.
- Assist Full Support Mode Available!

**Assist Full Support Mode (/assistfs)**
A lot of Flyff players are playing with two characters in perallel for either leech or support using Assist/RM.
However, it causes a lot of inconvenience having to deal with two different game windows at the same time for each and every action needed.
Currently available using assistfs:
- **Auto Heal** - Enabling Auto Heal will constantly heal the target selected using the Action Slot.
    - Heal skill must be in the Action Slot.
    - Action Slot keybind must be configured with C key (Default).
- **Auto Buff** - Select the required Task Bars ID & Taskbar Slots and press Auto Buff to use them one after another allowing you to buff with a single click!
    -  Task Bars 1 - 3 must be configured with Keybinds F1-F3 (Default).
	-  Task Bar Slots must be configured with Keybinds 0-9 (Default).
    -  Since this is sequencial, please don't move with target character since you can miss out some buffs.
	-  You may select to Auto-Buff every certain amount of time OR manually by pressing the button
	-  Each time Auto Buff is initiated, Auto Heal stops.
- **Auto Follow** - Sometimes autofollow in Flyff breaks and you find your support not following you anymore. Auto Follow make sure each 5 seconds that you're following the selected target!
    - Follow Key must be configured for Z key (default).
	
**Leech Mode (/leech)**
In case you need the Auto Follow feature avilable in assistfs mode you can use this flag to enable it!


## Tech

Flyff Universe Web Client is writted in C# on .Net 4.5.2 using CefSharp - Open Source Wrapper for chromium framework.
Forked out of Flawkee's Flyff Universe Web Client project.

## Installation

.Net 4.5.2 is required.

1. [Download the latest release from the project's GitHub.](https://github.com/HiddenUniverse/Hidden-Universe/releases)
2. Extract it in a convinient location.
3. Create a shortcut of _"Hidden Universe Webclient.exe"_ and send it to your desktop (or any other location).
4. Configure flags and run!

## Configuration Flags

You can launch the web client with default setting as follows:
1. DefaultPorfile will be created in %appdata%\FlyffUniverse\ saving your character settings and cache.
2. Window mode is selected.
3. Resolution is set to 1920x1080.

However it is recommended to set and customize the flags per character needs using multiple shortcuts.
1. Create a shortcut for the web client .exe
2. Right click the shortuct and select properties
3. On **Target** add a space after the execution file path and add required flags

| flag | example | description |
| ------ | ------ | ------ |
| /ProfileName=<ProfileName> | /ProfileName=Flawkee | Creates a profile for the character saving it's settings & allow auto login. Use on per-character basis.
| /Resolution=<WidthxHeight> | /Resolution=1920x1080 | Set resolution for Window Mode (default: 1920x1080). Does not have effect in Fullscreen mode.
| /Fullscreen | /Fullscreen | Set Fullscreen mode.
| /PixelLocation=<x,y> | /PixelLocation=3850,-80 | Launch the web client in a specific pixel location on your Windows envrionment. Override /DisplayID flag.
| /DisplayID=<ID>| /DisplayID=2 | Launch the web client in a specific display. Use your display ID shown in Window Display Settings. Override /PixelLocation flag.
| /assistfs | /assistfs | Enable Assist Full Support mode - explained in Features section.
| /delaybb=<sec-in-ms> | /delaybb=2500 | Set the delay between buffs in milliseconds. Usefull when you need to adjust based on your casting speed. Default is 1500ms (1.5 seconds).
| /leech | /leech| Enable Leech mode - explained in Features section.

**Examples:**

Shortcut 1: _"C:\SomeFolderPath\FlyffUniverse-WebClient\Flyff Universe Webclient.exe" /profilename=Flawkee /fullscreen /displayID=2_

Shortcut 2: _"C:\SomeFolderPath\FlyffUniverse-WebClient\Flyff Universe Webclient.exe" /profilename=FlawkeeRM /pixellocation=3850,-80 /resolution=1600x2000 /assistfs_

Shortcut 3: _"C:\SomeFolderPath\FlyffUniverse-WebClient\Flyff Universe Webclient.exe" /profilename=FlawkeeRM2 /resolution=1600x2000 /assistfs /delaybb=800_

## KNOWN ISSUES

1. When dragging the web client (window mode) keyboard keys no longer register ingame.
	**WORKAROUND:** Click the in-game chat with your mouse and it should be working again.
   
2. After updating the game it loads up without my profile!
	**WORKAROUND:** Close the game and re-open it.

3. AutoBuff/AutoHeal/AutoFollow do not work!
	**FOLLOW THE INSTRUCTIONS!**
	Make sure your keyboard is set to English. Currently if the keyboard set to any other language it sends the wrong keystrokes to the client!
	In addition, make sure your game is configured correctly. The keybinds must be as follows:
	AutoFollow - Requires the Z key to be configured for Follow (Default).
	AutoHeal - Requires you to put the Heal Skill in your Action Bar and set the Action Bar Key to C Key (Default).
	AutoBuff - Required you to configure the Taskbars 1 to 3 keybinds to F1-F3 and your Taskbar Slots to 0-9 (Where 0 is the last one).