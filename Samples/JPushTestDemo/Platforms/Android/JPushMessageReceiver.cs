using Android.Content;
using Android.Util;
using CN.Jpush.Android.Api;
using CN.Jpush.Android.Service;
using Org.Json;

namespace JPushTestDemo.Platforms.Android
{
    /// <summary>
    /// JPush推送消息接收器
    /// 完全按照官方Demo配置 - 类名和位置都模仿官方
    /// 继承cn.jpush.android.service.JPushMessageReceiver来处理JPush的各种推送事件
    /// </summary>
    public class JPushMessageReceiver : CN.Jpush.Android.Service.JPushMessageReceiver
    {
        private const string TAG = "JPushMessageReceiver";

        /// <summary>
        /// 收到通知回调
        /// </summary>
        public override void OnNotifyMessageArrived(Context context, CN.Jpush.Android.Api.NotificationMessage message)
        {
            try
            {
                Log.Info(TAG, $"[OnNotifyMessageArrived] 收到通知");
                if (message != null)
                {
                    Log.Info(TAG, $"[OnNotifyMessageArrived] 标题: {message.NotificationTitle}");
                    Log.Info(TAG, $"[OnNotifyMessageArrived] 内容: {message.NotificationContent}");
                    Log.Info(TAG, $"[OnNotifyMessageArrived] 附加数据: {message.NotificationExtras}");
                }
                
                // 调用父类方法以保持兼容性
                base.OnNotifyMessageArrived(context, message);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"[OnNotifyMessageArrived] 处理通知接收事件时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 点击通知回调
        /// </summary>
        public override void OnNotifyMessageOpened(Context context, CN.Jpush.Android.Api.NotificationMessage message)
        {
            try
            {
                Log.Info(TAG, $"[OnNotifyMessageOpened] 用户点击了通知");
                if (message != null)
                {
                    Log.Info(TAG, $"[OnNotifyMessageOpened] 标题: {message.NotificationTitle}");
                    Log.Info(TAG, $"[OnNotifyMessageOpened] 内容: {message.NotificationContent}");
                    Log.Info(TAG, $"[OnNotifyMessageOpened] 附加数据: {message.NotificationExtras}");
                }
                
                // 调用父类方法以保持兼容性
                base.OnNotifyMessageOpened(context, message);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"[OnNotifyMessageOpened] 处理通知点击事件时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 自定义消息回调
        /// </summary>
        public override void OnMessage(Context context, CN.Jpush.Android.Api.CustomMessage customMessage)
        {
            try
            {
                Log.Info(TAG, $"[OnMessage] 收到自定义消息");
                if (customMessage != null)
                {
                    Log.Info(TAG, $"[OnMessage] 标题: {customMessage.Title}");
                    Log.Info(TAG, $"[OnMessage] 内容: {customMessage.Message}");
                    Log.Info(TAG, $"[OnMessage] 附加数据: {customMessage.Extra}");
                }
                
                // 调用父类方法以保持兼容性
                base.OnMessage(context, customMessage);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"[OnMessage] 处理自定义消息时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 注册成功回调
        /// </summary>
        public override void OnRegister(Context context, string registrationId)
        {
            try
            {
                Log.Info(TAG, $"[OnRegister] JPush注册成功，Registration ID: {registrationId}");
                
                // 调用父类方法以保持兼容性
                base.OnRegister(context, registrationId);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"[OnRegister] 处理注册成功回调时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 标签操作回调
        /// </summary>
        public override void OnTagOperatorResult(Context context, CN.Jpush.Android.Api.JPushMessage jPushMessage)
        {
            try
            {
                Log.Info(TAG, $"[OnTagOperatorResult] 标签操作结果");
                if (jPushMessage != null)
                {
                    Log.Info(TAG, $"[OnTagOperatorResult] 错误码: {jPushMessage.ErrorCode}");
                    Log.Info(TAG, $"[OnTagOperatorResult] 序列号: {jPushMessage.Sequence}");
                }
                
                // 调用父类方法以保持兼容性
                base.OnTagOperatorResult(context, jPushMessage);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"[OnTagOperatorResult] 处理标签操作结果时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 别名操作回调
        /// </summary>
        public override void OnAliasOperatorResult(Context context, CN.Jpush.Android.Api.JPushMessage jPushMessage)
        {
            try
            {
                Log.Info(TAG, $"[OnAliasOperatorResult] 别名操作结果");
                if (jPushMessage != null)
                {
                    Log.Info(TAG, $"[OnAliasOperatorResult] 错误码: {jPushMessage.ErrorCode}");
                    Log.Info(TAG, $"[OnAliasOperatorResult] 序列号: {jPushMessage.Sequence}");
                }
                
                // 调用父类方法以保持兼容性
                base.OnAliasOperatorResult(context, jPushMessage);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"[OnAliasOperatorResult] 处理别名操作结果时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 长连接状态回调
        /// </summary>
        public override void OnConnected(Context context, bool isConnected)
        {
            try
            {
                Log.Info(TAG, $"[OnConnected] JPush连接状态变化: {(isConnected ? "已连接" : "已断开")}");
                
                // 调用父类方法以保持兼容性
                base.OnConnected(context, isConnected);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"[OnConnected] 处理连接状态变化时发生错误: {ex.Message}", ex);
            }
        }
    }
}
