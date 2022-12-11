using Mpms.Common;
using Mpms.Models.Base;
using Mpms.Models.Enums;

namespace Mpms.Models;

public class FileItem : FileSystemItem
{
    public FileItem(string name, string path, SongMetadata metadata) : base(name, path)
    {
        Metadata = metadata;
    }

    public override ContentTypes Type => ContentTypes.File;

    /// <summary>
    /// Song metadata.
    /// </summary>
    public SongMetadata Metadata { get; set; }
}