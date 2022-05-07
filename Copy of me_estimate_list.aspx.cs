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

public partial class me_estimate_list : System.Web.UI.Page
{
    int nSalesPersonId = 0;
    int nUserId = 6;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["oUser"] == null)
        {
            Response.Redirect("MergrchCRMLogin.aspx");
        }
        userinfo obj = (userinfo)Session["oUser"];
        nSalesPersonId = obj.sales_person_id;
        nUserId = obj.user_id;
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
            Session.Remove("CustomerId");
            
            MeEstimate();
            if (nUserId != 6)
            {
                lblTemplate1.Text = "Your Templates";
                lblEstimate2.Visible = true;
                grdPublicEstimationList.Visible = true;
                MeEstimatePublic();
            }
            else
            {
                lblTemplate1.Text = "All Templates";
                lblEstimate2.Visible = false;
                grdPublicEstimationList.Visible = false;
            }
           
           
        }
    }
    protected void MeEstimate()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        List<EstimateTemplateModel> EstTemp = new List<EstimateTemplateModel>();

        if (nSalesPersonId == 0)
        {
            var item = from me in _db.model_estimates
                       join sp in _db.sales_persons on me.sales_person_id equals sp.sales_person_id
                       where me.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                       orderby me.sales_person_id,me.model_estimate_id,me.model_estimate_name
                       select new EstimateTemplateModel()
                                       {
                                           template_id = (int)me.template_id,
                                           model_estimate_id = (int)me.model_estimate_id,
                                           sales_person_id = (int)me.sales_person_id,
                                           client_id = (int)me.client_id,
                                           status_id = (int)me.status_id,
                                           model_estimate_name = me.model_estimate_name,
                                           create_date = (DateTime)me.create_date,
                                           last_update_date = (DateTime)me.last_udated_date,
                                           estimate_comments = me.estimate_comments,
                                           sales_person_name = sp.first_name + " " + sp.last_name,
                                       };

            if (txtSearch.Text.Trim() != "")
            {
                string str = txtSearch.Text.Trim();
                item = from me in _db.model_estimates
                       join sp in _db.sales_persons on me.sales_person_id equals sp.sales_person_id
                       where me.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && me.model_estimate_name.Contains(str)
                       orderby me.sales_person_id, me.model_estimate_id, me.model_estimate_name
                       select new EstimateTemplateModel()
                       {
                           template_id = (int)me.template_id,
                           model_estimate_id = (int)me.model_estimate_id,
                           sales_person_id = (int)me.sales_person_id,
                           client_id = (int)me.client_id,
                           status_id = (int)me.status_id,
                           model_estimate_name = me.model_estimate_name,
                           create_date = (DateTime)me.create_date,
                           last_update_date = (DateTime)me.last_udated_date,
                           estimate_comments = me.estimate_comments,
                           sales_person_name = sp.first_name + " " + sp.last_name,
                       };
            }
            EstTemp = item.ToList();
        }
        else
        {
            var item = from me in _db.model_estimates
                       join sp in _db.sales_persons on me.sales_person_id equals sp.sales_person_id
                       where me.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && me.sales_person_id == nSalesPersonId
                       orderby me.sales_person_id, me.model_estimate_id, me.model_estimate_name
                       select new EstimateTemplateModel()
                       {
                           template_id = (int)me.template_id,
                           model_estimate_id = (int)me.model_estimate_id,
                           sales_person_id = (int)me.sales_person_id,
                           client_id = (int)me.client_id,
                           status_id = (int)me.status_id,
                           model_estimate_name = me.model_estimate_name,
                           create_date = (DateTime)me.create_date,
                           last_update_date = (DateTime)me.last_udated_date,
                           estimate_comments = me.estimate_comments,
                           sales_person_name = sp.first_name + " " + sp.last_name,
                       };

            if (txtSearch.Text.Trim() != "")
            {
                string str = txtSearch.Text.Trim();
                item = from me in _db.model_estimates
                       join sp in _db.sales_persons on me.sales_person_id equals sp.sales_person_id
                       where me.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && me.model_estimate_name.Contains(str) && me.sales_person_id == nSalesPersonId
                       orderby me.sales_person_id, me.model_estimate_id, me.model_estimate_name
                       select new EstimateTemplateModel()
                       {
                           template_id = (int)me.template_id,
                           model_estimate_id = (int)me.model_estimate_id,
                           sales_person_id = (int)me.sales_person_id,
                           client_id = (int)me.client_id,
                           status_id = (int)me.status_id,
                           model_estimate_name = me.model_estimate_name,
                           create_date = (DateTime)me.create_date,
                           last_update_date = (DateTime)me.last_udated_date,
                           estimate_comments = me.estimate_comments,
                           sales_person_name = sp.first_name + " " + sp.last_name,
                       };
            }
            EstTemp = item.ToList();
 
        }
       
       
        grdEstimationList.DataSource = EstTemp;
        grdEstimationList.DataKeyNames = new string[] { "template_id", "model_estimate_id", "sales_person_id", "model_estimate_name" };
        grdEstimationList.DataBind();

        

    }

    protected void MeEstimatePublic()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        List<EstimateTemplateModel> PublicEstTemp = new List<EstimateTemplateModel>();

            var item = from me in _db.model_estimates
                       join sp in _db.sales_persons on me.sales_person_id equals sp.sales_person_id
                       where me.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && me.sales_person_id != nSalesPersonId && me.IsPublic == true
                       orderby me.sales_person_id, me.model_estimate_id, me.model_estimate_name
                       select new EstimateTemplateModel()
                       {
                           template_id = (int)me.template_id,
                           model_estimate_id = (int)me.model_estimate_id,
                           sales_person_id = (int)me.sales_person_id,
                           client_id = (int)me.client_id,
                           status_id = (int)me.status_id,
                           model_estimate_name = me.model_estimate_name,
                           create_date = (DateTime)me.create_date,
                           last_update_date = (DateTime)me.last_udated_date,
                           estimate_comments = me.estimate_comments,
                           sales_person_name = sp.first_name + " " + sp.last_name,
                       };

            if (txtSearch.Text.Trim() != "")
            {
                string str = txtSearch.Text.Trim();
                item = from me in _db.model_estimates
                       join sp in _db.sales_persons on me.sales_person_id equals sp.sales_person_id
                       where me.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && me.model_estimate_name.Contains(str) && me.sales_person_id != nSalesPersonId && me.IsPublic == true
                       orderby me.sales_person_id, me.model_estimate_id, me.model_estimate_name
                       select new EstimateTemplateModel()
                       {
                           template_id = (int)me.template_id,
                           model_estimate_id = (int)me.model_estimate_id,
                           sales_person_id = (int)me.sales_person_id,
                           client_id = (int)me.client_id,
                           status_id = (int)me.status_id,
                           model_estimate_name = me.model_estimate_name,
                           create_date = (DateTime)me.create_date,
                           last_update_date = (DateTime)me.last_udated_date,
                           estimate_comments = me.estimate_comments,
                           sales_person_name = sp.first_name + " " + sp.last_name,
                       };
            }
            PublicEstTemp = item.ToList();


        grdPublicEstimationList.DataSource = PublicEstTemp;
        grdPublicEstimationList.DataKeyNames = new string[] { "template_id", "model_estimate_id", "sales_person_id", "model_estimate_name" };
        grdPublicEstimationList.DataBind();

       

    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("me_locations.aspx");
    }

    
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSearch.ID, btnSearch.GetType().Name, "Click"); 
       MeEstimate();
    }
   
    protected void grdEstimationList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int nModel_estimate_id = Convert.ToInt32(grdEstimationList.DataKeys[e.Row.RowIndex].Values[1].ToString());
            int nsales_person_id = Convert.ToInt32(grdEstimationList.DataKeys[e.Row.RowIndex].Values[2].ToString());
            string strEstName =grdEstimationList.DataKeys[e.Row.RowIndex].Values[3].ToString();
            HyperLink hypEstName = (HyperLink)e.Row.Cells[0].FindControl("hypEstName");
            hypEstName.Text = strEstName;
            hypEstName.NavigateUrl = "me_locations.aspx?meid=" + nModel_estimate_id + "&spid=" + nsales_person_id;

        }

    }
    protected void grdPublicEstimationList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int nModel_estimate_id = Convert.ToInt32(grdPublicEstimationList.DataKeys[e.Row.RowIndex].Values[1].ToString());
            int nsales_person_id = Convert.ToInt32(grdPublicEstimationList.DataKeys[e.Row.RowIndex].Values[2].ToString());
            string strEstName = grdPublicEstimationList.DataKeys[e.Row.RowIndex].Values[3].ToString();
            HyperLink hypEstName1 = (HyperLink)e.Row.Cells[0].FindControl("hypEstName1");
            hypEstName1.Text = strEstName;
            hypEstName1.NavigateUrl = "PublicMe_Pricing.aspx?meid=" + nModel_estimate_id + "&spid=" + nsales_person_id;

        }

    }
}
