# IPTV DVR - Features Documentation

## Complete Feature List

### 1. Channel Management

#### 1.1 Channel Browsing
- **Category Navigation**: Browse channels by provider-defined categories
- **All Channels View**: View complete channel list
- **Lazy Loading**: Efficient loading of large channel lists
- **Channel Details**: Stream ID, name, and icon (when available)

#### 1.2 Search & Filter
- **Real-time Search**: Filter channels as you type
- **Case-Insensitive**: Matches regardless of letter case
- **Partial Matching**: Find channels with partial name matches
- **Category-Scoped**: Search within selected category only
- **Clear Search**: Empty search box to restore full list

#### 1.3 Channel Information
- **Stream URL**: View and copy stream URLs
- **EPG Data**: View program guide when available
- **Quick Actions**: Context menu with common operations

### 2. Recording Capabilities

#### 2.1 Instant Recording
- **One-Click Recording**: Start recording with single button click
- **Duration Options**:
  - Fixed duration (hours + minutes)
  - Indefinite (until manually stopped)
- **Preview Window**: Real-time recording statistics
- **Multiple Simultaneous**: Record several channels at once

#### 2.2 Scheduled Recording
- **One-Time Scheduling**:
  - Set specific date and time
  - Define duration
  - Automatic start/stop
- **Recurring Recordings**:
  - Daily repeats
  - Select specific days of week
  - Same time each day
  - Perfect for series/shows
- **Persistent Schedules**: Saved across app restarts
- **Auto-Cleanup**: Removes past one-time schedules

#### 2.3 Recording Management
- **Recording List**: View all recordings with detailed info
- **Status Tracking**: Recording, Completed, Stopped, Failed
- **File Information**:
  - Channel name
  - Start/end time
  - Duration
  - File size
- **Actions**:
  - Stop active recordings
  - Play completed recordings
  - Delete recordings (removes file)
  - Open recording folder

#### 2.4 Recording Quality
- **Original Quality**: No transcoding, preserves stream quality
- **Fast Recording**: Direct stream copy (no CPU overhead)
- **Format**: MP4 container (universal compatibility)
- **Codecs**: Preserves original video/audio codecs
- **Bitrate**: Same as source stream

### 3. User Interface

#### 3.1 Main Window
- **Tabbed Interface**: Organize features logically
- **Menu Bar**: File, Tools, Help menus
- **Status Bar**: Real-time application status
- **Responsive Design**: Adapts to window resizing

#### 3.2 Tabs
- **Channels Tab**:
  - Split view (categories | channels)
  - Search box
  - Action buttons
- **Recordings Tab**:
  - Detailed list view
  - Multiple column sorting
  - Action buttons
- **Scheduled Tab**:
  - Upcoming recordings
  - Recurring indicators
  - Management buttons
- **Settings Tab**:
  - Connection configuration
  - Recording path setup
  - Credential management

#### 3.3 Context Menus
- **Channel Context Menu**:
  - Record Now
  - Schedule Recording
  - View EPG
  - Copy Stream URL
- **Recording Context Menu**:
  - Play
  - Stop Recording
  - Open Folder
  - Delete

#### 3.4 Keyboard Shortcuts
- `F5`: Refresh current tab
- `Ctrl+R`: Record selected channel
- `Ctrl+O`: Open recordings folder
- `Ctrl+I`: View statistics
- `Delete`: Delete selected recording/schedule
- `Alt+F4`: Exit application
- `Double-Click`: Quick play or info

### 4. EPG (Electronic Program Guide)

#### 4.1 EPG Viewing
- **Channel EPG**: View program guide for specific channel
- **Program Details**:
  - Title
  - Start/end time
  - Description
- **Visual Indicators**:
  - Green: Currently airing
  - Gray: Already aired
  - White: Upcoming

#### 4.2 EPG Integration
- **Provider-Dependent**: Requires EPG support from IPTV provider
- **Multiple Programs**: Shows next 50 programs
- **Time-Based Navigation**: Chronological display
- **Future Integration**: Schedule from EPG (planned)

### 5. Configuration & Settings

#### 5.1 Connection Settings
- **Server Configuration**:
  - Server URL
  - Port (included in URL)
  - Protocol selection (HTTP/HTTPS)
- **Authentication**:
  - Username
  - Password
  - Credential persistence
- **Auto-Save**: Settings saved on successful connection

#### 5.2 Recording Settings
- **Path Configuration**:
  - Custom recording folder
  - Browse dialog
  - Auto-folder creation
- **Persistence**: Settings preserved across sessions

#### 5.3 Data Storage
- **Configuration File**: `%AppData%\IPTV_DVR\config.json`
- **Recordings Metadata**: `%AppData%\IPTV_DVR\recordings.json`
- **Scheduled Recordings**: `%AppData%\IPTV_DVR\scheduled.json`
- **Auto-Load**: Restores data on startup

### 6. System Integration

#### 6.1 FFmpeg Integration
- **Automatic Detection**: Checks FFmpeg on startup
- **Version Check**: Displays installed FFmpeg version
- **Installation Guide**: Built-in help for installation
- **Command Generation**: Optimal FFmpeg parameters
- **Process Management**: Handles FFmpeg lifecycle

#### 6.2 File System
- **Folder Operations**: Browse, open, create folders
- **File Naming**: Automatic sanitization and timestamping
- **Size Tracking**: Real-time file size monitoring
- **Cleanup**: Delete recordings with files

#### 6.3 External Integration
- **Default Player**: Opens recordings in system default
- **VLC Support**: Optimized for VLC media player
- **Explorer Integration**: Opens folders in Windows Explorer
- **Clipboard**: Copy stream URLs

### 7. Reliability Features

