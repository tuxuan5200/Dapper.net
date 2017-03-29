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
    [Table("dapper.组织机构")]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, Id = "组织机构")]
    public class 组织机构
    {
        [Key]
        [Column("ID", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("机构名称", Order = 1)]
        public string 机构名称 { get; set; }

        [Column("是否删除", Order = 2)]
        [DefaultValue(false)]
        public bool 是否删除 { get; set; }

        [Column("父级ID", Order = 3)]
        [DefaultValue(0)]
        public int 父级ID { get; set; } = 0;

        [Column("创建时间", Order = 4)]
        public DateTime 创建时间 { get; set; }= DateTime.Now;

        [Column("更新时间", Order = 5)]
        public DateTime 更新时间 { get; set; }= DateTime.Now;

        public List<用户信息> 用户信息列表 { get; set; }
    }
}
