# Multi-Source IPTV Configuration Guide

## Overview

The StreamingDVR application now supports multiple IPTV sources, allowing you to aggregate channels from different providers and connection types. This guide explains how to configure and manage multiple IPTV sources.

## Supported Source Types

### 1. **Xtream Codes API**
The most common IPTV provider format used by many services.

**Configuration Requirements:**
- Server URL (e.g., `http://example.com:8080`)
- Username
- Password
- Optional: Custom port

**Features:**
- Full channel listing
- Category support
- Built-in EPG support
- VOD support (if enabled)

### 2. **Enigma2 Box**
Connect directly to Enigma2-based set-top boxes (Dreambox, Vu+, etc.)

**Configuration Requirements:**
- Server URL (IP address or hostname)
- Username (typically "root")
- Password
- Port (typically 80 or 8001)

**Features:**
- Live channel streaming
- Channel bouquets
- EPG from the box
- Timer/recording integration

### 3. **M3U Playlist**
Standard M3U/M3U8 playlist files or URLs

**Configuration Requirements:**
- Either:
  - M3U URL (remote playlist)
  - Local M3U file path

**Features:**
- Wide compatibility
- Simple setup
- Can be used with various providers
- Support for extended M3U attributes

## Managing Sources

### Opening Source Manager

1. Go to the **Settings** tab
2. Click **Manage Sources...** button
3. The Source Manager window will open

### Adding a New Source

1. Click **Add Source** in the Source Manager
2. Enter a descriptive name for your source
3. Select the source type from the dropdown
4. Fill in the required connection details
5. Optionally add additional EPG sources
6. Check **Active** to enable the source immediately
7. Click **Save**

### Editing an Existing Source

1. Select the source from the list
2. Click **Edit**
3. Modify the settings as needed
4. Click **Save**

### Deleting a Source

1. Select the source from the list
2. Click **Delete**
3. Confirm the deletion

### Reordering Sources

Use the **Move Up** and **Move Down** buttons to change the priority order of sources. Channels from higher-priority sources will be displayed first.

### Testing Connections

1. Select a source from the list
2. Click **Test Connection**
3. The application will verify connectivity and credentials

## Additional EPG Sources

Each IPTV source can have additional EPG (Electronic Program Guide) sources configured.

### Why Add Additional EPG Sources?

- Some M3U playlists don't include EPG data
- You may want more detailed program information
- Combine multiple EPG sources for better coverage

### Adding EPG Sources

1. In the Source Editor, find the "Additional EPG Sources" section
2. Enter an EPG URL (typically XMLTV format)
   - Example: `http://example.com/epg.xml.gz`
3. Click **Add**
4. Repeat for multiple EPG sources

### Common EPG Formats

- **XMLTV**: The standard format (`.xml` or `.xml.gz`)
- **JSON EPG**: Some providers use JSON format
- **Compressed**: EPG files are often gzip-compressed to save bandwidth

## Best Practices

### Organizing Multiple Sources

- **Use descriptive names**: "Main Provider (Xtream)", "Backup M3U", etc.
- **Set priorities**: Put your most reliable source first
- **Test before activating**: Use the Test Connection feature
- **Keep credentials secure**: The app stores them encrypted

### Managing Active Sources

- Only activate sources you're currently using
- Inactive sources won't consume resources but remain configured
- You can quickly enable/disable sources as needed

### Performance Considerations

- **Too many active sources**: May slow down channel loading
- **Recommended limit**: 3-5 active sources for best performance
- **Large M3U files**: Can take time to parse initially

## Troubleshooting

### Connection Issues

**Xtream Codes:**
- Verify server URL includes port (e.g., `:8080`)
- Check if server requires `http://` or `https://`
- Ensure credentials are correct
- Some providers have connection limits

**Enigma2:**
- Default port is usually 80 or 8001
- Username is typically "root"
- Enable web interface on the box
- Check firewall settings

**M3U:**
- Verify URL is accessible from your network
- For local files, use full path
- Check file encoding (should be UTF-8)
- Ensure playlist format is valid

### EPG Not Loading

- EPG URLs must be accessible
- Some EPG sources update only once daily
- Check EPG URL format and compression
- XMLTV IDs must match channel IDs in playlist

### Channels Not Appearing

- Ensure source is marked as **Active**
- Check connection test results
- Verify provider credentials haven't expired
- For M3U: Check playlist URL/file is valid

## Migration from Single Source

If you're upgrading from an older version:

1. Your existing Xtream Codes connection will be automatically migrated
2. It will appear as "Default Xtream Codes" in the source list
3. You can rename it or add additional sources
4. Old settings will be preserved

## Examples

### Example 1: Primary Xtream Codes with Backup M3U

```
Source 1: "Main Provider" (Xtream Codes)
- Server: http://provider.com:8080
- Username: myuser
- Password: mypass
- Active: Yes

Source 2: "Backup Playlist" (M3U)
- M3U URL: http://backup.com/channels.m3u
- Active: Yes
- EPG: http://backup.com/epg.xml.gz
```

### Example 2: Enigma2 Box with Additional EPG

```
Source: "Living Room Vu+ Box" (Enigma2)
- Server: http://192.168.1.100
- Username: root
- Password: dreambox
- Port: 80
- Active: Yes
- Additional EPG: http://epg-provider.com/enhanced-epg.xml
```

### Example 3: Multiple M3U Sources

```
Source 1: "Sports Channels" (M3U)
- Local File: C:\IPTV\sports.m3u
- Active: Yes

Source 2: "Movies & Series" (M3U)
- M3U URL: http://movies.com/playlist.m3u8
- Active: Yes
- EPG: http://movies.com/epg.xml.gz

Source 3: "International" (M3U)
- Local File: C:\IPTV\international.m3u
- Active: No
```

## Advanced Features

### Source Priority

Sources are processed in order. If a channel appears in multiple sources:
- The first occurrence is used
- Channel metadata from the first source is preserved
- You can reorder sources to change priority

### Selective Activation

- Keep multiple sources configured but inactive
- Quickly switch between provider configurations
- Useful for testing or seasonal content

### EPG Aggregation

- Multiple EPG sources are merged
- If a channel has EPG from multiple sources, the most detailed one is used
- EPG refresh happens in the background

## Future Enhancements

Planned features for multi-source support:
- Source-specific recording paths
- Per-source bandwidth management
- Automatic failover between sources
- Channel deduplication options
- Source health monitoring
- Scheduled source switching

## Support

For issues or questions about multi-source configuration:
1. Check the troubleshooting section above
2. Verify your source credentials with the provider
3. Test connection using the built-in test feature
4. Check the application logs for detailed error messages
