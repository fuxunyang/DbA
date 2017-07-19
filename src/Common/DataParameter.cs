using System;
using System.Data;

namespace DbA.Common
{
    /// <summary>
    /// Command对象的数据参数
    /// </summary>
    public sealed class DataParameter : IDataParameter, IDbDataParameter
    {
        /// <summary>
        /// 表示空的参数
        /// </summary>
        public static readonly DataParameter Null = new DataParameter()
        {
            DbType = DbType.Object,
            Direction = System.Data.ParameterDirection.Input,
            ParameterName = String.Empty,
            SourceVersion = DataRowVersion.Default,
            Value = DBNull.Value
        };

        #region 公共属性
        /// <summary>
        /// 获取或设置数据参数的类型
        /// </summary>
        public DbType DbType { get; set; }
        /// <summary>
        /// 获取或设置一个值，该值指示参数是只可输入、只可输出、双向还是存储过程返回值参数。
        /// </summary>
        public ParameterDirection Direction { get; set; }
        /// <summary>
        /// 获取或设置一个值，该值指示参数是否接受空值。
        /// </summary>
        public bool IsNullable { get; set; }
        /// <summary>
        /// 获取或设置数据参数的名称。
        /// </summary>
        public string ParameterName { get; set; }
        /// <summary>
        /// 获取或设置源列的名称。
        /// </summary>
        public string SourceColumn { get; set; }
        /// <summary>
        /// SourceVersion
        /// </summary>
        public DataRowVersion SourceVersion { get; set; }
        /// <summary>
        /// 获取或设置该参数的值。
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 实际使用的数据参数
        /// </summary>
        internal IDataParameter Inner { get; set; }

        /// <summary>
        /// 获取或设置用来表示 数据参数 属性的最大位数。
        /// </summary>
        public byte Precision { get; set; }
        /// <summary>
        /// 获取或设置 数据参数 解析为的小数位数。
        /// </summary>
        public byte Scale { get; set; }
        /// <summary>
        /// 列中数据的最大大小（以字节为单位）。 从参数值推断默认值。
        /// </summary>
        public int Size { get; set; }
        #endregion

        /// <summary>
        /// 获取Output类型参数的值
        /// </summary>
        /// <returns></returns>
        public object GetOutput()
        {
            return Inner == null ? null : Inner.Value;
        }

        /// <summary>
        /// 创建一个输入类型的参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>输入类型的数据参数</returns>
        public static DataParameter Create(string parameterName, object value)
        {
            return Create(parameterName, value, ParameterDirection.Input);
        }
        /// <summary>
        /// 以指定类型创建一个数据参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="pd">参数类型</param>
        /// <returns>指定类型的数据参数</returns>
        public static DataParameter Create(string parameterName, object value, ParameterDirection pd)
        {
            if (value is DataParameter)
            {
                var param = value as DataParameter;
                param.ParameterName = parameterName;
                param.Direction = pd;
                return param;
            }

            var p = new System.Data.SqlClient.SqlParameter(parameterName, value);
            
            return new DataParameter()
            {
                DbType = p.DbType,
                Direction = pd,
                ParameterName = parameterName,
                SourceVersion = DataRowVersion.Current,
                Value = value
            };
        }
    }
}
