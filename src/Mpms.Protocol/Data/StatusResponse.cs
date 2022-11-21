using System.Text.RegularExpressions;
using Mpms.Protocol.Base;

namespace Mpms.Protocol.Data;

/// <summary>
/// Displays the current status of the player.
/// </summary>
public class StatusResponse : ResponseBase
{
    private const string PATTERN = @"^(?<key>\S+):\s(?<value>.+)$";
    
    /// <summary>
    /// The name of the current partition.
    /// </summary>
    public string? Partition { get; private set; }
    
    /// <summary>
    /// Volume level 0-100 (deprecated: -1 if the volume cannot be determined).
    /// </summary>
    public int? Volume { get; private set; }
    
    /// <summary>
    /// Returns True if the Repeat Mode is enabled.
    /// </summary>
    public bool Repeat { get; private set; }
    
    /// <summary>
    /// Returns True if the Random Mode is enabled.
    /// </summary>
    public bool Random { get; private set; }
    
    /// <summary>
    /// Returns current value for Single mode.
    /// </summary>
    public SingleModes? Single { get; private set; }
    
    /// <summary>
    /// Returns True if the Consume Mode is enabled.
    /// </summary>
    public bool Consume { get; private set; }
    
    /// <summary>
    /// The playlist version number.
    /// </summary>
    public int Playlist { get; private set; }
    
    /// <summary>
    /// The length of the playlist.
    /// </summary>
    public int PlaylistLength { get; private set; }
    
    /// <summary>
    /// Returns current state.
    /// </summary>
    public StateModes? State { get; private set; }
    
    /// <summary>
    /// Playlist song number of the current song stopped on or playing.
    /// </summary>
    public int? Song { get; private set; }
    
    /// <summary>
    /// Playlist song ID of the current song stopped on or playing.
    /// </summary>
    public int? SongId { get; private set; }
    
    /// <summary>
    /// Playlist song number of the next song to be played.
    /// </summary>
    public int? NextSong { get; private set; }
    
    /// <summary>
    /// Playlist song ID of the next song to be played.
    /// </summary>
    public int? NextSongId { get; private set; }
    
    /// <summary>
    /// Total time elapsed within the current song in seconds.
    /// </summary>
    public float? Elapsed { get; private set; }
    
    /// <summary>
    /// Duration of the current song in seconds.
    /// </summary>
    public float? Duration { get; private set; }
    
    /// <summary>
    /// Instantaneous bitrate in Kbps.
    /// </summary>
    public int? Bitrate { get; private set; }
    
    /// <summary>
    /// Cross-fade in seconds.
    /// </summary>
    public int? CrossFade { get; private set; }
    
    /// <summary>
    /// MixRamp threshold in dB.
    /// MixRamp tags describe the loudness levels at start and end of a song and can be used by MPD to
    /// find the best time to begin cross-fading.
    /// </summary>
    public int? MixRampDb { get; private set; }
    
    /// <summary>
    /// MixRamp delay in seconds.
    /// MixRamp tags describe the loudness levels at start and end of a song and can be used by MPD to
    /// find the best time to begin cross-fading.
    /// </summary>
    public int? MixRampDelay { get; private set; }
    
    /// <summary>
    /// The format emitted by the decoder plugin during playback, format: SampleRate:Bits:Channels.
    /// </summary>
    public string? Audio { get; private set; }
    
    /// <summary>
    /// A positive number identifying the music database update job.
    /// </summary>
    public int? UpdatingDatabaseJobId { get; private set; }
    
    /// <summary>
    /// If there is an error, returns message here.
    /// </summary>
    public string? Error { get; private set; }

    public override void ParseData(byte[] data)
    {
        string inputString = Parser.GetString(data);
        
        // clear current values
        Partition = null;
        
        
        foreach(Match match in Regex.Matches(inputString, PATTERN, RegexOptions.Multiline))
        {
            string value = match.Groups["value"].Value;
            
            switch (match.Groups["key"].Value)
            {
                case "partition":
                    Partition = value;
                    break;
                case "volume":
                    Volume = Int32.Parse(value);
                    break;
                case "repeat":
                    Repeat = Boolean.Parse(value);
                    break;
                case "random":
                    Random = Boolean.Parse(value);
                    break;
                case "single":
                    Single = value switch
                    {
                        "0" => SingleModes.False,
                        "1" => SingleModes.True,
                        "oneshot" => SingleModes.OneShot,
                        _ => null
                    };
                    break;
                case "consume":
                    Consume = Boolean.Parse(value);
                    break;
                case "playlist":
                    Playlist = Int32.Parse(value);
                    break;
                case "playlistlength":
                    PlaylistLength = Int32.Parse(value);
                    break;
                case "state":
                    State = value switch
                    {
                        "play" => StateModes.Play,
                        "stop" => StateModes.Stop,
                        "pause" => StateModes.Pause,
                        _ => null
                    };
                    break;
                case "song":
                    Song = Int32.Parse(value);
                    break;
                case "songid":
                    SongId = Int32.Parse(value);
                    break;
                case "nextsong":
                    NextSong = Int32.Parse(value);
                    break;
                case "nextsongid":
                    NextSongId = Int32.Parse(value);
                    break;
                case "elapsed":
                    Elapsed = System.Single.Parse(value);
                    break;
                case "duration":
                    Duration = System.Single.Parse(value);
                    break;
                case "bitrate":
                    Bitrate = Int32.Parse(value);
                    break;
                case "xfade":
                    CrossFade = Int32.Parse(value);
                    break;
                case "mixrampdb":
                    MixRampDb = Int32.Parse(value);
                    break;
                case "mixrampdelay":
                    MixRampDelay = Int32.Parse(value);
                    break;
                case "audio":
                    Audio = value;
                    break;
                case "updating_db":
                    UpdatingDatabaseJobId = Int32.Parse(value);
                    break;
                case "error":
                    Error = value;
                    break;
            }
        }
    }
}