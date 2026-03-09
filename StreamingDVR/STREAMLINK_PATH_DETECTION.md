# Streamlink Path Detection - Implementation Guide

## Overview
Enhanced the StreamlinkValidator utility to detect and display the full path to the Streamlink executable. This helps with troubleshooting installation issues and verifying the Streamlink setup.

## New Features Added

### 1. GetStreamlinkPath() Method
**Purpose:** Detects the full path to the Streamlink executable

**Search Strategy:**
1. Uses Windows `where` command to find Streamlink in PATH
2. Checks common Python installation locations:
   - User Profile Python Scripts folders (Python 3.10, 3.11, 3.12)
   - Program Files Python folders
   - Standalone Streamlink installation in Program Files
3. Manually scans PATH environment variable

**Returns:** Full path to `streamlink.exe` or `null` if not found

#### Code Example:
```csharp
var streamlinkPath = StreamlinkValidator.GetStreamlinkPath();
if (streamlinkPath != null)
{
    Console.WriteLine($"Streamlink found at: {streamlinkPath}");
}
else
{
    Console.WriteLine("Streamlink not found");
}
```

### 2. GetStreamlinkInfo() Method
**Purpose:** Provides comprehensive installation information

**Returns:** Formatted string containing:
- Installation status (available/not available)
- Version information
- Full path to executable

#### Example Output:
```
Streamlink Installation Info:

✓ Status: Available and functional
✓ Version: streamlink 6.5.1
✓ Path: C:\Users\YourName\AppData\Roaming\Python\Python311\Scripts\streamlink.exe
```

### 3. Enhanced ShowStreamlinkInstallationInstructions()
**Changes:** Now detects if Streamlink is installed but not functioning

**Behavior:**
- If path is found but Streamlink doesn't work → Shows repair instructions
- If path is not found → Shows installation instructions

#### Repair Instructions (when path detected):
```
Streamlink appears to be installed but is not functioning correctly.

Detected Location:
C:\Users\YourName\...\streamlink.exe

Try the following:

1. Reinstall Streamlink:
   pip install --upgrade --force-reinstall streamlink

2. Check your PATH environment variable

3. Restart this application
```

## Detected Paths

### Python User Installation Paths
```
%USERPROFILE%\AppData\Roaming\Python\Python310\Scripts\streamlink.exe
%USERPROFILE%\AppData\Roaming\Python\Python311\Scripts\streamlink.exe
%USERPROFILE%\AppData\Roaming\Python\Python312\Scripts\streamlink.exe
%USERPROFILE%\AppData\Local\Programs\Python\Python310\Scripts\streamlink.exe
%USERPROFILE%\AppData\Local\Programs\Python\Python311\Scripts\streamlink.exe
%USERPROFILE%\AppData\Local\Programs\Python\Python312\Scripts\streamlink.exe
```

### Program Files Paths
```
%ProgramFiles%\Streamlink\bin\streamlink.exe
%ProgramFiles(x86)%\Streamlink\bin\streamlink.exe
%ProgramFiles%\Python310\Scripts\streamlink.exe
%ProgramFiles%\Python311\Scripts\streamlink.exe
%ProgramFiles%\Python312\Scripts\streamlink.exe
```

### PATH Environment Variable
The method also scans all directories in the system PATH variable looking for `streamlink.exe`.

## Integration in Form1.cs

### Updated CheckStreamlinkAvailability Method

