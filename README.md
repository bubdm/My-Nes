# MY NES
my NES is a portable open source NES/Famicom emulator written in C#.


**Please note that this is the official repository of the program, that means official updates on source and releases will be commited/published here.**

### [DOWNLOAD LATEST RELEASE (VERSION)](https://github.com/alaahadid/My-Nes/releases)
### [GETTING STARTED ? CLICK HERE](https://github.com/alaahadid/My-Nes/wiki)

For Linux users, or users who experince performance issues, there is a version of My Nes written in C language can be found here: <https://github.com/alaahadid/My-C-Nes/tree/v1.0>. My C Nes have the same emulation features, better performance but still basic command-line application.

## Introduction
My Nes is A Nintendo Entertainment System / Family Computer (Nes/Famicom) emulator written in C#. 
An open source .net freeware, licensed under the GNU GENERAL PUBLIC LICENSE; Version 3, 29 June 2007.

The main goal of My Nes is to be as accurate as possible and brings the best game play experince possible.

My Nes can pass all basic nes tests that test nes hardware behaviors, such as cpu 6502 instructions, 
ppu timing .... etc. However, My Nes pass most of these tests (known at development time of current version) 
without any kind of work-arounds (changing some code to make a test pass), by emulating the exact hardware behavior.

My Nes Able to decode nes colors without the need of using palette, includes gray scale and emphasize. For NTSC, PALB and DENDY.
Expect real NES COLORS as they should be !!
Also the exact audio mixer as descriped at http://wiki.nesdev.com/w/index.php/APU_Mixer (i.e. low-pass and high-pass filters) is implemented.
Video and audio outputs are configured carefully to produce video and sound as accurate as possible.

If you are looking for an easy-to-use Nes emulator, meant for game play expericne and provides accuracy that very close to the real hardware, 
My Nes is a one that worth a try !

## Features An Specification
- My Nes uses simple GUI (Graphical User Interface) which allows to change game, configure emulation directly 
  and as simple as possible.
- My Nes include built-in Launcher which can be used to organize games, it can provide detailed information about
  games, also it can record user data such as rating, play time… etc
- My Nes Uses NesCart DB to show and use accurate game information.
- My Nes include built-in rendering engine that allows to switch between renderers easily and effectively. This can 
  help to select the proper renderer that suits up your system, for example, if the SlimDX video renderer run with 
  problems with you machine, simply switch to SDL2 video renderer, which may run smooth in you pc. 
- All useful video options are included, such as running the emu in windowed or fullscreen mode with “keep aspect ratio” 
  turned on or off.
- Ability to save snapshots of current game.
- Ability to record sound for each game.
- Save and load state ability.
- My Nes runs very fast in low end machines (such as low end laptops, old pcs ..etc), My Nes is tested in old machines, 
  both in Windows and runs perfectly 60 fps.

### My Nes Core Features

- Accuracy, My Nes pass almost all known nes tests by emulating the real hardware behaviors without any kind of emulation 
  hack.
- Multi-threaded Emulator, the emulation process run in thread separated from renderer threads. 
  This may improve performance especially with multi core cpus. 

### My Nes Emulation Specification

- CPU 6502: All CPU 6502 instructions implemented including the so called illegal opcodes.
  Exact interrupt timings like interrupt check before the last instruction behavior.
- APU: all Nes 5 sound channels, MMC5 external sound channels and VRC6 external sound channels.
  Emulates the APU-CPU write/read behaviors (exact apu clock timing)
  Exact Mixer as descriped at http://wiki.nesdev.com/w/index.php/APU_Mixer (i.e. low-pass and high-pass filters) 
  Thus, the audio process works like this:
  EMU OUTPUT at ~1.79 MHz => 
  high-pass filter at 90 Hz => 
  high-pass filter at 440 Hz => 
  low-pass filter at 14 kHz => 
  write samples into playback buffer (i.e. play sound).
- PPU: Picture Processor Unit as described in the wiki docs <http://wiki.nesdev.com/w/index.php/PPU_rendering> .
  is implemented (as close as possible) with exact timing, also ppu vram-bus and io-bus are implemented as well.
- Colors: Ability to decode nes colors without the need of using palette, includes gray scale and emphasize. For NTSC, PALB and DENDY.
- TV Formats: NTSC, PALB and DENDY. 
- Video Output: Resolution can be upscaled from res 256 x 240 nes basic till 1920 x 1080 Full HD and even higher resolutions. 
  Uses Resolution-Blocks-Upscaler method <https://github.com/alaahadid/Resolution-Blocks-Upscaler>.
- Sound Playback: playback frequency can be 22050 Hz, 44100 Hz or 48000 HZ. 
  Bit rate fixed to 16 bit, channels fixed to Mono. Includes built-in wave recorder.
