namespace ModernShop.FileServer.Dto
{
    public class FileUploadDto : BaseFileManagerDto
    {
        public IFormFile File { get; set; }
        public string FileName { get; set; }
    }
}
