# StreamingDVR

> ⚠️ **Early Development** — This project is in active early development. Features may be incomplete, APIs may change, and bugs are expected. Contributions and feedback are welcome.

A Windows desktop application for recording live IPTV streams from multiple source types. Built with .NET 8 and Windows Forms.

---

## Features

### Multi-Source IPTV Support
- **Xtream Codes** — Connect to Xtream Codes compatible servers
- **Enigma2** — Connect to Enigma2/Dreambox receivers
- **M3U** — Load channels from local M3U files or remote M3U URLs
- Manage multiple active sources simultaneously with per-source enable/disable

### Recording
- Record live streams to MP4 using **FFmpeg**
- Optional **Streamlink** backend for improved stream compatibility
- Set recording duration or record indefinitely
- Scheduled recordings with one-time or recurring (by day of week) support
- Recording persistence — history survives application restarts
- Recording statistics (total count, size, duration, largest/longest recordings)

### EPG (Electronic Programme Guide)
- Fetch EPG data from Xtream Codes sources
- Manage multiple EPG sources
- Assign EPG channels to individual IPTV channels
- Per-channel EPG viewer

### Channel Management
- Browse channels by category
- Text search/filter across all channels
- Right-click context menu per channel:
  - Record Now
  - Schedule Recording
  - View EPG
  - Assign EPG
  - Copy Stream URL

### Diagnostics & Debugging
- Persistent debug log at `%APPDATA%\IPTV_DVR\debug.log`
- FFmpeg availability check
- Streamlink availability check with installation path detection
- Connection status and error reporting per source

---

## Requirements

