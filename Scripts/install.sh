#! /bin/sh

# Example install script for Unity3D project. See the entire example: https://github.com/JonathanPorta/ci-build

# This link changes from time to time. I haven't found a reliable hosted installer package for doing regular
# installs like this. You will probably need to grab a current link from: http://unity3d.com/get-unity/download/archive
echo 'Downloading from https://download.unity3d.com/download_unity/5d30cf096e79/MacEditorInstaller/Unity-2017.1.1f1.pkg?_ga=2.75024402.128732811.1516556197-1007191227.1512993446: '
curl -o Unity.pkg https://download.unity3d.com/download_unity/5d30cf096e79/MacEditorInstaller/Unity-2017.1.1f1.pkg?_ga=2.75024402.128732811.1516556197-1007191227.1512993446

echo 'Installing Unity.pkg'
sudo installer -dumplog -package Unity.pkg -target /