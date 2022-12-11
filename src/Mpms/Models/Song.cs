namespace Mpms.Models;

public class Song
{
    /// <summary>
    /// The song ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// File name of the song.
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// The song title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The artist name. Its meaning is not well-defined; see “composer” and “performer” for more specific tags
    /// </summary>
    public string? Artist { get; set; }

    /// <summary>
    /// The duration of the song in seconds. May contain a fractional part
    /// </summary>
    public float Duration { get; set; }

    /// <summary>
    /// If this is a queue item referring only to a portion of the song file, then this attribute contains the time range.
    /// Both <see cref="RangeFrom"/> and <see cref="RangeTo"/> are time stamps within the song in seconds (may contain a fractional part)
    /// </summary>
    public float? RangeFrom { get; set; }

    /// <summary>
    /// If this is a queue item referring only to a portion of the song file, then this attribute contains the time range.
    /// Both <see cref="RangeFrom"/> and <see cref="RangeTo"/> are time stamps within the song in seconds (may contain a fractional part)
    /// </summary>
    public float? RangeTo { get; set; }

    /// <summary>
    /// The album name
    /// </summary>
    public string? Album { get; set; }

    /// <summary>
    /// On multi-artist albums, this is the artist name which shall be used for the whole album. The exact meaning of this tag is not well-defined
    /// </summary>
    public string? AlbumArtist { get; set; }

    /// <summary>
    /// A name for this song. This is not the song title. The exact meaning of this tag is not well-defined.
    /// It is often used by badly configured internet radio stations with broken tags to squeeze both the
    /// artist name and the song title in one tag
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The decimal track number within the album
    /// </summary>
    public int? Track { get; set; }

    /// <summary>
    /// The song’s release date. This is usually a 4-digit year
    /// </summary>
    public int? Date { get; set; }

    /// <summary>
    /// The music genre
    /// </summary>
    public string? Genre { get; set; }

    /// <summary>
    /// The artist who composed the song
    /// </summary>
    public string? Composer { get; set; }

    /// <summary>
    /// The decimal disc number in a multi-disc album
    /// </summary>
    public int? Disc { get; set; }

    /// <summary>
    /// The name of the label or publisher
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// The audio format of the song (or an approximation to a format supported by MPD and the decoder plugin being used)
    /// </summary>
    public string? Format { get; set; }

    /// <summary>
    /// The time stamp of the last modification of the underlying file
    /// </summary>
    public DateTime? LastModified { get; set; }
}