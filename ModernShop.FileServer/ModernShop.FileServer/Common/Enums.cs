using System.ComponentModel;

namespace ModernShop.FileServer.Common
{
    public enum FileType
    {
        [Description(".jpg,.png,.jpeg")]
        Image = 10,
        [Description(".mp4,.avl")]
        Video = 20,
    }
}