- Mappers And Boards: Implement about 97% of known and documented mappers
- Controllers: 4 players joypads, each joypad is playable through Keyboard, Joystick or XBox360 Game Controller (XInput). 
  Game Genie is implemented as well.

## System Requirements
Usually My Nes comes in portable package, which can be installed simply by extracting the content of that package 
anywhere in your machine.
Note that My Nes save settings and user files (such as states, snapshots, sound records ...etc) at the documents.
In order to run My Nes correctly in your machine, please make sure that your machine meets up these requirements:

- My Nes can run at any version of windows that can run .net framework 4, such as Windows 7, 8, 8.1 and 10.
- .Net Framework version 4 is required.
- Latest DirectX package from Microsoft.
- Latest C++ Runtime package from Microsoft. (Try latest, if My Nes doensn't work, installing older version of this package may work.)
- CPU: 2400 MHz or faster, multicore cpu is recommended for better performance. My Nes is built for x86 cpus, 
  but it should run without problems with x64 cpus (tested and runs perfectly)
- RAM: My Nes usually uses about 30 to 60 MB ram. When launcher is used, it may use up to 200 MB. In other words, 
  since Windows is running perfectly in your machine, you should not worry about ram at all when using My Nes.

## Notes
- My Nes doesn't work:
Please make sure that these packages are installed in pc:

**.Net framework 4**

**C++ Runtime** (Try latest, if My Nes doensn't work, installing older version of this package may work.)

If the problem isn't solved, please try to install SlimDX latest runtime, one can be found here: <https://code.google.com/archive/p/slimdx/downloads> (SlimDX Runtime .NET 4.0 x86 (January 2012).msi).


- If some games doesn't work
  Please use Hard Reset (F4) to reset the emulation, happen sometimes when changing games. If the game stil doesn't work, that's mean it is a not-implemented mapper issue or emulation bug. Reporting the game problem to the <https://github.com/alaahadid/My-Nes/issues> will be appreciated.

- If My Nes fails to start (mostly on Windows 7), please intstall SlimDX Runtime .NET 4.0 x86 (January 2012).
  <https://github.com/likeleon/Micro/blob/master/Externals/SlimDX%20SDK%20(January%202012)/Runtime/SlimDX%20Runtime%20.NET%204.0%20x86%20(January%202012).msi>

- If MyNesGTK fails to launch (in Windows):
  Go to folder "C:\Program Files (x86)\GtkSharp\2.12\bin", copy all files then paste them in MyNesGTK folder.

- If My Nes GTK crashes when attempting to open a settings dialog, please try to run a game first then try again.
  This issue is related to the configuration files that are in documents/home folder, if they are missing, My Nes GTK may crashes when try to access them.

- My Nes may fail to launch with SDL2 video renderer, or just "White Screen".
  If that happens, you need to locate the settings file in:

  Documents>MyNes>sdlsettings.ini

  Open that file then edit the line (set it to):
  "Video_Driver=opengl" or "Video_Driver=direct3d" then try again.
  direct3d: is the direct3d driver
  opengl: is the opengl driver

  If both drivers doesn't work, there is no choice but to switch into SlimDX video renderer.

- There is no sound whith SDL2 Audio.
  Please try to change audio device selection in SDL2 Settings window.

  If that doesn't work, you need to locate the settings file in:

  Documents>MyNes>sdlsettings.ini

  Open that file then find the line :
  "Audio_Device_Index=0"
  This is the audio device index, 0 is the first audio device, 1 is the second ...etc
  Can be set to any "Enabled" audio device, please set the index to the default audio device in Windows audio settings.

  If both drivers doesn't work, there is no choice but to switch into SlimDX audio renderer.
  
- Video Output: Resolution can be upscaled from res 256 x 240 nes basic till 1920 x 1080 Full HD and even higher resolutions. 

  Uses Resolution-Blocks-Upscaler method <https://github.com/alaahadid/Resolution-Blocks-Upscaler>.

  Resolution rendering is done in real time, directly after producing video output of quality 256 x 240. The result is creystal clear next to real tv video rendering.
 
  Resolution upscale can be disabled from Main Menu > Resolution Upscale. The image quality also depends on filter used for scaling from selected res: linear or point.

  Custom resolution can be set by locating the settings file in:

  `Documents>MyNes>renderersettings.ini`

  Open that file then find the lines :

  `Vid_Res_W=640`

  `Vid_Res_H=480`

  Change `Vid_Res_W=640` to the width of any resolution needed (for example `Vid_Res_W=1920`) and `Vid_Res_H=480` to the height of the new res (for example `Vid_Res_H=1080`).

  Wrong resolution results distorted images. 

  **My Nes can renderer resolution higher than full-hd but requires very powerfull pc. Custom resolution need to be set for reolutions higher than 1920 x 1080**
