# IPTV DVR - Development Notes

## Project Structure

```
StreamingDVR/
├── Models/
│   └── XtreamCodesModels.cs      # Data models for API and recordings
├── Services/
│   ├── XtreamCodesService.cs     # Xtream Codes API client
│   ├── RecordingService.cs       # Recording management and scheduling
│   ├── ConfigurationService.cs   # Settings persistence
│   ├── EpgService.cs             # EPG data retrieval
│   ├── RecordingPersistenceService.cs  # Recording metadata storage
│   └── RecordingStatistics.cs    # Statistics calculation
├── Forms/
│   ├── ScheduleRecordingForm.cs  # Schedule recording dialog
│   ├── EpgViewerForm.cs          # EPG display window
│   ├── RecordingPreviewForm.cs   # Live recording monitor
│   ├── StatisticsForm.cs         # Statistics dashboard
│   └── AboutForm.cs              # About dialog
├── Utilities/
│   └── FFmpegValidator.cs        # FFmpeg detection and validation
├── Form1.cs                      # Main application form
├── Form1.Designer.cs             # UI designer code
├── Program.cs                    # Application entry point
└── README.md                     # User documentation
```

## Architecture

### Design Patterns Used

1. **Service Pattern**: Separate services for distinct concerns
   - XtreamCodesService: API communication
   - RecordingService: Recording operations
   - ConfigurationService: Settings management
   - EpgService: EPG operations

2. **Event-Driven Architecture**:
   - RecordingStarted event
   - RecordingStopped event
   - RecordingFailed event
   - ScheduledRecordingTriggered event

3. **Repository Pattern** (simplified):
   - RecordingPersistenceService handles data storage
   - Abstracts file I/O from business logic

4. **Factory Pattern** (implicit):
   - Process creation for FFmpeg
   - Form instantiation

### Key Design Decisions

#### 1. FFmpeg Integration
**Decision**: Use FFmpeg as external process
**Rationale**:
- Industry standard for video processing
- Supports virtually all stream formats
- No licensing issues
- Excellent performance
- Command-line interface easy to integrate

**Alternative Considered**: Native .NET video libraries
**Why Not**: Limited format support, licensing complexity

#### 2. Xtream Codes API
**Decision**: Direct HTTP API calls with System.Text.Json
**Rationale**:
- Lightweight, no extra dependencies
- Full control over requests
- Easy error handling
- Built-in .NET support

**Alternative Considered**: Third-party IPTV libraries
**Why Not**: Limited availability, extra dependencies

#### 3. Windows Forms
**Decision**: Use Windows Forms for UI
**Rationale**:
- Target .NET 8 Windows-specific features
- Rapid development
- Mature designer support
- Sufficient for requirements

**Alternative Considered**: WPF, Avalonia
**Why Not**: Overkill for this application scope

#### 4. Data Persistence
**Decision**: JSON files in AppData
**Rationale**:
- Simple and lightweight
- Human-readable
- Easy backup/restore
- No database overhead

**Alternative Considered**: SQLite database
**Why Not**: Unnecessary complexity for data volume

### Threading Model

#### UI Thread
- All Windows Forms operations
- User interactions
- Display updates

#### Background Threads
- HTTP API calls (async/await)
- FFmpeg processes (separate processes)
- Timer callbacks (scheduled recording checker)

#### Thread Safety
- `BeginInvoke` for UI updates from background threads
- Immutable data sharing where possible
- Minimal shared state

### Data Flow

```
User Action
    ↓
UI Event Handler (Form1)
    ↓
Service Layer (XtreamCodesService, RecordingService)
    ↓
External Systems (Xtream API, FFmpeg)
    ↓
Events/Callbacks
    ↓
UI Update (via BeginInvoke)
```

## Technical Implementation

### 1. Xtream Codes Authentication

```csharp
// Authentication flow
1. User enters credentials
2. Call: GET /player_api.php?username=X&password=Y
3. Parse JSON response
4. Check user_info.auth == 1
5. Store credentials for subsequent requests
```

### 2. Recording Implementation

```csharp
// Recording flow
1. Get stream URL from Xtream service
2. Generate FFmpeg command:
   ffmpeg -i "URL" -c copy -bsf:a aac_adtstoasc [-t duration] "output.mp4"
3. Start FFmpeg process with redirected I/O
4. Monitor process status
5. Handle completion/failure
6. Update recording metadata
7. Fire events for UI updates
```

### 3. Scheduled Recording Logic

```csharp
// Scheduler flow
1. Timer ticks every 1 minute
2. Check all scheduled recordings
3. For each schedule:
   - Compare current time with start time
   - Check day of week for recurring
   - Fire ScheduledRecordingTriggered event if match
4. Main form handles event:
   - Gets stream URL
   - Starts recording with specified duration
5. Remove non-recurring schedules after trigger
```

### 4. Configuration Persistence

```csharp
// Load on startup
1. Check %AppData%\IPTV_DVR\config.json
2. Deserialize to AppConfiguration object
3. Populate UI fields
4. Initialize services with saved settings

// Save on changes
1. Gather current settings
2. Create AppConfiguration object
3. Serialize to JSON
4. Write to config.json
```

