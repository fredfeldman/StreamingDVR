# Streamlink Support - User Guide

## What is Streamlink?

Streamlink is a command-line utility that extracts streams from various services and pipes them into a video player or saves them to a file. It's an open-source project that provides better compatibility with many streaming sources compared to FFmpeg alone.

## Why Use Streamlink?

### Advantages over FFmpeg alone:
- **Better Stream Compatibility:** Handles HLS, DASH, and other adaptive streaming protocols more reliably
- **Built-in Authentication:** Supports OAuth and other authentication methods
- **Automatic Quality Selection:** Can automatically select the best available quality
- **Platform-Specific Plugins:** Includes plugins for Twitch, YouTube, and many IPTV providers
- **Retry Logic:** Automatically retries failed connections
- **Stream Validation:** Verifies stream before downloading

### When to Use Streamlink:
✅ Streams that fail with FFmpeg  
✅ HLS/DASH adaptive streams  
✅ Authenticated IPTV services  
✅ Streams requiring headers or cookies  
✅ Twitch, YouTube Live, and similar platforms  

### When to Use FFmpeg:
✅ Simple RTMP/RTSP streams  
✅ Direct stream URLs  
✅ When Streamlink is not installed  
✅ For maximum compatibility with all recordings  

## Installation

### Windows

#### Option 1: pip (Recommended if you have Python)
```bash
pip install streamlink
```

#### Option 2: Chocolatey
```bash
choco install streamlink
```

#### Option 3: winget
```bash
winget install streamlink.streamlink
```

#### Option 4: Windows Installer
1. Download from https://streamlink.github.io/install.html
2. Run the installer
3. Restart StreamingDVR application

### Verify Installation
Open Command Prompt and run:
```bash
streamlink --version
```

You should see output like:
```
streamlink 6.4.2
```

## Using Streamlink in StreamingDVR

### 1. Check if Streamlink is Installed

1. Open **StreamingDVR**
2. Go to **Settings** tab
3. Find **Streamlink Settings** section
4. Click **Check Streamlink** button
5. You'll see a message indicating if Streamlink is available

### 2. Enable Streamlink

1. In **Settings** tab
2. Find **Streamlink Settings** section
3. Check ☑ **Use Streamlink for recording**
4. Settings are saved automatically

### 3. Configure Quality

Select the desired quality from the dropdown:

- **best** (Default) - Automatically selects the highest quality available
- **worst** - Lowest quality (useful for bandwidth-limited connections)
- **1080p, 720p, 480p, 360p** - Specific resolutions (if available)
- **source** - Original source quality (platform-dependent)

💡 **Tip:** Use "best" for most situations. Streamlink will automatically select the highest available quality.

### 4. Start Recording

1. Go to **Channels** tab
2. Select a channel
3. Click **Record Now** or right-click → **Record Now**
4. The recording will use Streamlink if enabled, otherwise FFmpeg

## Quality Options Explained

| Quality Setting | Description | Use When |
|----------------|-------------|----------|
| **best** | Highest available quality | General use, good bandwidth |
| **worst** | Lowest available quality | Limited bandwidth, testing |
| **1080p** | 1920x1080 resolution | Specific quality needed |
| **720p** | 1280x720 resolution | Balance quality/size |
| **480p** | 854x480 resolution | Lower bandwidth |
| **360p** | 640x360 resolution | Very limited bandwidth |
| **source** | Original stream | Platform-specific |

⚠️ **Note:** Not all streams offer all quality options. If the requested quality is unavailable, Streamlink will automatically select the closest available option.

## Advanced Settings

### Custom Streamlink Options (Advanced Users)

For advanced users, additional Streamlink options can be passed. Common examples:

```
--http-header "User-Agent=Mozilla/5.0"
--hls-segment-threads 4
--hls-timeout 60
--stream-segment-attempts 3
```

⚠️ **Warning:** Only use custom options if you understand Streamlink command-line parameters.

## Troubleshooting

### Problem: "Streamlink not found"

**Possible Causes:**
- Streamlink not installed
- Not in system PATH
- Application needs restart after installation

**Solutions:**
1. Install Streamlink using one of the methods above
2. Restart StreamingDVR application
3. Click "Check Streamlink" in Settings to verify
4. If still not found, reinstall and ensure "Add to PATH" is checked

### Problem: Recording fails with Streamlink

**Solutions:**
1. **Test the stream URL:**
   ```bash
   streamlink URL best --output test.mp4
   ```

2. **Check the stream quality:**
   ```bash
   streamlink URL
   ```
   This shows available qualities

3. **Try different quality:**
   - Change from "best" to "worst" or specific resolution
   - Some streams may not support all qualities

