# skytunes_be
Scripts and instructions associated with setting up/ supporting skytunes server infrastructure

## Installation Scripts
FFmpeg installation script: run with sudo to install ffmpeg and dependencies on ubuntu 14.04 Trusty
  sudo ./ffmpeg-install-scriptname
  ---- if that doesn't work right due to cd behavior in shell scripts:
  sudo . ./ffmpeg-install-scriptname

## Downsample Music
*In progress*
Planned script functionality:
- Audio file --passed to--> getID3 --meta determines downsampling--> FFmpeg --downsampled files labeled and saved--|
  - TODO: File distribution
  - TODO: Finish and test console commands called
  - TODO: File name sanity check to prevent exploit via filename

## Upload Script
*In progress*

## Determine client bandwidth from server
*In progress*
TODO: Script being adapted needs to be altered to fit our usecase.

## External Extentions
FFmpeg: Script used to downsample audio files
getID3: Class that gets all assocaited song meta for all song extentions
