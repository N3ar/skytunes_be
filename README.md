# skytunes_be
Scripts and instructions associated with setting up/ supporting skytunes server infrastructure

## Installation Scripts
FFmpeg installation script: run with sudo to install ffmpeg and dependencies on ubuntu 14.04 Trusty

## Script to downsample file
*In progress*
Planned script functionality:
- Audio file --passed to--> getID3 --meta determines downsampling--> FFmpeg --downsampled files labeled and saved--|
  - TODO: Metadata from getID3 needs to be saved or passed to appropriate handler
  - TODO: File distribution via vsync

## External Extentions
FFmpeg: Script used to downsample audio files
getID3: Class that gets all assocaited song meta for all song extentions
