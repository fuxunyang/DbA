using System.Data;

namespace DbA
{
    /// <summary>
    /// 数据访问器接口
    /// </summary>
    public interface IDbAccessor
    {
        /// <summary>
        /// 数据源连接字符串
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// 执行数据库命令并返回受影响行数
        /// </summary>
        /// <param name="dbTransaction">表示要在数据源上执行的事物</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串/存储过程名</param>
        /// <param name="dataParameters">表示执行命令所需要的参数</param>
        /// <returns>受影响行数</returns>
        int ExecuteNonQuery(IDbTransaction dbTransaction, CommandType commandType, string strCommand, params Common.DataParameter[] dataParameters);
        /// <summary>
        /// 执行数据库命令并返回所有结果中第一行的第一列
        /// </summary>
        /// <param name="dbTransaction">表示要在数据源上执行的事物</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串/存储过程名</param>
        /// <param name="dataParameters">表示执行命令所需要的参数</param>
        /// <returns>所有结果中第一行的第一列</returns>
        object ExecuteScalar(IDbTransaction dbTransaction, CommandType commandType, string strCommand, params Common.DataParameter[] dataParameters);
        /// <summary>
        /// 执行数据库命令并返回所有结果
        /// </summary>
        /// <param name="dbTransaction">表示要在数据源上执行的事物</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串/存储过程名</param>
        /// <param name="dataParameters">表示执行命令所需要的参数</param>
        /// <returns>所有结果</returns>
        DataTable ExecuteDataTable(IDbTransaction dbTransaction, CommandType commandType, string strCommand, params Common.DataParameter[] dataParameters);
    }
}
