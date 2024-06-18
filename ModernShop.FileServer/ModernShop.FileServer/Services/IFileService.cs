using ModernShop.FileServer.Dto;

namespace ModernShop.FileServer.Services
{
    public interface IFileService
    {
        Task<FileUploadResultDto> Upload(FileUploadDto fileUploadInfo);
        Task<FileUploadsResultDto> Uploads(List<FileUploadDto> files);
    }
}