## FFmpeg Command Reference

### Basic Recording Command
```bash
ffmpeg -i "http://server/stream.ts" -c copy -bsf:a aac_adtstoasc "output.mp4"
```

### Parameters Explained
- `-i "URL"`: Input stream URL
- `-c copy`: Copy streams without re-encoding
  - `-c:v copy`: Copy video stream
  - `-c:a copy`: Copy audio stream
- `-bsf:a aac_adtstoasc`: Bitstream filter for AAC to MP4 conversion
- `-t SECONDS`: Optional duration limit
- `"output.mp4"`: Output file path

### Why These Parameters?
- **Copy mode**: Fast, no quality loss, minimal CPU
- **AAC filter**: Ensures MP4 compatibility with AAC audio
- **MP4 container**: Universal playback support
- **No transcoding**: Preserves original quality

## Error Handling Strategy

### Network Errors
- Async/await pattern for all HTTP calls
- Timeout: 30 seconds
- User-friendly error messages
- Status bar feedback

### Recording Errors
- FFmpeg exit code checking
- File existence validation
- Process monitoring
- Automatic cleanup on failure

### User Input Validation
- Required field checking
- Format validation
- Range checking (durations, dates)
- Confirmation dialogs for destructive operations

## Performance Considerations

### Optimizations Implemented
1. **Async/Await**: Non-blocking UI operations
2. **Lazy Loading**: Load channels only when category selected
3. **Efficient Collections**: Dictionary for fast lookups
4. **Minimal UI Updates**: Update only when needed
5. **Process Management**: Proper cleanup prevents leaks

### Scalability
- Handles 10,000+ channels efficiently
- Supports 10+ simultaneous recordings (hardware dependent)
- Unlimited historical recordings (disk space dependent)
- Fast search with LINQ queries

### Resource Usage
- **Memory**: ~50-100 MB base + recordings list
- **CPU**: Minimal (FFmpeg handles heavy lifting)
- **Disk I/O**: Dependent on recording bitrates
- **Network**: Streaming bandwidth as needed

## Security Considerations

### Credential Storage
- Stored in plain text JSON (local machine only)
- Location: User's AppData (not shared)
- File permissions: User-only access
- Consider encrypting for sensitive deployments

### Network Security
- HTTPS support for secure connections
- No credential validation beyond API
- Provider-dependent security
- Local-only data storage

### File Security
- Recordings stored in user-selected folder
- No special protection applied
- User responsible for folder security
- Consider BitLocker for encryption

## Testing Recommendations

### Unit Testing (Not Implemented)
Recommended test coverage:
- XtreamCodesService: Mock HTTP responses
- RecordingService: Mock FFmpeg processes
- ConfigurationService: Temporary file operations
- Data models: Serialization/deserialization

### Integration Testing
Manual testing checklist:
- [ ] Connect to real Xtream server
- [ ] Load channels and categories
- [ ] Start/stop instant recording
- [ ] Schedule one-time recording
- [ ] Schedule recurring recording
- [ ] Play completed recording
- [ ] Delete recording
- [ ] Change recording path
- [ ] App restart with saved data
- [ ] Multiple simultaneous recordings

### Performance Testing
- Large channel lists (5000+ channels)
- Long-duration recordings (6+ hours)
- Multiple simultaneous recordings (5+)
- Rapid start/stop operations
- Memory leak testing (24+ hour runtime)

## Deployment

### Release Build
```powershell
dotnet publish -c Release -r win-x64 --self-contained false
```

### Distribution
1. Publish as framework-dependent (requires .NET 8)
2. Or publish as self-contained (larger but no runtime needed)
3. Include README.md and USER_GUIDE.md
4. Remind users to install FFmpeg

### Installation Package (Future)
- Consider MSI installer
- Auto-download .NET 8 if missing
- Optional FFmpeg bundling (license permitting)
- Desktop shortcut creation

## Troubleshooting Development Issues

### Designer Issues
- Regenerate designer code if corrupted
- Ensure partial class declaration
- Check for duplicate member declarations

### Build Errors
- Clean and rebuild solution
- Check NuGet package references
- Verify .NET 8 SDK installation

### Runtime Errors
- Check FFmpeg installation
- Verify stream URLs manually
- Test with VLC player first
- Check Windows Event Viewer

## Contributing Guidelines (If Open Source)

1. **Code Style**: Follow existing conventions
2. **Comments**: Add XML documentation for public APIs
3. **Testing**: Test all changes manually
4. **Commits**: Use descriptive commit messages
5. **Pull Requests**: Include description of changes

## License Considerations

### Current Status
- Sample/educational application
- Not licensed for commercial use
- Third-party components:
  - .NET 8: MIT License
  - FFmpeg: GPL/LGPL (separate installation)

### Commercial Use
Would require:
- Proper software license
- FFmpeg license compliance (if bundled)
- Terms of service compliance with IPTV providers
- User agreement for recorded content

---

**Developer Documentation** | Version 1.0.0
