# EPG Source Management Feature

## Overview
This update adds comprehensive EPG (Electronic Program Guide) source management capabilities to the StreamingDVR application. Users can now manage multiple EPG sources and assign specific EPG sources to individual channels.

## New Features

### 1. EPG Source Management
- Add, edit, delete, and toggle EPG sources
- Support for multiple EPG sources (XMLTV format)
- Track last update time for each EPG source
- Enable/disable EPG sources without deleting them

### 2. Channel EPG Assignment
- Right-click on any channel to assign a specific EPG source
- Optional EPG Channel ID for precise matching
- Auto-matching support when Channel ID is not specified
- Per-channel EPG customization

## New Files Created

### Models
1. **StreamingDVR\Models\EpgSource.cs**
   - Defines the EPG source data structure
   - Properties: Id, Name, Url, IsActive, LastUpdated, LastError

2. **StreamingDVR\Models\ChannelEpgMapping.cs**
   - Maps channels to EPG sources
   - Properties: StreamId, ChannelName, EpgSourceId, EpgChannelId

### Forms
3. **StreamingDVR\Forms\EpgSourceManagerForm.cs**
   - Main EPG source management interface
   - Features: Add, Edit, Delete, Toggle Active status
   - Double-click to edit

4. **StreamingDVR\Forms\EpgSourceManagerForm.Designer.cs**
   - UI designer file for EPG Source Manager

5. **StreamingDVR\Forms\EpgSourceEditorForm.cs**
   - Form for adding/editing individual EPG sources
   - Input validation for URLs
   - Active/Inactive toggle

6. **StreamingDVR\Forms\EpgSourceEditorForm.Designer.cs**
   - UI designer file for EPG Source Editor

7. **StreamingDVR\Forms\AssignEpgForm.cs**
   - Form for assigning EPG sources to channels
   - Dropdown list of active EPG sources
   - Optional Channel ID input

8. **StreamingDVR\Forms\AssignEpgForm.Designer.cs**
   - UI designer file for Assign EPG Form

## Updated Files

### Configuration Service
- **StreamingDVR\Services\ConfigurationService.cs**
  - Added `EpgSources` list to AppConfiguration
  - Added `ChannelEpgMappings` list to AppConfiguration
  - Automatic persistence of EPG settings

### Main Form
- **StreamingDVR\Form1.cs**
  - Added `BtnManageEpgSources_Click` event handler
  - Added `MenuAssignEpg_Click` event handler
  - EPG management integrated into main workflow

- **StreamingDVR\Form1.Designer.cs**
  - Added new GroupBox for EPG Sources in Settings tab
  - Added "Manage EPG..." button in Settings
  - Added "Assign EPG..." context menu item for channels
  - Reorganized Settings tab layout

## User Interface Changes

### Settings Tab
The Settings tab now includes a new section:

```
┌─ EPG Sources ────────────────────────┐
│ Manage EPG (Electronic Program       │
│ Guide) sources for enhanced program  │
│ information.                         │
│                                      │
│ [Manage EPG...]                      │
└──────────────────────────────────────┘
```

### Channel Context Menu
Right-clicking on a channel now shows:
- Record Now
- Schedule Recording
- View EPG
- **Assign EPG...** ← NEW
- Copy Stream URL

## Usage Guide

### Managing EPG Sources

1. Go to the **Settings** tab
2. Click **"Manage EPG..."** button
3. In the EPG Source Manager:
   - Click **Add** to add a new EPG source
   - Enter a name and URL (typically an XMLTV file URL)
   - Click **Save**
   - Use **Edit** to modify existing sources
   - Use **Delete** to remove sources
   - Use **Toggle Active** to enable/disable sources

### Assigning EPG to Channels

1. Go to the **Channels** tab
2. Right-click on any channel
3. Select **"Assign EPG..."**
4. Choose an EPG source from the dropdown
5. (Optional) Enter a specific EPG Channel ID for precise matching
6. Click **Save**

### EPG URL Examples
Typical EPG URLs use XMLTV format:
- `http://example.com/epg.xml`
- `http://example.com/epg.xml.gz`
- `https://yourprovider.com/xmltv/epg.xml`

## Technical Details

### Data Storage
EPG sources and channel mappings are stored in:
```
%AppData%\IPTV_DVR\config.json
```

The configuration includes:
```json
{
  "EpgSources": [
    {
      "Id": "guid",
      "Name": "My EPG Source",
      "Url": "http://example.com/epg.xml",
      "IsActive": true,
      "LastUpdated": "2024-01-01T00:00:00",
      "LastError": null
    }
  ],
  "ChannelEpgMappings": [
    {
      "StreamId": 12345,
      "ChannelName": "Channel Name",
      "EpgSourceId": "guid",
      "EpgChannelId": "channel.id"
    }
  ]
}
```

### Integration Points
- EPG sources are filtered to show only active sources in assignment dialog
- Channel mappings are persisted across application restarts
- Removing an EPG source does not remove channel mappings (for data safety)
- Multiple channels can use the same EPG source

## Future Enhancements
Potential improvements for future versions:
- Automatic EPG data download and caching
- EPG data parsing and display enhancements
- Scheduled EPG updates
- EPG source validation and testing
- Bulk channel EPG assignment
- EPG source templates for common providers

## Benefits
1. **Flexibility**: Support multiple EPG sources
2. **Customization**: Per-channel EPG assignment
3. **Convenience**: Easy management interface
4. **Reliability**: Persistent storage of settings
5. **Scalability**: Ready for future EPG features

## Testing Recommendations
1. Test adding, editing, and deleting EPG sources
2. Test toggling EPG source active status
3. Test assigning EPG to various channels
4. Test removing EPG assignment (select "(None)")
5. Verify settings persist after application restart
6. Test with multiple EPG sources
7. Test with invalid URLs (validation should prevent)
