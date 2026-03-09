# Streamlink Implementation - Quick Reference

## What's Been Done ✅

### Backend Implementation (Complete)
1. ✅ **ConfigurationService.cs** - Streamlink properties added to AppConfiguration
2. ✅ **RecordingService.cs** - Streamlink fields added (lines 16-21)
3. ✅ **RecordingService.cs** - StartStreamlinkRecordingAsync method implemented (lines 280-357)
4. ✅ **RecordingService.cs** - StartRecordingAsync modified to choose FFmpeg/Streamlink (lines 108-115)
5. ✅ **StreamlinkValidator.cs** - Complete utility class created
6. ✅ **Form1.cs** - SaveConfiguration has TODO comments for Streamlink (lines 144-147)
7. ✅ **Form1.cs** - InitializeRecordingService has TODO comments (lines 164-172)

## What Still Needs to Be Done ⚠️

### Missing Code (Needs Manual Addition)

#### 1. RecordingService.cs - ConfigureStreamlink Method ⚠️
**File:** `StreamingDVR\Services\RecordingService.cs`  
**Location:** After line 87 (after `SetScheduledRecordingCallback` method)

**Add this method:**
```csharp
public void ConfigureStreamlink(bool useStreamlink, string quality, bool retryOpen, int retryStreams, string options)
{
    _useStreamlink = useStreamlink;
    _streamlinkQuality = quality;
    _streamlinkRetryOpen = retryOpen;
    _streamlinkRetryStreams = retryStreams;
    _streamlinkOptions = options;
}
```

#### 2. Form1.cs - Event Handler Methods ⚠️
**File:** `StreamingDVR\Form1.cs`  
**Location:** At end of file, before final closing braces (around line 1235)

**Add these three methods:**
```csharp
// Streamlink Event Handlers

private void ChkUseStreamlink_CheckedChanged(object sender, EventArgs e)
{
    // Enable/disable Streamlink quality controls based on checkbox
    cboStreamlinkQuality.Enabled = chkUseStreamlink.Checked;
    btnCheckStreamlink.Enabled = chkUseStreamlink.Checked;
    
    // Save configuration when changed
    SaveConfiguration();
    
    // Reconfigure recording service
    var config = _configService.LoadConfiguration();
    _recordingService?.ConfigureStreamlink(
        config.UseStreamlink,
        config.StreamlinkQuality,
        config.StreamlinkRetryOpen,
        config.StreamlinkRetryStreams,
        config.StreamlinkOptions
    );
    
    UpdateStatus(chkUseStreamlink.Checked 
        ? "Streamlink enabled for recordings" 
        : "Using FFmpeg for recordings");
}

private void BtnCheckStreamlink_Click(object sender, EventArgs e)
{
    CheckStreamlinkAvailability();
}

private void CheckStreamlinkAvailability()
{
    if (StreamlinkValidator.IsStreamlinkAvailable())
    {
        var version = StreamlinkValidator.GetStreamlinkVersion();
        MessageBox.Show(
            $"Streamlink is installed and available!\n\n{version}",
            "Streamlink Status",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
    else
    {
        StreamlinkValidator.ShowStreamlinkInstallationInstructions();
    }
}
```

### UI Controls (Must Add via Visual Studio Designer)

#### Step 1: Open Form1 in Designer
1. Right-click `Form1.cs` in Solution Explorer
2. Select "View Designer"

#### Step 2: Add GroupBox
1. Click on TabControl → Select "Settings" tab
2. From Toolbox → Drag "GroupBox" onto Settings tab
3. Set properties:
   - Name: `groupBoxStreamlink`
   - Text: "Streamlink Settings"
   - Location: 25, 350
   - Size: 750, 125

#### Step 3: Add Controls to GroupBox

**Label (lblStreamlinkInfo):**
- Text: "Use Streamlink for better compatibility with various stream sources"
- Location: 25, 28

**CheckBox (chkUseStreamlink):**
- Text: "Use Streamlink for recording"
- Location: 25, 58
- Events: CheckedChanged → `ChkUseStreamlink_CheckedChanged`

**Label (lblStreamlinkQuality):**
- Text: "Quality:"
- Location: 310, 60

