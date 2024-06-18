using Catalog.Domain.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Core.AggregatesModel.ProductAggregate
{
    public class ProductAttachment : Entity<Guid>
    {
        public ProductId ProductId { get; private set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int Size { get; set; }
        public ProductAttachmentFileType FileType { get; set; }
    }

    public enum ProductAttachmentFileType
    {
        Thumbnail = 10,
        Gallery = 20,
        Video = 30,
        Catalog = 40,
    }
}
