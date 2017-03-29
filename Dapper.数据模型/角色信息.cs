using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.数据模型
{
    [Table("dapper.角色信息")]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, Id = "角色信息")]
    public class 角色信息
    {
        [Key]
        [Column("ID", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("角色名称", Order = 1)]
        public string 角色名称 { get; set; }

        [Column("创建时间", Order = 2)]
        public DateTime 创建时间 { get; set; }= DateTime.Now;

        [Column("更新时间", Order = 3)]
        public DateTime 更新时间 { get; set; }= DateTime.Now;

        public List<用户信息> 用户信息列表 { get; set; }
    }
}
