# EPG Management - Quick Start

## Add an EPG Source

1. **Settings Tab** → **Manage EPG...** button
2. Click **Add**
3. Enter:
   - **Name**: A friendly name (e.g., "Main EPG")
   - **URL**: XMLTV file URL (e.g., `http://example.com/epg.xml`)
   - **Active**: Check to enable
4. Click **Save**

## Assign EPG to a Channel

1. **Channels Tab**
2. Right-click on a channel
3. Select **Assign EPG...**
4. Choose EPG source from dropdown
5. (Optional) Enter EPG Channel ID for precise matching
6. Click **Save**

## Remove EPG Assignment

1. Right-click channel → **Assign EPG...**
2. Select **(None)** from dropdown
3. Click **Save**

## Tips

- Leave EPG Channel ID blank for auto-matching
- Disable EPG sources instead of deleting them
- Only active EPG sources appear in assignment dropdown
- One EPG source can serve multiple channels

## Common EPG Formats

- **XMLTV**: `.xml` or `.xml.gz`
- Usually provided by IPTV service providers
- Can be hosted locally or remotely

## Troubleshooting

**No EPG sources in dropdown?**
- Check that EPG sources are marked as "Active"
- Go to Settings → Manage EPG to verify

**EPG not showing for channel?**
- Verify channel has EPG assigned
- Check EPG Channel ID if specified
- Ensure EPG source URL is accessible
