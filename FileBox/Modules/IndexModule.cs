namespace FileBox
{
    using Nancy;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using FileBox.Domain;
    using FileBox.Entities;
    using System.IO;

    public class IndexModule : NancyModule
    {
        public IndexModule(IRootPathProvider pathProvider)
        {
            Get["/"] = parameters =>
            {
                string usrAgent = Request.Headers.UserAgent;
                int posIdentifier=usrAgent.IndexOf("Trident/");
                if (posIdentifier > -1)
                {
                    string version=usrAgent.Substring(posIdentifier + 8, 3);
                    double browserVersion=Convert.ToDouble(version);
                    if (browserVersion < 5)
                    {
                        return View["indexIE"];
                    }
                }
                return View["index"];
            };
            Get["/iexplorer"] = parameters =>
           {
               return View["indexIE"];
           };

            Get["/templates/main/"] = y =>
            {
                return View["uploadMain"];
            };
            Get["/templates/files/"] = y =>
            {
                return View["uploadFile"];
            };

            Post["/filesIE"] = parameters =>
            {
                Models.File file1 = new Models.File();
                file1 = CreateFileInServer(pathProvider, Request.Files.Count());

                return View["ResulsIE",file1];
            };
            Post["/files"] = parameters =>
            {
                int filesCount = Request.Files.Count();
                Models.File file1 = new Models.File();
                List<Models.File> files = new List<Models.File>();
                file1 = CreateFileInServer(pathProvider, filesCount);
                return Response.AsJson(file1 );
            };

            Get["/files/{id}"] = parameters =>
            {
                Models.UserFile outFile = GetFileInfoFromDB((string)parameters.id, pathProvider);
                var response =new Response();
                response.Headers.Add("Content-Disposition", "attachment; filename=" + Uri.EscapeDataString(outFile.FileName));
                response.ContentType = "text/plain";
                response.Contents = stream =>
                {
                    outFile.Data.CopyTo(stream);
                };
                return response;
            };
            Get["/test"] = param1 =>
            {
                return View["test"];
            };

        }

        private Models.File CreateFileInServer(IRootPathProvider pathProvider, int filesCount)
        {
            Models.File file1=null;
            if (filesCount > 0)
            {
                var file = Request.Files.FirstOrDefault();
                var name = file.Name;
                file1 = CreateFile(file.Name, file.Value, pathProvider);
                file1.Url = Request.Url.SiteBase.Replace(":80", "") + Request.Url.BasePath + "/files/" + file1.Url;
            }
            return file1;
        }

        private Models.UserFile GetFileInfoFromDB(string Urlkey,IRootPathProvider pathProvider)
        {
            Models.UserFile result = new Models.UserFile();
            string repoPath = Path.Combine(pathProvider.GetRootPath(), "Files");
            StoredFile file = DataDomain.Instance.GetFileInfo(Urlkey);
            result.FileName = file.OriginalFileName;
            var data=File.ReadAllBytes(Path.Combine(repoPath, file.CurrentFileName));
            MemoryStream memstr = new MemoryStream(data);
            result.Data = memstr;
            return result;
        }

        private Models.File CreateFile(string FileName, System.IO.Stream fileStream, IRootPathProvider pathProvider)
        {
            StoredFile file = new StoredFile();
            file.OriginalFileName = FileName;
            file.UniqueId = Guid.NewGuid();
            file.Extension = Path.GetExtension(FileName);
            file.FileSize = fileStream.Length;
            file.CurrentFileName = file.UniqueId.ToString() + file.Extension;
            file.UrlKey = string.Empty;
            DataDomain.Instance.CreateFile(file);
            WriteFileToDisk(pathProvider, file, fileStream);
            return new Models.File(file);
        }

        private void WriteFileToDisk(IRootPathProvider pathProvider, StoredFile file, Stream fileStream)
        {
            FileStream outStream;
            string repoPath = Path.Combine(pathProvider.GetRootPath(), "Files");
            string fileOutputPath = Path.Combine(repoPath, file.CurrentFileName);
            if (!Directory.Exists(repoPath))
            {
                Directory.CreateDirectory(repoPath);
            }
            if (File.Exists(fileOutputPath))
            {
                outStream = new FileStream(fileOutputPath, FileMode.Truncate);
            }
            else
            {
                outStream = new FileStream(fileOutputPath, FileMode.CreateNew);
            }
            fileStream.CopyTo(outStream);
            outStream.Flush();
            outStream.Close();
        }
    }
}