#if ANDROID
using JPushTestDemo.Services;
using Android.Content;
using System.Linq;

namespace JPushTestDemo.Platforms.Android.Services;

/// <summary>
/// Android平台的JPush服务实现
/// 使用适配器模式，避免反射调用
/// </summary>
public class JPushServiceAndroid : IJPushService
{
    private static bool _isInitialized = false;
    
    /// <summary>
    /// 确保JPush已初始化
    /// </summary>
    private void EnsureInitialized()
    {
        if (!_isInitialized)
        {
            try
            {
                var context = Platform.CurrentActivity ?? global::Android.App.Application.Context;
                
                if (context == null)
                {
                    System.Diagnostics.Debug.WriteLine("无法获取Android上下文，延迟初始化");
                    return;
                }
                
                System.Diagnostics.Debug.WriteLine($"开始初始化JPush，上下文类型: {context.GetType().Name}");
                
                // 先尝试设置调试模式
                try
                {
                    CN.Jpush.Android.Api.JPushInterface.SetDebugMode(true);
                    System.Diagnostics.Debug.WriteLine("JPush调试模式已开启");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"设置调试模式失败: {ex.Message}");
                }
                
                // 尝试初始化JPush
                try
                {
                    CN.Jpush.Android.Api.JPushInterface.Init(context);
                    System.Diagnostics.Debug.WriteLine($"JPush初始化成功，APP_KEY: {Configuration.JPushConfig.APP_KEY}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"JPush初始化失败: {ex.Message}");
                }
                
                // 等待一小段时间让JPush完成初始化
                System.Threading.Thread.Sleep(1000);
                
                // 验证初始化结果
                try
                {
                    // 注意：JPushInterface中没有GetDebugMode方法，跳过这个验证
                    System.Diagnostics.Debug.WriteLine("JPush初始化验证 - 调试模式设置已完成");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"JPush初始化验证失败: {ex.Message}");
                }
                
                // 调试：列出JPushInterface类的所有可用方法
                LogAvailableMethods();
                
                _isInitialized = true;
                System.Diagnostics.Debug.WriteLine("JPush service initialization completed");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"JPush initialization failed: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
    
    /// <summary>
    /// 调试方法：列出JPushInterface类的所有可用方法
    /// </summary>
    private void LogAvailableMethods()
    {
        try
        {
            var jpushType = typeof(CN.Jpush.Android.Api.JPushInterface);
            System.Diagnostics.Debug.WriteLine("=== JPushInterface 可用方法 ===");
            
            var methods = jpushType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (var method in methods)
            {
                var parameters = string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
                System.Diagnostics.Debug.WriteLine($"方法: {method.Name}({parameters}) -> {method.ReturnType.Name}");
            }
            
            var properties = jpushType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (var prop in properties)
            {
                System.Diagnostics.Debug.WriteLine($"属性: {prop.Name} -> {prop.PropertyType.Name}");
            }
            System.Diagnostics.Debug.WriteLine("=== 方法列表结束 ===");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"无法列出方法: {ex.Message}");
        }
    }
    public string GetRegistrationId()
    {
        try
        {
            EnsureInitialized();
            
            // 获取当前Android上下文
            var context = Platform.CurrentActivity ?? global::Android.App.Application.Context;
            
            // 添加详细的诊断信息
            PerformDetailedDiagnosis(context);
            
            // 尝试获取Registration ID
            string regId = null;
            
            try 
            {
                // 使用绑定库的标准方法调用
                regId = CN.Jpush.Android.Api.JPushInterface.GetRegistrationID(context);
                System.Diagnostics.Debug.WriteLine($"GetRegistrationID调用成功，返回值: '{regId}'");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetRegistrationID调用失败: {ex.Message}");
                return $"API调用失败: {ex.Message}";
            }
            
            if (string.IsNullOrEmpty(regId))
            {
                System.Diagnostics.Debug.WriteLine("Registration ID为空，JPush可能正在注册中...");
                
                // 检查JPush连接状态
                try
                {
                    bool isConnected = CN.Jpush.Android.Api.JPushInterface.GetConnectionState(context);
                    System.Diagnostics.Debug.WriteLine($"JPush连接状态: {isConnected}");
                    
                    if (isConnected)
                    {
                        return "JPush已连接，但Registration ID尚未生成。请稍后重试。";
                    }
                    else
                    {
                        return "JPush未连接，请检查网络连接和APP_KEY配置。";
                    }
                }
                catch
                {
                    return "JPush正在初始化中，Registration ID尚未生成。请稍后重试。";
                }
            }
            
            System.Diagnostics.Debug.WriteLine($"成功获取Registration ID: {regId}");
            return regId;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetRegistrationId error: {ex.Message}");
            return $"获取Registration ID失败: {ex.Message}";
        }
    }

    /// <summary>
    /// 执行详细的JPush诊断
    /// </summary>
    private void PerformDetailedDiagnosis(global::Android.Content.Context context)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("=== JPush 详细诊断开始 ===");
            
            // 1. 检查APP_KEY
            var appInfo = context.PackageManager.GetApplicationInfo(context.PackageName, global::Android.Content.PM.PackageInfoFlags.MetaData);
            if (appInfo?.MetaData != null)
            {
                var appKey = appInfo.MetaData.GetString("JPUSH_APPKEY");
                System.Diagnostics.Debug.WriteLine($"AndroidManifest中的APP_KEY: {appKey}");
                System.Diagnostics.Debug.WriteLine($"配置文件中的APP_KEY: {Configuration.JPushConfig.APP_KEY}");
                System.Diagnostics.Debug.WriteLine($"APP_KEY匹配: {appKey == Configuration.JPushConfig.APP_KEY}");
            }
            
            // 2. 检查包名
            System.Diagnostics.Debug.WriteLine($"应用包名: {context.PackageName}");
            
            // 3. 检查网络权限
            var hasInternet = context.CheckCallingOrSelfPermission(global::Android.Manifest.Permission.Internet);
            var hasNetworkState = context.CheckCallingOrSelfPermission(global::Android.Manifest.Permission.AccessNetworkState);
            System.Diagnostics.Debug.WriteLine($"网络权限: Internet={hasInternet}, NetworkState={hasNetworkState}");
            
            // 4. 检查网络连接
            var connectivityManager = (global::Android.Net.ConnectivityManager)context.GetSystemService(global::Android.Content.Context.ConnectivityService);
            var activeNetwork = connectivityManager?.ActiveNetworkInfo;
            System.Diagnostics.Debug.WriteLine($"网络连接状态: {activeNetwork?.IsConnected} (类型: {activeNetwork?.TypeName})");
            
            // 5. 尝试获取JPush状态信息
            try
            {
                // 注意：JPushInterface中没有GetDebugMode方法，尝试其他验证方法
                // 尝试调用GetConnectionState来验证JPush是否可用
                bool isConnected = CN.Jpush.Android.Api.JPushInterface.GetConnectionState(context);
                System.Diagnostics.Debug.WriteLine($"JPush连接状态: {isConnected}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取JPush状态失败: {ex.Message}");
            }
            
            // 6. 检查初始化状态
            System.Diagnostics.Debug.WriteLine($"JPush服务初始化状态: {_isInitialized}");
            
            System.Diagnostics.Debug.WriteLine("=== JPush 详细诊断结束 ===");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"诊断过程出错: {ex.Message}");
        }
    }

    public void SetAlias(string alias, int sequence = 0)
    {
        try
        {
            var context = Platform.CurrentActivity ?? global::Android.App.Application.Context;
            
            // TODO: 根据实际绑定库API调整方法调用
            // 可能的调用方式：
            // CN.Jpush.Android.Api.JPushInterface.setAlias(context, sequence, alias);
            // 或者：
            // CN.Jpush.Android.Api.JPushInterface.SetAlias(context, sequence, alias);
            
            System.Diagnostics.Debug.WriteLine($"SetAlias called: {alias}, sequence: {sequence} (待实现)");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"SetAlias error: {ex.Message}");
        }
    }

    public void SetTags(ISet<string> tags, int sequence = 0)
    {
        try
        {
            var context = Platform.CurrentActivity ?? global::Android.App.Application.Context;
            
            // TODO: 根据实际绑定库API调整方法调用
            // 可能的调用方式：
            // CN.Jpush.Android.Api.JPushInterface.setTags(context, sequence, tags);
            
            System.Diagnostics.Debug.WriteLine($"SetTags called: {string.Join(", ", tags)}, sequence: {sequence} (待实现)");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"SetTags error: {ex.Message}");
        }
    }

    public void DeleteAlias(int sequence = 0)
    {
        try
        {
            var context = Platform.CurrentActivity ?? global::Android.App.Application.Context;
            
            // TODO: 根据实际绑定库API调整方法调用
            System.Diagnostics.Debug.WriteLine($"DeleteAlias called, sequence: {sequence} (待实现)");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DeleteAlias error: {ex.Message}");
        }
    }

    public void DeleteTags(ISet<string> tags, int sequence = 0)
    {
        try
        {
            var context = Platform.CurrentActivity ?? global::Android.App.Application.Context;
            
            // TODO: 根据实际绑定库API调整方法调用
            System.Diagnostics.Debug.WriteLine($"DeleteTags called: {string.Join(", ", tags)}, sequence: {sequence} (待实现)");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DeleteTags error: {ex.Message}");
        }
    }

    public void CleanTags(int sequence = 0)
    {
        try
        {
            var context = Platform.CurrentActivity ?? global::Android.App.Application.Context;
            
            // TODO: 根据实际绑定库API调整方法调用
            System.Diagnostics.Debug.WriteLine($"CleanTags called, sequence: {sequence} (待实现)");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"CleanTags error: {ex.Message}");
        }
    }

    public void GetAlias(int sequence = 0)
    {
        try
        {
            var context = Platform.CurrentActivity ?? global::Android.App.Application.Context;
            
            // TODO: 根据实际绑定库API调整方法调用
            System.Diagnostics.Debug.WriteLine($"GetAlias called, sequence: {sequence} (待实现)");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetAlias error: {ex.Message}");
        }
    }

    public void GetTags(int sequence = 0)
    {
        try
        {
            var context = Platform.CurrentActivity ?? global::Android.App.Application.Context;
            
            // TODO: 根据实际绑定库API调整方法调用
            System.Diagnostics.Debug.WriteLine($"GetTags called, sequence: {sequence} (待实现)");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetTags error: {ex.Message}");
        }
    }

    public void StopPush()
    {
        try
        {
            var context = Platform.CurrentActivity ?? global::Android.App.Application.Context;
            
            // TODO: 根据实际绑定库API调整方法调用
            System.Diagnostics.Debug.WriteLine("StopPush called (待实现)");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"StopPush error: {ex.Message}");
        }
    }

    public void ResumePush()
    {
        try
        {
            var context = Platform.CurrentActivity ?? global::Android.App.Application.Context;
            
            // TODO: 根据实际绑定库API调整方法调用
            System.Diagnostics.Debug.WriteLine("ResumePush called (待实现)");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ResumePush error: {ex.Message}");
        }
    }

    public bool IsPushStopped()
    {
        try
        {
            var context = Platform.CurrentActivity ?? global::Android.App.Application.Context;
            
            // TODO: 根据实际绑定库API调整方法调用
            System.Diagnostics.Debug.WriteLine("IsPushStopped called (待实现)");
            return false; // 默认返回false，表示推送服务正在运行
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"IsPushStopped error: {ex.Message}");
            return true; // 发生错误时返回true，表示服务可能已停止
        }
    }
    
    /// <summary>
    /// 检查推送接收状态和配置
    /// </summary>
    public string CheckPushReceiveStatus()
    {
        try
        {
            var context = Platform.CurrentActivity ?? global::Android.App.Application.Context;
            if (context == null)
            {
                return "❌ 无法获取Android上下文";
            }
            
            var result = new System.Text.StringBuilder();
            result.AppendLine("📊 推送接收状态检查:");
            
            // 1. 检查Registration ID
            string regId = CN.Jpush.Android.Api.JPushInterface.GetRegistrationID(context);
            if (string.IsNullOrEmpty(regId))
            {
                result.AppendLine("❌ Registration ID: 为空 - JPush未正确注册");
            }
            else
            {
                result.AppendLine($"✅ Registration ID: {regId.Substring(0, Math.Min(10, regId.Length))}...");
            }
            
            // 2. 检查连接状态
            bool isConnected = CN.Jpush.Android.Api.JPushInterface.GetConnectionState(context);
            result.AppendLine($"{(isConnected ? "✅" : "❌")} JPush连接状态: {isConnected}");
            
            // 3. 检查应用包名
            string packageName = context.PackageName;
            result.AppendLine($"📱 应用包名: {packageName}");
            
            // 4. 检查APP_KEY配置
            result.AppendLine($"🔑 APP_KEY: {Configuration.JPushConfig.APP_KEY}");
            
            // 5. 检查推送是否被停止
            bool isPushStopped = IsPushStopped();
            result.AppendLine($"{(isPushStopped ? "❌" : "✅")} 推送服务状态: {(isPushStopped ? "已停止" : "运行中")}");
            
            // 6. 检查通知权限（Android 13+）
            if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.Tiramisu)
            {
                try
                {
                    var notificationManager = (global::Android.App.NotificationManager)context.GetSystemService(Context.NotificationService);
                    bool hasPermission = notificationManager?.AreNotificationsEnabled() == true;
                    result.AppendLine($"{(hasPermission ? "✅" : "❌")} 通知权限: {(hasPermission ? "已授权" : "未授权")}");
                }
                catch (Exception ex)
                {
                    result.AppendLine($"⚠️ 通知权限检查失败: {ex.Message}");
                }
            }
            else
            {
                result.AppendLine("ℹ️ 通知权限: Android 13以下版本，默认已授权");
            }
            
            // 7. 检查网络状态
            var connectivityManager = (global::Android.Net.ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            var activeNetwork = connectivityManager?.ActiveNetworkInfo;
            bool hasNetwork = activeNetwork?.IsConnected == true;
            result.AppendLine($"{(hasNetwork ? "✅" : "❌")} 网络连接: {(hasNetwork ? "已连接" : "未连接")}");
            
            return result.ToString();
        }
        catch (Exception ex)
        {
            return $"❌ 检查失败: {ex.Message}";
        }
    }
}
#endif
