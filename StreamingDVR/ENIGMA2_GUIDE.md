# Enigma2 Box Connection Guide

## Overview

StreamingDVR now supports connecting to Enigma2-based set-top boxes including:
- **Dreambox** (DM500, DM600, DM7025, DM800, DM8000, etc.)
- **Vu+** (Solo, Duo, Uno, Ultimo, Zero, etc.)
- **GigaBlue** HD series
- **Mutant** HD series
- **Other Enigma2-compatible receivers**

## What is Enigma2?

Enigma2 is an open-source DVB receiver operating system used by many satellite and IPTV set-top boxes. These boxes expose a web interface that allows external applications to:
- Browse channel lists (bouquets)
- Stream live TV
- Access EPG data
- Control the receiver

## Requirements

### On Your Enigma2 Box

1. **Web Interface Enabled**
   - Most Enigma2 boxes have web interface enabled by default
   - Access via browser: `http://box-ip-address:80`

2. **Network Connection**
   - Box must be on same network as your PC
   - Or accessible via internet if port forwarding configured

3. **Authentication Credentials**
   - Default is often `root` / no password or `root` / `dreambox`
   - Can be changed in box settings: Menu → Setup → System → Network → Web Interface

### Finding Your Box's IP Address

**Method 1: From Box Menu**
- Menu → Setup → System → Network → Network Adapter
- Look for "IP Address"

**Method 2: Router's DHCP List**
- Log into your router
- Check connected devices
- Look for device named "dreambox", "vuplus", etc.

**Method 3: Network Scanner**
- Use tool like Advanced IP Scanner (Windows)
- Scan your local network (e.g., 192.168.1.0/24)
- Look for devices with port 80 open

## Adding an Enigma2 Source

1. **Open Source Manager**
   - Go to Settings tab
   - Click "Manage Sources"

2. **Add New Source**
   - Click "Add Source"
   - Name: Give it a descriptive name (e.g., "Living Room Dreambox")
   - Type: Select "Enigma2"

3. **Enter Connection Details**
   - **Server URL**: IP address or hostname of your box
     - Examples: `192.168.1.100` or `http://dreambox.local`
   - **Username**: Usually `root`
   - **Password**: Your box's web interface password
   - **Port**: Usually `80` (leave empty for default)

4. **Test & Save**
   - Click "Test Connection" to verify
   - If successful, click "OK"
   - Check "Active" checkbox in source manager
   - Click "Save & Close"

## Connection Examples

### Example 1: Dreambox on Local Network
```
Name: Living Room Dreambox
Type: Enigma2
Server URL: 192.168.1.150
Username: root
Password: dreambox
Port: (empty)
Active: ✓
```

### Example 2: Vu+ Box with Custom Password
```
Name: Bedroom Vu+ Solo
Type: Enigma2
Server URL: 192.168.1.200
Username: root
Password: MySecurePass123
Port: (empty)
Active: ✓
```

### Example 3: Box with Custom Port
```
Name: Remote Enigma2 Box
Type: Enigma2
Server URL: mybox.dyndns.org
Username: root
Password: secure123
Port: 8080
Active: ✓
```

## What Gets Loaded

When connecting to an Enigma2 box, StreamingDVR loads:

### Bouquets (Categories)
- Bouquets are channel groups configured on your box
- Each bouquet becomes a category in StreamingDVR
- Examples: "Favorites", "Sky Deutschland", "Sports", etc.

### Channels
- All channels from all bouquets
- Channel names as configured on your box
- Service references (unique IDs for streaming)

### Stream URLs
- Automatically generated from service references
- Format: `http://box-ip/web/stream.m3u?ref={service_ref}`
- Can be opened in VLC or recorded

## Troubleshooting

### Cannot Connect

**Check Network Access:**
```
1. Open browser
2. Go to: http://your-box-ip
3. Should see Enigma2 web interface
4. If not, check:
   - Box is powered on
   - Network cable connected (or WiFi active)
   - Correct IP address
   - Firewall not blocking
```

**Check Authentication:**
```
Error: "Authentication failed"
→ Verify username/password
→ Try default: root / (no password)
→ Try common: root / dreambox
→ Check box settings for web interface password
```

**Check Web Interface Enabled:**
```
On box:
Menu → Setup → System → Network → Web Interface
- Enable: Yes
- Port: 80 (or your custom port)
- Authentication: Yes (recommended)
```

### No Channels Showing

**Check Bouquets:**
```
On box:
Menu → Information → Service Searching → Bouquets
- Make sure you have bouquets configured
- If empty, run a channel scan first
```

**Check Debug Logs:**
```
Open: %APPDATA%\IPTV_DVR\debug.log
Look for:
- "Loaded X bouquets"
- "Loaded X channels"
If counts are 0, bouquets are empty on box
```

### Streams Not Playing

**Try in VLC First:**
```
1. Double-click channel in StreamingDVR
2. Copy the stream URL shown
3. Open VLC → Media → Open Network Stream
4. Paste URL and test

If VLC can't play:
- Stream URL format is wrong
- Box's streaming is not working
- Network/firewall issue

If VLC CAN play:
- Issue is with StreamingDVR recording
- Check FFmpeg installation
```

