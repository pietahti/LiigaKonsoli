using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeikkausliigaKonsoli
{
    class Program
    {
        // Decompress netcontent
        static byte[] Decompress(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip),
                                  CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        static void Main(string[] args)
        {
            //string matchUrl = "http://adafyvlstorage.blob.core.windows.net/2014/finland/veikkausliiga/matches/1302637";
            string matchUrl = "http://adafyvlstorage.blob.core.windows.net/2014/finland/veikkausliiga/matches";
            string deCompressedString = "";

            try
            {
                Console.WriteLine("*** Decompress web page ***");
                Console.WriteLine("    Specify file to download");
                Console.WriteLine("Downloading: {0}", matchUrl);

                // Download url.
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
                    byte[] data = client.DownloadData(matchUrl);
                    byte[] decompress = Decompress(data);
                    string text = System.Text.ASCIIEncoding.ASCII.GetString(decompress);

                    Console.WriteLine("Size from network: {0}", data.Length);
                    Console.WriteLine("Size decompressed: {0}", decompress.Length);
                    Console.WriteLine("First chars:       {0}", text.Substring(0, 70));
                    deCompressedString = text; // This could be returned, i.e. if data.Length > 5 for example
                }
            }
            finally
            {
                Console.WriteLine("Uncompressed length: {0}", deCompressedString.Length);
                Console.WriteLine("[Done]");
                //Console.ReadLine();
            }

            // Haetaan ottelutunnukset ja ottelulistaussivun tiedot taulukkoon.
            uint[,] Match = new uint[100, 100];         // tulokset
            string[,] Team = new string[100, 100];      // joukkueet: kotijoukkue, vierasjoukkue
            int index = deCompressedString.IndexOf(deCompressedString, 50);
            // = deCompressedString.Contains("\"Id\":");
            Console.WriteLine("ensimmäinen löytymä {0}", index);
            Console.ReadLine();
        }
    }
}






