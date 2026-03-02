# 🚀 Quick Start - IPTV DVR

Get started recording IPTV streams in 5 minutes!

## ⚡ Prerequisites (2 minutes)

### 1. Install FFmpeg
Choose ONE method:

**Easiest (winget):**
```powershell
winget install FFmpeg
```

**Or Chocolatey:**
```powershell
choco install ffmpeg
```

**Verify:**
```powershell
ffmpeg -version
```
You should see version information.

## 🎯 Setup (3 minutes)

### 1. Launch App
- Run StreamingDVR.exe
- App checks FFmpeg automatically

### 2. Connect to IPTV Service

Go to **Settings** tab:

```
Server URL: http://your-server.com:8080
Username: your_username
Password: your_password
```

Click **Connect** ➜ Should see "Connected successfully"

### 3. Start Recording

**Channels** tab:
1. Select a category (left side)
2. Choose a channel
3. Click **Record Now**
4. Select duration or "No" for continuous
5. Done! 🎉

## 📍 Key Locations

- **Recordings Folder**: `Documents\My Videos\IPTV Recordings`
- **Settings**: `%AppData%\IPTV_DVR\config.json`

## 🎮 Essential Controls

| Action | Button | Shortcut |
|--------|--------|----------|
| Record Channel | Record Now | Ctrl+R |
| Refresh List | Refresh | F5 |
| Open Recordings | Open Folder | Ctrl+O |
| View Stats | File > Statistics | Ctrl+I |
| Exit | File > Exit | Alt+F4 |

## 🆘 Common Issues

**Can't connect?**
- Check URL has `http://` or `https://`
- Verify username/password
- Test internet connection

**Recording failed?**
- Run: `ffmpeg -version` in PowerShell
- If not found, reinstall FFmpeg
- Add to PATH and restart app

**File won't play?**
- Install VLC Media Player
- Right-click file > Open With > VLC

## 💡 Pro Tips

1. **Test First**: Record 1 minute before long recordings
2. **Search**: Use search box instead of scrolling
3. **Shortcuts**: Learn keyboard shortcuts for speed
4. **Schedule**: Set up recurring recordings for shows
5. **Space**: HD streams use 2-5 GB per hour

## 📚 Need More Help?

- Full guide: Read `USER_GUIDE.md`
- Features: Read `FEATURES.md`
- Troubleshooting: Read `README.md`

## ✅ You're Ready!

**Start Recording Now:**
1. Go to Channels tab
2. Pick a channel
3. Click Record Now
4. Enjoy! 🎬

---

**Questions?** Check README.md for detailed troubleshooting.
