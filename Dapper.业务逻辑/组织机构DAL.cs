using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.数据模型;

namespace Dapper.业务逻辑
{
    public class 组织机构DAL
    {
        #region 新增
        public static List<组织机构> 查询()
        {
            using (var 数据库上下文 = new 数据库上下文())
            {
                try
                {
                    return 数据库上下文.组织机构.ToList();
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }
        #endregion
    }
}
