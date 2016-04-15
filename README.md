# findbigfiles
A dumb utility to find those big files that eat your precious free hdd space in the depth of the file system tree.

## Usage:
**findbigfiles** -dir=&lt;path&gt; [-size=&lt;XX&gt;] [-time=&lt;XX&gt;&lt;m|h|d|w&gt;] [-exec="a command to execute"]
  + -dir	Directory to traverse
  + -size	Skip files, smaller than this value. By default, the value is in bytes. You may add K,M, and G suffixes as well.
  + -time	Skip files changed earlier than the given timespan. You must use suffixes m,h,d,w for minutes, hours, days, and weeks respectively.
  + -exec Execute a command on the files found instead of printing the list. File name will be appened to the end of the command line, or placed instead of {}.