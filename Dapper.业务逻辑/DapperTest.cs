using Dapper.数据模型;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.业务逻辑
{
    public class DapperTest
    {
        public static readonly string connectionStr = ConfigurationManager.ConnectionStrings["DapperTest"].ConnectionString;

        public static List<组织机构> 查询()
        {
            using (var conn = new SqlConnection(connectionStr))
            {
                try
                {
                    conn.Open();
                    string sql = string.Format("select * from dapper.组织机构");
                    return conn.Query<组织机构>(sql).ToList();
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public static bool 新增()
        {
            using (var 数据库上下文 = new 数据库上下文())
            {
                数据库上下文.组织机构.Add(new 组织机构
                {
                    机构名称 = "第一级机构" + System.Guid.NewGuid().ToString(),
                    是否删除 = false,
                });

                return 数据库上下文.SaveChanges() >= 0;
            }
        }

        public static bool 删除()
        {
            using (var conn = new SqlConnection(connectionStr))
            {
                try
                {
                    conn.Open();
                    string sql = "delete from dapper.组织机构";
                    return conn.Execute(sql) > 0;
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
