#! /bin/sh

BASE_URL=https://download.unity3d.com/download_unity
HASH=5d30cf096e79
VERSION=2017.1.1f1

download() {
  file=$1
  url="$BASE_URL/$HASH/$package"

  echo "Downloading from $url: "
  cd Unity
  curl -o `basename "$package"` "$url"
  cd ../
}

install() {
  package=$1
  filename='basename "$package"'
  packagePath="Unity/$filename"
  if [ ! -f $packagePath ] ; then
    echo "$packagePath not found. Downloading 'basename "$packagePath"'"
	download "$package"
  fi

  echo "Installing "`basename "$package"`
  sudo installer -dumplog -package `basename "$package"` -target /
}

if [ ! -d "Unity" ] ; then
  mkdir -p -m 777 Unity
fi

install "MacEditorInstaller/Unity-$VERSION.pkg"
install "MacEditorTargetInstaller/UnitySetup-Windows-Support-for-Editor-$VERSION.pkg"