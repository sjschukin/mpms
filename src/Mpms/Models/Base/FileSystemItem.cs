using Mpms.Models.Enums;

namespace Mpms.Models.Base;

public abstract class FileSystemItem
{
    protected FileSystemItem(string name, string path)
    {
        Name = name;
        Path = path;
    }

    public virtual ContentTypes Type => throw new NotImplementedException();
    public string Name { get; set; }
    public string Path { get; set; }
}