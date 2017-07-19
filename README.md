# DbA
> 创建一个用来访问指定数据源的对象，如果通过工厂创建则保持一个单例

## 使用方法
**首先创建一个数据源的访问器**
```C#
// 方法一：直接创建访问器
// 在DbA.Clinet目录下已经有一个实现的SQLServer的访问器，这里用来做例子
var da = new DbA.Clinet.SqlServerDA();

// 方法二：使用单例模式创建访问器
var da = DbA.Common.DAFactory.GetInstance<DbA.Clinet.SqlServerDA>();// 使用默认的连接字符串
// 默认的连接字符串需要在配置文件的connectionStrings标签里 add一个name=MSSQLSERVER 的标签
// var da = DbA.Common.DAFactory.GetInstance<DbA.Clinet.SqlServerDA>("指定连接字符串");
// 单例工厂创建的实例通过连接字符串保持一个唯一实例，相同的连接字符串会从缓存返回实例
```
**通过访问器执行SQL语句**
```C#
da.ExecuteNonQuery("SELECT 1");// 不要参数的SQL
da.ExecuteScalar("SELECT COUNT(0) FROM Person WHERE Age > @age",
    DataParameter.Create("@age", 18));// 使用一个参数的SQL
da.ExecuteDataTable(CommandType.StoredProcedure, "UpdatePersonAge",
    DataParameter.Create("@name", "cx"),
    DataParameter.Create("@age", 20));// 使用存储过程名字和多个参数的SQL
```