**Check Box's Streaming Service:**
```
Some boxes require:
- OpenWebif plugin installed
- Transcoding enabled for external access
- Correct network interface selected
```

### Slow Channel Loading

**Normal Behavior:**
- First connection: 5-10 seconds for large channel lists
- Subsequent refreshes: 2-5 seconds

**If Very Slow (>30 seconds):**
- Network latency between PC and box
- Box is under heavy load (recording, watching TV)
- Many bouquets with many channels

**Optimization Tips:**
- Reduce number of bouquets on box
- Use wired connection instead of WiFi
- Close unused applications on box

## Features

### ✅ Supported
- Browse all bouquets/channels
- View channel names
- Get stream URLs
- Record live streams
- Multiple Enigma2 boxes simultaneously
- Mix with Xtream Codes sources

### ⚠️ Limited
- EPG: Only if box has EPG data loaded
- Channel logos: Not yet implemented
- Remote control: Not available

### ❌ Not Supported
- Timer management on box
- Channel scanning
- Box configuration changes
- File playback from box recordings

## Advanced Configuration

### HTTPS Access
```
If box uses HTTPS (rare):
Server URL: https://192.168.1.100
Port: 443

Note: Self-signed certificates may cause issues
```

### Multiple Boxes
```
You can add multiple Enigma2 boxes:
1. Add first box (e.g., "Living Room")
2. Add second box (e.g., "Bedroom")
3. Both appear as separate sources
4. Channels from both shown together
5. Can filter by source if needed
```

### Port Forwarding (Remote Access)
```
If accessing box over internet:
1. Router: Forward external port → box IP:80
2. Get your public IP or use DynDNS
3. Server URL: your-public-ip or domain
4. Port: your-forwarded-port

Security Warning:
- Use strong password
- Consider VPN instead
- Be aware of bandwidth usage
```

## Box-Specific Notes

### Dreambox
- Web interface path: `/web/`
- Default user: `root`
- Default pass: `dreambox` or no password
- Requires OpenWebif plugin for best compatibility

### Vu+
- Web interface path: `/web/`
- Default user: `root`
- Default pass: No password or `vuplus`
- OpenWebif usually pre-installed

### GigaBlue
- Similar to Dreambox
- Check specific model documentation
- Some models need OpenWebif installed

## Security Recommendations

1. **Change Default Password**
   - Don't use default `root` / `dreambox`
   - Use strong password on web interface

2. **Local Network Only**
   - Keep box on private network
   - Don't expose to internet without VPN

3. **Regular Updates**
   - Keep Enigma2 firmware updated
   - Update OpenWebif plugin

4. **Firewall Rules**
   - Only allow PC's IP to access box
   - Block other devices if possible

## FAQ

**Q: Can I use this with OTT/IPTV services on my Enigma2 box?**
A: Yes! If channels are configured in bouquets, they'll appear.

**Q: Will this interfere with watching TV on the box?**
A: No. Streaming is independent of what you're watching.

**Q: Can I record while box is recording?**
A: Yes, but box hardware may limit simultaneous streams.

**Q: Do I need OpenWebif installed?**
A: Highly recommended. Most boxes include it or can install via plugin manager.

**Q: Can I use this with Enigma1?**
A: No, only Enigma2 is supported.

**Q: What if my box is behind a router?**
A: As long as PC and box are on same network, it works.

**Q: Can I access box from different location?**
A: Yes with port forwarding or VPN, but be careful with security.

## Debug Logging

All Enigma2 connection attempts are logged:

**Log Location:** `%APPDATA%\IPTV_DVR\debug.log`

**What's Logged:**
- Connection attempts
- Authentication results
- Bouquet/channel counts
- XML parsing errors
- Stream URL generation

**Example Log:**
```
[14:23:45] Attempting Enigma2 connection to Living Room Dreambox
[14:23:45] Server URL: 192.168.1.150
[14:23:45] Username: root
[14:23:45]   >> ConnectToEnigma2Source: Living Room Dreambox
[14:23:45]   Credentials validation passed
[14:23:45]   Creating new Enigma2Service instance
[14:23:45]   Calling AuthenticateAsync...
[14:23:46]   Authentication result: True
[14:23:46]   Loading bouquets (categories)...
[14:23:47]   Loaded 8 bouquets
[14:23:47]   Loading channels from all bouquets...
[14:23:49]   Loaded 432 channels
[14:23:49]   ✓ Successfully connected to Living Room Dreambox
```

See **DEBUG_LOGGING_GUIDE.md** for complete logging documentation.

## Related Documentation

- **MULTI_SOURCE_GUIDE.md** - Managing multiple sources
- **DEBUG_LOGGING_GUIDE.md** - Troubleshooting with logs
- **USER_GUIDE.md** - General application usage

## Support

If you encounter issues:
1. Check debug log for errors
2. Test access via browser first
3. Verify OpenWebif is installed/working
4. Check box's network settings
5. Report issue with log excerpt
