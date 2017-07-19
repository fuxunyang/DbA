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
        /// 数据源连接字符串
        /// </summary>
        public string ConnectionString { get; protected set; }
        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; protected set; }

        #region ExecuteNonQuery
        /// <summary>
        /// 执行数据库命令并返回受影响行数
        /// </summary>
        /// <param name="strCommand">命令字符串</param>
        /// <param name="dataParameters">表示执行命令所需要的参数</param>
        /// <returns>受影响行数</returns>
        public virtual int ExecuteNonQuery(string strCommand, params DataParameter[] dataParameters)
        {
            return ExecuteNonQuery(null, CommandType.Text, strCommand, dataParameters);
        }
        /// <summary>
        /// 执行数据库命令并返回受影响行数
        /// </summary>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串/存储过程名</param>
        /// <param name="dataParameters">表示执行命令所需要的参数</param>
        /// <returns>受影响行数</returns>
        public virtual int ExecuteNonQuery(CommandType commandType, string strCommand, params DataParameter[] dataParameters)
        {
            return ExecuteNonQuery(null, commandType, strCommand, dataParameters);
        }
        /// <summary>
        /// 执行数据库命令并返回受影响行数
        /// </summary>
        /// <param name="dbTransaction">表示要在数据源上执行的事物</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串/存储过程名</param>
        /// <param name="dataParameters">表示执行命令所需要的参数</param>
        /// <returns>受影响行数</returns>
        public abstract int ExecuteNonQuery(IDbTransaction dbTransaction, CommandType commandType, string strCommand, params DataParameter[] dataParameters);
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// 执行数据库命令并返回所有结果中第一行的第一列
        /// </summary>
        /// <param name="strCommand">命令字符串</param>
        /// <param name="dataParameters">表示执行命令所需要的参数</param>
        /// <returns>所有结果中第一行的第一列</returns>
        public virtual object ExecuteScalar(string strCommand, params DataParameter[] dataParameters)
        {
            return ExecuteScalar(null, CommandType.Text, strCommand, dataParameters);
        }
        /// <summary>
        /// 执行数据库命令并返回所有结果中第一行的第一列
        /// </summary>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串/存储过程名</param>
        /// <param name="dataParameters">表示执行命令所需要的参数</param>
        /// <returns>所有结果中第一行的第一列</returns>
        public virtual object ExecuteScalar(CommandType commandType, string strCommand, params DataParameter[] dataParameters)
        {
            return ExecuteScalar(null, commandType, strCommand, dataParameters);
        }
        /// <summary>
        /// 执行数据库命令并返回所有结果中第一行的第一列
        /// </summary>
        /// <param name="dbTransaction">表示要在数据源上执行的事物</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串/存储过程名</param>
        /// <param name="dataParameters">表示执行命令所需要的参数</param>
        /// <returns>所有结果中第一行的第一列</returns>
        public abstract object ExecuteScalar(IDbTransaction dbTransaction, CommandType commandType, string strCommand, params DataParameter[] dataParameters);
        #endregion

        #region ExecuteDataTable
        /// <summary>
        /// 执行数据库命令并返回所有结果
        /// </summary>
        /// <param name="strCommand">命令字符串</param>
        /// <param name="dataParameters">表示执行命令所需要的参数</param>
        /// <returns>所有结果</returns>
        public virtual DataTable ExecuteDataTable(string strCommand, params DataParameter[] dataParameters)
        {
            return ExecuteDataTable(null, CommandType.Text, strCommand, dataParameters);
        }
        /// <summary>
        /// 执行数据库命令并返回所有结果
        /// </summary>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串/存储过程名</param>
        /// <param name="dataParameters">表示执行命令所需要的参数</param>
        /// <returns>所有结果</returns>
        public virtual DataTable ExecuteDataTable(CommandType commandType, string strCommand, params DataParameter[] dataParameters)
        {
            return ExecuteDataTable(null, commandType, strCommand, dataParameters);
        }
        /// <summary>
        /// 执行数据库命令并返回所有结果
        /// </summary>
        /// <param name="dbTransaction">表示要在数据源上执行的事物</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串/存储过程名</param>
        /// <param name="dataParameters">表示执行命令所需要的参数</param>
        /// <returns>所有结果</returns>
        public abstract DataTable ExecuteDataTable(IDbTransaction dbTransaction, CommandType commandType, string strCommand, params DataParameter[] dataParameters);
        #endregion

        /// <summary>
        /// 设置要执行的命令
        /// </summary>
        /// <param name="dbCommand">表示连接到数据源时执行的命令</param>
        /// <param name="dbConnection">表示到数据源的已打开连接</param>
        /// <param name="dbTransaction">表示要在数据源上执行的事物</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串/存储过程名</param>
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
                throw new Exception(String.Format("创建数据源操作命令时发生异常；CommandText = {0},异常信息：{1}", strCommand, ex.Message));
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
