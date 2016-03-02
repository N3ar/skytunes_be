#!/bin/bash
echo "Beginning ffmpeg installation"
function dependencies {
	echo "Installing dependencies"
	apt-get update
	apt-get -y --force-yes install autoconf automake build-essential libass-dev libfreetype6-dev \
	  libsdl1.2-dev libtheora-dev libtool libva-dev libvdpau-dev libvorbis-dev libxcb1-dev libxcb-shm0-dev \
	  libxcb-xfixes0-dev pkg-config texinfo zlib1g-dev
	mkdir ~/ffmpeg_sources
	echo "Dependency installation complete"
}
function yasm {
	echo "Installing yasm"
	cd ~/ffmpeg_sources || exit
	wget http://www.tortall.net/projects/yasm/releases/yasm-1.3.0.tar.gz
	tar xzvf yasm-1.3.0.tar.gz
	cd yasm-1.3.0 || exit
	./configure --prefix="$HOME/ffmpeg_build" --bindir="$HOME/bin"
	make
	make install
	make distclean
	echo "Yasm installation complete"
}
function libx264 {
	echo "Installing libx264"
	cd ~/ffmpeg_sources || exit
	wget http://download.videolan.org/pub/x264/snapshots/last_x264.tar.bz2
	tar xjvf last_x264.tar.bz2
	cd x264-snapshot* || exit
	PATH="$HOME/bin:$PATH" ./configure --prefix="$HOME/ffmpeg_build" --bindir="$HOME/bin" --enable-static
	PATH="$HOME/bin:$PATH" make
	make install
	make distclean
	echo "Libx264 installation complete"
}
function libx265 {
	echo "Installing cmake mercurial"
	apt-get install cmake mercurial
	cd ~/ffmpeg_sources || exit
	hg clone https://bitbucket.org/multicoreware/x265
	cd ~/ffmpeg_sources/x265/build/linux || exit
	PATH="$HOME/bin:$PATH" cmake -G "Unix Makefiles" -DCMAKE_INSTALL_PREFIX="$HOME/ffmpeg_build" -DENABLE_SHARED:bool=off ../../source
	make
	make install
	make distclean
	echo "Cmake Mercurial installation complete"
}
function libfdk-aac {
	echo "Installing libfdk-aac"
	cd ~/ffmpeg_sources || exit
	wget -O fdk-aac.tar.gz https://github.com/mstorsjo/fdk-aac/tarball/master
	tar xzvf fdk-aac.tar.gz
	cd mstorsjo-fdk-aac* || exit
	autoreconf -fiv
	./configure --prefix="$HOME/ffmpeg_build" --disable-shared
	make
	make install
	make distclean
	echo "libfdk-aac installation complete"
}
function libmp3lame {
	echo "Installing libmp3lame"
	apt-get install nasm
	cd ~/ffmpeg_sources || exit
	wget http://downloads.sourceforge.net/project/lame/lame/3.99/lame-3.99.5.tar.gz
	tar xzvf lame-3.99.5.tar.gz
	cd lame-3.99.5 || exit
	./configure --prefix="$HOME/ffmpeg_build" --enable-nasm --disable-shared
	make
	make install
	make distclean
	echo "libmp3lame installation complete"
}
function libopus {
	echo "Installing libopus"
	cd ~/ffmpeg_sources || exit
	wget http://downloads.xiph.org/releases/opus/opus-1.1.tar.gz
	tar xzvf opus-1.1.tar.gz
	cd opus-1.1 || exit
	./configure --prefix="$HOME/ffmpeg_build" --disable-shared
	make
	make install
	make clean
	echo "Libopus installation complete"
}
function libvpx {
	echo "Installing libvpx"
	cd ~/ffmpeg_sources || exit
	wget http://storage.googleapis.com/downloads.webmproject.org/releases/webm/libvpx-1.5.0.tar.bz2
	tar xjvf libvpx-1.5.0.tar.bz2
	cd libvpx-1.5.0 || exit
	PATH="$HOME/bin:$PATH" ./configure --prefix="$HOME/ffmpeg_build" --disable-examples --disable-unit-tests
	PATH="$HOME/bin:$PATH" make
	make install
	make clean
	echo "Libvpx installation complete"
}
function ffmpeg {
	echo "************FINALLY INSTALLING FFMPEG**************"
	cd ~/ffmpeg_sources || exit
	wget http://ffmpeg.org/releases/ffmpeg-snapshot.tar.bz2
	tar xjvf ffmpeg-snapshot.tar.bz2
	cd ffmpeg || exit
	PATH="$HOME/bin:$PATH" PKG_CONFIG_PATH="$HOME/ffmpeg_build/lib/pkgconfig" ./configure \
	  --prefix="$HOME/ffmpeg_build" \
	  --pkg-config-flags="--static" \
	  --extra-cflags="-I$HOME/ffmpeg_build/include" \
	  --extra-ldflags="-L$HOME/ffmpeg_build/lib" \
	  --bindir="$HOME/bin" \
	  --enable-gpl \
	  --enable-libass \
	  --enable-libfdk-aac \
	  --enable-libfreetype \
	  --enable-libmp3lame \
	  --enable-libopus \
	  --enable-libtheora \
	  --enable-libvorbis \
	  --enable-libvpx \
	  --enable-libx264 \
	  --enable-libx265 \
	  --enable-nonfree
	PATH="$HOME/bin:$PATH" make
	make install
	make distclean
	hash -r
	source ~/.profile
	echo "************FFMPEG INSTALLATION COMPLETE*************"
}
echo "Attempting to install dependencies:"
dependencies
echo "Attempting to install Yasm:"
yasm
echo "Attempting to install libx264:"
libx264
echo "Attempting to install libx265:"
libx265
echo "Attempting to install libfdk-aac:"
libfdk-aac
echo "Attempting to install libmp3lame:"
libmp3lame
echo "Attempting to install libopus:"
libopus
echo "Attempting to install libvpx:"
libvpx
echo "Attempting to install ffmpeg:"
ffmpeg
echo "Installation sequence complete. Terminating..."
