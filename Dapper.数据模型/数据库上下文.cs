using Dapper.数据模型.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.数据模型
{
    public class 数据库上下文 : DbContext
    {
        public 数据库上下文() : base("name=DapperTest")
        {
            //var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<数据库上下文, Configuration>());
        }

        public DbSet<组织机构> 组织机构 { get; set; }

        public DbSet<用户信息> 用户信息 { get; set; }

        public DbSet<角色信息> 角色信息 { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