4. **Disable Streamlink:**
   - Uncheck "Use Streamlink for recording"
   - Try recording with FFmpeg instead

5. **Check logs:**
   - Look at debug logs in `%APPDATA%\IPTV_DVR\debug.log`
   - Search for "Streamlink" errors

### Problem: Stream quality is poor

**Solutions:**
1. Change quality setting from "worst" to "best" or "1080p"
2. Check available qualities for the stream
3. Ensure you have sufficient bandwidth
4. Verify the stream source provides higher quality

### Problem: Recording starts but stops immediately

**Possible Causes:**
- Stream URL expired or invalid
- Authentication required
- Network issues

**Solutions:**
1. Refresh the channel list
2. Try recording again (stream URLs may be temporary)
3. Check network connectivity
4. Try with FFmpeg to compare behavior

### Problem: Streamlink works in command line but not in app

**Solutions:**
1. Ensure Streamlink is in system PATH
2. Restart the application
3. Run application as administrator (one time)
4. Reinstall Streamlink

## Command-Line Testing

### Test if Streamlink can access a stream:
```bash
streamlink "http://your-stream-url" best --output test.mp4 --force-progress
```

### Check available qualities:
```bash
streamlink "http://your-stream-url"
```

Output will show:
```
Available streams: 1080p (best), 720p, 480p, 360p (worst)
```

### Test without saving (just validation):
```bash
streamlink "http://your-stream-url" best --output NUL
```

## Performance Comparison

| Feature | FFmpeg | Streamlink |
|---------|--------|-----------|
| Simple streams | ✅ Excellent | ✅ Excellent |
| HLS streams | ⚠️ Good | ✅ Excellent |
| DASH streams | ⚠️ Limited | ✅ Excellent |
| Authentication | ⚠️ Manual | ✅ Built-in |
| Quality selection | Manual | ✅ Automatic |
| Retry logic | Manual | ✅ Built-in |
| Platform plugins | ❌ None | ✅ Many |

## Best Practices

1. **Use "best" quality by default** - Let Streamlink choose optimal quality
2. **Test new streams** - Try recording for a few seconds first
3. **Keep Streamlink updated** - New versions improve compatibility
4. **Fall back to FFmpeg** - If Streamlink fails, disable it and try FFmpeg
5. **Monitor disk space** - Higher quality = larger files
6. **Check stream legality** - Only record streams you have permission to record

## Supported Platforms

Streamlink includes plugins for:
- Twitch
- YouTube Live
- Facebook Live
- DailyMotion
- Vimeo
- UStream
- And 100+ more platforms

For IPTV:
- Xtream Codes (supported through URLs)
- M3U playlists
- HLS streams
- RTMP streams
- HTTP streams

## FAQ

### Q: Do I need both FFmpeg and Streamlink?
**A:** Yes. Streamlink requires FFmpeg for some operations. Both should be installed.

### Q: Which is better, FFmpeg or Streamlink?
**A:** Neither is universally better. Streamlink excels at handling modern streaming protocols, while FFmpeg is more versatile for direct streams.

### Q: Can I switch between them?
**A:** Yes! Just toggle the "Use Streamlink for recording" checkbox in Settings.

### Q: Does Streamlink use more resources?
**A:** Minimal difference. Both are efficient. Streamlink may use slightly more CPU for stream processing.

### Q: Will my existing recordings work?
**A:** Yes! Streamlink only affects new recordings. Existing recordings are unaffected.

### Q: Can I use Streamlink for live playback?
**A:** Currently, StreamingDVR uses Streamlink only for recording, not live playback.

### Q: Does Streamlink work with all my IPTV sources?
**A:** Streamlink should work with most IPTV sources. If a stream works with FFmpeg, it should work with Streamlink.

## Getting Help

If you encounter issues:

1. **Check this guide** - Most common issues are covered
2. **Review logs** - `%APPDATA%\IPTV_DVR\debug.log`
3. **Test with command line** - Isolate if it's a Streamlink or app issue
4. **Try FFmpeg instead** - Quick workaround while troubleshooting
5. **Report issues** - Include error messages and log excerpts

## Resources

- **Streamlink Official Site:** https://streamlink.github.io/
- **Streamlink Documentation:** https://streamlink.github.io/cli.html
- **Plugin List:** https://streamlink.github.io/plugins.html
- **GitHub Issues:** https://github.com/streamlink/streamlink/issues

## Conclusion

Streamlink integration provides StreamingDVR with enhanced stream compatibility and recording reliability. Most users should enable Streamlink and use "best" quality for optimal results. FFmpeg remains available as a fallback for maximum compatibility.
