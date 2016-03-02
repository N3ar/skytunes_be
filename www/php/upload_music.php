<?php

// include getID3() library (can be in a different directory if full path is specified)
// TODO: need to check whether we actually need entire folder= ...
require_once('getid3/getid3.php');

define("VALID_MUSIC_FORMATS", array(
    'mp3',
    'mp4',
    'wav',
    'm4a'
));
define('kB', 1024);
define('MB', 1000*kB);
define("MAX_FILE_SIZE", 20*MB); // set to 20 MB
define("MUSIC_PATH_DEPTH", 3); //

// Initialize getID3 engine
$getID3 = new getID3;

// Returns appropriate directory path where the file should be stored.
function directory_path($file) {
    // TODO: use $fileName or some other paramter (audio fingerprint) to determine where to store files in the system
    // Might use MD5, SHA1, or some other hashing algorithm. To prevent directories from getting too large, can use user directories as well

    $sha1 = sha1_file($file); // hashes the actual file
    //$sha1 = sha1(basename( $file )); // 40 character length string is returned
    $str_array = str_split($sha1, MUSIC_PATH_DEPTH); // MUSIC_PATH_DEPTH cannot be greater than 40 (unless some other hash algorithm is used)

    $dir_path = "";
    for ($i = 0; $i < MUSIC_PATH_DEPTH; $i++) {
        $dir_path .= $str_array[$i] . "/";
    }

    return "../uploads/" . $dir_path;
}

// Check if $_FILES[][size] is bigger than allowed max size
// @param (integer) $size - Size of the uploaded file in bytes
function check_file_uploaded_size($size) {
    return (bool) ($size > MAX_FILE_SIZE);
}

function error_message($errorCode) {
    $message = "";
    switch($errorCode) {
        case UPLOAD_ERR_INI_SIZE:
            $message = "The uploaded file exceeds the upload_max_filesize directive in php.ini.\n";
            break;
        case UPLOAD_ERR_FORM_SIZE:
            $message = "The uploaded file exceeds the MAX_FILE_SIZE directive that was specified in the HTML form.\n";
            break;
        case UPLOAD_ERR_PARTIAL:
            $message = "The uploaded file was only partially uploaded.\n";
            break;
        case UPLOAD_ERR_NO_FILE:
            $message = "No file was uploaded.\n";
            break;
        case UPLOAD_ERR_NO_TMP_DIR:
            $message = "Missing a temporary folder.\n";
            break;
        case UPLOAD_ERR_CANT_WRITE:
            $message = "Failed to write file to disk.\n";
            break;
        case UPLOAD_ERR_EXTENSION:
            $message = "File upload stopped by extension.\n";
            break;
        default:
            $message = "Unknown upload error.\n";
            break;
    }

    return $message;
}

if ($_SERVER['REQUEST_METHOD'] == "POST" and isset($_POST["submit"]) and isset($_FILES['musicFilesToUpload'])) {
    // Loop through all files
    foreach ($_FILES["musicFilesToUpload"]["name"] as $i => $name) {
        if ($_FILES["musicFilesToUpload"]["error"][$i] !==  UPLOAD_ERR_OK) {
            $message = error_message($_FILES["musicFilesToUpload"]["error"][$i]);
            echo nl2br($message);
            continue;
        } else {
            $message = "";

            //$target_dir = directory_path( ($_FILES["musicFilesToUpload"]["name"][$i]) );
            $target_dir = directory_path( ($_FILES["musicFilesToUpload"]["tmp_name"][$i]) );

            $target_file = $target_dir . basename($_FILES["musicFilesToUpload"]["name"][$i]);
            //$target_file = $target_dir . basename($_FILES["musicFilesToUpload"]["tmp_name"][$i]);

            $uploadOk = 1;
            $musicFileType = pathinfo($target_file, PATHINFO_EXTENSION);

            // Allow only predefined file formats to be uploaded
            if (!in_array($musicFileType, VALID_MUSIC_FORMATS, true)) {
                // NOTE: in_array compares in case-sensitive manner
                $message .= "Sorry, " . $_FILES["musicFilesToUpload"]["name"][$i] . ", is not a valid format. Only MP3, MP4, WAV & M4A files are allowed.\n";
                $uploadOk = 0;
            }

            // Analyze file and store returned data in $ThisFileInfo
            $fileInfo = $getID3->analyze($_FILES["musicFilesToUpload"]["tmp_name"][$i]);
            //print_r($fileInfo);
            //echo nl2br("\n" . $fileInfo["fileformat"] . "\n");
            //echo nl2br($fileInfo["mime_type"] . "\n");

            // (Extra layer for checking types) Allow only predefined file formats to be uploaded
            if (!in_array($fileInfo["fileformat"], VALID_MUSIC_FORMATS, true)) {
                // NOTE: in_array compares in case-sensitive manner
                $message .= "Nice try ;) but " . $_FILES["musicFilesToUpload"]["name"][$i] . ", is not a valid format. Only MP3, MP4, WAV & M4A files are allowed.\n";
                $uploadOk = 0;
            }

            // Check file size (limit 20MB)
            if (check_file_uploaded_size($_FILES["musicFilesToUpload"]["size"][$i])) {
                $message .= "Sorry, " . basename( $_FILES["musicFilesToUpload"]["name"][$i]) . ", is too large.\n";
                $uploadOk = 0;
            }

            // Check if file already exists
            // TODO: might need to check if it exists only for current user, other people may have same file.
            if (file_exists($target_file)) {
                $message .= "Sorry, " . basename( $_FILES["musicFilesToUpload"]["name"][$i]) . ", already exists.\n";
                $uploadOk = 0;
            }

            // Check if $uploadOk is set to 0 by an error
            if ($uploadOk == 0){//0) {
                echo nl2br($message . "Sorry, " . basename( $_FILES["musicFilesToUpload"]["name"][$i]) . ", was not uploaded.\n");
            // if everything is ok, try to upload file
            } else {
                if ( ! is_dir($target_dir)) {
                    mkdir($target_dir, 0777, true);
                }

                if (move_uploaded_file($_FILES["musicFilesToUpload"]["tmp_name"][$i], $target_file)) {
                  echo nl2br("The file ". basename( $_FILES["musicFilesToUpload"]["name"][$i]) . ", has been uploaded.\n");

                  //$command = "../echoprint/echoprint-codegen " . $_FILES["musicFilesToUpload"]["tmp_name"][$i];
                  //echo exec($command, $out, $code);
                    //if ($code) {
                      //die("Error");
                    //} else {
                      //echo "Output was: ", join("\n", $out);
                    //};

                } else {
                    echo nl2br("Sorry, there was an error uploading " . basename( $_FILES["musicFilesToUpload"]["name"][$i]) . ".\n");
                }
            }
        }
    }
}
?>

