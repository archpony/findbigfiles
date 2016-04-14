# findbigfiles
A dumb utility to find those big files that eat your precious free hdd space in the depth of the file system tree.

## Usage:
**findbigfiles** -dir=<path> [-size=<XX>] [-time=<XX><m|h|d|w>]
  + -dir	Directory to traverse
  + -size	Skip files, smaller than this value. By default, the value is in bytes. You may add K,M, and G suffixes as well.
  + -time	Skip files changed earlier than the given timespan. You must use suffixes m,h,d,w for minutes, hours, days, and weeks respectively.
