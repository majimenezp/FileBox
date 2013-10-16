using FileBox.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileBox.Models
{
    public class File
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public File()
        {

        }
        public File(StoredFile originalFile)
        {
            this.Id = originalFile.Id;
            this.Url = originalFile.UrlKey;
            this.Name = originalFile.OriginalFileName;
            this.Size = originalFile.FileSize;
        }
    }
}