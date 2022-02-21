using System;
using System.Collections.Generic;
using System.IO;

namespace asset_pipeline
{
    class Program
    {
        static void Main(string[] args)
        {
            // load configuration
            Console.WriteLine("Loading configuration");
            PipelineConfiguration.LoadConfiguration("config.json");
            Console.WriteLine("");

            // sync google drive files
            Console.WriteLine("Trying cloud sync");
            GoogleDriveHelper.RunGoogleDrive("");
            Console.ReadKey();

            // detect files
            Console.WriteLine("Running file comparison");
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

            copy_flow(assetFiles, asset_path, target_path);
            copy_flow(targetFiles, target_path, asset_path);
        }

        public static void copy_flow(List<string> fromFiles, string from_path, string to_path)
        {

            Console.WriteLine("Comparing files");
            List<string> existingFiles = new List<string>();
            List<string> newFiles = new List<string>();
            List<string> updatedFiles = new List<string>();

            foreach (string file in fromFiles)
            {
                FileInfo assetFileInfo = new FileInfo(file);

                // try and find the original file in the assets
                string relativePath = file.Substring(from_path.Length, file.Length - from_path.Length);

                string targetFilePath = to_path + relativePath;

                FileInfo targetFileInfo = new FileInfo(targetFilePath);
                if (targetFileInfo.Exists)
                {
                    if (targetFileInfo.Length == assetFileInfo.Length && targetFileInfo.LastWriteTimeUtc == assetFileInfo.LastWriteTimeUtc)
                    {
                        // file has not changed
                        existingFiles.Add(file);
                    }
                    else
                    {
                        // file has changed
                        updatedFiles.Add(file);
                    }
                }
                else
                {
                    // file is new
                    newFiles.Add(file);
                }
            }
            Console.WriteLine(fromFiles.Count + " Files compared");
            Console.WriteLine("");
            Console.WriteLine(newFiles.Count + " New Files");
            Console.WriteLine(updatedFiles.Count + " Updated Files");
            Console.WriteLine("");

            Console.ReadKey();
            Console.WriteLine("");
            Console.WriteLine("New files");
            Console.WriteLine("");
            foreach (string file in newFiles)
            {
                Console.WriteLine(file);
            }

            Console.ReadKey();
            Console.WriteLine("");
            Console.WriteLine("Updated files");
            Console.WriteLine("");
            foreach (string file in updatedFiles)
            {
                Console.WriteLine(file);
            }
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
                    string relativePath = file.Substring(from_path.Length, file.Length - from_path.Length);
                    string targetFilePath = to_path + relativePath;

                    string directory = Path.GetDirectoryName(targetFilePath);

                    if (!Directory.Exists(directory))
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
                    string relativePath = file.Substring(from_path.Length, file.Length - from_path.Length);
                    string targetFilePath = to_path + relativePath;

                    string directory = Path.GetDirectoryName(targetFilePath);

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    File.Copy(file, targetFilePath, true);
                }
                Console.WriteLine("Updated files copied");
                Console.WriteLine("");
                Console.WriteLine("Sync pipeline complete");
                Console.ReadKey();
            }
            else
            {
                // exit
            }
        }
    }
}
