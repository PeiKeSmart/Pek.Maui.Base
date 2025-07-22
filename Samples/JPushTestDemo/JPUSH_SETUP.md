# JPush 配置说明

## 快速开始

### 1. 获取JPush应用密钥

1. 访问 [JPush控制台](https://www.jiguang.cn/)
2. 注册/登录账号
3. 创建新的应用
4. 在应用详情页面获取 `AppKey`

### 2. 配置应用密钥

编辑 `Configuration/JPushConfig.cs` 文件：

```csharp
public const string APP_KEY = "你的JPush应用密钥";
```

### 3. Android包名配置

确保您的JPush控制台中配置的包名与项目中的包名一致：

- **项目包名**: `com.companyname.jpushtestdemo`
- **配置位置**: `JPushTestDemo.csproj` 中的 `<ApplicationId>`

如需修改包名，请同时修改：
1. 项目文件中的 `<ApplicationId>`
2. JPush控制台中的应用配置

### 4. 测试步骤

1. **编译项目**:
   ```bash
   dotnet build -f net9.0-android
   ```

2. **部署到Android设备**:
   ```bash
   dotnet run -f net9.0-android
   ```

3. **获取Registration ID**:
   - 打开应用
   - 点击"获取Registration ID"按钮
   - 记录显示的Registration ID

4. **发送测试推送**:
   - 登录JPush控制台
   - 选择"推送" -> "立即推送"
   - 选择推送目标（可用Registration ID）
   - 输入推送内容并发送

### 5. 调试信息

- 查看Visual Studio输出窗口中的调试信息
- 标签格式: `[JPush Test]`
- Android日志可通过 `adb logcat` 查看

## 常见问题

### Q: Registration ID为空或"尚未生成"
**A**: 
- 检查网络连接
- 确认AppKey配置正确
- 等待几秒后重试
- 检查Android权限是否授予

### Q: 无法接收推送
**A**:
- 确认应用在前台或后台运行
- 检查设备通知权限
- 确认JPush控制台中的推送设置
- 验证Registration ID是否正确

### Q: 编译错误
**A**:
- 确保已安装Android SDK
- 检查绑定库引用是否正确
- 清理并重新构建项目

## 高级配置

### 自定义通知样式

可以在 `AndroidManifest.xml` 中添加更多配置：

```xml
<meta-data android:name="JPUSH_CHANNEL" android:value="default_channel"/>
<meta-data android:name="JPUSH_APPKEY" android:value="你的AppKey"/>
```

### 添加消息监听

可以创建自定义的广播接收器来处理JPush消息：

```csharp
[BroadcastReceiver(Enabled = true)]
[IntentFilter(new[] { "cn.jpush.android.intent.RECEIVE_MESSAGE" })]
public class JPushMessageReceiver : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        // 处理接收到的消息
    }
}
```