**ComboBox (cboStreamlinkQuality):**
- Location: 389, 56
- Size: 150, 33
- DropDownStyle: DropDownList
- Items (in order):
  - best
  - worst
  - 1080p
  - 720p
  - 480p
  - 360p
  - source

**Button (btnCheckStreamlink):**
- Text: "Check Streamlink"
- Location: 555, 56
- Size: 158, 35
- Events: Click → `BtnCheckStreamlink_Click`

#### Step 4: Wire Events in Designer
1. Select `chkUseStreamlink`
2. Properties → Events (⚡ icon)
3. CheckedChanged → Select `ChkUseStreamlink_CheckedChanged`

4. Select `btnCheckStreamlink`
5. Properties → Events (⚡ icon)
6. Click → Select `BtnCheckStreamlink_Click`

#### Step 5: Save Designer
Press Ctrl+S

### Code to Uncomment (After UI is Added)

#### 1. SaveConfiguration (Line 144-147)
**Remove // from these lines:**
```csharp
UseStreamlink = chkUseStreamlink.Checked,
StreamlinkQuality = cboStreamlinkQuality.SelectedItem?.ToString() ?? "best",
StreamlinkOptions = txtStreamlinkOptions.Text
```

#### 2. InitializeRecordingService (Line 164-172)
**Remove // from these lines:**
```csharp
var config = _configService.LoadConfiguration();
_recordingService.ConfigureStreamlink(
    config.UseStreamlink,
    config.StreamlinkQuality,
    config.StreamlinkRetryOpen,
    config.StreamlinkRetryStreams,
    config.StreamlinkOptions
);
```

#### 3. LoadConfiguration
**Add to LoadConfiguration method:**
```csharp
// Load Streamlink settings
chkUseStreamlink.Checked = config.UseStreamlink;
cboStreamlinkQuality.SelectedItem = config.StreamlinkQuality;
if (cboStreamlinkQuality.SelectedItem == null)
    cboStreamlinkQuality.SelectedIndex = 0;
txtStreamlinkOptions.Text = config.StreamlinkOptions;

// Enable/disable controls based on checkbox
cboStreamlinkQuality.Enabled = chkUseStreamlink.Checked;
btnCheckStreamlink.Enabled = chkUseStreamlink.Checked;
```

## Complete Step-by-Step Checklist

- [ ] **Step 1:** Add `ConfigureStreamlink()` method to RecordingService.cs (line 88)
- [ ] **Step 2:** Add three event handler methods to Form1.cs (line 1235)
- [ ] **Step 3:** Open Form1 in Designer (View → Designer)
- [ ] **Step 4:** Add `groupBoxStreamlink` GroupBox to Settings tab
- [ ] **Step 5:** Add 6 child controls to GroupBox
- [ ] **Step 6:** Wire up event handlers in Designer Properties
- [ ] **Step 7:** Save Designer (Ctrl+S)
- [ ] **Step 8:** Uncomment SaveConfiguration Streamlink lines (line 144-147)
- [ ] **Step 9:** Uncomment InitializeRecordingService lines (line 164-172)
- [ ] **Step 10:** Add LoadConfiguration Streamlink code
- [ ] **Step 11:** Build solution (Ctrl+Shift+B)
- [ ] **Step 12:** Run and test (F5)

## Documentation Created

1. ✅ **STREAMLINK_IMPLEMENTATION.md** - Complete implementation guide with before/after examples
2. ✅ **STREAMLINK_VALIDATOR.md** - StreamlinkValidator utility documentation
3. ✅ **STREAMLINK_GUIDE.md** - User guide
4. ✅ **STREAMLINK_SUMMARY.md** - Quick summary (existing)
5. ✅ **STREAMLINK_QUICKREF.md** - This file

## Testing After Implementation

Once everything is implemented:

1. Build solution
2. Run application
3. Go to Settings tab
4. See "Streamlink Settings" section
5. Click "Check Streamlink":
   - If installed → Shows version
   - If not installed → Shows installation instructions
6. Enable "Use Streamlink for recording"
7. Select quality (e.g., "best")
8. Try recording a channel
9. Verify recording works

## Support

All implementation details are in:
- **STREAMLINK_IMPLEMENTATION.md** - Comprehensive guide with code examples
- **STREAMLINK_VALIDATOR.md** - Utility class usage guide

Both files contain exact line numbers and complete before/after code examples.
