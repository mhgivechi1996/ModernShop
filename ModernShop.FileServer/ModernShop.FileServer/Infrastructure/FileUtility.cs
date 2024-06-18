using ModernShop.FileServer.Dto;
using System.Text.RegularExpressions;

namespace ModernShop.FileServer.Infrastructure
{
    public class FileUtility
    {
        public string GenerateFolderPathFromFileInfo(FileUploadDto fileUploadInfo, string fileExtension)
        {
            var path = string.Empty;
            switch (fileExtension)
            {
                case ".png":
                case "png":
                case ".jpg":
                case "jpg":
                case ".jpeg":
                case "jpeg":
                    {
                        path = Path.Combine("Files", fileUploadInfo.AppName, fileUploadInfo.EntityName, "Images");
                        break;

                    }

                case ".mp4":
                case "mp4":
                case ".ogg":
                case "ogg":
                    {
                        path = Path.Combine("Files", fileUploadInfo.AppName, fileUploadInfo.EntityName, "Videos");
                        break;
                    }

                case ".pdf":
                case "pdf":
                case ".docx":
                case "docx":
                    {
                        path = Path.Combine("Files", fileUploadInfo.AppName, fileUploadInfo.EntityName, "Documents");
                        break;
                    }

                default:
                    path = Path.Combine("Files", fileUploadInfo.AppName, fileUploadInfo.EntityName, "Others");
                    break;
            }

            return path;
        }

        public string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled);
        }

        private readonly Regex Whitespace = new(@"\s+");

        public string ReplaceWhitespace(string input, string replacement)
        {
            return Whitespace.Replace(input, replacement);
        }

        public string NextAvailableFilename(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return path;
            }

            if (Path.HasExtension(path))
            {
                return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path), StringComparison.Ordinal), NumberPattern));
            }

            return GetNextFilename(path + NumberPattern);
        }

        private const string NumberPattern = "-{0}";

        public string GetNextFilename(string pattern)
        {
            string tmp = string.Format(pattern, 1);

            if (!System.IO.File.Exists(tmp))
            {
                return tmp;
            }

            int min = 1, max = 2;

            while (System.IO.File.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                int pivot = (max + min) / 2;
                if (System.IO.File.Exists(string.Format(pattern, pivot)))
                {
                    min = pivot;
                }
                else
                {
                    max = pivot;
                }
            }

            return string.Format(pattern, max);
        }
    }
}