**Before:**
```csharp
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

**After:**
```csharp
private void CheckStreamlinkAvailability()
{
    if (StreamlinkValidator.IsStreamlinkAvailable())
    {
        var info = StreamlinkValidator.GetStreamlinkInfo();
        MessageBox.Show(
            info ?? "Streamlink is installed and available!",
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

## Usage Examples

### Example 1: Display Path in Application
```csharp
var path = StreamlinkValidator.GetStreamlinkPath();
if (path != null)
{
    lblStreamlinkPath.Text = $"Streamlink: {path}";
}
```

### Example 2: Diagnostic Logging
```csharp
var info = StreamlinkValidator.GetStreamlinkInfo();
LogDebug(info);
```

### Example 3: Validate Before Recording
```csharp
private async Task StartStreamlinkRecording(string url)
{
    var path = StreamlinkValidator.GetStreamlinkPath();
    if (path == null)
    {
        MessageBox.Show("Streamlink executable not found!");
        return;
    }
    
    // Proceed with recording using detected path if needed
    // ... recording logic ...
}
```

## Troubleshooting Scenarios

### Scenario 1: Multiple Python Versions
**Situation:** User has Python 3.10, 3.11, and 3.12 installed with Streamlink in each
**Detection:** Method returns the first working path found
**Resolution:** All versions should work, but can manually verify with `GetStreamlinkPath()`

### Scenario 2: Streamlink in Non-Standard Location
**Situation:** User installed Streamlink in a custom directory
**Detection:** If added to PATH, the `where` command will find it
**Resolution:** Ensure Streamlink directory is in system PATH variable

### Scenario 3: Permission Issues
**Situation:** Streamlink.exe exists but can't execute
**Detection:** `GetStreamlinkPath()` returns path, but `IsStreamlinkAvailable()` returns false
**Message:** Shows repair instructions with detected path

### Scenario 4: Portable Python Installation
**Situation:** User has portable Python with Streamlink
**Detection:** Won't be found automatically
**Resolution:** Add portable Python Scripts folder to PATH or install system-wide

## Testing the Implementation

### Test 1: Path Detection with Streamlink Installed
```csharp
[Test]
public void TestGetStreamlinkPath_WhenInstalled_ReturnsPath()
{
    var path = StreamlinkValidator.GetStreamlinkPath();
    Assert.IsNotNull(path);
    Assert.IsTrue(File.Exists(path));
}
```

### Test 2: Info Display
```csharp
[Test]
public void TestGetStreamlinkInfo_ContainsExpectedData()
{
    var info = StreamlinkValidator.GetStreamlinkInfo();
    Assert.IsNotNull(info);
    Assert.IsTrue(info.Contains("Status"));
    Assert.IsTrue(info.Contains("Version") || info.Contains("not available"));
}
```

### Test 3: Check Button Click
1. Click "Check Streamlink" button
2. Verify message box shows:
   - Status line
   - Version line
   - Path line
3. All should have checkmarks (✓) if installed correctly

## Benefits of Path Detection

1. **Better Diagnostics** - Can identify exactly where Streamlink is installed
2. **Troubleshooting** - Helps users fix PATH issues
3. **Multiple Installations** - Detects which Streamlink is being used
4. **Repair Guidance** - Shows specific repair steps when path is found but not working
5. **Documentation** - Users can report exact installation path when seeking support

## Future Enhancements

### Possible Improvements:

1. **Multiple Installations**
   ```csharp
   public static List<string> GetAllStreamlinkPaths()
   {
       // Return all found Streamlink installations
   }
   ```

2. **Path Selection UI**
   - If multiple installations found, let user choose
   - Remember user's preferred installation

3. **Python Version Detection**
   ```csharp
   public static string? GetStreamlinkPythonVersion()
   {
       // Detect which Python version Streamlink is using
   }
   ```

4. **Executable Validation**
   ```csharp
   public static bool ValidateStreamlinkPath(string path)
   {
       // Verify the executable is actually Streamlink
   }
   ```

5. **Configuration Storage**
   - Save detected path in configuration
   - Use stored path instead of searching each time

## Error Handling

All methods include comprehensive error handling:

```csharp
try
{
    // Path detection logic
}
catch
{
    // Fall through to alternative methods
    // Never throw exceptions to user
}
```

**Philosophy:** Path detection should never crash the application. If one method fails, try alternatives. If all fail, return null gracefully.

## Performance Considerations

- **Caching:** Consider caching the detected path
- **First Call:** Takes ~100-500ms to scan all locations
- **Subsequent Calls:** Can be optimized by caching result
- **Impact:** Minimal - only called when user clicks "Check Streamlink"

## Compatibility

**Operating Systems:**
- ✅ Windows 10/11 - Full support
- ⚠️ Windows 7/8 - Should work but less tested
- ❌ Linux/Mac - Would need different path detection logic

**Python Versions Detected:**
- Python 3.10
- Python 3.11
- Python 3.12
- Can easily add more versions to search list

## Summary

The path detection enhancement provides:

✅ **Automatic Detection** - Finds Streamlink without user input  
✅ **Comprehensive Search** - Checks all common locations  
✅ **Detailed Information** - Shows status, version, and path  
✅ **Better Diagnostics** - Helps troubleshoot issues  
✅ **Repair Guidance** - Specific instructions when path found but not working  
✅ **No Breaking Changes** - Fully backward compatible  
✅ **Error Tolerant** - Never crashes on path detection failure  

This makes Streamlink setup and troubleshooting much easier for end users.
