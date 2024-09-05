namespace Pek;

/// <summary>IO工具类</summary>
public static class IOHelper
{
    #region 复制数据流
    /// <summary>复制数组</summary>
    /// <param name="src">源数组</param>
    /// <param name="offset">起始位置。一般从0开始</param>
    /// <param name="count">复制字节数。用-1表示截取剩余所有数据</param>
    /// <returns>返回复制的总字节数</returns>
    public static Byte[] ReadBytes(this Byte[] src, Int32 offset, Int32 count)
    {
        if (count == 0) return [];

        // 即使是全部，也要复制一份，而不只是返回原数组，因为可能就是为了复制数组
        if (count < 0) count = src.Length - offset;

        var bts = new Byte[count];
        Buffer.BlockCopy(src, offset, bts, 0, bts.Length);
        return bts;
    }
    #endregion
}
