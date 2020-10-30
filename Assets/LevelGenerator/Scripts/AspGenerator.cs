using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LevelGenerator.Scripts
{
    internal static class AspGenerator
    {
        private const string FilePath = "Assets/Resources/";
        private const string DefaultAspLogicFile = "Assets/Resources/Brain.asp";
        
        internal static string AspString(string name, List<string> values = null)
        {
            var asp = char.ToLower(name[0]) + name.Substring(1);
            if (values == null || values.Count == 0) return $"{asp}.";

            asp += '(';
            asp = values.Aggregate(asp, (current, value) => current + $"{value},");
            return asp.Remove(asp.Length - 1, 1) + ").";
        }

        internal static void CreateNewAspFile(Level level, string levelName)
        {
            string start = null;
            string end = null;
            var paths = new List<string>();
            foreach (var asp in level.ToAsp())
            {
                var startingLetter = asp[0];
                switch (startingLetter)
                {
                    case 'p':
                        paths.Add(asp);
                        break;
                    case 's':
                        start = asp;
                        break;
                    case 'e':
                        end = asp;
                        break;
                }
            }

            var newFile = $"{FilePath}{levelName}.asp";
            File.AppendAllText(newFile, File.ReadAllText(DefaultAspLogicFile));
            using (var streamWriter = File.AppendText(newFile))
            {
                streamWriter.WriteLine(
                    "\n% Others\n" +
                    $"\t{start} {end}\n"
                );
                streamWriter.WriteLine("% Paths");
                foreach (var path in paths)
                {
                    streamWriter.WriteLine($"\t{path}");
                }
            }

        }
    }
}