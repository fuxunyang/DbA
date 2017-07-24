using System;
using System.Data;

namespace DbA.Common
{
    /// <summary>
    /// 数据访问器基类
    /// </summary>
    public abstract class DbAccessor : IDbAccessor
    {
        /// <summary>
        /// 默认设置
        /// </summary>
        public DbAccessor()
        {
            Timeout = 30;
        }

        /// <summary>
        /// 数据源连接字符串
        /// </summary>
        public string ConnectionString { get; protected set; }
        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; protected set; }

        /// <summary>
        /// 执行数据库命令并返回受影响行数
        /// </summary>
        /// <param name="dbTransaction">表示要在数据源上执行的事物</param>
        /// <param name="cmdType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串</param>
        /// <param name="dbParams">表示执行命令所需要的参数</param>
        /// <returns>受影响行数</returns>
        public abstract int ExecuteNonQuery(IDbTransaction dbTransaction, CommandType cmdType, string strCommand, params DataParameter[] dbParams);
        /// <summary>
        /// 执行数据库命令并返回所有结果中第一行的第一列
        /// </summary>
        /// <param name="dbTransaction">表示要在数据源上执行的事物</param>
        /// <param name="cmdType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串</param>
        /// <param name="dbParams">表示执行命令所需要的参数</param>
        /// <returns>所有结果中第一行的第一列</returns>
        public abstract object ExecuteScalar(IDbTransaction dbTransaction, CommandType cmdType, string strCommand, params DataParameter[] dbParams);
        /// <summary>
        /// 执行数据库命令并返回所有结果
        /// </summary>
        /// <param name="dbTransaction">表示要在数据源上执行的事物</param>
        /// <param name="cmdType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串</param>
        /// <param name="dbParams">表示执行命令所需要的参数</param>
        /// <returns>所有结果</returns>
        public abstract DataTable ExecuteDataTable(IDbTransaction dbTransaction, CommandType cmdType, string strCommand, params DataParameter[] dbParams);
        /// <summary>
        /// 设置要执行的命令
        /// </summary>
        /// <param name="dbCommand">表示连接到数据源时执行的命令</param>
        /// <param name="dbConnection">表示到数据源的已打开连接</param>
        /// <param name="dbTransaction">表示要在数据源上执行的事物</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串</param>
        /// <param name="dataParameters">表示执行命令所需要的参数</param>
        protected void SetCommand(IDbCommand dbCommand, IDbConnection dbConnection, IDbTransaction dbTransaction, CommandType commandType, string strCommand, DataParameter[] dataParameters)
        {
            try
            {
                if (!dbConnection.State.Equals(ConnectionState.Open))
                {
                    dbConnection.Open();
                }
                dbCommand.Connection = dbConnection;
                dbCommand.CommandType = commandType;
                dbCommand.CommandText = strCommand;
                dbCommand.CommandTimeout = Timeout;
                if (dbTransaction != null)
                {
                    dbCommand.Transaction = dbTransaction;
                }
                if (dataParameters != null)
                {
                    AttachParameters(dbCommand, dataParameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("创建数据源操作命令时发生异常；CommandText = {0},异常信息：{1}", strCommand, ex.Message), ex);
            }
        }
        /// <summary>
        /// 添加执行命令所需要的参数
        /// </summary>
        /// <param name="dbCommand">表示连接到数据源时执行的命令</param>
        /// <param name="dataParameters">需要添加的参数</param>
        protected abstract void AttachParameters(IDbCommand dbCommand, DataParameter[] dataParameters);
    }
}
