using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Drawing;

public partial class salespersonmanagement : System.Web.UI.Page
{
    public DataTable dtSalesperson;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);

            if (Session["oUser"] == null)
            {
                Response.Redirect("AwardCRMLogin.aspx");
            }
            if (Page.User.IsInRole("sales003") == false)
            {
                // No Permission Page.
                Response.Redirect("nopermission.aspx");
            }
            userinfo obj = (userinfo)Session["oUser"];
            int nuid = Convert.ToInt32(Request.QueryString.Get("uid"));
            hdnUserId.Value = nuid.ToString();
            LoadEmplyee();
            GetUsers();

        }

    }

    private void LoadEmplyee()
    {
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            string strQ = "select first_name+' '+last_name AS sales_person_name,sales_person_id from user_info WHERE role_id = 3 and salesperson_type_id = 1 order by sales_person_id asc";
            List<userinfo> mList = _db.ExecuteQuery<userinfo>(strQ, string.Empty).ToList();

            Session.Add("Salesperson", csCommonUtility.LINQToDataTable(mList));
            dtSalesperson = (DataTable)Session["Salesperson"];
        }
        catch (Exception ex)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("LoadEmplyee: " + ex.Message);
        }
    }

    protected void GetUsers()
    {
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            var salesheadIDlist = new int[] { 0, Convert.ToInt32(hdnUserId.Value) };
            var userIDlist = new int[] { 13, 14 };
            var item = from u in _db.user_infos
                       where u.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                       && u.role_id == 3
                       // && salesheadIDlist.Contains((int)u.sales_head_id)
                       // && !userIDlist.Contains((int)u.user_id)
                       orderby u.create_date, u.last_name ascending
                       select u;
            DataTable dt = csCommonUtility.LINQToDataTable(item);
            grdUserList.DataSource = item;
            grdUserList.DataBind();
        }
        catch (Exception ex)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("GetUsers: " + ex.Message);
        }
    }

    protected void grdUserList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            dtSalesperson = (DataTable)Session["Salesperson"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DataClassesDataContext _db = new DataClassesDataContext();
                user_info uinfo = new user_info();
                sales_person sp_info = new sales_person();

                Label lblAddiotional_Commission = (Label)e.Row.FindControl("lblAddiotional_Commission");
                TextBox txtAddiotional_Commission = (TextBox)e.Row.FindControl("txtAddiotional_Commission");
                DropDownList ddlSalesPersonType = (DropDownList)e.Row.FindControl("ddlSalesPersonType");
                Label lblgrdSalesPersonType = (Label)e.Row.FindControl("lblgrdSalesPersonType");
                DropDownList ddlAssignedTo = (DropDownList)e.Row.FindControl("ddlAssignedTo");
                Label lblAssignedTo = (Label)e.Row.FindControl("lblAssignedTo");

                int nRoleId = Convert.ToInt32(e.Row.Cells[4].Text);
                if (nRoleId == 1)
                    e.Row.Cells[4].Text = "Admin";
                else if (nRoleId == 2)
                    e.Row.Cells[4].Text = "Manager";
                else if (nRoleId == 3)
                    e.Row.Cells[4].Text = "Sales";
                else if (nRoleId == 4)
                    e.Row.Cells[4].Text = "Superintendent";
                else if (nRoleId == 5)
                    e.Row.Cells[4].Text = "Operation";

                int nuid = Convert.ToInt32(grdUserList.DataKeys[e.Row.RowIndex].Value.ToString());
                // Customer Address

                uinfo = _db.user_infos.Single(c => c.user_id == nuid);
                string strAddress = uinfo.address + "</br>" + uinfo.city + " " + uinfo.state + " " + uinfo.zip;
                e.Row.Cells[2].Text = strAddress;

                //sp_info = _db.sales_persons.Single(c => c.sales_person_id == uinfo.sales_person_id);
                //if (sp_info.additional_com_per != 0)
                //{
                //    lblAddiotional_Commission.Text = sp_info.additional_com_per.ToString();
                //    txtAddiotional_Commission.Text = sp_info.additional_com_per.ToString();
                //}

                //if (uinfo.salesperson_type_id == 2)
                //{
                //    ddlAssignedTo.SelectedValue = uinfo.sales_head_id.ToString();
                //    lblAssignedTo.Text = ddlAssignedTo.SelectedItem.Text;
                //}
                //else
                //{                   
                //    lblAssignedTo.Text = "";
                //}
                //ddlSalesPersonType.SelectedValue = uinfo.salesperson_type_id.ToString();
                lblgrdSalesPersonType.Text = ddlSalesPersonType.SelectedItem.Text;

                ddlSalesPersonType.Attributes["CommandArgument"] = string.Format("{0}", nuid);
            }
        }
        catch (Exception ex)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("DataBound: " + ex.Message);
        }
    }

    protected void grdUserList_RowEditing(object sender, GridViewEditEventArgs e)
    {
      
        try
        {
            KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, grdUserList.ID, grdUserList.GetType().Name, "RowEditing");
            lblResult.Text = "";
            DataClassesDataContext _db = new DataClassesDataContext();
            dtSalesperson = (DataTable)Session["Salesperson"];
            int nuid = Convert.ToInt32(grdUserList.DataKeys[e.NewEditIndex].Value.ToString());

            Label lblAddiotional_Commission = (Label)grdUserList.Rows[e.NewEditIndex].FindControl("lblAddiotional_Commission");
            TextBox txtAddiotional_Commission = (TextBox)grdUserList.Rows[e.NewEditIndex].FindControl("txtAddiotional_Commission");

            DropDownList ddlSalesPersonType = (DropDownList)grdUserList.Rows[e.NewEditIndex].FindControl("ddlSalesPersonType");
            DropDownList ddlAssignedTo = (DropDownList)grdUserList.Rows[e.NewEditIndex].FindControl("ddlAssignedTo");
            Label lblAssignedTo = (Label)grdUserList.Rows[e.NewEditIndex].FindControl("lblAssignedTo");
            Label lblgrdSalesPersonType = (Label)grdUserList.Rows[e.NewEditIndex].FindControl("lblgrdSalesPersonType");


            if (ddlSalesPersonType.SelectedValue == "2")
            {
                lblAddiotional_Commission.Visible = false;
                txtAddiotional_Commission.Visible = true;
                lblAssignedTo.Visible = false;
                ddlAssignedTo.Visible = true;
            }
            lblgrdSalesPersonType.Visible = false;
            ddlSalesPersonType.Visible = true;

            LinkButton btnUpdate = (LinkButton)grdUserList.Rows[e.NewEditIndex].Cells[8].Controls[0]; // Edit Button
            btnUpdate.Text = "Update";
            btnUpdate.CommandName = "Update";
        }
        catch (Exception ex)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("RowEditing: " + ex.Message);
        }
    }
    protected void grdUserList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

        try
        {
            KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, grdUserList.ID, grdUserList.GetType().Name, "RowUpdating");
            string strQ = string.Empty;
            string strQ2 = string.Empty;
            DataClassesDataContext _db = new DataClassesDataContext();
            dtSalesperson = (DataTable)Session["Salesperson"];
            int nuid = Convert.ToInt32(grdUserList.DataKeys[e.RowIndex].Value.ToString());
            user_info uinfo = new user_info();
            uinfo = _db.user_infos.Single(c => c.user_id == nuid);

            sales_person sp_info = new sales_person();
            sp_info = _db.sales_persons.Single(c => c.sales_person_id == uinfo.sales_person_id);

            Label lblAddiotional_Commission = (Label)grdUserList.Rows[e.RowIndex].FindControl("lblAddiotional_Commission");
            TextBox txtAddiotional_Commission = (TextBox)grdUserList.Rows[e.RowIndex].FindControl("txtAddiotional_Commission");

            DropDownList ddlSalesPersonType = (DropDownList)grdUserList.Rows[e.RowIndex].FindControl("ddlSalesPersonType");
            DropDownList ddlAssignedTo = (DropDownList)grdUserList.Rows[e.RowIndex].FindControl("ddlAssignedTo");
           

            if (ddlSalesPersonType.SelectedValue == "2")
            {
                if (txtAddiotional_Commission.Text != "")
                {
                    try
                    {
                        Convert.ToDecimal(txtAddiotional_Commission.Text.Trim());
                    }
                    catch
                    {
                        lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Commission is required");
                        return;
                    }
                }
                else
                {
                    lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Commission is required");
                }

                strQ = "UPDATE user_info SET sales_head_id = " + Convert.ToInt32(ddlAssignedTo.SelectedValue) + ", salesperson_type_id = " + Convert.ToInt32(ddlSalesPersonType.SelectedValue) + " WHERE user_id =" + nuid;
                strQ2 = "UPDATE sales_person SET additional_com_per = " + Convert.ToDecimal(txtAddiotional_Commission.Text.Trim()) + "  WHERE sales_person_id =" + sp_info.sales_person_id;
            }
            else
            {
                strQ = "UPDATE user_info SET sales_head_id = 0, salesperson_type_id = " + Convert.ToInt32(ddlSalesPersonType.SelectedValue) + " WHERE user_id =" + nuid;
                strQ2 = "UPDATE sales_person SET additional_com_per = 0 WHERE sales_person_id =" + sp_info.sales_person_id;
            }
            _db.ExecuteCommand(strQ, string.Empty);
            _db.ExecuteCommand(strQ2, string.Empty);
            GetUsers();
        }
        catch (Exception ex)
        {
          //  lblResult.Text = csCommonUtility.GetSystemErrorMessage("RowUpdatin: " + ex.Message);
        }
    }

    protected void ddlSalesPersonType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        
        try
        {
           
            DataClassesDataContext _db = new DataClassesDataContext();

            GridViewRow row = (GridViewRow)((DropDownList)sender).Parent.Parent;
            DropDownList ddlAssignedTo = (DropDownList)row.FindControl("ddlAssignedTo");
            Label lblAssignedTo = (Label)row.FindControl("lblAssignedTo");
            DropDownList ddlSalesPersonType = (DropDownList)row.FindControl("ddlSalesPersonType");
            Label lblAddiotional_Commission = (Label)row.FindControl("lblAddiotional_Commission");
            TextBox txtAddiotional_Commission = (TextBox)row.FindControl("txtAddiotional_Commission");

            int nSId = Convert.ToInt32(ddlSalesPersonType.Attributes["CommandArgument"]);
            string strQ = "select first_name+' '+last_name AS sales_person_name,sales_person_id from user_info " +
                       " WHERE role_id = 3 and salesperson_type_id = 1 and user_id != " + nSId + " order by sales_person_id asc";
            List<userinfo> mList = _db.ExecuteQuery<userinfo>(strQ, string.Empty).ToList();
            Session.Add("Salesperson", csCommonUtility.LINQToDataTable(mList));
            dtSalesperson = (DataTable)Session["Salesperson"];
            // ddlAssignedTo.DataSource = mList;
            ddlAssignedTo.DataBind();

            if (ddlSalesPersonType.SelectedValue == "2")
            {
                ddlAssignedTo.Visible = true;
                lblAssignedTo.Visible = false;
                lblAddiotional_Commission.Visible = false;
                txtAddiotional_Commission.Visible = true;
            }
            else
            {
                ddlAssignedTo.Visible = false;
                lblAssignedTo.Visible = true;
                lblAddiotional_Commission.Visible = true;
                txtAddiotional_Commission.Visible = false;
                lblAssignedTo.Text = "";
                lblAddiotional_Commission.Text = "";
            }
        }
        catch (Exception ex)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("ddlSalesPersonType: " + ex.Message);
        }

        #region BlockCode
        //try
        //{
        //    foreach (GridViewRow dr in grdUserList.Rows)
        //    {
        //        DataClassesDataContext _db = new DataClassesDataContext();
        //        DropDownList ddlAssignedTo = (DropDownList)dr.FindControl("ddlAssignedTo");
        //        Label lblAssignedTo = (Label)dr.FindControl("lblAssignedTo");
        //        DropDownList ddlSalesPersonType = (DropDownList)dr.FindControl("ddlSalesPersonType");
        //        Label lblAddiotional_Commission = (Label)dr.FindControl("lblAddiotional_Commission");
        //        TextBox txtAddiotional_Commission = (TextBox)dr.FindControl("txtAddiotional_Commission");

        //        int nSId = Convert.ToInt32(ddlSalesPersonType.Attributes["CommandArgument"]);

        //        string strQ = "select first_name+' '+last_name AS sales_person_name,sales_person_id from user_info " +
        //                      " WHERE role_id = 3 and user_id != " + nSId + " order by sales_person_id asc";

        //        List<userinfo> mList = _db.ExecuteQuery<userinfo>(strQ, string.Empty).ToList();
        //        Session.Add("Salesperson", csCommonUtility.LINQToDataTable(mList));
        //        dtSalesperson = (DataTable)Session["Salesperson"];
        //        // ddlAssignedTo.DataSource = mList;
        //        ddlAssignedTo.DataBind();

        //        if (ddlSalesPersonType.SelectedValue == "2")
        //        {
        //            ddlAssignedTo.Visible = true;
        //            lblAssignedTo.Visible = false;

        //            lblAddiotional_Commission.Visible = false;
        //            txtAddiotional_Commission.Visible = true;
        //        }
        //        else
        //        {
        //            ddlAssignedTo.Visible = false;
        //            lblAssignedTo.Visible = true;

        //            lblAddiotional_Commission.Visible = false;
        //            txtAddiotional_Commission.Visible = true;
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    lblResult.Text = csCommonUtility.GetSystemErrorMessage("ddlSalesPersonType: " + ex.Message);
        //}
        #endregion
    }
}
