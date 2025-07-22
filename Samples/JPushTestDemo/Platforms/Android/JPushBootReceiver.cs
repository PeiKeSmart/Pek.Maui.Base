using Android.App;
using Android.Content;
using Android.Util;

namespace JPushTestDemo.Platforms.Android
{
    /// <summary>
    /// 系统启动接收器，确保JPush在系统重启后能够正常工作
    /// </summary>
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { 
        Intent.ActionBootCompleted,
        Intent.ActionMyPackageReplaced,
        "android.intent.action.QUICKBOOT_POWERON",
        "com.htc.intent.action.QUICKBOOT_POWERON"
    }, Priority = 1000)]
    public class JPushBootReceiver : BroadcastReceiver
    {
        private const string TAG = "JPushBootReceiver";

        public override void OnReceive(Context? context, Intent? intent)
        {
            if (context == null || intent == null)
                return;

            try
            {
                string action = intent.Action ?? "";
                Log.Info(TAG, $"收到系统广播: {action}");

                switch (action)
                {
                    case Intent.ActionBootCompleted:
                    case "android.intent.action.QUICKBOOT_POWERON":
                    case "com.htc.intent.action.QUICKBOOT_POWERON":
                        Log.Info(TAG, "系统启动完成，重新初始化JPush");
                        InitializeJPushAfterBoot(context);
                        break;
                    case Intent.ActionMyPackageReplaced:
                        Log.Info(TAG, "应用更新完成，重新初始化JPush");
                        InitializeJPushAfterBoot(context);
                        break;
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"处理系统广播时发生错误: {ex.Message}", ex);
            }
        }

        private void InitializeJPushAfterBoot(Context context)
        {
            try
            {
                // 设置调试模式
                CN.Jpush.Android.Api.JPushInterface.SetDebugMode(true);
                
                // 重新初始化JPush
                CN.Jpush.Android.Api.JPushInterface.Init(context);
                
                // 设置保活配置
                CN.Jpush.Android.Api.JPushInterface.SetLatestNotificationNumber(context, 5);
                
                Log.Info(TAG, "JPush重新初始化成功");
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"JPush重新初始化失败: {ex.Message}", ex);
            }
        }
    }
}