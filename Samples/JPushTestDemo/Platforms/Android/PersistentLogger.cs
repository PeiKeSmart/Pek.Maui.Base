using Android.Content;
using Android.Util;
using System;
using System.IO;
using System.Text;

namespace JPushTestDemo.Platforms.Android
{
    /// <summary>
    /// 持久化日志记录器
    /// 将日志写入文件，应用重启后仍可查看
    /// </summary>
    public static class PersistentLogger
    {
        private const string TAG = "PersistentLogger";
        private const string LOG_FILE_NAME = "broadcast_diagnostics.log";
        private static readonly object _lockObject = new object();

        /// <summary>
        /// 记录诊断日志
        /// </summary>
        public static void LogDiagnostic(Context context, string tag, string message)
        {
            try
            {
                lock (_lockObject)
                {
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string logEntry = $"[{timestamp}] [{tag}] {message}\n";
                    
                    // 同时输出到Logcat和文件
                    Log.Info(tag, message);
                    WriteToFile(context, logEntry);
                }
            }
            catch (Exception ex)
            {
                Log.Error(TAG, $"记录诊断日志失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        public static void LogError(Context context, string tag, string message, Exception? ex = null)
        {
            try
            {
                lock (_lockObject)
                {
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string logEntry = $"[{timestamp}] [ERROR] [{tag}] {message}";
                    
                    if (ex != null)
                    {
                        logEntry += $"\n  Exception: {ex.Message}\n  StackTrace: {ex.StackTrace}";
                    }
                    logEntry += "\n";
                    
                    // 同时输出到Logcat和文件
                    if (ex != null)
                    {
                        Log.Error(tag, message, ex);
                    }
                    else
                    {
                        Log.Error(tag, message);
                    }
                    
                    WriteToFile(context, logEntry);
                }
            }
            catch (Exception logEx)
            {
                Log.Error(TAG, $"记录错误日志失败: {logEx.Message}", logEx);
            }
        }

        /// <summary>
        /// 记录重要事件
        /// </summary>
        public static void LogEvent(Context context, string eventName, string details = "")
        {
            string message = string.IsNullOrEmpty(details) ? 
                $"🎯 事件: {eventName}" : 
                $"🎯 事件: {eventName} - {details}";
            
            LogDiagnostic(context, "EVENT", message);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        private static void WriteToFile(Context context, string content)
        {
            try
            {
                var filesDir = context.FilesDir;
                var logFile = new Java.IO.File(filesDir, LOG_FILE_NAME);
                
                using (var fileWriter = new Java.IO.FileWriter(logFile, true))
                using (var bufferedWriter = new Java.IO.BufferedWriter(fileWriter))
                {
                    bufferedWriter.Write(content);
                    bufferedWriter.Flush();
                }
            }
            catch (Exception ex)
            {
                Log.Error(TAG, $"写入日志文件失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 读取所有日志
        /// </summary>
        public static string ReadAllLogs(Context context)
        {
            try
            {
                var filesDir = context.FilesDir;
                var logFile = new Java.IO.File(filesDir, LOG_FILE_NAME);
                
                if (!logFile.Exists())
                {
                    return "暂无日志记录";
                }

                var stringBuilder = new StringBuilder();
                using (var fileReader = new Java.IO.FileReader(logFile))
                using (var bufferedReader = new Java.IO.BufferedReader(fileReader))
                {
                    string line;
                    while ((line = bufferedReader.ReadLine()) != null)
                    {
                        stringBuilder.AppendLine(line);
                    }
                }

                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                Log.Error(TAG, $"读取日志文件失败: {ex.Message}", ex);
                return $"读取日志失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 读取最近的日志（指定行数）
        /// </summary>
        public static string ReadRecentLogs(Context context, int maxLines = 100)
        {
            try
            {
                string allLogs = ReadAllLogs(context);
                if (string.IsNullOrEmpty(allLogs) || allLogs == "暂无日志记录")
                {
                    return allLogs;
                }

                var lines = allLogs.Split('\n');
                if (lines.Length <= maxLines)
                {
                    return allLogs;
                }

                // 返回最后的maxLines行
                var recentLines = new string[maxLines];
                Array.Copy(lines, lines.Length - maxLines, recentLines, 0, maxLines);
                
                return string.Join("\n", recentLines);
            }
            catch (Exception ex)
            {
                Log.Error(TAG, $"读取最近日志失败: {ex.Message}", ex);
                return $"读取最近日志失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 清空日志文件
        /// </summary>
        public static void ClearLogs(Context context)
        {
            try
            {
                var filesDir = context.FilesDir;
                var logFile = new Java.IO.File(filesDir, LOG_FILE_NAME);
                
                if (logFile.Exists())
                {
                    logFile.Delete();
                    LogDiagnostic(context, TAG, "日志文件已清空");
                }
            }
            catch (Exception ex)
            {
                Log.Error(TAG, $"清空日志文件失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取日志文件大小
        /// </summary>
        public static string GetLogFileInfo(Context context)
        {
            try
            {
                var filesDir = context.FilesDir;
                var logFile = new Java.IO.File(filesDir, LOG_FILE_NAME);
                
                if (!logFile.Exists())
                {
                    return "日志文件不存在";
                }

                long sizeBytes = logFile.Length();
                string sizeStr = sizeBytes < 1024 ? 
                    $"{sizeBytes} B" : 
                    $"{sizeBytes / 1024.0:F1} KB";

                var lastModified = new DateTime(1970, 1, 1).AddMilliseconds(logFile.LastModified());
                
                return $"文件大小: {sizeStr}, 最后修改: {lastModified:yyyy-MM-dd HH:mm:ss}";
            }
            catch (Exception ex)
            {
                return $"获取日志文件信息失败: {ex.Message}";
            }
        }
    }
}