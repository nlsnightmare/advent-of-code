#if true
var disk = new Disk(File.ReadAllText("./inputs/input.txt"));
#else
var disk = new Disk(File.ReadAllText("./inputs/example.txt"));
#endif



disk.Compact2();
var checksum = disk.Checksum();
Console.WriteLine(checksum);
