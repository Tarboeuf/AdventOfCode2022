using System;

namespace AoC.Day7
{

    [Day(ExpectedValue = "24933642")]
    public class Day07Puzzle2 : Day7PuzzleBase, IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            var fileSystem = GetTree(input);
            var totalSize = fileSystem.GetSize();
            var minSizeToDelete = totalSize - 40000000;

            return fileSystem.GetFiles()
                .Where(f => f.IsDirectory)
                .Select(f => f.GetSize())
                .OrderBy(s => s)
                .SkipWhile(s => s < minSizeToDelete)
                .First().ToString();
        }
    }
    [Day(ExpectedValue = "95437")]
    public class Day07Puzzle1 : Day7PuzzleBase, IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return GetTree(input).GetFiles()
                .Where(d => d.IsDirectory)
                .Select(d => d.GetSize())
                .Where(s => s <= 100000)
                .Sum().ToString();
        }
    }
    public abstract class Day7PuzzleBase
    {
        public MyDirectory GetTree(string input)
        {
            MyDirectory fileSystem = new MyDirectory("", "/", true, null);

            MyDirectory currentDirectory = fileSystem;
            foreach (var cmd in input.Split(Environment.NewLine))
            {
                if (cmd.StartsWith("$ cd "))
                {
                    string path = cmd[5..];
                    if(path == "..")
                    {
                        currentDirectory = currentDirectory!.Parent!;
                    }
                    else
                    {
                        currentDirectory = GetOrCreateDirectory(currentDirectory!, path);
                    }
                }
                else if (cmd.StartsWith("$ ls"))
                {

                }
                else if (char.IsDigit(cmd[0]))
                {
                    EnsureFileExists(currentDirectory!, cmd);
                }
                else if (cmd.StartsWith("dir"))
                {
                    EnsureDirectoryExists(currentDirectory!, cmd[4..]);
                }
            }
            return fileSystem;
        }

        private void EnsureFileExists(MyDirectory currentDirectory, string cmd)
        {
            var split = cmd.Split(' ');
            if (!currentDirectory.Files.Any(f => f.Name == split[1]))
            {
                currentDirectory.Files.Add(new MyDirectory(currentDirectory.Name, split[1], false, currentDirectory)
                {
                    Size = long.Parse(split[0])
                });
            }
        }

        private void EnsureDirectoryExists(MyDirectory currentDirectory, string v)
        {
            if (!currentDirectory.Files.Any(f => f.Name.EndsWith($"/{v}")))
            {
                currentDirectory.Files.Add(new MyDirectory(currentDirectory.Name, v, true, currentDirectory));
            }
        }

        private MyDirectory GetOrCreateDirectory(MyDirectory fileSystem, string path)
        {
            var split = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            string currentPath = "/";
            foreach (var f in split)
            {
                var current = fileSystem.Files.FirstOrDefault(d => d.Name == f);
                if (current == null)
                {
                    current = new MyDirectory(currentPath, f, true, fileSystem);
                    fileSystem.Files.Add(current);
                }
                currentPath += f + "/";
                fileSystem = current;
            }
            return fileSystem;
        }
    }

    public class MyDirectory
    {
        public string Name { get; }
        public string Path { get; }
        public MyDirectory? Parent { get; set; }
        public long Size { get; set; }
        public bool IsDirectory { get; set; } = false;

        public List<MyDirectory> Files { get; } = new List<MyDirectory>();

        public MyDirectory(string path, string name, bool isDirectory, MyDirectory? parent)
        {
            Size = 0;
            Name = name;
            Path = path;
            IsDirectory = isDirectory;
            Parent = parent;
        }

        public long GetSize()
        {
            return Size + Files.Sum(d => d.GetSize());
        }

        private int GetNbParent()
        {
            int nbParent = 0;
            var parent = Parent;
            while (parent != null)
            {
                nbParent++;
                parent = parent.Parent;
            }
            return nbParent;
        }

        public override string ToString()
        {
            string tree = $"{string.Concat(Enumerable.Repeat(' ', GetNbParent()))}- {Name} ({(IsDirectory ? 'd' : 'f')}) {GetSize()}\r\n";
            foreach (var item in Files)
            {
                tree += item.ToString();
            }
            return tree;
        }

        public IEnumerable<MyDirectory> GetFiles()
        {
            yield return this;
            foreach (var item in Files.SelectMany(f => f.GetFiles()))
            {
                yield return item;
            };
        }
    }
}
