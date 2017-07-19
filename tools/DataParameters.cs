using System;
using System.Collections.Generic;
using System.Text;
using DbA.Common;

namespace DAL// 放在数据访问层里
{
    /// <summary>
    /// DataParameters
    /// </summary>
    public class DataParameters
    {
        private List<DataParameter> dps = new List<DataParameter>();

        public void Add(string name, object value)
        {
            dps.Add(DataParameter.Create(name, value));
        }
        
        /// <summary>
        /// 获得SQL参数
        /// </summary>
        public String KeyToSql()
        {
            var sql = new StringBuilder();
            foreach (var dp in dps)
            {
                sql.Append(dp.ParameterName + ",");
            }

            return sql.ToString().TrimEnd(',');
        }

        public DataParameter[] ToArray()
        {
            return dps.ToArray();
        }
    }
}
