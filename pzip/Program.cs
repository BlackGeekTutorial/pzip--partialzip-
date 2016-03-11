using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteZip;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace pzip
{

    /* learned a lot of code from: http://www.codeproject.com/Articles/8688/Extracting-files-from-a-remote-ZIP-archive */

    class Program
    {

        public static void catchemall(string url, string remotepath, string localpath)
        {

            /* Checking if local file exists */

            if (File.Exists(localpath))
            {
                File.Delete(localpath);
            }

            /* creating local file */
            File.Create(localpath).Dispose();

            RemoteZipFile zip;
            zip = new RemoteZipFile();

            if (zip.Load(url) == false)
            {
                Console.WriteLine("ERROR: Wasn't able to map the remote zip");
                Environment.Exit(1);
            } 
            Console.WriteLine("Loaded URL: " + url);

            int itemscount = 0;
            foreach (ZipEntry zipe in zip)
            {
                //Console.WriteLine(zipe.ToString()); <- this prints all zip files (could be helpful for a --list argument)
                
                if (zipe.ToString() == remotepath)
                {
                    Console.WriteLine("Found remote file: " + zipe.ToString());
                    break;
                }
                itemscount = itemscount + 1;
                 
                
            }
            ZipEntry zipee = zip[itemscount];

                Stream os = File.Open(localpath, FileMode.Open);
                Stream s = zip.GetInputStream(zipee);

                byte[] bb = new byte[65536];

                try
                {
                    while (true)
                    {
                        int r = s.Read(bb, 0, bb.Length);
                        if (r <= 0)
                            break;
                        os.Write(bb, 0, r);
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine("Unknown error: " + ee);
                }
                os.Close();
                s.Close();

        }

        static void Main(string[] args)
        {


            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Usage: pzip.exe <url> <remotpath> <localpath>");
                Environment.Exit(0);
            }

            catchemall(args[0], args[1], args[2]);

        }
    }
}
