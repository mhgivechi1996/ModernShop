namespace ModernShop.FileServer.Dto
{
    public class FileUploadResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string FileExtension { get; internal set; }
    }

    public class FileUploadsResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<FileResultDto> Files { get; set; } = new List<FileResultDto>();
    }

    public class FileResultDto
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string FileExtension { get; internal set; }
    }
}