### Runtime
- **Windows 10 / 11** (x64)
- **.NET 8 Desktop Runtime** — [Download](https://dotnet.microsoft.com/download/dotnet/8.0)

### Recording Backend (at least one required)
- **FFmpeg** (default) — [Download](https://ffmpeg.org/download.html)
  - Must be available in system `PATH`
- **Streamlink** (optional, alternative backend) — [Download](https://streamlink.github.io/install.html)
  - Install via pip: `pip install streamlink`
  - Install via winget: `winget install streamlink.streamlink`
  - Install via Chocolatey: `choco install streamlink`

---

## Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/fredfeldman/StreamingDVR.git
cd StreamingDVR
```

### 2. Open in Visual Studio
Open `StreamingDVR.sln` in Visual Studio 2022 or later.

### 3. Build
```
Build → Build Solution  (Ctrl+Shift+B)
```
No additional NuGet packages are required.

### 4. Run
Press **F5** or **Ctrl+F5**. On first launch the Welcome screen will appear.

### 5. Add an IPTV Source
1. Go to **Settings** tab
2. Click **Manage Sources**
3. Click **Add** and select a source type
4. Enter server URL, username, and password
5. Click **Save**

The application will auto-connect to all active sources on startup.

---

## Project Structure

```
StreamingDVR/
├── Forms/
│   ├── AboutForm              - About dialog
│   ├── AssignEpgForm          - Assign EPG to a channel
│   ├── EpgSourceEditorForm    - Add/edit EPG source
│   ├── EpgSourceManagerForm   - Manage EPG sources list
│   ├── EpgViewerForm          - View EPG listings for a channel
│   ├── RecordingDurationForm  - Pick a recording duration
│   ├── RecordingPreviewForm   - Preview a recording
│   ├── ScheduleRecordingForm  - Set up a scheduled recording
│   ├── SourceEditorForm       - Add/edit an IPTV source
│   ├── SourceManagerForm      - Manage IPTV sources list
│   ├── StatisticsForm         - Recording statistics overview
│   ├── StreamUrlDialog        - Display/copy a stream URL
│   └── WelcomeForm            - First-run welcome screen
├── Models/
│   ├── ChannelEpgMapping      - Maps a channel to an EPG source
│   ├── EpgSource              - EPG source data
│   ├── IptvSource             - IPTV source (Xtream/Enigma2/M3U)
│   └── XtreamCodesModels      - Xtream Codes API response models
├── Services/
│   ├── ConfigurationService   - Load/save app configuration (JSON)
│   ├── Enigma2Service         - Enigma2 API client
│   ├── EpgService             - EPG data fetching
│   ├── RecordingPersistenceService - Save/load recording history
│   ├── RecordingService       - Start/stop/schedule recordings
│   ├── RecordingStatistics    - Calculate recording statistics
│   └── XtreamCodesService     - Xtream Codes API client
├── Utilities/
│   ├── FFmpegValidator        - Detect FFmpeg installation
│   └── StreamlinkValidator    - Detect Streamlink installation and path
├── Form1.cs                   - Main application window
└── Program.cs                 - Application entry point
```

---

## Configuration

Settings are stored in JSON format at:
```
%APPDATA%\IPTV_DVR\config.json
```

### Key Settings

| Setting | Default | Description |
|---|---|---|
| `RecordingPath` | `Videos\IPTV Recordings` | Folder where recordings are saved |
| `UseStreamlink` | `false` | Use Streamlink instead of FFmpeg |
| `StreamlinkQuality` | `best` | Streamlink quality preset |
| `StreamlinkRetryOpen` | `true` | Retry on connection open failure |
| `StreamlinkRetryStreams` | `3` | Number of stream retries |
| `IptvSources` | `[]` | List of configured IPTV sources |
| `EpgSources` | `[]` | List of configured EPG sources |

---

## Debug Logging

A debug log is written to `%APPDATA%\IPTV_DVR\debug.log` on every run.

To open it quickly:
1. Press `Win + R`
2. Type `%APPDATA%\IPTV_DVR`
3. Open `debug.log`

See [DEBUG_LOGGING_GUIDE.md](StreamingDVR/DEBUG_LOGGING_GUIDE.md) for full details.

---

## Streamlink Support

Streamlink can be used as an alternative recording backend. It offers better compatibility with certain stream types and automatic retry logic.

To use Streamlink:
1. Install Streamlink (see Requirements above)
2. In the application, go to **Settings** tab
3. Enable **Use Streamlink for recording**
4. Click **Check Streamlink** to verify detection

See [STREAMLINK_GUIDE.md](StreamingDVR/STREAMLINK_GUIDE.md) for full details.

---

## Known Limitations

- Windows only (Windows Forms / .NET 8 Windows target)
- No built-in video preview (stream URLs can be copied for external players)
- M3U source type is defined but not fully implemented
- Scheduled recording callback is partially implemented
- No automatic EPG refresh

---

## Development Status

| Area | Status |
|---|---|
| Xtream Codes connection | ✅ Working |
| Enigma2 connection | ✅ Working |
| M3U source | ⚠️ Partial |
| FFmpeg recording | ✅ Working |
| Streamlink recording | ✅ Working |
| Scheduled recordings | ⚠️ Partial |
| EPG viewing | ✅ Working |
| EPG source management | ✅ Working |
| Per-channel EPG assignment | ✅ Working |
| Recording statistics | ✅ Working |
| Configuration persistence | ✅ Working |
| Debug logging | ✅ Working |
| Streamlink path detection | ✅ Working |

---

## Contributing

This project is in early development. If you'd like to contribute:

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/my-feature`
3. Commit your changes: `git commit -m "Add my feature"`
4. Push to the branch: `git push origin feature/my-feature`
5. Open a pull request

Please open an issue first for significant changes.

---

## License

This project does not currently have a license. All rights reserved until a license is chosen.

---

## Acknowledgements

- [FFmpeg](https://ffmpeg.org/) — Recording backend
- [Streamlink](https://streamlink.github.io/) — Alternative recording backend
- Built with [.NET 8](https://dotnet.microsoft.com/) and Windows Forms
