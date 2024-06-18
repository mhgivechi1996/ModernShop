using ModernShop.FileServer.Common;
using ModernShop.FileServer.Dto;
using ModernShop.FileServer.Infrastructure;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace ModernShop.FileServer.Services
{
    public class FileService : IFileService
    {
        private readonly FileUtility fileUtility;
        private readonly FileServerConfig fileServerConfig;


        public FileService(FileUtility fileUtility, IOptions<FileServerConfig> options)
        {
            this.fileUtility = fileUtility;
            this.fileServerConfig = options.Value;
        }

        #region Upload Multi Files

        public async Task<FileUploadsResultDto> Uploads(List<FileUploadDto> files)
        {
            var result = new FileUploadsResultDto();
            foreach (var item in files)
            {
                var fileSaveResult = await Upload(item);
                if(fileSaveResult.IsSuccess)
                {
                    result.Files.Add(new FileResultDto
                    {
                        FileExtension = fileSaveResult.FileExtension,
                        FileName = fileSaveResult.FileName,
                        FilePath = fileSaveResult.FilePath,
                        FileSize = fileSaveResult.FileSize,
                    });
                }
            }

            result.IsSuccess = true;
            return result;

        }

        #endregion

        #region Upload Single File

        public async Task<FileUploadResultDto> Upload(FileUploadDto fileUploadInfo)
        {
            var fileUploadResult = new FileUploadResultDto();

            //validation
            if (fileUploadInfo.File == null || fileUploadInfo.File.Length == 0)
            {
                fileUploadResult.IsSuccess = false;
                fileUploadResult.Message = "file is empty!";
                return fileUploadResult;
            }

            var fileExtension = Path.GetExtension(fileUploadInfo.File.FileName).ToLower();
            string folderPath = fileUtility.GenerateFolderPathFromFileInfo(fileUploadInfo, fileExtension);
            string pathToSave = string.Empty;

            if(fileServerConfig.SaveInLocalFolder)
            {
                pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderPath);
            } else
            {
                pathToSave = Path.Combine(fileServerConfig.FileServerUrl, folderPath);
            }

            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            var fileName = fileUploadInfo.FileName.Trim('"');
            fileName = fileUtility.RemoveSpecialCharacters(fileName);
            fileName = fileUtility.ReplaceWhitespace(fileName, "-");

            var uniqueFileNameWithExtension = fileName;
            if (!fileName.Contains(fileExtension))
            {
                uniqueFileNameWithExtension += fileExtension;
            }

            string fullPath = Path.Combine(pathToSave, uniqueFileNameWithExtension);
            if (System.IO.File.Exists(fullPath))
            {
                fullPath = fileUtility.NextAvailableFilename(fullPath);
                var fullPathParts = fullPath.Split("\\");
                uniqueFileNameWithExtension = fullPathParts[fullPathParts.Length - 1];
            }

            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await fileUploadInfo.File.CopyToAsync(fileStream);
            }

           
            fileUploadResult.IsSuccess = true;
            fileUploadResult.FileName = uniqueFileNameWithExtension;
            fileUploadResult.FilePath = Path.Combine(folderPath, uniqueFileNameWithExtension);
            fileUploadResult.FileSize = fileUploadInfo.File.Length;
            fileUploadResult.FileExtension = fileExtension;
            return fileUploadResult;
        }

        #endregion
    }
}
