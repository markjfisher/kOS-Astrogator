# kOS-Astrogator

[Astrogator](https://github.com/HebaruSan/Astrogator) functionality in kOS.

See [Release Readme](GameData/kOS-Astrogator/README.md) for mod usage information.

## Building

### Visual Studio 2022 in Windows

Create 2 symlinks in [Source](Source) as follows:

```
mklink /j Source\KSP_Data "C:\your\path\to\Steam\steamapps\common\Kerbal Space Program\KSP_x64_Data"
mklink /j Source\KSP_GameData "C:\your\path\to\dev\mods\"
```

The second link should go to a directory with any mods required for compiling this source, simply unpack
the mods as normal from installation zips, or copy them from KSP's mods folder.

Mods required are:
- Astrogator
- kOS

Warning: If you link this to your main KSP mods folder, you may get compilation issues, as some mods
have Assembly files included in their distribution, that will break the build of this module.
That took me a while to find :sad-panda:.

Now the base game libraries and mods are available, run Visual Studio 2022, and run a realease build.

You can use something like [this copy script](copyToKSP.bat) to copy the distribution to your KSP folder,
but you will have to change the various paths in it to suit your own installation.
