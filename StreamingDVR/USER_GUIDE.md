# IPTV DVR - User Guide

## Table of Contents
1. [Getting Started](#getting-started)
2. [Connecting to Xtream Codes](#connecting-to-xtream-codes)
3. [Browsing Channels](#browsing-channels)
4. [Recording Streams](#recording-streams)
5. [Managing Recordings](#managing-recordings)
6. [Scheduling Recordings](#scheduling-recordings)
7. [Viewing EPG](#viewing-epg)
8. [Tips and Tricks](#tips-and-tricks)

## Getting Started

### First Launch

When you first launch IPTV DVR:

1. **FFmpeg Check**: The app will automatically check if FFmpeg is installed
   - If not found, you'll see installation instructions
   - FFmpeg is required for recording functionality
   - You can still browse channels without FFmpeg

2. **Default Settings**: The app loads with these defaults:
   - Recording path: `%UserProfile%\Videos\IPTV Recordings`
   - Auto-saves all settings
   - Empty credentials (you need to enter yours)

### System Requirements

- Windows 10/11 (64-bit recommended)
- .NET 8 Runtime
- FFmpeg (for recording)
- Stable internet connection
- Sufficient disk space for recordings

## Connecting to Xtream Codes

### Getting Your Credentials

Your IPTV provider should have given you:
- **Server URL**: Format like `http://server.example.com:8080`
- **Username**: Your unique username
- **Password**: Your account password

### Connection Steps

1. Open the **Settings** tab
2. Enter connection details:
   ```
   Server URL: http://your-server.com:8080
   Username: your_username
   Password: your_password
   ```
3. Click **Connect**
4. Wait for success message
5. Credentials are automatically saved

### Connection Status

- **Status Bar**: Shows connection status at bottom
- **Green Message**: "Connected successfully"
- **Red Message**: Check credentials and try again

## Browsing Channels

### Category Navigation

The left panel shows all available categories:
- **All Channels**: Shows every channel
- **Sports, Movies, News, etc.**: Organized categories
- Click any category to filter

### Channel List

The main panel displays channels with:
- Channel Name
- Stream ID

### Search Feature

Use the search box at the top:
1. Type channel name (partial matches work)
2. Results filter instantly
3. Clear search to show all category channels
4. Search is case-insensitive

### Channel Actions

**Double-Click a Channel:**
- Shows the stream URL
- Copy to use in external players

**Right-Click Menu:**
- **Record Now**: Start immediate recording
- **Schedule Recording**: Plan a future recording
- **View EPG**: See program guide (if available)
- **Copy Stream URL**: Copy URL to clipboard

## Recording Streams

### Instant Recording

**Method 1: Button**
1. Select a channel
2. Click **Record Now** button
3. Choose duration option
4. Recording starts immediately

**Method 2: Context Menu**
1. Right-click channel
2. Select "Record Now"
3. Follow prompts

### Duration Options

When starting a recording:

**Specific Duration:**
- Choose "Yes" when prompted
- Enter hours and minutes
- Recording stops automatically

**Until Manually Stopped:**
- Choose "No" when prompted
- Recording continues indefinitely
- Stop manually from Recordings tab or preview window

### Recording Preview Window

While recording, a preview window shows:
- **Channel Name**: What you're recording
- **Duration**: Live counter (HH:MM:SS)
- **File Size**: Updates every second
- **Progress Bar**: Animated indicator
- **Stop Button**: End recording immediately
- **Open Folder**: View file location

### Multiple Recordings

You can record multiple channels simultaneously:
- Each recording runs independently
- Each has its own preview window
- System resources limit maximum count
- Monitor disk space carefully

## Managing Recordings

### Recordings Tab

The Recordings tab shows:
- Channel Name
- Status (Recording, Completed, Stopped, Failed)
- Start Time
- Duration
- File Size

### Recording Actions

**Stop Recording:**
1. Select active recording
2. Click **Stop** button
3. Recording finalizes and saves

**Play Recording:**
1. Select completed recording
2. Click **Play** button
3. Opens in default video player
4. Or double-click recording

**Delete Recording:**
1. Select recording
2. Click **Delete** button
3. Confirm deletion
4. File is permanently deleted

**Open Folder:**
- Click **Open Folder** button
- Opens Windows Explorer
- Shows all recordings

### Recording Status

- 🔴 **Recording**: Currently recording
- ✅ **Completed**: Successfully finished
- ⏹️ **Stopped**: Manually stopped
- ❌ **Failed**: Error occurred
- 🕒 **Scheduled**: Waiting to start

## Scheduling Recordings

### Create Schedule

1. Select a channel from Channels tab
2. Click **Schedule Recording**
3. Configure options:
   - **Date**: When to record (one-time only)
   - **Time**: Start time
   - **Duration**: How long to record
   - **Recurring**: Enable for daily repeats

### Recurring Recordings

For shows that air daily:
1. Check "Recurring Recording"
2. Select days of the week
3. Set time and duration
4. Schedule saves for future use

### Managing Scheduled Recordings

**View Scheduled:**
- Go to **Scheduled** tab
- See all upcoming recordings
- Shows time and frequency

**Remove Schedule:**
1. Select scheduled recording
2. Click **Remove**
3. Confirm removal

**Refresh List:**
- Click **Refresh** to update display
- Scheduled recordings persist across app restarts

### Important Notes

⚠️ **App Must Be Running**: Scheduled recordings only trigger when app is open
⚠️ **Time Accuracy**: Checks every minute for scheduled recordings
⚠️ **Past Schedules**: One-time schedules in the past are auto-removed

## Viewing EPG

### What is EPG?

Electronic Program Guide shows:
- Program title
- Start and end times
- Program description
- Current program highlighted in green

### Viewing EPG

**Method 1:**
1. Select channel
2. Click **View EPG** button

**Method 2:**
1. Right-click channel
2. Select "View EPG"

### EPG Window

- **Time Column**: Program schedule
- **Title Column**: Program name
- **Description**: Program details
- **Green Highlight**: Currently airing
- **Gray Text**: Already aired
- **Record Button**: Schedule recording for program (coming soon)

## Tips and Tricks

### Power User Features

**Keyboard Shortcuts:**
- `Alt+F4`: Exit application
- `Double-click`: Quick play or view
- `Right-click`: Context menus everywhere

**Workflow Optimization:**
1. Keep app running for scheduled recordings
2. Use categories for faster browsing
3. Use search instead of scrolling
4. Set up recurring recordings once
5. Regularly clean old recordings

### Recording Best Practices

**For Best Quality:**
- Use wired internet (not WiFi)
- Close bandwidth-heavy applications
- Don't pause/resume network during recording
- Verify stream quality before long recordings

**For Reliability:**
- Test recording first (5-10 minutes)
- Schedule with buffer time (start 5 min early)
- Keep sufficient disk space
- Don't move app while recording

**For Organization:**
- Use descriptive channel names
- Regularly organize files
- Delete failed recordings
- Keep recordings folder clean

### Troubleshooting Tips

**If channels won't load:**
1. Click Refresh button
2. Check internet connection
3. Reconnect to server
4. Contact IPTV provider

**If recording fails:**
1. Verify FFmpeg: Tools > Check FFmpeg
2. Test with different channel
3. Check disk space
4. Verify stream URL works externally

**If playback has issues:**
1. Install VLC media player
2. Try opening file directly
3. Check file size (should be >0)
4. File may be corrupted if small

## Advanced Usage

### External Players

To use stream URL in external players:
1. Right-click channel
2. Select "Copy Stream URL"
3. Open VLC or other player
4. Network Stream / Open URL
5. Paste and play

### Manual FFmpeg Commands

For advanced users, recorded files can be:
- Converted: `ffmpeg -i input.mp4 -c:v libx264 output.mp4`
- Trimmed: `ffmpeg -i input.mp4 -ss 00:05:00 -t 00:10:00 output.mp4`
- Merged: `ffmpeg -f concat -i list.txt -c copy output.mp4`

### Backup and Restore

**Backup Your Data:**
1. Recordings: Copy files from recording folder
2. Settings: `%AppData%\IPTV_DVR\config.json`
3. History: `%AppData%\IPTV_DVR\recordings.json`
4. Scheduled: `%AppData%\IPTV_DVR\scheduled.json`

**Restore:**
1. Copy files back to original locations
2. Restart application
3. Recordings and schedules reload automatically

## Frequently Asked Questions

**Q: Can I record while watching?**
A: Yes, recording and playback are independent

**Q: How many channels can I record at once?**
A: Limited only by system resources and disk I/O

**Q: Do recordings work when PC is asleep?**
A: No, PC must be awake and app running

**Q: Can I pause/resume recordings?**
A: No, but you can stop and start new recordings

**Q: Are recordings encrypted?**
A: No, files are standard MP4 format

**Q: Can I edit recordings?**
A: Use external video editing software

**Q: Does this work with other IPTV formats?**
A: Only Xtream Codes API is supported

**Q: Is there a mobile version?**
A: No, Windows only currently

## Support

For assistance:
- Check Troubleshooting section
- Verify FFmpeg installation
- Contact IPTV provider for service issues
- Check status bar messages for errors

---

**Happy Recording! 🎬**
