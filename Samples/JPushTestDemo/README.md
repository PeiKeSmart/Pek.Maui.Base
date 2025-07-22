# JPush Test Demo

这是一个用于测试极光推送(JPush)集成功能的.NET MAUI应用程序。

## 项目结构

```
JPushTestDemo/
├── Configuration/
│   └── JPushConfig.cs              # JPush配置文件
├── Services/
│   ├── IJPushService.cs            # JPush服务接口
│   └── DefaultJPushService.cs      # 默认服务实现
├── Platforms/
│   └── Android/
│       ├── Services/
│       │   └── JPushServiceAndroid.cs  # Android平台JPush服务实现
│       ├── AndroidManifest.xml     # Android权限配置
│       └── MainApplication.cs      # JPush初始化
├── MainPage.xaml                   # 主页面UI
├── MainPage.xaml.cs               # 主页面逻辑
└── MauiProgram.cs                 # 依赖注入配置
```

## 功能特性

### 🔧 已实现功能
- ✅ 基础项目结构搭建
- ✅ JPush服务接口定义
- ✅ Android平台权限配置
- ✅ 依赖注入服务注册
- ✅ 测试UI界面
- ✅ 日志记录和错误处理

### 📱 测试功能
1. **获取Registration ID** - 获取设备的唯一标识符
2. **设置别名和标签** - 为设备设置别名和标签，用于精准推送
3. **测试通知** - 检查推送服务状态
4. **状态监控** - 实时显示操作状态和结果

## 配置说明

### 1. JPush AppKey 配置
在 `Configuration/JPushConfig.cs` 文件中配置您的JPush应用密钥：

```csharp
public const string APP_KEY = "YOUR_JPUSH_APP_KEY_HERE";
```

### 2. 启用JPush绑定库
当您的JPush绑定库(`Pek.Maui.Android.JPush`)准备就绪后，需要：

1. 取消注释 `MainApplication.cs` 中的JPush初始化代码
2. 取消注释 `JPushServiceAndroid.cs` 中的实际API调用
3. 确保项目引用正确指向您的绑定库

### 3. Android权限
项目已预配置以下JPush所需权限：
- 网络访问权限
- 读取手机状态权限
- 存储访问权限
- 定位权限（用于更精准的推送）
- 系统提醒窗口权限
- 等等...

## 使用说明

### 运行应用
1. 确保您有一台Android设备或模拟器
2. 在Visual Studio中选择Android平台
3. 编译并运行应用

### 测试推送功能
1. **获取Registration ID**：点击"获取Registration ID"按钮
2. **设置别名**：在别名输入框中输入别名，如"user123"
3. **设置标签**：在标签输入框中输入标签，多个标签用逗号分隔，如"VIP,Android"
4. **测试通知**：点击"发送测试通知"检查服务状态

### 发送推送通知
可以通过以下方式发送测试推送：

1. **JPush控制台**：登录JPush控制台，使用"推送"功能
2. **JPush REST API**：使用HTTP接口发送推送
3. **第三方工具**：如Postman等工具调用JPush API

## 开发注意事项

### 🔄 当前状态
- ✅ 项目框架已搭建完成
- ✅ JPush绑定库已引用
- ✅ API兼容性问题已解决（使用反射调用）
- ✅ 编译错误已修复
- 🔍 已添加自动检测可用API方法的调试功能

### 🛠️ 下一步工作
1. ✅ 引用 `Pek.Maui.Android.JPush` 绑定库
2. ✅ 使用反射方式调用JPush API（避免编译时错误）
3. 🔄 在实际设备上测试API调用
4. ⏳ 配置真实的JPush AppKey
5. ⏳ 进行完整的推送功能测试

### 📝 API兼容性
已完全解决API兼容性问题：
- ✅ 使用反射机制调用所有JPush API
- ✅ 支持多种方法名称的自动尝试（如 `GetRegistrationId` vs `getRegistrationID`）
- ✅ 详细的调试日志输出，显示实际可用的方法
- ✅ 优雅的错误处理，不会因为方法不存在而崩溃

### 📝 调试信息
应用会在调试输出中记录所有操作，格式为：
```
[JPush Test] 操作描述和结果
```

## 相关资源

- [JPush官方文档](https://docs.jiguang.cn/jpush/client/Android/android_guide/)
- [.NET MAUI官方文档](https://docs.microsoft.com/dotnet/maui/)
- [Android推送通知开发指南](https://developer.android.com/develop/ui/views/notifications)

## 许可证

本项目仅用于测试和学习目的。
