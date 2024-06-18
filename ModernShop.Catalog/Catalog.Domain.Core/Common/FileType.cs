using System.ComponentModel;

namespace Catalog.Domain.Core.Common;
public enum FileType
{
    [Description(".jpg,.png,.jpeg")]
    Image = 10,
    [Description(".mp4,.avl")]
    Video = 20,
}
