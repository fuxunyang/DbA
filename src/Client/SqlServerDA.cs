using System;
using System.Data;
using System.Data.SqlClient;
using DbA.Common;

namespace DbA.Client
{
    /// <summary>
    /// SQL Server数据库访问器
    /// </summary>
    public sealed class SqlServerDA : DbAccessor
    {
        #region Constructors
        /// <summary>
        /// 创建一个SQLServer数据访问器实例
        /// </summary>
        /// <exception cref="System.NullReferenceException">未配置默认的连接字符串</exception>
        /// <remarks>默认的连接字符串为项目配置文件里ConnectionStrings子项，name值为"MSSQLSERVER"</remarks>
        public SqlServerDA()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MSSQLSERVER"].ConnectionString;
        }
        /// <summary>
        /// 以指定连接字符串创建一个SQLServer数据访问器实例
        /// </summary>
        /// <param name="strConnect">连接字符串</param>
        public SqlServerDA(string strConnect)
        {
            ConnectionString = strConnect;
        }
        #endregion

        /// <summary>
        /// 执行数据库命令并返回受影响行数
        /// </summary>
        /// <param name="dbTransaction">表示要在数据源上执行的事物</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串/存储过程名</param>
        /// <param name="dataParameters">表示执行命令所需要的参数</param>
        /// <returns>受影响行数</returns>
        public override int ExecuteNonQuery(IDbTransaction dbTransaction, CommandType commandType, string strCommand, params DataParameter[] dataParameters)
        {
            try
            {
                using (var sqlCommand = new SqlCommand())
                {
                    using (var sqlConnection = new SqlConnection(ConnectionString))
                    {
                        SetCommand(sqlCommand, sqlConnection, dbTransaction, commandType, strCommand, dataParameters);
                        return sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("ExecuteNonQuery时发生异常；CommandText = {0},异常信息：{1}", strCommand, ex.Message));
            }
        }
        /// <summary>
        /// 执行数据库命令并返回所有结果中第一行的第一列
        /// </summary>
        /// <param name="dbTransaction">表示要在数据源上执行的事物</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串/存储过程名</param>
        /// <param name="dataParameters">表示执行命令所需要的参数</param>
        /// <returns>所有结果中第一行的第一列</returns>
        public override object ExecuteScalar(IDbTransaction dbTransaction, CommandType commandType, string strCommand, params DataParameter[] dataParameters)
        {
            try
            {
                using (var sqlCommand = new SqlCommand())
                {
                    using (var sqlConnection = new SqlConnection(ConnectionString))
                    {
                        SetCommand(sqlCommand, sqlConnection, dbTransaction, commandType, strCommand, dataParameters);
                        return sqlCommand.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("ExecuteScalar时发生异常；CommandText = {0},异常信息：{1}", strCommand, ex.Message));
            }
        }
        /// <summary>
        /// 执行数据库命令并返回所有结果
        /// </summary>
        /// <param name="dbTransaction">表示要在数据源上执行的事物</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="strCommand">命令字符串/存储过程名</param>
        /// <param name="dataParameters">表示执行命令所需要的参数</param>
        /// <returns>所有结果</returns>
        public override DataTable ExecuteDataTable(IDbTransaction dbTransaction, CommandType commandType, string strCommand, params DataParameter[] dataParameters)
        {
            try
            {
                using (var sqlCommand = new SqlCommand())
                {
                    using (var sqlConnection = new SqlConnection(ConnectionString))
                    {
                        SetCommand(sqlCommand, sqlConnection, dbTransaction, commandType, strCommand, dataParameters);
                        var data = new DataTable();
                        var adapter = new SqlDataAdapter(sqlCommand);
                        adapter.Fill(data);
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("ExecuteDataTable时发生异常；CommandText = {0},异常信息：{1}", strCommand, ex.Message));
            }
        }

        /// <summary>
        /// 添加执行命令所需要的参数
        /// </summary>
        /// <param name="dbCommand">表示连接到数据源时执行的命令</param>
        /// <param name="dataParameters">需要添加的参数</param>
        protected override void AttachParameters(IDbCommand dbCommand, DataParameter[] dataParameters)
        {
            foreach (var p in dataParameters)
            {
                if (p.Value == null)
                {
                    p.Value = DBNull.Value;
                }

                var sqlParam = new SqlParameter()
                {
                    DbType = p.DbType,
                    ParameterName = p.ParameterName,
                    Direction = p.Direction,
                    IsNullable = p.IsNullable,
                    SourceColumn = p.SourceColumn,
                    SourceVersion = p.SourceVersion,

                    Value = p.Value,
                    Precision = p.Precision,
                    Scale = p.Scale,
                    Size = p.Size
                };
                p.Inner = sqlParam;
                dbCommand.Parameters.Add(sqlParam);
            }
        }
    }
}
