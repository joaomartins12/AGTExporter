using System;
using System.IO;

namespace RiceConvert
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.Title = "RiceConvert CLI";
                Console.WriteLine("RiceConvert is the command-line converter for .hit/.chpath files.");
                Console.WriteLine();
                Console.WriteLine("For AGT/TDF browsing, run AGTExporter.exe from RicePack\\bin\\Release instead.");
                Console.WriteLine();
                Console.WriteLine("Press any key to close...");
                Console.ReadKey(true);
                return;
            }

            foreach (var path in args)
            {
                if (!File.Exists(path))
                    continue;

                string pathLower = path.ToLowerInvariant();

                if (pathLower.EndsWith(".hit"))
                {
                    var model = HITFile.LoadHIT(path);
                    model.SaveOBJ(path.Replace(".hit", ".hit.obj"));
                }

                if (pathLower.EndsWith(".hit.obj"))
                {
                    var model = HITFile.LoadOBJ(path);
                    model.SaveHIT(path.Replace(".hit.obj", ".hit"));
                }

                if (pathLower.EndsWith(".chpath"))
                {
                    var model = CHPATHFile.LoadCHPATH(path);
                    model.SaveOBJ(path.Replace(".chpath", ".chpath.obj"));
                }
            }
        }
    }
}
