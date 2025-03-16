using LiteDB;

namespace Pek.Maui.LiteDB;

public class LiteDBHelper
{
    /// <summary>
    /// 获取指定路径的数据库
    /// </summary>
    /// <param name="liteDbPath"></param>
    /// <returns></returns>
    public static LiteDatabase GetDatabase(String liteDbPath)
    {
        LiteDatabase? liteDatabaseCurrent = null;

        using (var liteDatabase = new LiteDatabase(liteDbPath))
        {
            liteDatabaseCurrent = liteDatabase;
        }

        return liteDatabaseCurrent;
    }

    /// <summary>
    /// 清空表
    /// </summary>
    /// <param name="liteDbPath"></param>
    /// <param name="liteDbCollectionName"></param>
    /// <returns></returns>
    public static Boolean TruncateCollection(String liteDbCollectionName, String liteDbPath)
    {
        var liteDatabase = GetDatabase(liteDbPath);

        return liteDatabase.DropCollection(liteDbCollectionName);
    }

    /// <summary>
    /// 获取集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="liteDbPath"></param>
    /// <param name="liteDbCollectionName"></param>
    /// <returns></returns>
    public static ILiteCollection<T> GetCollection<T>(String liteDbCollectionName, String liteDbPath)
    {
        ILiteCollection<T>? liteCollection = null;

        using (var liteDatabase = new LiteDatabase(liteDbPath))
        {
            liteCollection = liteDatabase.GetCollection<T>(liteDbCollectionName);
        }

        return liteCollection;
    }

    /// <summary>
    /// 获取全部集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="liteDbPath"></param>
    /// <param name="liteDbCollectionName"></param>
    /// <returns></returns>
    public static IEnumerable<T> FindAll<T>(String liteDbCollectionName, String liteDbPath)
    {
        var liteCollection = GetCollection<T>(liteDbPath, liteDbCollectionName);
        return liteCollection.FindAll();
    }

    /// <summary>
    /// 获取实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="liteDatabasePath"></param>
    /// <param name="liteDbCollectionName"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> Find<T>(String liteDbCollectionName, System.Linq.Expressions.Expression<Func<T, Boolean>> predicate, String liteDatabasePath)
    {
        var liteCollection = GetCollection<T>(liteDatabasePath, liteDbCollectionName);
        return liteCollection.Find(predicate);
    }

    /// <summary>
    /// 获取实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="liteDbPath"></param>
    /// <param name="liteDbCollectionName"></param>
    /// <param name="liteDBQuery"></param>
    /// <returns></returns>
    public static IEnumerable<T> Find<T>(String liteDbCollectionName, Query liteDBQuery, String liteDbPath)
    {
        var liteCollection = GetCollection<T>(liteDbPath, liteDbCollectionName);
        return liteCollection.Find(liteDBQuery);
    }

    /// <summary>
    /// 根据指定Id获取数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="liteDbPath"></param>
    /// <param name="liteDbCollectionName"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static T FindById<T>(String liteDbCollectionName, Int32 id, String liteDbPath)
    {
        var liteCollection = GetCollection<T>(liteDbPath, liteDbCollectionName);
        return liteCollection.FindById(new BsonValue(id));
    }

    /// <summary>
    /// 插入
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="liteDbPath"></param>
    /// <param name="liteDbCollectionName"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static Boolean Insert<T>(String liteDbCollectionName, T item, String liteDbPath)
    {
        var liteCollection = GetCollection<T>(liteDbPath, liteDbCollectionName);
        var bsonValueInsertResult = liteCollection.Insert(item);
        return bsonValueInsertResult != null;
    }

    /// <summary>
    /// 插入
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="liteDbPath"></param>
    /// <param name="liteDbCollectionName"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    public static Boolean Insert<T>(String liteDbCollectionName, IEnumerable<T> items, String liteDbPath)
    {
        var liteCollection = GetCollection<T>(liteDbPath, liteDbCollectionName);
        BsonValue? bsonValueInsertResult = liteCollection.Insert(items);
        return bsonValueInsertResult != null;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="liteDbPath"></param>
    /// <param name="liteDbCollectionName"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static Boolean Update<T>(String liteDbCollectionName, T item, String liteDbPath)
    {
        var liteCollection = GetCollection<T>(liteDbPath, liteDbCollectionName);
        BsonValue? bsonValueUpdateResult = liteCollection.Update(item);
        return bsonValueUpdateResult != null;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="liteDbPath"></param>
    /// <param name="liteDbCollectionName"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Boolean Delete<T>(String liteDbCollectionName, Int32 id, String liteDbPath)
    {
        var liteCollection = GetCollection<T>(liteDbPath, liteDbCollectionName);
        return liteCollection.Delete(id);
    }

    public static Int32 Delete<T>(String liteDbCollectionName, System.Linq.Expressions.Expression<Func<T, Boolean>> predicate, String liteDbPath)
    {
        var liteCollection = GetCollection<T>(liteDbPath, liteDbCollectionName);
        return liteCollection.DeleteMany(predicate);
    }

    public static Boolean Exist<T>(String liteDbCollectionName, System.Linq.Expressions.Expression<Func<T, Boolean>> predicate, String liteDbPath)
    {
        var liteCollection = GetCollection<T>(liteDbPath, liteDbCollectionName);
        return liteCollection.Exists(predicate);
    }

    public static Boolean Exist<T>(String liteDbCollectionName, Query liteDBQuery, String liteDbPath)
    {
        var liteCollection = GetCollection<T>(liteDbPath, liteDbCollectionName);
        return liteCollection.Exists(liteDBQuery);
    }
}
