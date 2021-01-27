using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace asset_pipeline
{
    public static class FileHelper
    {
        public static List<String> DirSearch(string sDir)
        {
            List<string> files = new List<string>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(DirSearch(d));
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }

            return files;
        }

        public static List<string> GetAllFiles(string path)
        {
            return DirSearch(path);
        }

        public static List<string> GetAllValidFiles(string path)
        {
            List<string> files = GetAllFiles(path);

            string extension = PipelineConfiguration.Get("allowed_extension");

            // filter files by an allowed file extension
            files = files.Where(f => f.Split('.').Last() == extension).ToList();

            return files;
        }
    }
}
