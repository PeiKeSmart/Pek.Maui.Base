using static Microsoft.Maui.ApplicationModel.Permissions;

namespace Pek;

/// <summary>
/// 权限动态请求
/// </summary>
public class PermissionUtil
{
    private PermissionUtil() { }

    static PermissionUtil? permissionUtil = null;

    public static PermissionUtil Instance
    {
        get
        {
            if (permissionUtil == null)
            {
                permissionUtil = new PermissionUtil();
            }
            return permissionUtil;
        }
    }

    public async Task<PermissionStatus> CheckPermissionAndRequestPermissionAsync<T>(T permissionReq)
             where T : BasePermission
    {
        var status = await permissionReq.CheckStatusAsync();

        if (status != PermissionStatus.Granted)
        {
            status = await permissionReq.RequestAsync();
        }

        return status;
    }
}