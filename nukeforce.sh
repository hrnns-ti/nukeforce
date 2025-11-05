#!/bin/bash
cd ./ # lokasi .dll, sesuaikan jika perlu
minlen=1
maxlen=6
blocks=16

for len in $(seq 1 6); do
  kitty bash -c "dotnet bin/Debug/net9.0/NukeForce.dll $len 16; exec bash" &
done
