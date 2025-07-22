using Android.Content;
using Android.Util;
using CN.Jpush.Android.Api;
using CN.Jpush.Android.Service;
using Org.Json;

namespace JPushTestDemo.Platforms.Android.Services
{
    /// <summary>
    /// JPush推送消息接收器
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
                Log.Info(TAG, $"收到通知 - 标题: {message?.NotificationTitle}, 内容: {message?.NotificationContent}");
                
                // 调用父类方法以保持兼容性
                base.OnNotifyMessageArrived(context, message);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"处理通知接收事件时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 点击通知回调
        /// </summary>
        public override void OnNotifyMessageOpened(Context context, CN.Jpush.Android.Api.NotificationMessage message)
        {
            try
            {
                Log.Info(TAG, $"用户点击通知 - 标题: {message?.NotificationTitle}, 内容: {message?.NotificationContent}");
                
                // 处理通知点击逻辑
                // 例如：跳转到特定页面
                
                // 调用父类方法以保持兼容性
                base.OnNotifyMessageOpened(context, message);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"处理通知点击事件时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 自定义消息回调
        /// </summary>
        public override void OnMessage(Context context, CN.Jpush.Android.Api.CustomMessage customMessage)
        {
            try
            {
                Log.Info(TAG, $"收到自定义消息 - 标题: {customMessage?.Title}, 消息: {customMessage?.Message}");
                
                // 处理自定义消息的逻辑
                // 自定义消息不会显示通知栏，需要开发者自己处理
                
                // 调用父类方法以保持兼容性
                base.OnMessage(context, customMessage);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"处理自定义消息时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 注册成功回调
        /// </summary>
        public override void OnRegister(Context context, string registrationId)
        {
            try
            {
                Log.Info(TAG, $"Registration ID: {registrationId}");
                
                // 调用父类方法以保持兼容性
                base.OnRegister(context, registrationId);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"处理Registration ID时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 标签操作回调
        /// </summary>
        public override void OnTagOperatorResult(Context context, CN.Jpush.Android.Api.JPushMessage jPushMessage)
        {
            try
            {
                Log.Info(TAG, $"标签操作结果 - 错误码: {jPushMessage?.ErrorCode}, 序列号: {jPushMessage?.Sequence}");
                
                // 调用父类方法以保持兼容性
                base.OnTagOperatorResult(context, jPushMessage);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"处理标签操作结果时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 别名操作回调
        /// </summary>
        public override void OnAliasOperatorResult(Context context, CN.Jpush.Android.Api.JPushMessage jPushMessage)
        {
            try
            {
                Log.Info(TAG, $"别名操作结果 - 错误码: {jPushMessage?.ErrorCode}, 序列号: {jPushMessage?.Sequence}");
                
                // 调用父类方法以保持兼容性
                base.OnAliasOperatorResult(context, jPushMessage);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"处理别名操作结果时发生错误: {ex.Message}", ex);
            }
        }
    }
}
