
# Unity Audio System

## Table of contents
1. [Description](#Description)
2. [Install](#Install)
3. [How to use](#How-to-Use)
	- [Namespace Usage](#Namespace-Usage)
	- [Components of Audio System](#Components-of-Audio-System)
	- [Play One Shot Sounds](#Play-One-Shot-Sounds)
	- [Play using Custom Sounds](#Play-using-Custom-Sounds)
	- [Change Audio Player Settings](#Change-Audio-Player-Settings)
4. [License](#License)

## Description
Simple, lightweight and Easy to use audio system for your own game in Unity Game Engine. 

---
## Install
- use the command at your desired download location 

git clone https://github.com/mohakdev/Unity-Audio-System.git

- Once installed open the repository folder in Unity
![FolderImage](https://i.imgur.com/zqmBTCm.png)
- Drag the Audio System GameObject into your first scene of the game
![DragPrefab](https://i.imgur.com/ZfhoMkf.png)  
- Go to Scripts/SoundClips.cs
![AudioManagerDir](https://i.imgur.com/MTrDwiw.png)
- Find SoundTypes enum. This is the place where you need to enter all the types of sound you want to play.
![SoundTypesEnum](https://i.imgur.com/hADJ2uF.png)
- Once added go back to Inspector, Click on AudioSystem GameObject and find Audio List.
![AudioList](https://i.imgur.com/lMt1IZv.png)
- Drag & Drop the sounds you want to play when a certain Sound Type is called.
![DragDropAudio](https://i.imgur.com/O0GbeDa.gif)
- Once all the audios are dragged and dropped you will be finished with the installation.
## How to Use
### Namespace usage
In whichever script you wanna play audio simply add 
```cs
using RadiantTools.AudioSystem;
```
**Note -> If you are using Assembly Definitions then don't forget to add this assembly as a reference.**

### Components of Audio System
***Audio Manager ->*** It controls and manages all the existing audio players. You can make or delete or get audio players using `AudioManager.cs`

***Audio Player ->*** Basically it controls an audio track. It is nothing but a script attached to a gameobject with `AudioSource` as a component. You can play Audio using a Audio Player. By default 2 AudioTracks exists which are Music and SoundSFX. Music Track has settings to make audio loop forever. While Ideally you should use SoundSFX to play one shot sounds like button click or victory sound etc. For more sounds like for example walking, car engine sound or something it is ideal to make your own custom audio track using Audio Manager.
![Heirarchy](https://imgur.com/x7xVu2w.png)

### Play One Shot Sounds
```cs
AudioPlayer soundSFX = AudioManager.Instance.GetAudioPlayer("SoundSFX");
soundSFX.PlayAudioOnce(SoundTypes.AudioOne);

//Using Custom Player to Play One Shot Sounds
AudioPlayer customPlayer = AudioManager.Instance.MakeAudioPlayer("Test");
customPlayer.PlayAudioOnce(SoundTypes.AudioOne);
```
### Play using Custom Sounds
To Play continuous sounds like walking , car engine use a custom track and manage it settings that way
```cs
AudioPlayer customPlayer = AudioManager.Instance.MakeAudioPlayer("Test");
customPlayer.SetAudioClip(customPlayer.GetAudioClip(SoundTypes.AudioTwo));
customPlayer.SetAudioSettings(loop: true);
customPlayer.PlayAudio();
```
### Change Audio Player Settings
 ```cs
 //Changing settings of audio as well
customPlayer.SetAudioSettings(loop: true, startOnPlay : true, volume : 1, pitch : 1);
 ```
## License
This code is licensed under MIT License which you can read [here](https://github.com/mohakdev/Unity-Audio-System/blob/main/LICENSE)
