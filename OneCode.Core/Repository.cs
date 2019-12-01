﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OneCode.Core
{
    public class Repository
    {
        public string Folder { get; set; }
        public List<CodeFile> Files { get; set; }

        public static Repository Load(string folder)
        {
            return new Repository
            {
                Folder = folder,
                Files = Directory
                    .EnumerateFiles(folder, "*.cs", SearchOption.AllDirectories)
                    .Where(value => !value.EndsWith("AssemblyInfo.cs"))
                    .Select(path => CodeFile.Load(path, folder))
                    .ToList()
            };
        }
    }
}
