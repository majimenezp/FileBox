using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FileBox.Models
{
    public class UserFile
    {
        public string FileName { get; set; }

        public Stream Data { get; set; }
    }
}