# Changes

Please when pushing updates, add your update info below. We will delete this after project is not private.

## Changes:

+ Removed Item Builder
+ Added in Loot Table Builder.

# Mod the Gungeon

MtG is my go at modding Enter the Gungeon!

### Want to contribute?

You can:

* Add Harmony Patches - events. Please follow the design pattern
* Create Item Building 
* Add in utilities
* Create plugins

## Why choose this loader?

* Open source
* Plugins
* Free feeling mods, like your are making your own Unity script
* Harmony Patching
* Settings
* Custom Mod Loading with plugins
* Custom Debugging
* Extensive debugging with Unity Explorer

## Install

1. Download this release [file](https://github.com/BIGDummyHead/MtG/releases/tag/1.0.0.0)
2. Unpack into Enter the Gungeon.exe directory.
3. Enjoy

## Enable Developer Mode

#### Simple Install
1. You can enable developer mode with UnityExplorer.
2. Simply download the plugin provided in the source
3. Drop and unpack into Gungeon.exe

### Create 

1. Create your .Net Framework 3.5 project, make either a IPlugin or IMod class
2. In the load method, include this piece ``Boot.DeveloperModeEnabled = true;``
3. Create a Plugins or Mods folder in the Gungeon.exe
4. Drop your .dll file into the Plugins or Mods folder

## Licensing

#### This project is licensed under the [MIT](https://github.com/BIGDummyHead/MtG/blob/master/LICENSE) license

## Wiki

This project has a wiki which can be found [here](https://github.com/BIGDummyHead/MtG/wiki)

## Dependencies

#### [Harmony](https://github.com/pardeike/Harmony) by [pardeike](https://github.com/pardeike)

#### [Unity Explorer](https://github.com/sinai-dev/UnityExplorer) by [sini-dev](https://github.com/sinai-dev)

## Helpful Modding Tools

#### [dnSpy](https://github.com/dnSpy/dnSpy), .Net Debugger and Assembly editor. Contributors can be found [here](https://github.com/dnSpy/dnSpy/graphs/contributors)

_________________________________________________
