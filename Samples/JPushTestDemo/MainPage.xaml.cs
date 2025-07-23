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

	private async void OnViewHistoryClicked(object? sender, EventArgs e)
	{
		try
		{
			// 让用户选择查看的条数
			string action = await DisplayActionSheet("选择查看条数", "取消", null, 
				"最近 100 条", "最近 200 条", "最近 500 条", "最近 1000 条", "全部记录");
			
			if (action == "取消" || string.IsNullOrEmpty(action))
				return;
			
			int maxLines = action switch
			{
				"最近 100 条" => 100,
				"最近 200 条" => 200,
				"最近 500 条" => 500,
				"最近 1000 条" => 1000,
				"全部记录" => int.MaxValue,
				_ => 200
			};
			
			UpdateStatus($"正在获取{action}...");
			
			// 获取历史记录
			var history = _jPushService.GetBroadcastHistory(maxLines);
			var fileInfo = _jPushService.GetHistoryFileInfo();
			
			UpdateStatus("历史记录获取完成");
			HistoryInfoLabel.Text = $"文件信息: {fileInfo}";
			
			// 显示历史记录
			await DisplayAlert($"📋 广播历史记录 ({action})", history, "确定");
		}
		catch (Exception ex)
		{
			UpdateStatus($"获取历史记录失败: {ex.Message}");
			HistoryInfoLabel.Text = "获取文件信息失败";
		}
	}

	private async void OnDetailHistoryClicked(object? sender, EventArgs e)
	{
		try
		{
			UpdateStatus("正在打开详细历史记录页面...");
			
			// 导航到详细历史记录页面
			var historyPage = new HistoryPage(_jPushService);
			await Navigation.PushAsync(historyPage);
		}
		catch (Exception ex)
		{
			UpdateStatus($"打开详细页面失败: {ex.Message}");
			await DisplayAlert("错误", $"无法打开详细历史记录页面: {ex.Message}", "确定");
		}
	}

	private async void OnQuickViewRecentClicked(object? sender, EventArgs e)
	{
		try
		{
			UpdateStatus("正在获取最近100条记录...");
			
			// 直接获取最近100条记录
			var history = _jPushService.GetBroadcastHistory(100);
			var fileInfo = _jPushService.GetHistoryFileInfo();
			
			UpdateStatus("最近记录获取完成");
			HistoryInfoLabel.Text = $"文件信息: {fileInfo}";
			
			// 显示历史记录
			await DisplayAlert("⚡ 最近100条广播记录", history, "确定");
		}
		catch (Exception ex)
		{
			UpdateStatus($"获取最近记录失败: {ex.Message}");
			HistoryInfoLabel.Text = "获取记录失败";
		}
	}

	private async void OnClearHistoryClicked(object? sender, EventArgs e)
	{
		try
		{
			// 确认清空操作
			bool confirm = await DisplayAlert("⚠️ 确认清空", 
				"确定要清空所有广播历史记录吗？\n\n此操作不可撤销！\n清空后将无法恢复已记录的推送和广播信息。", 
				"确定清空", "取消");
			
			if (confirm)
			{
				_jPushService.ClearBroadcastHistory();
				UpdateStatus("历史记录已清空");
				HistoryInfoLabel.Text = "历史记录已清空，新的推送将重新开始记录";
				
				await DisplayAlert("✅ 操作完成", "广播历史记录已成功清空", "确定");
			}
		}
		catch (Exception ex)
		{
			UpdateStatus($"清空历史记录失败: {ex.Message}");
			await DisplayAlert("❌ 清空失败", $"清空历史记录时发生错误：{ex.Message}", "确定");
		}
	}

	private void UpdateStatus(string message)
	{
		StatusLabel.Text = $"状态: {message}";
		System.Diagnostics.Debug.WriteLine($"[JPush Test] {message}");
	}
}
