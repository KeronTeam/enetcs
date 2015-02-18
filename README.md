# ENetCS
![Linux Build Status](https://img.shields.io/travis/KeronTeam/enetcs.svg?label=linux)
![Windows Build Status](https://img.shields.io/appveyor/ci/gregoire-astruc/enetcs.svg?label=windows)

C#.NET binding for ENet

## Building
Get a copy of [Premake 5](http://premake.bitbucket.org/download.html#v5) for your OS.

Do not forget to update the `enet` submodule:

```sh
git submodule init
git submodule update
```

Now run premake:
```sh
premake5 --cc=clang gmake # Linux flavor. GCC would work as well.
premake5 vs2013           # Windows build.
```

Your solution/Makefile files will be in the `build` directory.


## Credits
Original credits to James *Zer* Bellinger. See `Licence.txt` for the original licensing terms,
and check out [ENet for C#](http://www.zer7.com/software/enetcs) for the original code.
