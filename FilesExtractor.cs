using System;
using System.Collections.Generic;
using System.Linq;

namespace css_optimizer
{
    public static class FilesExtractor 
    {
        public static List<FileInfo> Extract(DirectoryInfo folder,Predicate<FileInfo> filter, IEnumerable<string> excludes)
        {
            List<FileInfo> result=new List<FileInfo>();
            Search(folder,filter,excludes,result);            
            return result;
        }
        public static List<FileInfo> Extract(DirectoryInfo folder, Predicate<FileInfo> filter)
            => Extract(folder, filter, []);
        private static void Search(DirectoryInfo folder, Predicate<FileInfo> filter, IEnumerable<string> excludes,List<FileInfo> result)
        {
            var folderFiles = folder.EnumerateFiles();
            if (folderFiles.Count() > 0)
                foreach (var file in folderFiles)
                    if (filter(file))
                        result.Add(file);
            var subFolders = folder.GetDirectories();
            foreach (var subFolder in subFolders)
                if (!excludes.Contains(subFolder.Name))
                    Search(subFolder, filter, excludes,result);
        }
    }
}
