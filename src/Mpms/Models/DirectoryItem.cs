using Mpms.Models.Base;
using Mpms.Models.Enums;

namespace Mpms.Models;

public class DirectoryItem : FileSystemItemBase
{
    public DirectoryItem(string name, string path) : base(name, path)
    {
    }

    public override ContentTypes Type => ContentTypes.Directory;
}