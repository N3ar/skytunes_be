#!/bin/bash
echo "Attempting to install getID3"
function fetch {
	echo "Fetching gitID3"
	wget https://github.com/JamesHeinrich/getID3/archive/master.zip
	echo "getID3 retrieved"
	unzip master.zip -d /opt
	rm master.zip
}
fetch
echo "gitID3() function installed"
