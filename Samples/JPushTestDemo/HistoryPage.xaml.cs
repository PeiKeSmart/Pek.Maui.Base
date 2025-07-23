using JPushTestDemo.Services;

namespace JPushTestDemo;

public partial class HistoryPage : ContentPage
{
    private readonly IJPushService _jPushService;
    private int _currentMaxLines = 500;

    public HistoryPage(IJPushService jPushService)
    {
        InitializeComponent();
        _jPushService = jPushService;
        
        // 设置默认选择
        MaxLinesPicker.SelectedIndex = 2; // 500条
        
        // 加载历史记录
        LoadHistory();
    }

    private void LoadHistory()
    {
        try
        {
            StatusLabel.Text = "正在加载历史记录...";
            
            // 获取历史记录
            string history = _jPushService.GetBroadcastHistory(_currentMaxLines);
            string fileInfo = _jPushService.GetHistoryFileInfo();
            
            // 更新显示
            HistoryLabel.Text = history;
            StatsLabel.Text = fileInfo;
            string displayText = _currentMaxLines == int.MaxValue ? "全部" : _currentMaxLines.ToString();
            StatusLabel.Text = $"✅ 加载完成 - 显示最近 {displayText} 条记录";
        }
        catch (Exception ex)
        {
            HistoryLabel.Text = $"加载失败: {ex.Message}";
            StatusLabel.Text = "加载失败";
        }
    }

    private void OnRefreshClicked(object? sender, EventArgs e)
    {
        LoadHistory();
    }

    private async void OnExportClicked(object? sender, EventArgs e)
    {
        try
        {
            StatusLabel.Text = "正在导出...";
            
            // 获取完整历史记录
            string fullHistory = _jPushService.GetBroadcastHistory(int.MaxValue);
            
            // 在实际应用中，这里可以保存到文件或分享
            await DisplayAlert("导出功能", 
                "历史记录已准备就绪。\n\n" +
                "在实际应用中，可以将记录保存到文件或通过邮件分享。\n\n" +
                $"记录长度: {fullHistory.Length} 字符", 
                "确定");
            
            StatusLabel.Text = "导出完成";
        }
        catch (Exception ex)
        {
            await DisplayAlert("导出失败", ex.Message, "确定");
            StatusLabel.Text = "导出失败";
        }
    }

    private async void OnClearClicked(object? sender, EventArgs e)
    {
        try
        {
            bool confirm = await DisplayAlert("确认清空", 
                "确定要清空所有广播历史记录吗？\n\n此操作不可撤销！", 
                "确定", "取消");
            
            if (confirm)
            {
                _jPushService.ClearBroadcastHistory();
                LoadHistory();
                await DisplayAlert("操作完成", "历史记录已清空", "确定");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("清空失败", ex.Message, "确定");
        }
    }

    private void OnMaxLinesChanged(object? sender, EventArgs e)
    {
        if (MaxLinesPicker.SelectedIndex >= 0)
        {
            string selected = MaxLinesPicker.Items[MaxLinesPicker.SelectedIndex];
            
            if (selected == "全部")
            {
                _currentMaxLines = int.MaxValue;
            }
            else if (int.TryParse(selected, out int maxLines))
            {
                _currentMaxLines = maxLines;
            }
            
            LoadHistory();
        }
    }

    private async void OnFilterClicked(object? sender, EventArgs e)
    {
        try
        {
            // 简单的筛选功能 - 可以根据需要扩展
            string keyword = await DisplayPromptAsync("筛选记录", 
                "输入关键词筛选记录:", 
                "确定", "取消", 
                "例如: ERROR, EVENT, JPush");
            
            if (!string.IsNullOrEmpty(keyword))
            {
                StatusLabel.Text = $"正在筛选包含 '{keyword}' 的记录...";
                
                string fullHistory = _jPushService.GetBroadcastHistory(int.MaxValue);
                var lines = fullHistory.Split('\n');
                var filteredLines = lines.Where(line => 
                    line.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToArray();
                
                if (filteredLines.Length > 0)
                {
                    string filteredHistory = $"=== 筛选结果 ===\n关键词: {keyword}\n找到 {filteredLines.Length} 条匹配记录\n\n" +
                                           string.Join("\n", filteredLines);
                    HistoryLabel.Text = filteredHistory;
                    StatusLabel.Text = $"筛选完成 - 找到 {filteredLines.Length} 条记录";
                }
                else
                {
                    HistoryLabel.Text = $"=== 筛选结果 ===\n关键词: {keyword}\n未找到匹配的记录";
                    StatusLabel.Text = "未找到匹配记录";
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("筛选失败", ex.Message, "确定");
            StatusLabel.Text = "筛选失败";
        }
    }
}