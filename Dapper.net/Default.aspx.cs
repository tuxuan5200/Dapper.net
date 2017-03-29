using Dapper.业务逻辑;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dapper.net
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var list = DapperTest.查询();
                rptOrgin.DataSource = list;
                rptOrgin.DataBind();
            }
        }

        protected void btnOrgin_Click(object sender, EventArgs e)
        {
            if (DapperTest.新增())
            {
                Response.Write("<script>alert(\"新增成功！\")</script>");
                Response.Redirect("/");
            }
            else
            {
                Response.Write("<script>alert(\"新增失败！\")</script>");
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            if (DapperTest.删除())
            {
                Response.Write("<script>alert(\"删除成功！\")</script>");
                Response.Redirect("/");
            }
            else
            {
                Response.Write("<script>alert(\"删除失败！\")</script>");
            }
        }
    }
}