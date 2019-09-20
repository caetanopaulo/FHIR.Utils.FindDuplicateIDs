using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace FHIR.Utils.FindDuplicateIDs
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            int fileCount = 0;
            int repeatedIdsCount = 0;
            List<string> ignoredFiles = new List<string>();

          
            if (args.Length < 1) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Missing FHIR resources directory path! Please specify a full path.");
                Console.ResetColor();
                return;
            }

            string targetDirectory =  args[0];

            try
            {
                string[] fileEntries = Directory.GetFiles(targetDirectory);
                foreach (string filePath in fileEntries)
                {

                    Console.ResetColor();
                    var resource = JObject.Parse(File.ReadAllText(filePath));
                    string fileName = Path.GetFileName(filePath);

                    string resourceId = resource.GetValue("id") == null ? resource.GetValue("url") + "" : resource.GetValue("id") + "";
                    string resourceType = resource.GetValue("resourceType") + "";

                    fileCount++;
                    if (string.IsNullOrEmpty(resourceId))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("{0} > [IGNORED] > Ignored filename: '{1}'.", fileCount, fileName);
                        ignoredFiles.Add(fileName);
                    }
                    else
                    {
                        Console.WriteLine("{0} > RESOURCE ID/URL: '{1}'.", fileCount, resourceId);
                        List<string> repeatedIdsInFilenames = GetFilesWithRepeatedId(resourceId, fileEntries, resourceType);

                        if (repeatedIdsInFilenames.Count >= 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            repeatedIdsInFilenames.ForEach(filename => Console.WriteLine(" > [REPEATED FOUND] >>  Repeated ID in Filename '{0}' with ResourceType '{1}'", filename, resourceType));
                            repeatedIdsCount++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[EXCEPTION] > Exception: {0}", e.Message);
                Console.ResetColor();
            }

            WriteResults(fileCount, repeatedIdsCount, ignoredFiles);
        }

        private static List<string> GetFilesWithRepeatedId(string targetId, string[] fileEntries, string targetResourceType)
        {
            List<string> foundFiles = new List<string>();
            foreach (string filePath in fileEntries)
            {
                var resource = JObject.Parse(File.ReadAllText(filePath));
                string fileName = Path.GetFileName(filePath);
                string resourceId = resource.GetValue("id") == null ? resource.GetValue("url") + "" : resource.GetValue("id") + "";
                string resourceType = resource.GetValue("resourceType") + "";

                if (resourceId == targetId && resourceType == targetResourceType)
                {
                    foundFiles.Add(fileName);
                }
            }
            return foundFiles;

        }

        private static void WriteResults(int fileCount, int repeatedIdsCount, List<string> ignoredFiles)
        {
            Console.WriteLine("----------------= RESULTS =----------------");
            Console.WriteLine("Number of files: {0} ", fileCount);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Number of repeated Ids: {0} ", repeatedIdsCount);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Number of ignored files: {0} ", ignoredFiles.Count);
            Console.WriteLine("Ignored Files: ");
            ignoredFiles.ForEach(files => Console.WriteLine(" -> {0}", files));
            Console.ResetColor();
        }

    }
}
