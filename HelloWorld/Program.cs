// -----------------------------------------------------------------------------
// PROJECT   : PupNet
// COPYRIGHT : Andy Thomas (C) 2022-24
// LICENSE   : GPL-3.0-or-later
// HOMEPAGE  : https://github.com/kuiperzone/PupNet
//
// PupNet is free software: you can redistribute it and/or modify it under
// the terms of the GNU Affero General Public License as published by the Free Software
// Foundation, either version 3 of the License, or (at your option) any later version.
//
// PupNet is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License along
// with PupNet. If not, see <https://www.gnu.org/licenses/>.
// -----------------------------------------------------------------------------

using System.Reflection;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(nameof(HelloWorld));
            Console.WriteLine("Version: {0}", GetVersion());
#if DEBUG
            Console.WriteLine("Build: DEBUG");
#else
            Console.WriteLine("Build: RELEASE");
#endif
            Console.WriteLine();

            // Ensure arguments are passed
            Console.WriteLine("Args: {0}", string.Join(", ", args));
            Console.WriteLine();

            Console.WriteLine("WORKING DIRECTORY");
            Console.WriteLine("GetCurrentDirectory: {0}", Directory.GetCurrentDirectory());
            Console.WriteLine();

            Console.WriteLine("HOME");
            Console.WriteLine("Environment.SpecialFolder.UserProfile: {0}", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            Console.WriteLine();

            var dir = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine("EXECUTABLE DIRECTORY");
            Console.WriteLine("AppDomain.CurrentDomain.BaseDirectory: {0}", AppDomain.CurrentDomain.BaseDirectory);
            Console.WriteLine();

            // Executable path (warning IL3000 for "single-file" is expected)
            Console.WriteLine("ENTRY ASSEMBLY");
            Console.WriteLine("Assembly.GetEntryAssembly().Location: {0}", Assembly.GetEntryAssembly()?.Location ?? "null");
            Console.WriteLine();



            // Look for sub-directory created by DotnetPostPublish conf parameter
            // This ensures custom content is packaged.
            Console.WriteLine("POST PUBLISH");
            dir = Path.Combine(dir, "subdir");
            Console.WriteLine("Packaged subdir exists: " + Directory.Exists(dir));
            Console.WriteLine("Packaged subdir/file.test exists: " + File.Exists(Path.Combine(dir, "file.test")));
            Console.WriteLine("If the above is 'True', it indicates the DotnetPostPublish script was run during the build");


            // Test for passing property from command line when publishing project
            Console.WriteLine();
            Console.WriteLine("PUBLISH PROPERTIES");
#if FLAG1
            Console.WriteLine("FLAG1 defined OK!!!");
            Console.WriteLine("pupnet was called with: --property DefineConstants=FLAG1");
#else
            Console.WriteLine("FLAG1 NOT defined");
            Console.WriteLine("pupnet was NOT called with: --property DefineConstants=FLAG1");
#endif
            Console.WriteLine();
#if FLAG2
            Console.WriteLine("FLAG2 defined OK!!!");
            Console.WriteLine("pupnet was called with: --property DefineConstants=FLAG2");
#else
            Console.WriteLine("FLAG2 NOT defined");
            Console.WriteLine("pupnet was NOT called with: --property DefineConstants=FLAG2");
#endif

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press any key to finish");

            // This line was proving problematic on arm64 AppImage (but have changed runtime since)
            Console.ReadKey(false);

            Console.WriteLine();
        }

        private static string GetVersion()
        {
            try
            {
                // Wasn't expecting this to work for:
                // -p:PublishSingleFile=true
                // But it seems to work OK
                var ea = Assembly.GetEntryAssembly();

                if (ea != null)
                {
                    var ver = ea.GetName().Version;

                    if (ver != null)
                    {
                        return ver.ToString();
                    }

                    throw new Exception($"{ver} is null");
                }

                throw new Exception($"{ea} is null");
            }
#if DEBUG
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
#else
            catch
            {
            }
#endif

            // Fallback
            return "Unknown";
        }
    }

}
