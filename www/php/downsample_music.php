<?php
/**
 * Created by PhpStorm.
 * User: jwarne
 * Date: 3/2/16
 * Time: 2:21 PM
 */

define("MAX_FILE_BITRATE", 1411); // Currently highest supported bitrate by filetype in kb/s

define("WAV_LOSSLESS", 9);
define("OTHER_LOSSLESS", 8);
define("HIGH_QUALITY", 7);
define("GREAT_QUALITY", 6);
define("GOOD_QUALITY", 5);
define("MID_QUALITY", 4);
define("LOW_MID_QUALITY", 3);
define("LOW_QUALITY", 2);
define("ABYSMAL", 1);

// Check file bitrate to confirm file validity
// TODO: Integrate into proper error checking in upload_music.php
function check_file_bitrate($bitrate) {
    return (bool) ($bitrate > MAX_FILE_BITRATE);
}

// Assign downsampling index
// TODO: Test this
function assign_downsample_index($bitrate, $index) {
    // Not currently supported bitrates in Kb/s: 9600, 6144, 5644.8, 1411.2
    if($bitrate >= 1411) {
        echo nl2br("A .wav file.\n");
        $index = 9;
    } else if($bitrate >= 400) {
        echo nl2br("Lossless quality.\n");
        $index = 8;
    } else if($bitrate >= 320) {
        echo nl2br("High quality.\n");
        $index = 7;
    } else if($bitrate >= 256) {
        echo nl2br("Great quality.\n");
        $index = 6;
    } else if($bitrate >= 192) {
        echo nl2br("Good quality.\n");
        $index = 5;
    } else if($bitrate >= 160) {
        echo nl2br("Mid quality.\n");
        $index = 4;
    } else if($bitrate >= 128) {
        echo nl2br("Low mid quality.\n");
        $index = 3;
    } else if($bitrate >= 96) {
        echo nl2br("Low quality.\n");
        $index = 2;
    } else if($bitrate >= 32) {
        echo nl2br("Barely worth it.\n");
        $index = 1;
    }

    return $index;
}

// TODO: Check to ensure filename isn't a shell hack
// TODO: Finish and test console commands
function create_downsamples($index, $path) {
    while ($index != 0) {
        switch($index) {
            case WAV_LOSSLESS:
                // exec(ffmpeg -i /*filename*/ -c:a libfdk_aac -b:a 400k /*filename_400.mp4);
                break;
            case OTHER_LOSSLESS:
                // exec(ffmpeg -i /*filename*/ -c:a libfdk_aac -b:a 400k /*filename_400.mp4);
                break;
            case HIGH_QUALITY:
                // exec(ffmpeg -i /*filename*/ -c:a libfdk_aac -b:a 400k /*filename_400.mp4);
                break;
            case GREAT_QUALITY:
                // exec(ffmpeg -i /*filename*/ -c:a libfdk_aac -b:a 400k /*filename_400.mp4);
                break;
            case GOOD_QUALITY:
                // exec(ffmpeg -i /*filename*/ -c:a libfdk_aac -b:a 400k /*filename_400.mp4);
                break;
            case MID_QUALITY:
                // exec(ffmpeg -i /*filename*/ -c:a libfdk_aac -b:a 400k /*filename_400.mp4);
                break;
            case LOW_MID_QUALITY:
                // exec(ffmpeg -i /*filename*/ -c:a libfdk_aac -b:a 400k /*filename_400.mp4);
                break;
            case LOW_QUALITY:
                // exec(ffmpeg -i /*filename*/ -c:a libfdk_aac -b:a 400k /*filename_400.mp4);
                break;
            case ABYSMAL:
                // exec(ffmpeg -i /*filename*/ -c:a libfdk_aac -b:a 400k /*filename_400.mp4);
                break;
        }
        $index -= 1;
    }
    // File original file
    // exec(ffmpeg -i /*filename*/ -c:a libfdk_aac /*filename_ORIG.mp4);
}