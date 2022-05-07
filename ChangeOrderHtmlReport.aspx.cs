using DataStreams.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChangeOrderHtmlReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
            string Title = string.Empty;

            if (Session["Title"] != null)
            {
                Title = (string)Session["Title"];
                lblTitle.Text = Title;
            }

            BindGrid();


        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("changeorder_report.aspx");
    }
    protected void btnExpList_Click(object sender, ImageClickEventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnExpList.ID, btnExpList.GetType().Name, "Click");
        
        DataTable dtTmp = (DataTable)Session["COTable"];
        Response.Clear();
        Response.ClearHeaders();

        using (CsvWriter writer = new CsvWriter(Response.OutputStream, ',', System.Text.Encoding.Default))
        {

            writer.WriteAll(dtTmp, false);
        }
        // Response.ContentType = "application/vnd.ms-excel";
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        Response.AddHeader("Content-Disposition", "attachment; filename=COReport.csv");
        Response.End();
    }

    public void BindGrid()
    {
        DataTable dtTable = new DataTable();

        if (Session["CODataTable"] != null)
        {
            dtTable = (DataTable)Session["CODataTable"];
        }
        grdChangeOrders.DataSource = dtTable;
        grdChangeOrders.DataBind();


    }
}