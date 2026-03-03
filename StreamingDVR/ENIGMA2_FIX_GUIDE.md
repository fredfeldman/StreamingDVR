# Fixed: Enigma2 Box Configuration Error

## Problem
If you only had an Enigma2 box configured (no Xtream Codes), the application would show errors about Xtream Codes when trying to use any features.

## Solution
The application has been updated to properly support multiple source types without requiring Xtream Codes.

## How to Use

### First Time Setup

1. **Launch the application**
   - The app will show "No active sources configured"

2. **Go to Settings tab**
   - Click the Settings tab at the top

3. **Click "Manage Sources..."**
   - Opens the Source Manager dialog

4. **Add your IPTV source**
   - Click "Add Source"
   - Enter a name (e.g., "My Enigma2 Box")
   - Select source type:
     - **Xtream Codes** - For Xtream Codes API providers
     - **Enigma2 Box** - For Dreambox, Vu+, etc. *(coming soon)*
     - **M3U Playlist** - For M3U/M3U8 files or URLs *(coming soon)*

5. **Enter connection details**
   - For Xtream Codes:
     - Server URL (e.g., `http://server.com:8080`)
     - Username
     - Password
   - For Enigma2 Box: *(implementation in progress)*
     - Server URL (box IP address)
     - Port (usually 80 or 8001)
     - Username (usually "root")
     - Password

6. **Make sure "Active" is checked**
   - Only active sources will be used

7. **Click Save**
   - Your source is saved

8. **Click Close** in Source Manager
   - The app will automatically connect to your active sources

### Managing Multiple Sources

You can have multiple sources configured:
- Multiple Xtream Codes providers
- Multiple Enigma2 boxes
- Multiple M3U playlists
- Or any combination

**Benefits:**
- All channels shown in one list
- Easy switching between providers
- Automatic failover if one source is down
- Centralized recording management

### Current Status

✅ **Working Now:**
- Multiple Xtream Codes sources
- Auto-connect on startup
- Source management UI
- No more Xtream Codes requirement

⏳ **Coming Soon:**
- Enigma2 box connection
- M3U playlist parsing
- Additional EPG sources per source

### Troubleshooting

**"No active sources configured"**
- Go to Settings → Manage Sources
- Add at least one source
- Make sure "Active" is checked

**"Unable to get stream URL"**
- Check that your source is connected
- Try clicking Refresh
- Check source credentials in Manage Sources

**Channels not loading**
- Click Refresh button
- Check Settings → Manage Sources
- Test connection for each source
- Make sure at least one source is Active

**Old settings not working**
- Your old Xtream Codes config was auto-migrated
- Check Manage Sources to see "Default Xtream Codes"
- Update credentials if needed

### Migration from Old Version

If you had Xtream Codes configured:
1. Your settings are automatically migrated
2. A source called "Default Xtream Codes" is created
3. Everything works as before
4. You can now add more sources

## Technical Notes

### Auto-Connect
- App connects to all active sources on startup
- No manual connection needed
- Progress shown in status bar

### Channel List
- Channels from all sources are combined
- Categories are merged
- Each channel remembers its source

### Recording
- Works with any source type
- Stream URL generated based on source
- Same recording features for all sources

### Refresh
- Reconnects to all active sources
- Reloads channel lists
- Updates categories

## Known Issues

### Enigma2 & M3U Support
Currently, only Xtream Codes sources are fully functional. Enigma2 and M3U support is in development.

**Workaround:**
- Use Xtream Codes sources for now
- Check for updates for Enigma2/M3U support

### Performance with Many Sources
- Having many sources (>5) may slow channel loading
- Recommended: Keep 3-5 active sources

## Need Help?

1. Check that source is Active in Manage Sources
2. Use Test Connection button in Source Manager
3. Check status bar for error messages
4. Try clicking Refresh

## What Changed

**Before:**
- Only supported single Xtream Codes connection
- Manual connect required
- Would error if Xtream Codes not configured

**After:**
- Supports multiple sources of different types
- Auto-connects on startup
- Works without Xtream Codes
- Better error messages
