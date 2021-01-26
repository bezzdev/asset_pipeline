using System;
using System.Collections.Generic;
using System.IO;

namespace asset_pipeline
{
    class Program
    {
        static void Main(string[] args)
        {
            PipelineConfiguration.LoadConfiguration("config.json");
            Console.WriteLine("");
            
            Console.WriteLine("asset_path:");
            string asset_path = PipelineConfiguration.Get("asset_path");
            Console.WriteLine(asset_path);
            Console.WriteLine("");

            Console.WriteLine("target_path:");
            string target_path = PipelineConfiguration.Get("target_path");
            Console.WriteLine(target_path);
            Console.WriteLine("");

            Console.WriteLine("getting asset files:");
            List<string> assetFiles = FileHelper.GetAllValidFiles(asset_path);
            Console.WriteLine("");

            Console.WriteLine("getting target files:");
            List<string> targetFiles = FileHelper.GetAllValidFiles(target_path);
            Console.WriteLine("");

            Console.WriteLine("Comparing files");
            List<string> existingFiles = new List<string>();
            List<string> newFiles = new List<string>();
            List<string> updatedFiles = new List<string>();

            foreach (string file in assetFiles)
            {
                FileInfo assetFileInfo = new FileInfo(file);

                // try and find the original file in the assets
                string relativePath = file.Substring(asset_path.Length, file.Length - asset_path.Length);

                string targetFilePath = target_path + relativePath;

                FileInfo targetFileInfo = new FileInfo(targetFilePath);
                if (targetFileInfo.Exists)
                {
                    if (targetFileInfo.Length == assetFileInfo.Length && targetFileInfo.LastWriteTimeUtc == assetFileInfo.LastWriteTimeUtc)
                    {
                        // file has not changed
                        existingFiles.Add(file);
                    } else
                    {
                        // file has changed
                        updatedFiles.Add(file);
                    }
                } else
                {
                    // file is new
                    newFiles.Add(file);
                }
            }
            Console.WriteLine(assetFiles.Count + " Files compared");
            Console.WriteLine("");
            Console.WriteLine(newFiles.Count + " New Files");
            Console.WriteLine(updatedFiles.Count + " Updated Files");
            Console.WriteLine("");

            Console.WriteLine("Copy files? (yes)");
            string input = Console.ReadLine();
            if (input.ToLower() == "yes")
            {
                Console.WriteLine("Copying files");

                Console.WriteLine("Copying new files");
                foreach (string file in newFiles)
                {
                    // copy the file to the new file path
                    string relativePath = file.Substring(asset_path.Length, file.Length - asset_path.Length);
                    string targetFilePath = target_path + relativePath;

                    string directory = Path.GetDirectoryName(targetFilePath);

                    if(!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    File.Copy(file, targetFilePath);
                }
                Console.WriteLine("New files copied");

                Console.WriteLine("Copying updated files");
                foreach (string file in updatedFiles)
                {
                    // copy the file to the new file path
                    string relativePath = file.Substring(asset_path.Length, file.Length - asset_path.Length);
                    string targetFilePath = target_path + relativePath;

                    string directory = Path.GetDirectoryName(targetFilePath);

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    File.Copy(file, targetFilePath);
                }
                Console.WriteLine("Updated files copied");
                Console.ReadKey();
            } else
            {
                // exit
            }
        }
    }
}
