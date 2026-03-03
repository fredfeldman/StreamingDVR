# Debug Logging Guide

## Overview

The StreamingDVR application now includes comprehensive debug logging to help troubleshoot connection issues with IPTV sources. This is especially useful when diagnosing problems with Xtream Codes, Enigma2, or M3U connections.

## Where Logs Are Stored

Debug logs are written to two locations:

### 1. Visual Studio Debug Output
- Visible in Visual Studio's Output window (Debug tab)
- Real-time logging during development
- Lost when application closes

### 2. Persistent Log File
- **Location:** `%APPDATA%\IPTV_DVR\debug.log`
- **Full Path Example:** `C:\Users\YourName\AppData\Roaming\IPTV_DVR\debug.log`
- **Format:** Text file with timestamps
- Persists across application sessions
- Can be shared for troubleshooting

## Opening the Log File

### Quick Access
1. Press `Win + R`
2. Type: `%APPDATA%\IPTV_DVR`
3. Press Enter
4. Open `debug.log` with Notepad or any text editor

### From File Explorer
1. Open File Explorer
2. Go to: `C:\Users\<YourUsername>\AppData\Roaming\IPTV_DVR`
3. Open `debug.log`

## What Gets Logged

### Application Startup
```
========================================
=== Application Starting ===
Version: 1.0.0.0
OS: Microsoft Windows NT 10.0.22631.0
.NET Version: 8.0.0
========================================
```

### Configuration Loading
```
Loading configuration from file...
  Legacy ServerUrl: http://server.com:8080
  Recording Path: C:\Users\...\Videos\IPTV Recordings
  Remember Credentials: True
  IPTV Sources count: 2
    - My Xtream Provider (XtreamCodes) - Active: True
    - Backup Source (XtreamCodes) - Active: False
```

### Source Connection Process
```
=== ConnectToActiveSources: Starting ===
Total sources in config: 2
Active sources: 1

--- Source 1/1 ---
Name: My Xtream Provider
Type: XtreamCodes
ID: 12345678-1234-1234-1234-123456789012
Attempting Xtream Codes connection to My Xtream Provider
Server URL: http://server.com:8080
Username: myuser
Port: 

  >> ConnectToXtreamSource: My Xtream Provider
  Credentials validation passed
  Creating new XtreamCodesService instance
  Calling AuthenticateAsync...
  Authentication result: True
  Setting EPG credentials
  Loading channels...
  Loaded 5432 channels
  Loading categories...
  Loaded 45 categories
  Tagging channels with source ID
  << ConnectToXtreamSource: Success (Total channels: 5432)

✓ Successfully connected to My Xtream Provider

=== Connection Summary ===
Successful: 1/1
Total channels loaded: 5432
Total categories loaded: 45
Timer started for periodic refresh
=== ConnectToActiveSources: Complete ===
```

### Connection Failures
```
--- Source 1/1 ---
Name: Bad Source
Type: XtreamCodes
ID: abcdef12-3456-7890-abcd-ef1234567890

  >> ConnectToXtreamSource: Bad Source
  Credentials validation passed
  Creating new XtreamCodesService instance
  Calling AuthenticateAsync...
  Authentication result: False
  << ConnectToXtreamSource: Failed (authentication returned false)

✗ Failed to connect to Bad Source (returned false)

=== Connection Summary ===
Successful: 0/1
Total channels loaded: 0
Total categories loaded: 0
No sources connected successfully
=== ConnectToActiveSources: Complete ===
```

### Missing Credentials
```
  >> ConnectToXtreamSource: Incomplete Source
  ✗ Missing connection details
    ServerUrl empty: False
    Username empty: True
    Password empty: False

✗ Exception connecting to Incomplete Source:
  Exception Type: Exception
  Message: Missing connection details
  Stack Trace: ...
```

## Log Timestamps

Every log entry includes precise timestamps:
```
[2024-01-15 14:23:45.123] === Application Starting ===
[2024-01-15 14:23:45.234] Checking FFmpeg availability...
[2024-01-15 14:23:45.345] Loading configuration...
```

Format: `[YYYY-MM-DD HH:MM:SS.mmm]`

## Interpreting Common Log Patterns

### Successful Connection
Look for these indicators:
- ✓ Successfully connected to [Source Name]
- Authentication result: True
- Loaded X channels
- Loaded X categories
- Total channels loaded: X

### Failed Authentication
```
Authentication result: False
✗ Failed to connect to [Source Name] (returned false)
```
**Meaning:** Credentials are wrong or server rejected the connection

### Missing Configuration
```
✗ Missing connection details
ServerUrl empty: True/False
Username empty: True/False
Password empty: True/False
```
**Meaning:** Source not properly configured in Manage Sources

### Network/Connection Errors
```
✗ Exception connecting to [Source Name]:
  Exception Type: HttpRequestException
  Message: Connection refused
```
**Meaning:** Cannot reach the server (check URL, firewall, internet)

### Timeout Errors
```
✗ Exception connecting to [Source Name]:
  Exception Type: TaskCanceledException
  Message: The operation was canceled
```
**Meaning:** Server didn't respond in time

## Debugging Workflows

### Issue: No Channels Loading

1. **Check if sources are configured:**
   ```
   IPTV Sources count: 0
   Active sources: 0
   ```
   → Go to Settings → Manage Sources

2. **Check if sources are active:**
   ```
   IPTV Sources count: 2
   Active sources: 0
   ```
   → Edit sources and check "Active"

3. **Check authentication:**
   ```
   Authentication result: False
   ```
   → Verify credentials in Manage Sources

### Issue: Some Sources Not Connecting

Look for each source in the log:
```
--- Source 1/2 ---
[Details...]
✓ Successfully connected

--- Source 2/2 ---
[Details...]
✗ Failed to connect
```

