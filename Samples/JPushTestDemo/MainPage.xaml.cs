using JPushTestDemo.Services;

namespace JPushTestDemo;

public partial class MainPage : ContentPage
{
	private readonly IJPushService _jPushService;

	public MainPage(IJPushService jPushService)
	{
		InitializeComponent();
		_jPushService = jPushService;
		UpdateStatus("应用已启动，等待JPush操作...");
	}

	private void OnGetRegistrationIdClicked(object? sender, EventArgs e)
	{
		try
		{
			var registrationId = _jPushService.GetRegistrationId();
			RegistrationIdLabel.Text = $"Registration ID: {registrationId}";
			UpdateStatus($"Registration ID: {registrationId}");
		}
		catch (Exception ex)
		{
			UpdateStatus($"获取Registration ID失败: {ex.Message}");
		}
	}

	private void OnSetAliasTagsClicked(object? sender, EventArgs e)
	{
		try
		{
			var alias = AliasEntry.Text?.Trim();
			var tagsText = TagsEntry.Text?.Trim();
			
			if (string.IsNullOrEmpty(alias) && string.IsNullOrEmpty(tagsText))
			{
				UpdateStatus("请输入别名或标签");
				return;
			}

			// Set alias
			if (!string.IsNullOrEmpty(alias))
			{
				_jPushService.SetAlias(alias);
			}
			
			// Set tags
			if (!string.IsNullOrEmpty(tagsText))
			{
				var tags = tagsText.Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ToHashSet();
				_jPushService.SetTags(tags);
			}
			
			UpdateStatus($"设置别名: {alias ?? "未设置"}, 标签: {tagsText ?? "未设置"}");
		}
		catch (Exception ex)
		{
			UpdateStatus($"设置别名和标签失败: {ex.Message}");
		}
	}

	private void OnTestNotificationClicked(object? sender, EventArgs e)
	{
		try
		{
			// Check if push service is running
			var isPushStopped = _jPushService.IsPushStopped();
			var status = isPushStopped ? "已停止" : "运行中";
			
			UpdateStatus($"推送服务状态: {status}");
			
			// Show test notification dialog
			DisplayAlert("测试通知", 
				$"推送服务状态: {status}\n\n" +
				"要发送真实的推送通知，需要：\n" +
				"1. 配置JPush AppKey\n" +
				"2. 通过JPush控制台或API发送推送\n" +
				"3. 确保应用已获取到有效的Registration ID", 
				"确定");
		}
		catch (Exception ex)
		{
			UpdateStatus($"测试通知失败: {ex.Message}");
		}
	}

	private void OnCheckPushStatusClicked(object? sender, EventArgs e)
	{
		try
		{
			var statusReport = _jPushService.CheckPushReceiveStatus();
			UpdateStatus("推送状态检查完成，查看详细信息");
			
			// Show detailed status in alert
			DisplayAlert("📊 推送接收状态报告", statusReport, "确定");
		}
		catch (Exception ex)
		{
			UpdateStatus($"检查推送状态失败: {ex.Message}");
		}
	}

	private void UpdateStatus(string message)
	{
		StatusLabel.Text = $"状态: {message}";
		System.Diagnostics.Debug.WriteLine($"[JPush Test] {message}");
	}
}
