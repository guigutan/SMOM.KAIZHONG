using System;
using System.IO;
using System.Reflection;

namespace WpfClient
{
    public static class AssemblyResolver
    {
        internal static void Hook(params string[] folders)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                Console.WriteLine(args.Name);
                string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                foreach (var dir in folders)
                {
                    string assemblyPath = Path.Combine(assemblyFolder, dir, new AssemblyName(args.Name).Name + ".dll");

                    if (File.Exists(assemblyPath))
                        return Assembly.LoadFrom(assemblyPath);
                }
                    

                return null;
            };
        }
    }
}