The failed source will show the specific error.

### Issue: Channels Loaded But Not Showing

Check these entries:
```
Total channels loaded: 5432
Total categories loaded: 45
UI updated with 46 categories
```

If channels loaded but UI not updated, it's a UI refresh issue.

## Triggering Additional Logging

### Manual Refresh
Click the "Refresh" button to trigger a new connection attempt:
```
=== Manual Refresh Clicked ===
Refreshing 2 active sources
```

### Opening Source Manager
Saving sources triggers reconnection:
```
Saved 2 source(s)
=== ConnectToActiveSources: Starting ===
```

## Log File Management

### File Size
The log file grows over time. You can:
- Delete it anytime (will be recreated)
- Clear it by opening and deleting content
- Archive old logs for history

### Sharing Logs for Support

When reporting issues:
1. Reproduce the problem
2. Open the log file immediately
3. Copy the relevant section (look for timestamps)
4. Include the section from "Application Starting" to the error

### Privacy Note

Logs contain:
- ✅ Server URLs
- ✅ Usernames
- ❌ **NOT passwords** (passwords are never logged)
- ✅ Timestamps and counts
- ✅ Error messages

Safe to share, but review for any sensitive server URLs if needed.

## Advanced: Filtering Logs

### Find Connection Attempts
Search for: `=== ConnectToActiveSources: Starting ===`

### Find Specific Source
Search for: `Name: My Source Name`

### Find Errors Only
Search for: `✗` (X with cross mark)

### Find Successful Connections
Search for: `✓` (check mark)

## Log Levels

Currently all logs are at DEBUG level. Future versions may include:
- INFO: General information
- WARN: Warnings that don't stop execution
- ERROR: Errors that affect functionality
- DEBUG: Detailed diagnostic information

## Disabling Logging

Currently there's no UI option to disable logging. To disable:
1. The logging fails silently if there are file permission issues
2. To fully disable, the `LogDebug()` method can be modified to return early

## Performance Impact

Debug logging has minimal performance impact:
- File writes are fast
- Failures are silently caught
- No UI blocking
- Suitable for production use

## Example Full Log Session

```
[14:23:45.123] ========================================
[14:23:45.123] === Application Starting ===
[14:23:45.124] Version: 1.0.0.0
[14:23:45.124] OS: Microsoft Windows NT 10.0.22631.0
[14:23:45.124] .NET Version: 8.0.0
[14:23:45.125] ========================================
[14:23:45.125] Checking FFmpeg availability...
[14:23:45.234] Loading configuration...
[14:23:45.235]   Loading configuration from file...
[14:23:45.245]   Legacy ServerUrl: 
[14:23:45.245]   Recording Path: C:\Users\Fred\Videos\IPTV Recordings
[14:23:45.245]   Remember Credentials: False
[14:23:45.246]   IPTV Sources count: 1
[14:23:45.246]     - Test Source (XtreamCodes) - Active: True
[14:23:45.246]   Configuration loaded successfully
[14:23:45.247] Set default recording path: C:\Users\Fred\Videos\IPTV Recordings
[14:23:45.247] Initializing recording service...
[14:23:45.345] Auto-connecting to active sources...
[14:23:45.346] === ConnectToActiveSources: Starting ===
[14:23:45.346] Total sources in config: 1
[14:23:45.346] Active sources: 1
[14:23:45.347] --- Source 1/1 ---
[14:23:45.347] Name: Test Source
[14:23:45.347] Type: XtreamCodes
[14:23:45.347] ID: 12345678-90ab-cdef-1234-567890abcdef
[14:23:45.348] Attempting Xtream Codes connection to Test Source
[14:23:45.348] Server URL: http://example.com:8080
[14:23:45.348] Username: testuser
[14:23:45.348] Port: 
[14:23:45.349]   >> ConnectToXtreamSource: Test Source
[14:23:45.349]   Credentials validation passed
[14:23:45.349]   Creating new XtreamCodesService instance
[14:23:45.350]   Calling AuthenticateAsync...
[14:23:47.123]   Authentication result: True
[14:23:47.124]   Setting EPG credentials
[14:23:47.125]   Loading channels...
[14:23:48.456]   Loaded 2341 channels
[14:23:48.457]   Loading categories...
[14:23:48.678]   Loaded 32 categories
[14:23:48.679]   Tagging channels with source ID
[14:23:48.789]   << ConnectToXtreamSource: Success (Total channels: 2341)
[14:23:48.790] ✓ Successfully connected to Test Source
[14:23:48.790] === Connection Summary ===
[14:23:48.791] Successful: 1/1
[14:23:48.791] Total channels loaded: 2341
[14:23:48.791] Total categories loaded: 32
[14:23:48.792] === LoadChannelsAndCategoriesFromCache ===
[14:23:48.792] Total channels in cache: 2341
[14:23:48.793] Total categories in cache: 32
[14:23:48.793] Unique categories: 28
[14:23:48.834] Selected 'All Channels' category
[14:23:48.835] UI updated with 29 categories
[14:23:48.835] === LoadChannelsAndCategoriesFromCache: Complete ===
[14:23:48.835] Timer started for periodic refresh
[14:23:48.836] === ConnectToActiveSources: Complete ===
```

## Troubleshooting the Logging System

### Log File Not Created
- Check folder permissions on `%APPDATA%\IPTV_DVR`
- Run application as administrator once
- Check antivirus isn't blocking file creation

### Logs Not Updating
- Refresh the file in your text editor
- Close and reopen the log file
- Application may be caching writes (check after app closes)

### Can't Find AppData Folder
- AppData folder is hidden by default
- In File Explorer: View → Show → Hidden Items
- Or use `%APPDATA%` in Run dialog or File Explorer address bar
