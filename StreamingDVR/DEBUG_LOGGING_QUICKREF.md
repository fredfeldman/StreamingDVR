# Debug Logging Quick Reference

## 📍 Log File Location
```
%APPDATA%\IPTV_DVR\debug.log
```

## 🚀 Quick Access
1. Press `Win + R`
2. Type: `%APPDATA%\IPTV_DVR`
3. Open `debug.log`

## 🔍 Common Log Patterns

### ✅ Success Pattern
```
✓ Successfully connected to [Source]
Authentication result: True
Loaded X channels
```

### ❌ Failure Patterns

**Wrong Credentials**
```
Authentication result: False
✗ Failed to connect (returned false)
```

**Missing Configuration**
```
✗ Missing connection details
ServerUrl empty: True
```

**Network Error**
```
✗ Exception connecting
Exception Type: HttpRequestException
Message: Connection refused
```

**Timeout**
```
Exception Type: TaskCanceledException
Message: The operation was canceled
```

## 🔎 Quick Search Terms

| Problem | Search For |
|---------|-----------|
| Find errors | `✗` |
| Find successes | `✓` |
| Find specific source | `Name: [Your Source]` |
| Find authentication | `Authentication result:` |
| Find startup | `=== Application Starting ===` |
| Find connections | `=== ConnectToActiveSources` |

## 📊 Key Metrics

```
Total sources in config: X
Active sources: X
Loaded X channels
Loaded X categories
Successful: X/X
```

## 🔐 Privacy

**Logged:** URLs, usernames, counts, errors
**NOT Logged:** Passwords, tokens, stream URLs

## 💡 Quick Troubleshooting

| Symptom | Log Check | Solution |
|---------|-----------|----------|
| No channels | `Active sources: 0` | Activate sources |
| Can't connect | `Authentication result: False` | Check credentials |
| Timeout | `TaskCanceledException` | Check network/firewall |
| Missing config | `IPTV Sources count: 0` | Add sources |

## 📝 Log Format
```
[YYYY-MM-DD HH:MM:SS.mmm] Message
```

## 🛠️ For Support
When reporting issues, include:
1. "Application Starting" section
2. Connection attempt section
3. Error messages

## 📚 Full Documentation
- `DEBUG_LOGGING_GUIDE.md` - Complete guide
- `DEBUG_LOGGING_SUMMARY.md` - Implementation details