#### 7.1 Error Handling
- **Connection Retry**: Graceful handling of network errors
- **Recording Failure Detection**: Identifies failed recordings
- **User Notifications**: Clear error messages
- **Status Tracking**: Visual feedback for all operations

#### 7.2 Data Persistence
- **Auto-Save**: Regular saving of metadata
- **Crash Recovery**: Recordings metadata survives crashes
- **Schedule Persistence**: Scheduled recordings survive restarts
- **Configuration Backup**: Settings always saved

#### 7.3 Resource Management
- **Process Cleanup**: Stops all recordings on exit
- **Memory Management**: Efficient data structures
- **Timer Management**: Proper disposal of timers
- **Form Cleanup**: Closes preview windows on exit

### 8. Advanced Features

#### 8.1 Statistics Dashboard
- **Recording Metrics**:
  - Total recordings count
  - Completed/Failed/Active counts
  - Total storage used
  - Total duration recorded
- **Historical Data**:
  - Oldest recording date
  - Newest recording date
  - Largest recording
  - Longest recording

#### 8.2 Multi-Recording
- **Simultaneous Recordings**: Record multiple channels
- **Independent Management**: Each recording has own controls
- **Resource Aware**: Limited by system capabilities
- **Preview Windows**: One per active recording

#### 8.3 Smart Features
- **Auto-Cleanup**: Removes expired schedules
- **Duplicate Prevention**: Prevents duplicate scheduled recordings
- **File Validation**: Checks file existence before operations
- **Smart Naming**: Sanitizes channel names for filenames

### 9. Xtream Codes API Support

#### 9.1 Authentication
- **Player API**: Full authentication support
- **Session Management**: Maintains authentication state
- **Token Handling**: Automatic credential inclusion

#### 9.2 Content Discovery
- **Live Streams**: All live channel support
- **Categories**: Category hierarchy support
- **Filtering**: Category-based filtering
- **Metadata**: Channel icons, EPG IDs, archive info

#### 9.3 Stream Access
- **URL Generation**: Automatic stream URL creation
- **Format Support**: .ts, .m3u8, and other formats
- **Direct Source**: Accesses direct stream sources
- **Archive Support**: Detects archive-capable channels

### 10. Performance Optimizations

#### 10.1 UI Performance
- **Async Operations**: All network calls are asynchronous
- **Background Loading**: Channels load without UI freeze
- **Throttled Refresh**: 2-second refresh interval
- **Lazy Rendering**: Only renders visible items

#### 10.2 Recording Performance
- **Stream Copy**: No transcoding overhead
- **Direct Write**: Streams directly to disk
- **Buffer Management**: Optimal FFmpeg buffer sizes
- **Parallel Processing**: Multiple recordings don't block

#### 10.3 Memory Management
- **Disposable Pattern**: Proper resource cleanup
- **Event Unsubscription**: Prevents memory leaks
- **List Management**: Efficient collection handling
- **Timer Disposal**: Proper cleanup on exit

## Feature Matrix

| Feature | Status | Notes |
|---------|--------|-------|
| Xtream Codes Authentication | ✅ Complete | Full API support |
| Channel Browsing | ✅ Complete | With categories |
| Channel Search | ✅ Complete | Real-time filtering |
| Instant Recording | ✅ Complete | With duration control |
| Scheduled Recording | ✅ Complete | One-time and recurring |
| Recording Management | ✅ Complete | Full CRUD operations |
| EPG Viewing | ✅ Complete | When available from provider |
| Configuration Persistence | ✅ Complete | Auto-save/load |
| FFmpeg Validation | ✅ Complete | Startup check |
| Statistics Dashboard | ✅ Complete | Detailed metrics |
| Context Menus | ✅ Complete | Quick actions |
| Keyboard Shortcuts | ✅ Complete | Power user features |
| Multi-Recording | ✅ Complete | Limited by resources |
| Recording Preview | ✅ Complete | Real-time monitoring |
| Built-in Player | ❌ Future | Uses system default |
| VOD Support | ❌ Future | Movies/series |
| Catchup TV | ❌ Future | Time-shift recording |
| Recording Templates | ❌ Future | Preset configurations |
| Dark Mode | ❌ Future | Theme support |

## API Coverage

### Implemented Endpoints
- ✅ Authentication: `player_api.php` (base)
- ✅ Live Categories: `get_live_categories`
- ✅ Live Streams: `get_live_streams`
- ✅ Category Streams: `get_live_streams&category_id=X`
- ✅ Short EPG: `get_short_epg&stream_id=X`

### Not Yet Implemented
- ⏳ VOD Categories: `get_vod_categories`
- ⏳ VOD Streams: `get_vod_streams`
- ⏳ Series: `get_series`
- ⏳ Full EPG: `get_simple_data_table`

## Limitations

### Current Limitations
1. **Windows Only**: No cross-platform support
2. **Single Instance**: Best with one app instance
3. **Active Scheduling**: App must run for scheduled recordings
4. **No Transcoding**: Records in original format only
5. **Manual Organization**: No auto-folder organization

### Technical Limitations
1. **FFmpeg Dependency**: External dependency required
2. **Network Dependent**: Requires stable internet
3. **Provider Dependent**: Features vary by IPTV provider
4. **Format Support**: Limited to FFmpeg-supported formats

## Future Roadmap

### Short Term (v1.1)
- [ ] Built-in video player
- [ ] Recording quality options
- [ ] Automatic retry on failure
- [ ] Recording notifications

### Medium Term (v1.2)
- [ ] VOD section support
- [ ] Series recording
- [ ] Catchup TV support
- [ ] Recording templates

### Long Term (v2.0)
- [ ] Cloud storage integration
- [ ] Mobile companion app
- [ ] Web interface
- [ ] Multi-language support

---

**Last Updated**: Version 1.0.0
