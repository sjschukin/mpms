# MPMS

## Configuration

### MpdConnection section

* Type - Connection method to MPD service (UnixSocket | Network)
* Address - Path to stream point for UnixSocket or IP address for Network
* Port - Port number. Use only for Network connection type
* HeartBeatInterval - Time interval in seconds for keeping the connection open
* CommandTimeout - Command execution timeout in seconds