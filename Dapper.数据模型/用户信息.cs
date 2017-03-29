using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.数据模型
{
    [Table("dapper.用户信息")]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, Id = "用户信息")]
    public class 用户信息
    {
        [Key]
        [Column("ID", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("账号", Order = 1)]
        public string 账号 { get; set; }

        [Column("真实姓名", Order = 2)]
        public string 真实姓名 { get; set; }

        [Column("密码", Order = 3)]
        public string 密码 { get; set; }

        [Column("电话", Order = 4)]
        public string 电话 { get; set; }

        [Column("是否删除", Order = 5)]
        [DefaultValue(false)]
        public bool 是否删除 { get; set; }

        [Column("创建时间", Order = 6)]
        [DefaultValue("GETDATE()")]
        public DateTime 创建时间 { get; set; }= DateTime.Now;

        [Column("更新时间", Order = 7)]
        public DateTime 更新时间 { get; set; } = DateTime.Now;

        [Required]
        public 组织机构 组织机构 { get; set; }

        public List<角色信息> 角色信息列表 { get; set; }
    }
}
