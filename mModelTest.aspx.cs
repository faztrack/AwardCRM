using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mModelTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
       
        DataClassesDataContext _db = new DataClassesDataContext();          

        lblResult.Text = "";

        if (txtName.Text.Trim() == "")
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: Name.<br />");
            return;
        }

        if (txtDesignation.Text.Trim() == "")
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: Designation.<br />");
            return;
        }


    }
}