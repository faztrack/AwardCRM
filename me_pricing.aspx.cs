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
using System.Web.Services;

public partial class me_pricing : System.Web.UI.Page
{
    string strDetails = "";
    public static string strDetailsFull = "";
    private double subtotal = 0.0;
    private double grandtotal = 0.0;

    private double subtotal_diect = 0.0;
    private double grandtotal_direct = 0.0;
    [WebMethod]
    public static string[] GetItemName(String prefixText, Int32 count)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        if (HttpContext.Current.Session["cParent"] != null)
        {

            int nPId = Convert.ToInt32(HttpContext.Current.Session["cParent"]);
            return (from c in _db.sectioninfos
                    where c.section_name.Contains(prefixText) && c.parent_id == nPId
                    select c.section_name).Take<String>(count).ToArray();
        }
        else
        {

            return (from c in _db.sectioninfos
                    where c.section_name.Contains(prefixText)
                    select c.section_name).Take<String>(count).ToArray();
        }

    }
    [WebMethod]
    public static string[] GetItemNameAll(String prefixText, Int32 count)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        if (HttpContext.Current.Session["gspSearch"] != null)
        {
            DataTable dt = (DataTable)(HttpContext.Current.Session["gspSearch"]);
            return (from c in dt.AsEnumerable()
                    where c.Field<string>("section_name").ToLower().Contains(prefixText.ToLower())
                    select c.Field<string>("section_name")).ToArray();

        }
        else
        {

            DataTable dt = LoadFullSection();
            return (from c in dt.AsEnumerable()
                    where c.Field<string>("section_name").ToLower().Contains(prefixText.ToLower())
                    select c.Field<string>("section_name")).ToArray();
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
            if (Session["oUser"] == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["LoginPage"].ToString());
            }

            Session.Remove("AddMoreMELocation");
            Session.Remove("AddMoreMESection");
            Session.Remove("gspSearch");

            if (Request.QueryString.Get("meid") != null)
                hdnEstimateId.Value = Request.QueryString.Get("meid");

            userinfo obj = (userinfo)Session["oUser"];

            if (Request.QueryString.Get("spid") != null)
            {
                hdnSalesPersonId.Value = Convert.ToInt32(Request.QueryString.Get("spid")).ToString();
            }
            else
            {
                hdnSalesPersonId.Value = obj.sales_person_id.ToString();
            }


            DataClassesDataContext _db = new DataClassesDataContext();
            tdlOther.Visible = false;
            model_estimate me = new model_estimate();
            me = _db.model_estimates.Single(mest => mest.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && mest.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && mest.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));

            lblModelEstimateName.Text = me.model_estimate_name;
            lblExistingModelEstimateName.Text = me.model_estimate_name;
            txtComments.Text = me.estimate_comments;
            chkIsPublic.Checked = Convert.ToBoolean(me.IsPublic);

            //lblspModelEstName.Text = me.model_estimate_name;
            //lblCreateDate.Text = Convert.ToDateTime(me.create_date).ToShortDateString();
            //lblLastUpdatedDate.Text = Convert.ToDateTime(me.last_udated_date).ToShortDateString();

            var item = from loc in _db.locations
                       join mel in _db.model_estimate_locations on loc.location_id equals mel.location_id
                       where mel.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && mel.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && mel.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                       select new LocationModel()
                       {
                           location_id = (int)mel.location_id,
                           location_name = loc.location_name
                       };

            ddlCustomerLocations.DataSource = item;
            ddlCustomerLocations.DataTextField = "location_name";
            ddlCustomerLocations.DataValueField = "location_id";
            ddlCustomerLocations.DataBind();
            ddlCustomerLocations.Items.Insert(0, "Select Location");
            ddlCustomerLocations.SelectedValue = "0";

            var section = from sec in _db.sectioninfos
                          join mes in _db.model_estimate_sections on sec.section_id equals mes.section_id
                          where mes.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && mes.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && mes.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                          select new SectionInfo()
                          {
                              section_id = (int)mes.section_id,
                              section_name = sec.section_name
                          };

            DataTable dtSectionId = csCommonUtility.LINQToDataTable(section);

            ddlSections.DataSource = section;
            ddlSections.DataTextField = "section_name";
            ddlSections.DataValueField = "section_id";
            ddlSections.DataBind();
            ddlSections.Items.Insert(0, "Select Section");
            ddlSections.SelectedValue = "0";
            ddlSections.Items.Insert(1, "All Selected Section");
            ddlSections.SelectedValue = "-1";
            int[] terms = new int[dtSectionId.Rows.Count]; ;
            for (int i = 0; i < dtSectionId.Rows.Count; i++)
            {
                terms[i] = Convert.ToInt32(dtSectionId.Rows[i]["section_id"]);
            }
            Session["myIds"] = terms;
            // DataTable dtNew = LoadFullSection();
            hdnSortDesc.Value = "0";
            BindSelectedItemGrid();
            BindSelectedItemGrid_Direct();
            if (grdGrouping.Rows.Count == 0)
            {
                lblRetailPricingHeader.Visible = false;
            }
            else
            {
                lblRetailPricingHeader.Visible = true;
            }
            if (grdGroupingDirect.Rows.Count == 0)
            {
                lblDirectPricingHeader.Visible = false;
            }
            else
            {
                lblDirectPricingHeader.Visible = true;
            }
            Calculate_Total();
            if (grdGrouping.Rows.Count == 0 && grdGroupingDirect.Rows.Count == 0)
            {
                rdoSort.Visible = false;
                //tblTotalProjectPrice.Visible = false;
                if (grdGrouping.Rows.Count == 0)
                {
                    lblRetailPricingHeader.Visible = false;
                }
                else
                {
                    lblRetailPricingHeader.Visible = true;
                }
                if (grdGroupingDirect.Rows.Count == 0)
                {
                    lblDirectPricingHeader.Visible = false;
                }
                else
                {
                    lblDirectPricingHeader.Visible = true;
                }
            }
            else
            {
                rdoSort.Visible = true;
                //tblTotalProjectPrice.Visible = true;
                if (grdGrouping.Rows.Count == 0)
                {
                    lblRetailPricingHeader.Visible = false;
                }
                else
                {
                    lblRetailPricingHeader.Visible = true;
                }
                if (grdGroupingDirect.Rows.Count == 0)
                {
                    lblDirectPricingHeader.Visible = false;
                }
                else
                {
                    lblDirectPricingHeader.Visible = true;
                }
            }
            if (ddlSections.SelectedItem.Text == "Select Section")
            {
                trvSection.Nodes.Clear();
            }
        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSubmit.ID, btnSubmit.GetType().Name, "Click"); 
        lblMessage.Text = "";
        if (txtNewEstimateName.Text.Trim() == "")
        {
            lblMessage.Text = csCommonUtility.GetSystemErrorMessage("New Estimate Name is required field");

            modUpdateEstimate.Show();
            return;
        }

        string strNewName = txtNewEstimateName.Text.Trim();
        DataClassesDataContext _db = new DataClassesDataContext();
        model_estimate m_est = new model_estimate();
        int meid = Convert.ToInt32(hdnEstimateId.Value);
        if (Convert.ToInt32(hdnEstimateId.Value) > 0)
            m_est = _db.model_estimates.Single(me => me.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && me.model_estimate_id == meid && me.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));

        m_est.model_estimate_name = strNewName;

        _db.SubmitChanges();

        lblMessage.Text = "";
        txtNewEstimateName.Text = "";
        modUpdateEstimate.Hide();

        Response.Redirect("me_pricing.aspx?meid=" + hdnEstimateId.Value + "&spid=" + hdnSalesPersonId.Value);
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnClose.ID, btnClose.GetType().Name, "Click"); 
        lblMessage.Text = "";
        txtNewEstimateName.Text = "";
        modUpdateEstimate.Hide();
    }
    protected void lnkAddMoreLocation_Click(object sender, EventArgs e)
    {
        Session.Add("AddMoreMELocation", "AddMoreMElocation");
        Response.Redirect("me_locations.aspx?meid=" + hdnEstimateId.Value + "&spid=" + hdnSalesPersonId.Value);
    }
    protected void lnkAddMoreSections_Click(object sender, EventArgs e)
    {
        Session.Add("AddMoreMESection", "AddMoreMESection");
        Response.Redirect("me_sections.aspx?meid=" + hdnEstimateId.Value + "&spid=" + hdnSalesPersonId.Value);
    }
    private void LoadTree(int nSectionLevel)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        string strQ = " SELECT * FROM sectioninfo WHERE is_disable = 0 AND section_level=" + nSectionLevel + " AND client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
        IEnumerable<sectioninfo> list = _db.ExecuteQuery<sectioninfo>(strQ, string.Empty);
        trvSection.Nodes.Clear();
        foreach (sectioninfo sec in list)
        {
            string name = sec.section_name;
            if (sec.parent_id == 0)
            {
                TreeNode node = new TreeNode(sec.section_name, sec.section_id.ToString());
                trvSection.Nodes.Add(node);
                AddChildMenu(node, sec);
            }
        }
    }
    private void AddChildMenu(TreeNode parentNode, sectioninfo sec)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        string strQ = " SELECT * FROM sectioninfo WHERE is_disable = 0 AND client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + " AND section_id NOT IN (SELECT item_id FROM item_price WHERE client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + ")";
        IEnumerable<sectioninfo> list = _db.ExecuteQuery<sectioninfo>(strQ, string.Empty);
        //List<sectioninfo> list = _db.sectioninfos.Where(c => c.client_id == 1).ToList();
        foreach (sectioninfo subsec in list)
        {
            if (subsec.parent_id.ToString() == parentNode.Value)
            {
                TreeNode node = new TreeNode(subsec.section_name, subsec.section_id.ToString());
                parentNode.ChildNodes.Add(node);
                AddChildMenu(node, subsec);
            }
        }
    }
    public void BindGrid()
    {
        lblResult1.Text = "";
        lblAdd.Text = "";
        lblSelectLocation.Text = "";

        DataClassesDataContext _db = new DataClassesDataContext();
        string strCondition = string.Empty;
        if (txtSearchItemName.Text.Trim() != "")
        {
            strCondition = "  AND si.section_name LIKE '%" + txtSearchItemName.Text.Trim() + "%'";
        }
        sectioninfo sinfo = new sectioninfo();
        sinfo = _db.sectioninfos.Single(c => c.section_id == Convert.ToInt32(hdnSectionId.Value) && c.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));
        hdnSectionLevel.Value = Convert.ToInt32(sinfo.section_level).ToString();
        hdnParentId.Value = Convert.ToInt32(sinfo.parent_id).ToString();
        hdnSectionSerial.Value = Convert.ToDecimal(sinfo.section_serial).ToString();

        string StrQ = " SELECT it.client_id,it.item_id,si.section_name,si.is_mandatory,it.measure_unit,it.item_cost*it.retail_multiplier AS item_cost,it.minimum_qty,it.retail_multiplier,it.labor_rate,it.update_time,si.section_serial,it.labor_id,((it.item_cost + it.labor_rate) * it.retail_multiplier) * it.minimum_qty AS ext_item_cost " +
                     " FROM item_price it " +
                     " INNER JOIN sectioninfo si on si.section_id =  it.item_id " +
                     " WHERE si.is_active = 1 AND si.is_disable = 0 AND si.parent_id = " + Convert.ToInt32(trvSection.SelectedValue) + "  " + strCondition + " ";
        List<ItemPriceModel> ItemList = _db.ExecuteQuery<ItemPriceModel>(StrQ, string.Empty).ToList();
        grdItemPrice.DataSource = ItemList;
        grdItemPrice.DataKeyNames = new string[] { "item_id", "labor_rate", "retail_multiplier" };
        grdItemPrice.DataBind();

    }
    private void GetItemId_other()
    {
        int nItemId = 0;
        DataClassesDataContext _db = new DataClassesDataContext();
        var result = (from it in _db.item_prices
                      join si in _db.sectioninfos on it.item_id equals si.section_id
                      where si.section_level == Convert.ToInt32(ddlSections.SelectedValue)
                      select it.item_id);
        int n = result.Count();
        if (result != null && n > 0)
            nItemId = result.Max();

        nItemId = nItemId + 1;
        hdnOtherId.Value = nItemId.ToString();
    }


    public string GetItemDetialsForUpdateItem(int SectionId)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        List<sectioninfo> list = _db.sectioninfos.Where(c => c.section_id == SectionId && c.parent_id > 0 && c.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])).ToList();
        foreach (sectioninfo sec1 in list)
        {
            strDetails = sec1.section_name + " >> " + strDetails;
            GetItemDetialsForUpdateItem(Convert.ToInt32(sec1.parent_id));
        }
        return strDetails;
    }
    public string GetItemDetials_forBreadCome(int SectionId)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        List<sectioninfo> list = _db.sectioninfos.Where(c => c.section_id == SectionId && c.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])).ToList();
        foreach (sectioninfo sec1 in list)
        {
            strDetails = sec1.section_name + " >> " + strDetails;
            GetItemDetials_forBreadCome(Convert.ToInt32(sec1.parent_id));
        }
        return strDetails;
    }
    public string GetSectionName(int section_level)
    {
        string str = "";
        DataClassesDataContext _db = new DataClassesDataContext();
        sectioninfo si = new sectioninfo();
        si = _db.sectioninfos.Single(c => c.section_level == section_level && c.parent_id == 0 && c.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));
        str = si.section_name;
        return str;
    }
    private int GetSerial()
    {
        int nSerial = 0;
        DataClassesDataContext _db = new DataClassesDataContext();
        var result = (from pd in _db.model_estimate_pricings
                      where pd.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && pd.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && pd.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && pd.pricing_type == "A" && pd.section_level == Convert.ToInt32(hdnSectionLevel.Value)
                      select pd.item_cnt);
        int n = result.Count();
        if (result != null && n > 0)
            nSerial = result.Max();

        return nSerial + 1;
    }
    private int GetSerialMultiple(int sectionLvel)
    {
        int nSerial = 0;
        DataClassesDataContext _db = new DataClassesDataContext();
        var result = (from pd in _db.model_estimate_pricings
                      where pd.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && pd.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && pd.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && pd.pricing_type == "A" && pd.section_level == sectionLvel
                      select pd.item_cnt);
        int n = result.Count();
        if (result != null && n > 0)
            nSerial = result.Max();

        return nSerial + 1;
    }

    private int GetSerial_other(int nOtherId)
    {
        int nSerial = 0;
        DataClassesDataContext _db = new DataClassesDataContext();
        var result = (from pd in _db.model_estimate_pricings
                      where pd.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && pd.item_id == nOtherId && pd.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && pd.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && pd.section_level == Convert.ToInt32(hdnSectionLevel.Value)
                      select pd.other_item_cnt);
        int n = result.Count();
        if (result != null && n > 0)
            nSerial = result.Max();

        return nSerial + 1;
    }



    private decimal GetRetailTotal()
    {
        decimal dRetail = 0;
        DataClassesDataContext _db = new DataClassesDataContext();
        var result = (from pd in _db.model_estimate_pricings
                      where (from clc in _db.model_estimate_locations
                             where clc.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && clc.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && clc.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                             select clc.location_id).Contains(pd.location_id) &&
                             (from cs in _db.model_estimate_sections
                              where cs.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && cs.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && cs.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                              select cs.section_id).Contains(pd.section_level) && pd.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && pd.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && pd.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && pd.pricing_type == "A"
                      select pd.total_retail_price);
        int n = result.Count();
        if (result != null && n > 0)
            dRetail = result.Sum();

        return dRetail;
    }
    private decimal GetDirctTotal()
    {
        decimal dDirect = 0;
        DataClassesDataContext _db = new DataClassesDataContext();
        var result = (from pd in _db.model_estimate_pricings
                      where (from clc in _db.model_estimate_locations
                             where clc.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && clc.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && clc.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                             select clc.location_id).Contains(pd.location_id) &&
                             (from cs in _db.model_estimate_sections
                              where cs.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && cs.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && cs.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                              select cs.section_id).Contains(pd.section_level) && pd.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && pd.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && pd.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && pd.pricing_type == "A"
                      select pd.total_direct_price);
        int n = result.Count();
        if (result != null && n > 0)
            dDirect = result.Sum();

        return dDirect;
    }
    public void BindSelectedItemGrid()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        string strQ = "";
        if (rdoSort.SelectedValue == "2")
        {
            strQ = " select DISTINCT section_level AS colId,'SECTION: '+ section_name as colName from model_estimate_pricing where model_estimate_pricing.location_id IN (Select location_id from model_estimate_locations WHERE model_estimate_locations.model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND model_estimate_locations.sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND model_estimate_locations.client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + " ) " +
                   " AND model_estimate_pricing.section_level IN (Select section_id from model_estimate_sections WHERE model_estimate_sections.model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND model_estimate_sections.sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND model_estimate_sections.client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + " ) " +
                   " AND model_estimate_pricing.model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND model_estimate_pricing.sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND is_direct=1 AND model_estimate_pricing.client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + "  order by section_level asc";
        }
        else
        {
            strQ = "select DISTINCT model_estimate_pricing.location_id AS colId,'LOCATION: '+ location.location_name as colName,sort_id from model_estimate_pricing  INNER JOIN location on location.location_id = model_estimate_pricing.location_id where model_estimate_pricing.location_id IN (Select location_id from model_estimate_locations WHERE model_estimate_locations.model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND model_estimate_locations.sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND model_estimate_locations.client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + " ) " +
                " AND model_estimate_pricing.section_level IN (Select section_id from model_estimate_sections WHERE model_estimate_sections.model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND model_estimate_sections.sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND model_estimate_sections.client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + " ) " +
                " AND model_estimate_pricing.model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND model_estimate_pricing.sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND is_direct=1 AND model_estimate_pricing.client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + " order by sort_id asc";
        }
        List<PricingMaster> mList = _db.ExecuteQuery<PricingMaster>(strQ, string.Empty).ToList();
        grdGrouping.DataSource = mList;
        grdGrouping.DataKeyNames = new string[] { "colId" };
        grdGrouping.DataBind();

    }
    public void BindSelectedItemGrid_Direct()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        string strQ = "";
        if (rdoSort.SelectedValue == "2")
        {
            strQ = "select DISTINCT section_level AS colId,'SECTION: '+ section_name as colName from model_estimate_pricing where model_estimate_pricing.location_id IN (Select location_id from model_estimate_locations WHERE model_estimate_locations.model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND model_estimate_locations.sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND model_estimate_locations.client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + " ) " +
                   " AND model_estimate_pricing.section_level IN (Select section_id from model_estimate_sections WHERE model_estimate_sections.model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND model_estimate_sections.sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND model_estimate_sections.client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + " ) " +
                   " AND model_estimate_pricing.model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND model_estimate_pricing.sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND is_direct=2 AND model_estimate_pricing.client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + "  order by section_level asc";
        }
        else
        {
            strQ = "select DISTINCT model_estimate_pricing.location_id AS colId,'LOCATION: '+ location.location_name as colName,sort_id from model_estimate_pricing  INNER JOIN location on location.location_id = model_estimate_pricing.location_id where model_estimate_pricing.location_id IN (Select location_id from model_estimate_locations WHERE model_estimate_locations.model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND model_estimate_locations.sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND model_estimate_locations.client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + " ) " +
                   " AND model_estimate_pricing.section_level IN (Select section_id from model_estimate_sections WHERE model_estimate_sections.model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND model_estimate_sections.sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND model_estimate_sections.client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + " ) " +
                   " AND model_estimate_pricing.model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND model_estimate_pricing.sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND is_direct=2 AND model_estimate_pricing.client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + " order by sort_id asc";
        }
        List<PricingMaster> mList = _db.ExecuteQuery<PricingMaster>(strQ, string.Empty).ToList();
        grdGroupingDirect.DataSource = mList;
        grdGroupingDirect.DataKeyNames = new string[] { "colId" };
        grdGroupingDirect.DataBind();

    }
    public void Calculate_Total()
    {
        decimal direct = 0;
        decimal retail = 0;
        decimal grandtotal = 0;
        direct = GetDirctTotal();
        retail = GetRetailTotal();
        grandtotal = direct + retail;

        lblDirctTotalCost.Text = direct.ToString("c");
        lblRetailTotalCost.Text = retail.ToString("c");
        lblGrandTotalCost.Text = grandtotal.ToString("c");

    }


    protected void ddlSections_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlSections.SelectedItem.Text == "All Selected Section")
        {
            lblResult1.Text = "";
            trvSection.Nodes.Clear();
            lblSelectLocation.Text = "";
            lblResult1.Text = "";
            lblAdd.Text = "";
            lblParent.Text = "";


            tblMultiPricing.Visible = true;


            tblPricingWrapper.Visible = false;

        }
        else
        {
            tblMultiPricing.Visible = false;
            txtSearchAll.Text = string.Empty;
            BindGridAllPrice();
            if (ddlSections.SelectedItem.Text != "Select Section")
            {
                lblSelectLocation.Text = "";
                lblResult1.Text = "";
                lblAdd.Text = "";
                lblParent.Text = "";
                grdItemPrice.Visible = false;
                txtSearchItemName.Visible = false;
                btnSearch.Visible = false;
                LinkButton1.Visible = false;
                LoadTree(Convert.ToInt32(ddlSections.SelectedValue));
                tdlOther.Visible = false;
                GetItemId_other();
                tblPricingWrapper.Visible = true;
            }
            else
            {
                lblResult1.Text = "";
                trvSection.Nodes.Clear();
                tblPricingWrapper.Visible = false;
            }
        }
    }
    protected void trvSection_SelectedNodeChanged(object sender, EventArgs e)
    {
        hdnSectionId.Value = trvSection.SelectedValue;
        Session.Add("cParent", hdnSectionId.Value);
        grdItemPrice.Visible = true;
        tdlOther.Visible = true;
        BindGrid();
        lblParent.Text = GetItemDetials_forBreadCome(Convert.ToInt32(trvSection.SelectedValue));
    }

    protected void grdItemPrice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Add")
        {
            lblSelectLocation.Text = "";
            lblResult1.Text = "";
            lblMessage.Text = "";
            lblAdd.Text = "";
            string strPriceId = "0";
            decimal ditem_cost = 0;
            decimal dlabor_rate = 0;
            decimal dretail_multiplier = 0;
            string smeasure_unit = "";
            decimal dminimum_qty = 0;

            DataClassesDataContext _db = new DataClassesDataContext();
            int index = Convert.ToInt32(e.CommandArgument);
            hdnSectionId.Value = grdItemPrice.Rows[index].Cells[1].Text;
            TextBox txtQty = (TextBox)grdItemPrice.Rows[index].FindControl("txtQty");
            TextBox txtShortNote = (TextBox)grdItemPrice.Rows[index].FindControl("txtShortNote");           
            DropDownList ddlLabor = (DropDownList)grdItemPrice.Rows[index].FindControl("ddlLabor");
            DropDownList ddlDirect = (DropDownList)grdItemPrice.Rows[index].FindControl("ddlDirect");
            strPriceId = grdItemPrice.DataKeys[index].Values[0].ToString();

            dlabor_rate = Convert.ToDecimal(grdItemPrice.DataKeys[index].Values[1].ToString());
            dretail_multiplier = Convert.ToDecimal(grdItemPrice.DataKeys[index].Values[2].ToString());
            smeasure_unit = grdItemPrice.Rows[index].Cells[7].Text;
            ditem_cost = Convert.ToDecimal(grdItemPrice.Rows[index].Cells[8].Text.Replace("$", "").Replace("(", "-").Replace(")", ""));
            dminimum_qty = Convert.ToDecimal(grdItemPrice.Rows[index].Cells[9].Text);

            decimal nQty = Convert.ToDecimal(txtQty.Text);
            decimal nCost = Convert.ToDecimal(grdItemPrice.Rows[index].Cells[8].Text.Replace("$", "").Replace("(", "-").Replace(")", ""));
            decimal nTotalPrice = 0;
            if (nQty > 0)
            {
                decimal nLaborRate = dlabor_rate * dretail_multiplier;
                nLaborRate = nLaborRate * nQty;

                if (ddlLabor.SelectedValue == "2")
                    nTotalPrice = nCost * nQty + nLaborRate;
                else
                    nTotalPrice = nCost * nQty;
            }
            else
            {
                nQty = Convert.ToDecimal(0.01);
                nCost = Convert.ToDecimal(0);
                nTotalPrice = nCost * nQty;
            }

            if (ddlCustomerLocations.SelectedItem.Text == "Select Location")
            {
                lblAdd.Text = csCommonUtility.GetSystemErrorMessage("Please select location.");
                lblSelectLocation.Text = csCommonUtility.GetSystemErrorMessage("Please select location.");
                ddlCustomerLocations.Focus();
                return;
            }
            sectioninfo sinfo = _db.sectioninfos.Single(s => s.section_id == Convert.ToInt32(hdnSectionId.Value) && s.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));
            hdnParentId.Value = Convert.ToInt32(sinfo.parent_id).ToString();
            hdnSectionLevel.Value = Convert.ToInt32(sinfo.section_level).ToString();
            hdnSectionSerial.Value = Convert.ToDecimal(sinfo.section_serial).ToString();
            userinfo obj = (userinfo)Session["oUser"];
            model_estimate_pricing price_detail = new model_estimate_pricing();


            price_detail.item_name = GetItemDetialsForUpdateItem(Convert.ToInt32(hdnSectionId.Value)).ToString();
            price_detail.measure_unit = smeasure_unit;
            price_detail.minimum_qty = dminimum_qty;
            price_detail.retail_multiplier = dretail_multiplier;
            price_detail.labor_rate = dlabor_rate;
            price_detail.item_cost = ditem_cost;

            price_detail.client_id = Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
            if (Convert.ToInt32(hdnSalesPersonId.Value) > 0)
                price_detail.sales_person_id = Convert.ToInt32(hdnSalesPersonId.Value);
            else
                price_detail.sales_person_id = obj.sales_person_id;
            price_detail.model_estimate_id = Convert.ToInt32(hdnEstimateId.Value);
            price_detail.location_id = Convert.ToInt32(ddlCustomerLocations.SelectedValue);
            price_detail.section_level = Convert.ToInt32(hdnSectionLevel.Value);
            price_detail.item_id = Convert.ToInt32(hdnSectionId.Value);
            price_detail.location_id = Convert.ToInt32(ddlCustomerLocations.SelectedValue);
            price_detail.section_name = GetSectionName(Convert.ToInt32(hdnSectionLevel.Value));
            price_detail.quantity = Convert.ToDecimal(txtQty.Text);
            price_detail.labor_id = Convert.ToInt32(ddlLabor.SelectedValue);
            price_detail.is_direct = Convert.ToInt32(ddlDirect.SelectedValue);

            if (Convert.ToInt32(ddlDirect.SelectedValue) == 1)
            {
                price_detail.total_retail_price = Convert.ToDecimal(nTotalPrice.ToString().Replace("$", "").Replace("(", "-").Replace(")", ""));
                price_detail.total_direct_price = 0;
            }
            else
            {
                price_detail.total_retail_price = 0;
                price_detail.total_direct_price = Convert.ToDecimal(nTotalPrice.ToString().Replace("$", "").Replace("(", "-").Replace(")", ""));
            }

            if (Convert.ToInt32(ddlLabor.SelectedValue) == 1)
                price_detail.labor_rate = 0;
            price_detail.section_serial = Convert.ToDecimal(hdnSectionSerial.Value);
            hdnItemCnt.Value = GetSerial().ToString();
            price_detail.item_cnt = Convert.ToInt32(hdnItemCnt.Value);
            price_detail.pricing_type = "A";
            price_detail.short_notes = txtShortNote.Text;
            price_detail.create_date = DateTime.Now;
            price_detail.last_updated_date = DateTime.Now;
            price_detail.sort_id = 0;


            _db.model_estimate_pricings.InsertOnSubmit(price_detail);
            _db.SubmitChanges();
            lblAdd.Text = csCommonUtility.GetSystemMessage("Item added to estimate list, select another item or Location/Section");
            hdnSortDesc.Value = "1";
            BindSelectedItemGrid();
            BindSelectedItemGrid_Direct();
            Calculate_Total();
            //Item updated successfully to estimate list, select another item or Location/Section

            if (grdGrouping.Rows.Count == 0)
            {
                lblRetailPricingHeader.Visible = false;
            }
            else
            {
                lblRetailPricingHeader.Visible = true;
            }
            if (grdGroupingDirect.Rows.Count == 0)
            {
                lblDirectPricingHeader.Visible = false;
            }
            else
            {
                lblDirectPricingHeader.Visible = true;
            }
        }
    }
    protected void grdSelectedItem_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblSelectLocation.Text = "";
        lblAdd.Text = "";
        lblMessage.Text = "";
        lblResult1.Text = "";

        GridView grdSelecterdItem = (GridView)sender;
        TextBox txtquantity = (TextBox)grdSelecterdItem.Rows[e.NewEditIndex].FindControl("txtquantity");
        Label lblquantity = (Label)grdSelecterdItem.Rows[e.NewEditIndex].FindControl("lblquantity");
        TextBox txtshort_notes = (TextBox)grdSelecterdItem.Rows[e.NewEditIndex].FindControl("txtshort_notes");
        Label lblshort_notes = (Label)grdSelecterdItem.Rows[e.NewEditIndex].FindControl("lblshort_notes");
        txtquantity.Visible = true;
        lblquantity.Visible = false;
        txtshort_notes.Visible = true;
        lblshort_notes.Visible = false;
        LinkButton btn = (LinkButton)grdSelecterdItem.Rows[e.NewEditIndex].Cells[10].Controls[0];
        btn.Text = "Update";
        btn.CommandName = "Update";


    }

    protected void grdSelectedItem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblshort_notes = (Label)e.Row.FindControl("lblshort_notes");
            TextBox txtshort_notes = (TextBox)e.Row.FindControl("txtshort_notes");
            string str = lblshort_notes.Text.Replace("&nbsp;", "");
            if (str != "" && str.Length > 50)
            {
                txtshort_notes.Text = str;
                lblshort_notes.Text = str.Substring(0, 50) + "...";
                lblshort_notes.ToolTip = str;

            }
            else
            {
                txtshort_notes.Text = str;
                lblshort_notes.Text = str;

            }

        }

    }
    protected void grdSelectedItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblSelectLocation.Text = "";
        lblAdd.Text = "";
        lblMessage.Text = "";
        lblResult1.Text = "";

        GridView grdSelectedItem = (GridView)sender;
        DataClassesDataContext _db = new DataClassesDataContext();
        hdnPricingId.Value = grdSelectedItem.DataKeys[e.RowIndex].Values[0].ToString();
        string strQ = "Delete model_estimate_pricing WHERE me_pricing_id =" + Convert.ToInt32(hdnPricingId.Value) + " AND client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
        _db.ExecuteCommand(strQ, string.Empty);
        hdnSortDesc.Value = "0";
        BindSelectedItemGrid();
        BindSelectedItemGrid_Direct();
        if (grdGrouping.Rows.Count == 0)
        {
            lblRetailPricingHeader.Visible = false;
        }
        else
        {
            lblRetailPricingHeader.Visible = true;
        }
        if (grdGroupingDirect.Rows.Count == 0)
        {
            lblDirectPricingHeader.Visible = false;
        }
        else
        {
            lblDirectPricingHeader.Visible = true;
        }
        Calculate_Total();
        hdnPricingId.Value = "0";
        lblResult1.Text = csCommonUtility.GetSystemMessage("Item deleted successfully");
        lblAdd.Text = csCommonUtility.GetSystemMessage("Item deleted successfully");

    }

    protected string GetTotalPrice()
    {
        return "Total: " + grandtotal.ToString("c");
    }
    protected string GetTotalPriceDirect()
    {
        return "Total: " + grandtotal_direct.ToString("c");
    }
    protected void grdGrouping_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            int colId = Convert.ToInt32(grdGrouping.DataKeys[e.Row.RowIndex].Values[0]);
            LinkButton lnkMove1 = (LinkButton)e.Row.FindControl("lnkMove1");
            if (rdoSort.SelectedValue == "1")
            {
                if (e.Row.RowIndex == 0)
                {
                    lnkMove1.Visible = false;
                }
                else
                {
                    lnkMove1.Visible = true;
                }
            }
            else
            {
                lnkMove1.Visible = false;

            }
            GridView gv = e.Row.FindControl("grdSelectedItem1") as GridView;
            int nDirectId = 1;
            GetData(colId, gv, nDirectId);
            GridViewRow footerRow = gv.FooterRow;
            GridViewRow headerRow = gv.HeaderRow;
            foreach (GridViewRow row in gv.Rows)
            {
                Label labelTotal = footerRow.FindControl("lblSubTotal") as Label;
                Label lblSubTotalLabel = footerRow.FindControl("lblSubTotalLabel") as Label;
                Label lblHeader = headerRow.FindControl("lblHeader") as Label;
                subtotal += Double.Parse((row.FindControl("lblTotal_price") as Label).Text.Replace("$", "").Replace("(", "-").Replace(")", ""));
                labelTotal.Text = subtotal.ToString("c");
                if (rdoSort.SelectedValue == "1")
                {
                    lblHeader.Text = "Section";
                }
                else
                {
                    lblHeader.Text = "Location";
                }
                lblSubTotalLabel.Text = "Sub Total:";
            }
            grandtotal += subtotal;
            subtotal = 0.0;
        }
    }
    private void GetData(int colId, GridView grd, int nDirectId)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        if (rdoSort.SelectedValue == "1")
        {
            var price_detail = from p in _db.model_estimate_pricings
                               join lc in _db.locations on p.location_id equals lc.location_id
                               where (from clc in _db.model_estimate_locations
                                      where clc.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && clc.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && clc.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                                      select clc.location_id).Contains(p.location_id) &&
                                      (from cs in _db.model_estimate_sections
                                       where cs.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && cs.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && cs.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                                       select cs.section_id).Contains(p.section_level) && p.location_id == colId && p.is_direct == nDirectId && p.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && p.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && p.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && p.pricing_type == "A"
                               orderby p.section_level ascending

                               select new PricingDetailModel()
                               {
                                   pricing_id = (int)p.me_pricing_id,
                                   item_id = (int)p.item_id,
                                   labor_id = (int)p.labor_id,
                                   section_serial = (decimal)p.section_serial,
                                   location_name = lc.location_name,
                                   section_name = p.section_name,
                                   item_name = p.item_name,
                                   measure_unit = p.measure_unit,
                                   item_cost = (decimal)p.item_cost,
                                   total_retail_price = (decimal)p.total_retail_price,
                                   total_direct_price = (decimal)p.total_direct_price,
                                   minimum_qty = (decimal)p.minimum_qty,
                                   quantity = (decimal)p.quantity,
                                   retail_multiplier = (decimal)p.retail_multiplier,
                                   labor_rate = (decimal)p.labor_rate,
                                   short_notes = p.short_notes,
                                   tmpCol = string.Empty,
                                   pricing_type = "A",
                                   last_update_date = (DateTime)p.last_updated_date
                               };
            if (hdnSortDesc.Value == "1")
                grd.DataSource = price_detail.ToList().OrderByDescending(c => c.last_update_date);
            else
                grd.DataSource = price_detail.ToList();
            grd.DataKeyNames = new string[] { "pricing_id" };
            grd.DataBind();
        }
        else
        {
            var price_detail = from p in _db.model_estimate_pricings
                               join lc in _db.locations on p.location_id equals lc.location_id
                               where (from clc in _db.model_estimate_locations
                                      where clc.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && clc.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && clc.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                                      select clc.location_id).Contains(p.location_id) &&
                                      (from cs in _db.model_estimate_sections
                                       where cs.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && cs.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && cs.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                                       select cs.section_id).Contains(p.section_level)
                                      && p.section_level == colId && p.is_direct == nDirectId && p.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && p.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && p.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && p.pricing_type == "A"
                               orderby lc.location_name ascending

                               select new PricingDetailModel()
                               {
                                   pricing_id = (int)p.me_pricing_id,
                                   item_id = (int)p.item_id,
                                   labor_id = (int)p.labor_id,
                                   section_serial = (decimal)p.section_serial,
                                   location_name = p.section_name,
                                   section_name = lc.location_name,
                                   item_name = p.item_name,
                                   measure_unit = p.measure_unit,
                                   item_cost = (decimal)p.item_cost,
                                   total_retail_price = (decimal)p.total_retail_price,
                                   total_direct_price = (decimal)p.total_direct_price,
                                   minimum_qty = (decimal)p.minimum_qty,
                                   quantity = (decimal)p.quantity,
                                   retail_multiplier = (decimal)p.retail_multiplier,
                                   labor_rate = (decimal)p.labor_rate,
                                   short_notes = p.short_notes,
                                   tmpCol = string.Empty,
                                   pricing_type = "A",
                                   last_update_date = (DateTime)p.last_updated_date
                               };
            if (hdnSortDesc.Value == "1")
                grd.DataSource = price_detail.ToList().OrderByDescending(c => c.last_update_date);
            else
                grd.DataSource = price_detail.ToList();
            grd.DataKeyNames = new string[] { "pricing_id" };
            grd.DataBind();
        }


    }
    protected void rdoSort_SelectedIndexChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, rdoSort.ID, rdoSort.GetType().Name, "SelectedIndexChanged"); 
        lblSelectLocation.Text = "";
        lblAdd.Text = "";
        lblMessage.Text = "";
        lblResult1.Text = "";
        hdnSortDesc.Value = "0";
        BindSelectedItemGrid();
        BindSelectedItemGrid_Direct();
        if (grdGrouping.Rows.Count == 0)
        {
            lblRetailPricingHeader.Visible = false;
        }
        else
        {
            lblRetailPricingHeader.Visible = true;
        }
        if (grdGroupingDirect.Rows.Count == 0)
        {
            lblDirectPricingHeader.Visible = false;
        }
        else
        {
            lblDirectPricingHeader.Visible = true;
        }
        Calculate_Total();
    }
    protected void grdSelectedItem2_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblSelectLocation.Text = "";
        lblAdd.Text = "";
        lblMessage.Text = "";
        lblResult1.Text = "";

        GridView grdSelectedItem2 = (GridView)sender;
        DataClassesDataContext _db = new DataClassesDataContext();
        hdnPricingId.Value = grdSelectedItem2.DataKeys[e.RowIndex].Values[0].ToString();
        string strQ = "Delete model_estimate_pricing WHERE me_pricing_id =" + Convert.ToInt32(hdnPricingId.Value) + " AND client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
        _db.ExecuteCommand(strQ, string.Empty);
        hdnSortDesc.Value = "0";
        BindSelectedItemGrid_Direct();
        BindSelectedItemGrid();
        if (grdGrouping.Rows.Count == 0)
        {
            lblRetailPricingHeader.Visible = false;
        }
        else
        {
            lblRetailPricingHeader.Visible = true;
        }
        if (grdGroupingDirect.Rows.Count == 0)
        {
            lblDirectPricingHeader.Visible = false;
        }
        else
        {
            lblDirectPricingHeader.Visible = true;
        }
        Calculate_Total();
        hdnPricingId.Value = "0";
        lblResult1.Text = csCommonUtility.GetSystemMessage("Item deleted successfully");
        lblAdd.Text = csCommonUtility.GetSystemMessage("Item deleted successfully");
    }

    protected void grdSelectedItem2_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblSelectLocation.Text = "";
        lblAdd.Text = "";
        lblMessage.Text = "";
        lblResult1.Text = "";

        GridView grdSelectedItem2 = (GridView)sender;
        TextBox txtquantity1 = (TextBox)grdSelectedItem2.Rows[e.NewEditIndex].FindControl("txtquantity1");
        Label lblquantity1 = (Label)grdSelectedItem2.Rows[e.NewEditIndex].FindControl("lblquantity1");
        TextBox txtshort_notes1 = (TextBox)grdSelectedItem2.Rows[e.NewEditIndex].FindControl("txtshort_notes1");
        Label lblshort_notes1 = (Label)grdSelectedItem2.Rows[e.NewEditIndex].FindControl("lblshort_notes1");
        txtquantity1.Visible = true;
        lblquantity1.Visible = false;
        txtshort_notes1.Visible = true;
        lblshort_notes1.Visible = false;
        LinkButton btn = (LinkButton)grdSelectedItem2.Rows[e.NewEditIndex].Cells[10].Controls[0];
        btn.Text = "Update";
        btn.CommandName = "Update";
    }
    protected void grdSelectedItem2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblshort_notes1 = (Label)e.Row.FindControl("lblshort_notes1");
            TextBox txtshort_notes1 = (TextBox)e.Row.FindControl("txtshort_notes1");
            string str = lblshort_notes1.Text.Replace("&nbsp;", "");
            if (str != "" && str.Length > 50)
            {
                txtshort_notes1.Text = str;
                lblshort_notes1.Text = str.Substring(0, 50) + "...";
                lblshort_notes1.ToolTip = str;

            }
            else
            {
                txtshort_notes1.Text = str;
                lblshort_notes1.Text = str;

            }

        }
    }
    protected void grdGroupingDirect_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            int colId = Convert.ToInt32(grdGroupingDirect.DataKeys[e.Row.RowIndex].Values[0]);
            int nDirectId = 2;
            LinkButton lnkMove2 = (LinkButton)e.Row.FindControl("lnkMove2");
            if (rdoSort.SelectedValue == "1")
            {
                if (e.Row.RowIndex == 0)
                {
                    lnkMove2.Visible = false;
                }
                else
                {
                    lnkMove2.Visible = true;

                }
            }
            else
            {
                lnkMove2.Visible = false;

            }

            GridView gv = e.Row.FindControl("grdSelectedItem2") as GridView;
            GetData(colId, gv, nDirectId);
            GridViewRow footerRow = gv.FooterRow;
            GridViewRow headerRow = gv.HeaderRow;
            foreach (GridViewRow row in gv.Rows)
            {
                Label labelTotal2 = footerRow.FindControl("lblSubTotal2") as Label;
                Label lblSubTotalLabel2 = footerRow.FindControl("lblSubTotalLabel2") as Label;
                Label lblHeader2 = headerRow.FindControl("lblHeader2") as Label;
                subtotal_diect += Double.Parse((row.FindControl("lblTotal_price2") as Label).Text.Replace("$", "").Replace("(", "-").Replace(")", ""));
                labelTotal2.Text = subtotal_diect.ToString("c");
                if (rdoSort.SelectedValue == "1")
                {
                    lblHeader2.Text = "Section";
                }
                else
                {
                    lblHeader2.Text = "Location";
                }
                lblSubTotalLabel2.Text = "Sub Total:";
            }
            grandtotal_direct += subtotal_diect;
            subtotal_diect = 0.0;
        }
    }
    protected void grdSelectedItem1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView gvr = (GridView)sender;
        GridRowUpdatingNonDirect(gvr, e.RowIndex);

        #region BlockCode
        //lblSelectLocation.Text = "";
        //lblAdd.Text = "";
        //lblMessage.Text = "";
        //lblResult1.Text = "";

        //GridView grdSelectedItem = (GridView)sender;
        //DataClassesDataContext _db = new DataClassesDataContext();

        //TextBox txtquantity = (TextBox)grdSelectedItem.Rows[e.RowIndex].FindControl("txtquantity");
        //Label lblTotal_price = (Label)grdSelectedItem.Rows[e.RowIndex].FindControl("lblTotal_price");
        //TextBox txtshort_notes = (TextBox)grdSelectedItem.Rows[e.RowIndex].FindControl("txtshort_notes");
        //int nPricingId = Convert.ToInt32(grdSelectedItem.DataKeys[Convert.ToInt32(e.RowIndex)].Values[0]);
        //decimal dTotalPrice = Convert.ToDecimal(lblTotal_price.Text.Replace("$", "").Replace("(", "-").Replace(")", ""));
        //string strQ = "UPDATE model_estimate_pricing SET quantity=" + Convert.ToDecimal(txtquantity.Text) + " ,total_retail_price=" + dTotalPrice + " ,short_notes='" + txtshort_notes.Text.Replace("'", "''") + "'  WHERE me_pricing_id =" + nPricingId + " AND client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
        //_db.ExecuteCommand(strQ, string.Empty);

        //subtotal = 0.0;
        //subtotal_diect = 0.0;
        //hdnSortDesc.Value = "0";
        //BindSelectedItemGrid();
        //BindSelectedItemGrid_Direct();
        //if (grdGrouping.Rows.Count == 0)
        //{
        //    lblRetailPricingHeader.Visible = false;
        //}
        //else
        //{
        //    lblRetailPricingHeader.Visible = true;
        //}
        //if (grdGroupingDirect.Rows.Count == 0)
        //{
        //    lblDirectPricingHeader.Visible = false;
        //}
        //else
        //{
        //    lblDirectPricingHeader.Visible = true;
        //}
        //hdnPricingId.Value = "0";
        //Calculate_Total();
        //lblResult1.Text = "Item updated successfully";
        //lblResult1.ForeColor = System.Drawing.Color.Green;
        //lblAdd.Text = "Item updated successfully";
        //lblAdd.ForeColor = System.Drawing.Color.Green;
        #endregion

    }

    private void GridRowUpdatingNonDirect(GridView gvr, int nIndex)
    {
        if (nIndex < 0) return;

        lblSelectLocation.Text = "";
        lblAdd.Text = "";
        lblMessage.Text = "";
        lblResult1.Text = "";

        DataClassesDataContext _db = new DataClassesDataContext();

        TextBox txtquantity = (TextBox)gvr.Rows[nIndex].FindControl("txtquantity");
        Label lblTotal_price = (Label)gvr.Rows[nIndex].FindControl("lblTotal_price");
        TextBox txtshort_notes = (TextBox)gvr.Rows[nIndex].FindControl("txtshort_notes");
        int nPricingId = Convert.ToInt32(gvr.DataKeys[Convert.ToInt32(nIndex)].Values[0]);
        decimal dTotalPrice = 0;
        try
        {
            dTotalPrice = dTotalPrice = Convert.ToDecimal(lblTotal_price.Text.Replace("$", "").Replace("(", "-").Replace(")", ""));
        }
        catch
        {
            dTotalPrice = 0;

        }

        decimal nQty = 0;
        try
        {
            nQty = Convert.ToDecimal(txtquantity.Text);
        }
        catch
        {
            nQty = 1;

        }
        string strQ = "UPDATE model_estimate_pricing SET quantity=" + nQty + " ,total_retail_price=" + dTotalPrice + " ,short_notes='" + txtshort_notes.Text.Replace("'", "''") + "' WHERE me_pricing_id =" + nPricingId + " AND client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
        _db.ExecuteCommand(strQ, string.Empty);


        subtotal = 0.0;
        subtotal_diect = 0.0;
        hdnSortDesc.Value = "0";
        BindSelectedItemGrid();
        BindSelectedItemGrid_Direct();
        if (grdGrouping.Rows.Count == 0)
        {
            lblRetailPricingHeader.Visible = false;
        }
        else
        {
            lblRetailPricingHeader.Visible = true;
        }
        if (grdGroupingDirect.Rows.Count == 0)
        {
            lblDirectPricingHeader.Visible = false;
        }
        else
        {
            lblDirectPricingHeader.Visible = true;
        }
        hdnPricingId.Value = "0";
        Calculate_Total();
        lblResult1.Text = csCommonUtility.GetSystemMessage("Item updated successfully");
        lblAdd.Text = csCommonUtility.GetSystemMessage("Item updated successfully");


    }


    protected void grdSelectedItem2_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView gvr = (GridView)sender;
        GridRowUpdatingDirect(gvr, e.RowIndex);
        #region BlockCode
        //lblSelectLocation.Text = "";
        //lblAdd.Text = "";
        //lblMessage.Text = "";
        //lblResult1.Text = "";

        //GridView grdSelectedItem2 = (GridView)sender;


        //DataClassesDataContext _db = new DataClassesDataContext();
        //TextBox txtquantity1 = (TextBox)grdSelectedItem2.Rows[e.RowIndex].FindControl("txtquantity1");
        //Label lblTotal_price2 = (Label)grdSelectedItem2.Rows[e.RowIndex].FindControl("lblTotal_price2");
        //TextBox txtshort_notes1 = (TextBox)grdSelectedItem2.Rows[e.RowIndex].FindControl("txtshort_notes1");
        //int nPricingId = Convert.ToInt32(grdSelectedItem2.DataKeys[Convert.ToInt32(e.RowIndex)].Values[0]);
        //decimal dTotalDirectPrice = Convert.ToDecimal(lblTotal_price2.Text.Replace("$", "").Replace("(", "-").Replace(")", ""));
        //string strQ = "UPDATE model_estimate_pricing SET quantity=" + Convert.ToDecimal(txtquantity1.Text) + " ,total_direct_price=" +dTotalDirectPrice + " ,short_notes='" + txtshort_notes1.Text.Replace("'", "''") + "'  WHERE me_pricing_id =" + nPricingId + " AND client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
        //_db.ExecuteCommand(strQ, string.Empty);

        //subtotal = 0.0;
        //subtotal_diect = 0.0;
        //hdnSortDesc.Value = "0";
        //BindSelectedItemGrid();
        //BindSelectedItemGrid_Direct();
        //if (grdGrouping.Rows.Count == 0)
        //{
        //    lblRetailPricingHeader.Visible = false;
        //}
        //else
        //{
        //    lblRetailPricingHeader.Visible = true;
        //}
        //if (grdGroupingDirect.Rows.Count == 0)
        //{
        //    lblDirectPricingHeader.Visible = false;
        //}
        //else
        //{
        //    lblDirectPricingHeader.Visible = true;
        //}
        //hdnPricingId.Value = "0";
        //Calculate_Total();
        //lblResult1.Text = "Item updated successfully";
        //lblResult1.ForeColor = System.Drawing.Color.Green;
        //lblAdd.Text = "Item updated successfully";
        //lblAdd.ForeColor = System.Drawing.Color.Green;
        #endregion

    }

    private void GridRowUpdatingDirect(GridView gvr, int nIndex)
    {
        if (nIndex < 0) return;

        lblSelectLocation.Text = "";
        lblAdd.Text = "";
        lblMessage.Text = "";
        lblResult1.Text = "";

        DataClassesDataContext _db = new DataClassesDataContext();
        TextBox txtquantity1 = (TextBox)gvr.Rows[nIndex].FindControl("txtquantity1");
        Label lblTotal_price2 = (Label)gvr.Rows[nIndex].FindControl("lblTotal_price2");
        TextBox txtshort_notes1 = (TextBox)gvr.Rows[nIndex].FindControl("txtshort_notes1");
        int nPricingId = Convert.ToInt32(gvr.DataKeys[Convert.ToInt32(nIndex)].Values[0]);

        decimal dTotalDirectPrice = 0;
        try
        {
            dTotalDirectPrice = Convert.ToDecimal(lblTotal_price2.Text.Replace("$", "").Replace("(", "-").Replace(")", ""));
        }
        catch
        {
            dTotalDirectPrice = 0;

        }

        decimal nQty = 0;
        try
        {
            nQty = Convert.ToDecimal(txtquantity1.Text);
        }
        catch
        {
            nQty = 1;

        }
        string strQ = "UPDATE model_estimate_pricing SET quantity=" + nQty + " ,total_direct_price=" + dTotalDirectPrice + " ,short_notes='" + txtshort_notes1.Text.Replace("'", "''") + "' WHERE me_pricing_id =" + nPricingId + " AND client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
        _db.ExecuteCommand(strQ, string.Empty);


        subtotal = 0.0;
        subtotal_diect = 0.0;
        hdnSortDesc.Value = "0";
        BindSelectedItemGrid();
        BindSelectedItemGrid_Direct();
        if (grdGrouping.Rows.Count == 0)
        {
            lblRetailPricingHeader.Visible = false;
        }
        else
        {
            lblRetailPricingHeader.Visible = true;
        }
        if (grdGroupingDirect.Rows.Count == 0)
        {
            lblDirectPricingHeader.Visible = false;
        }
        else
        {
            lblDirectPricingHeader.Visible = true;
        }
        hdnPricingId.Value = "0";
        Calculate_Total();

        lblResult1.Text = csCommonUtility.GetSystemMessage("Item updated successfully");
        lblAdd.Text = csCommonUtility.GetSystemMessage("Item updated successfully");


    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSave.ID, btnSave.GetType().Name, "Click"); 
        lblSelectLocation.Text = "";
        lblAdd.Text = "";
        lblMessage.Text = "";
        lblResult1.Text = "";

        model_estimate obj = new model_estimate();
        DataClassesDataContext _db = new DataClassesDataContext();
        obj = _db.model_estimates.Single(me => me.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value) && me.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && me.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));
        obj.estimate_comments = txtComments.Text;
        obj.IsPublic = Convert.ToBoolean(chkIsPublic.Checked);
        obj.last_udated_date = DateTime.Now;
        _db.SubmitChanges();

        lblResult1.Text = csCommonUtility.GetSystemMessage(lblModelEstimateName.Text + " updated successfully.");

    }
    protected void btnAssignToCustomer_Click(object sender, EventArgs e)
    {
        Response.Redirect("assigntoacustomer.aspx?meid=" + hdnEstimateId.Value + "&spid=" + hdnSalesPersonId.Value);
    }


    protected void btnAddOthers_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnAddOthers.ID, btnAddOthers.GetType().Name, "Click"); 
        lblSelectLocation.Text = "";
        lblAdd.Text = "";
        lblMessage.Text = "";
        lblResult1.Text = "";

        DataClassesDataContext _db = new DataClassesDataContext();

        if (ddlCustomerLocations.SelectedItem.Text == "Select Location")
        {
            lblAdd.Text = csCommonUtility.GetSystemErrorMessage("Please select location.");
            lblSelectLocation.Text = csCommonUtility.GetSystemErrorMessage("Please select location.");
            ddlCustomerLocations.Focus();
            return;
        }
        decimal nOtherQty = 1;
        decimal nOtherUnitPrice = 0;

        try
        {
            nOtherQty = Convert.ToDecimal(txtO_Qty.Text.Trim());
        }
        catch (Exception ex)
        {

            nOtherQty = 1;

        }
        if (nOtherQty < 0)
        {
            lblAdd.Text = csCommonUtility.GetSystemErrorMessage("Negative numbers are not allowed in Code.");
            txtO_Qty.Focus();
            return;
        }
        if (nOtherQty == 0)
        {
            lblAdd.Text = csCommonUtility.GetSystemErrorMessage("Code should be greater than zero.");
            txtO_Qty.Focus();
            return;
        }
        try
        {
            nOtherUnitPrice = Convert.ToDecimal(txtO_Price.Text.Replace("(", "-").Replace(")", "").Replace("$", "").Trim());
        }
        catch (Exception ex)
        {

            nOtherUnitPrice = 0;

        }
        decimal Other_TotalPrice = nOtherUnitPrice * nOtherQty;

        sectioninfo sinfo = _db.sectioninfos.Single(s => s.section_id == Convert.ToInt32(hdnSectionId.Value) && s.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));
        hdnParentId.Value = Convert.ToInt32(sinfo.parent_id).ToString();
        hdnSectionLevel.Value = Convert.ToInt32(sinfo.section_level).ToString();
        hdnSectionSerial.Value = Convert.ToDecimal(sinfo.section_serial).ToString();

        userinfo obj = (userinfo)Session["oUser"];
        model_estimate_pricing price_detail = new model_estimate_pricing();

        price_detail.item_name = GetItemDetialsForUpdateItem(Convert.ToInt32(hdnSectionId.Value)).ToString() + "" + txtOther.Text + ">>Other";
        price_detail.measure_unit = txtO_Unit.Text;
        price_detail.minimum_qty = 1;
        price_detail.retail_multiplier = 0;
        price_detail.labor_rate = 0;
        price_detail.item_cost = Convert.ToDecimal(txtO_Price.Text.Replace("$", "").Replace("(", "-").Replace(")", ""));
        price_detail.client_id = Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
        //price_detail.customer_id = Convert.ToInt32(hdnCustomerId.Value);
        price_detail.model_estimate_id = Convert.ToInt32(hdnEstimateId.Value);
        price_detail.location_id = Convert.ToInt32(ddlCustomerLocations.SelectedValue);
        if (Convert.ToInt32(hdnSalesPersonId.Value) > 0)
            price_detail.sales_person_id = Convert.ToInt32(hdnSalesPersonId.Value);
        else
            price_detail.sales_person_id = obj.sales_person_id;
        price_detail.section_level = Convert.ToInt32(hdnSectionLevel.Value);
        price_detail.item_id = Convert.ToInt32(hdnOtherId.Value);
        price_detail.section_name = GetSectionName(Convert.ToInt32(hdnSectionLevel.Value));
        price_detail.quantity = Convert.ToDecimal(txtO_Qty.Text);
        price_detail.labor_id = 1;
        price_detail.is_direct = Convert.ToInt32(ddlO_Direct.SelectedValue);

        if (Convert.ToInt32(ddlO_Direct.SelectedValue) == 1)
        {
            price_detail.total_retail_price = Other_TotalPrice;
            price_detail.total_direct_price = 0;
        }
        else
        {
            price_detail.total_retail_price = 0;
            price_detail.total_direct_price = Other_TotalPrice;
        }
        hdnItemCnt.Value = GetSerial().ToString();
        string strOtherCount = GetSerial_other(Convert.ToInt32(price_detail.item_id)).ToString();
        price_detail.item_cnt = Convert.ToInt32(hdnItemCnt.Value);
        price_detail.other_item_cnt = Convert.ToInt32(strOtherCount);
        string str = "0";
        if (Convert.ToInt32(strOtherCount) > 9)
            str = price_detail.item_id + "." + strOtherCount;
        else
            str = price_detail.item_id + ".0" + strOtherCount;
        price_detail.section_serial = Convert.ToDecimal(str);
        price_detail.short_notes = txtO_ShortNotes.Text;
        price_detail.create_date = DateTime.Now;
        price_detail.last_updated_date = DateTime.Now;
        price_detail.pricing_type = "A";
        price_detail.sort_id = 0;


        _db.model_estimate_pricings.InsertOnSubmit(price_detail);
        _db.SubmitChanges();
        lblResult1.Text = csCommonUtility.GetSystemMessage("Other Item added to Model Estimate, select another item or Location/Section");
        lblAdd.Text = csCommonUtility.GetSystemMessage("Other Item added to Model Estimate, select another item or Location/Section");
        txtO_Price.Text = "";
        txtO_Qty.Text = "";
        txtO_Unit.Text = "";
        txtOther.Text = "";
        txtO_ShortNotes.Text = "";
        hdnSortDesc.Value = "1";
        BindSelectedItemGrid();
        BindSelectedItemGrid_Direct();

    }

    protected void ItemPrice_calculation(object sender, EventArgs e)
    {
        //foreach (GridViewRow di in grdItemPrice.Rows)
        //{
        //    Label lblTotalPrice = (Label)di.FindControl("lblTotalPrice");
        //    TextBox txtQty = (TextBox)di.FindControl("txtQty");
        //    DropDownList ddlLabor = (DropDownList)di.FindControl("ddlLabor");
        //    DropDownList ddlDirect = (DropDownList)di.FindControl("ddlDirect");

        //    if (txtQty.Text.Trim() == "")
        //    {
        //        lblResult1.Text = "Missing  Quantity.";

        //        return;
        //    }
        //    else
        //    {
        //        try
        //        {
        //            Convert.ToDecimal(txtQty.Text.Trim());
        //        }
        //        catch (Exception ex)
        //        {
        //            lblResult1.Text = "Invalid  quantity.";

        //            return;
        //        }
        //    }
        //    if (Convert.ToDecimal(di.Cells[6].Text) > Convert.ToDecimal(txtQty.Text))
        //    {
        //        lblResult1.Text = "Qty should be greater than minimum value";

        //        return;
        //    }
        //    if (Convert.ToDecimal(di.Cells[6].Text) <= Convert.ToDecimal(txtQty.Text))
        //    {
        //        //decimal nCost = Convert.ToDecimal(di.Cells[4].Text.Replace("$", ""));
        //        decimal nCost = 0;
        //        decimal nQty = Convert.ToDecimal(txtQty.Text);
        //        DataClassesDataContext _db = new DataClassesDataContext();
        //        item_price itm = _db.item_prices.Single(it => it.item_id == Convert.ToInt32(di.Cells[1].Text) && it.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));
        //        decimal nLaborRate = (decimal)itm.labor_rate * (decimal)itm.retail_multiplier;

        //        nCost = (decimal)itm.item_cost * (decimal)itm.retail_multiplier;
        //        nLaborRate = nLaborRate * nQty;
        //        decimal nTotalPrice = 0;
        //        if (ddlLabor.SelectedValue == "2")
        //            nTotalPrice = nCost * nQty + nLaborRate;
        //        else
        //            nTotalPrice = nCost * nQty;
        //        lblTotalPrice.Text = nTotalPrice.ToString("c");
        //    }

        //}

        string strSender = sender.ToString();
        int i = 0;
        if (strSender.IndexOf("TextBox") != -1)
        {
            TextBox txtQty1 = (TextBox)grdItemPrice.FindControl("txtQty");
            txtQty1 = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)txtQty1.NamingContainer;
            i = gvr.RowIndex;
        }

        DropDownList ddlLabor = (DropDownList)grdItemPrice.Rows[i].FindControl("ddlLabor");
        Label lblTotalPrice = (Label)grdItemPrice.Rows[i].FindControl("lblTotalPrice");
        TextBox txtQty = (TextBox)grdItemPrice.Rows[i].FindControl("txtQty");
        DropDownList ddlDirect = (DropDownList)grdItemPrice.Rows[i].FindControl("ddlDirect");
        TextBox txtShortNote = (TextBox)grdItemPrice.Rows[i].FindControl("txtShortNote");
        if (txtQty.Text.Trim() == "")
        {
            lblResult1.Text = csCommonUtility.GetSystemRequiredMessage("Missing  Quantity.");
            lblAdd.Text = csCommonUtility.GetSystemRequiredMessage("Missing  Quantity.");
            return;
        }
        else
        {
            try
            {
                Convert.ToDecimal(txtQty.Text.Trim());
            }
            catch (Exception ex)
            {
                lblResult1.Text = csCommonUtility.GetSystemRequiredMessage("Invalid  quantity.");
                lblAdd.Text = csCommonUtility.GetSystemRequiredMessage("Invalid  quantity.");
                return;
            }
        }

        decimal nQty = Convert.ToDecimal(txtQty.Text);
        decimal nCost = Convert.ToDecimal(grdItemPrice.Rows[i].Cells[8].Text.Replace("$", "").Replace("(", "-").Replace(")", ""));
        decimal nTotalPrice = 0;
        if (nQty > 0)
        {
            decimal dlabor_rate = Convert.ToDecimal(grdItemPrice.DataKeys[i].Values[1].ToString());
            decimal dretail_multiplier = Convert.ToDecimal(grdItemPrice.DataKeys[i].Values[2].ToString());
            decimal nLaborRate = dlabor_rate * dretail_multiplier;
            nLaborRate = nLaborRate * nQty;

            if (ddlLabor.SelectedValue == "2")
                nTotalPrice = nCost * nQty + nLaborRate;
            else
                nTotalPrice = nCost * nQty;
            lblTotalPrice.Text = nTotalPrice.ToString("c");
        }
        else
        {
            nQty = Convert.ToDecimal(0.01);
            nCost = Convert.ToDecimal(0);
            nTotalPrice = nCost * nQty;
            lblTotalPrice.Text = nTotalPrice.ToString("c");

        }

        lblSelectLocation.Text = "";
        lblResult1.Text = "";
        lblMessage.Text = "";
        lblAdd.Text = "";
        string strPriceId = "0";
        decimal ditem_cost = 0;
        decimal labor_rate = 0;
        decimal retail_multiplier = 0;
        string smeasure_unit = "";
        decimal dminimum_qty = 0;

        DataClassesDataContext _db = new DataClassesDataContext();
        hdnSectionId.Value = grdItemPrice.Rows[i].Cells[1].Text;

        strPriceId = grdItemPrice.DataKeys[i].Values[0].ToString();


        labor_rate = Convert.ToDecimal(grdItemPrice.DataKeys[i].Values[1].ToString());
        retail_multiplier = Convert.ToDecimal(grdItemPrice.DataKeys[i].Values[2].ToString());
        smeasure_unit = grdItemPrice.Rows[i].Cells[7].Text;
        ditem_cost = Convert.ToDecimal(grdItemPrice.Rows[i].Cells[8].Text.Replace("$", "").Replace("(", "-").Replace(")", ""));
        dminimum_qty = Convert.ToDecimal(grdItemPrice.Rows[i].Cells[9].Text);


        if (ddlCustomerLocations.SelectedItem.Text == "Select Location")
        {
            lblAdd.Text = csCommonUtility.GetSystemErrorMessage("Please select location.");
            lblSelectLocation.Text = csCommonUtility.GetSystemErrorMessage("Please select location.");
            lblSelectLocation.Font.Bold = true;
            ddlCustomerLocations.Focus();
            return;
        }
        sectioninfo sinfo = _db.sectioninfos.Single(s => s.section_id == Convert.ToInt32(hdnSectionId.Value) && s.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));
        hdnParentId.Value = Convert.ToInt32(sinfo.parent_id).ToString();
        hdnSectionLevel.Value = Convert.ToInt32(sinfo.section_level).ToString();
        hdnSectionSerial.Value = Convert.ToDecimal(sinfo.section_serial).ToString();
        userinfo obj = (userinfo)Session["oUser"];
        model_estimate_pricing price_detail = new model_estimate_pricing();


        price_detail.item_name = GetItemDetialsForUpdateItem(Convert.ToInt32(hdnSectionId.Value)).ToString();
        price_detail.measure_unit = smeasure_unit;
        price_detail.minimum_qty = dminimum_qty;
        price_detail.retail_multiplier = retail_multiplier;
        price_detail.labor_rate = labor_rate;
        price_detail.item_cost = ditem_cost;

        price_detail.client_id = Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
        if (Convert.ToInt32(hdnSalesPersonId.Value) > 0)
            price_detail.sales_person_id = Convert.ToInt32(hdnSalesPersonId.Value);
        else
            price_detail.sales_person_id = obj.sales_person_id;
        price_detail.model_estimate_id = Convert.ToInt32(hdnEstimateId.Value);
        price_detail.location_id = Convert.ToInt32(ddlCustomerLocations.SelectedValue);
        price_detail.section_level = Convert.ToInt32(hdnSectionLevel.Value);
        price_detail.item_id = Convert.ToInt32(hdnSectionId.Value);
        price_detail.location_id = Convert.ToInt32(ddlCustomerLocations.SelectedValue);
        price_detail.section_name = GetSectionName(Convert.ToInt32(hdnSectionLevel.Value));
        price_detail.quantity = Convert.ToDecimal(txtQty.Text);
        price_detail.labor_id = Convert.ToInt32(ddlLabor.SelectedValue);
        price_detail.is_direct = Convert.ToInt32(ddlDirect.SelectedValue);

        if (Convert.ToInt32(ddlDirect.SelectedValue) == 1)
        {
            price_detail.total_retail_price = Convert.ToDecimal(lblTotalPrice.Text.Replace("$", "").Replace("(", "-").Replace(")", ""));
            price_detail.total_direct_price = 0;
        }
        else
        {
            price_detail.total_retail_price = 0;
            price_detail.total_direct_price = Convert.ToDecimal(lblTotalPrice.Text.Replace("$", "").Replace("(", "-").Replace(")", ""));
        }

        if (Convert.ToInt32(ddlLabor.SelectedValue) == 1)
            price_detail.labor_rate = 0;
        price_detail.section_serial = Convert.ToDecimal(hdnSectionSerial.Value);
        hdnItemCnt.Value = GetSerial().ToString();
        price_detail.item_cnt = Convert.ToInt32(hdnItemCnt.Value);
        price_detail.pricing_type = "A";
        price_detail.short_notes = txtShortNote.Text;
        price_detail.create_date = DateTime.Now;
        price_detail.last_updated_date = DateTime.Now;
        price_detail.sort_id = 0;


        _db.model_estimate_pricings.InsertOnSubmit(price_detail);
        _db.SubmitChanges();
        lblAdd.Text = csCommonUtility.GetSystemMessage("Item added to estimate list, select another item or Location/Section");
        hdnSortDesc.Value = "1";
        BindSelectedItemGrid();
        BindSelectedItemGrid_Direct();
        Calculate_Total();
        //Item updated successfully to estimate list, select another item or Location/Section

        if (grdGrouping.Rows.Count == 0)
        {
            lblRetailPricingHeader.Visible = false;
        }
        else
        {
            lblRetailPricingHeader.Visible = true;
        }
        if (grdGroupingDirect.Rows.Count == 0)
        {
            lblDirectPricingHeader.Visible = false;
        }
        else
        {
            lblDirectPricingHeader.Visible = true;
        }

    }

    protected void NonDirect_calculation(object sender, EventArgs e)
    {
        GridView grvFind = null;
        TextBox txt = (TextBox)sender;
        int nIndex = -1;
        foreach (GridViewRow dimaster1 in grdGrouping.Rows)
        {
            int i = -1;
            GridView grdSelectedItem1 = (GridView)dimaster1.FindControl("grdSelectedItem1");
            foreach (GridViewRow diitem in grdSelectedItem1.Rows)
            {
                i++;
                TextBox txtquantity = (TextBox)diitem.FindControl("txtquantity");
                Label lblquantity = (Label)diitem.FindControl("lblquantity");
                Label lblTotal_price = (Label)diitem.FindControl("lblTotal_price");


                if (txtquantity.Text.Trim() == "")
                {
                    lblResult1.Text = csCommonUtility.GetSystemRequiredMessage("Missing  Quantity.");
                    lblAdd.Text = csCommonUtility.GetSystemRequiredMessage("Missing  Quantity.");
                    return;
                }
                else
                {
                    try
                    {
                        Convert.ToDecimal(txtquantity.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        lblResult1.Text = csCommonUtility.GetSystemRequiredMessage("Invalid  quantity.");
                        lblAdd.Text = csCommonUtility.GetSystemRequiredMessage("Invalid  quantity.");
                        return;
                    }
                }
                if (txtquantity == txt)
                {
                    nIndex = i;
                    grvFind = grdSelectedItem1;
                    DataClassesDataContext _db = new DataClassesDataContext();
                    hdnPricingId.Value = grdSelectedItem1.DataKeys[diitem.RowIndex].Values[0].ToString();
                    int LaborId = 1;
                    decimal nCost1 = 0;
                    if (_db.model_estimate_pricings.Where(ep => ep.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && ep.me_pricing_id == Convert.ToInt32(hdnPricingId.Value) && ep.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])).SingleOrDefault() != null)
                    {
                        model_estimate_pricing pd = _db.model_estimate_pricings.Single(p => p.item_id == Convert.ToInt32(diitem.Cells[0].Text) && p.me_pricing_id == Convert.ToInt32(hdnPricingId.Value) && p.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));
                        LaborId = Convert.ToInt32(pd.labor_id);
                        nCost1 = Convert.ToDecimal(pd.item_cost);
                    }

                    decimal nQty1 = Convert.ToDecimal(txtquantity.Text);
                    decimal nLaborRate = 0;
                    decimal nTotalPrice1 = 0;

                    if (LaborId == 2)
                    {
                        item_price itm = _db.item_prices.Single(it => it.item_id == Convert.ToInt32(diitem.Cells[0].Text) && it.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));
                        nLaborRate = (decimal)itm.labor_rate * (decimal)itm.retail_multiplier;
                        nLaborRate = nLaborRate * nQty1;
                    }

                    nTotalPrice1 = nCost1 * nQty1 + nLaborRate;
                    lblTotal_price.Text = nTotalPrice1.ToString("c");
                }


                if (nIndex > -1)
                    break;

            }
            if (nIndex > -1)
                break;
        }
        GridRowUpdatingNonDirect(grvFind, nIndex);
    }

    protected void Direct_calculation(object sender, EventArgs e)
    {
        GridView grvFind = null;
        TextBox txt = (TextBox)sender;
        int nIndex = -1;
        foreach (GridViewRow dimaster2 in grdGroupingDirect.Rows)
        {
            int i = -1;
            GridView grdSelectedItem2 = (GridView)dimaster2.FindControl("grdSelectedItem2");
            foreach (GridViewRow diitem2 in grdSelectedItem2.Rows)
            {
                i++;
                TextBox txtquantity1 = (TextBox)diitem2.FindControl("txtquantity1");
                Label lblquantity1 = (Label)diitem2.FindControl("lblquantity1");
                Label lblTotal_price2 = (Label)diitem2.FindControl("lblTotal_price2");


                if (txtquantity1.Text.Trim() == "")
                {
                    lblResult1.Text = csCommonUtility.GetSystemRequiredMessage("Missing  Quantity.");
                    return;
                }
                else
                {
                    try
                    {
                        Convert.ToDecimal(txtquantity1.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        lblResult1.Text = csCommonUtility.GetSystemRequiredMessage("Invalid  quantity.");
                        lblAdd.Text = csCommonUtility.GetSystemRequiredMessage("Invalid  quantity.");
                        return;
                    }
                }

                if (txtquantity1 == txt)
                {
                    nIndex = i;
                    grvFind = grdSelectedItem2;
                    DataClassesDataContext _db = new DataClassesDataContext();
                    hdnPricingId.Value = grdSelectedItem2.DataKeys[diitem2.RowIndex].Values[0].ToString();
                    int LaborId2 = 1;
                    decimal nLaborRate2 = 0;
                    //decimal nCost2 = Convert.ToDecimal(diitem2.Cells[5].Text.Replace("$", ""));
                    decimal nCost2 = 0;
                    decimal nQty2 = Convert.ToDecimal(txtquantity1.Text);
                    decimal nTotalPrice2 = 0;
                    if (_db.model_estimate_pricings.Where(ep => ep.model_estimate_id == Convert.ToInt32(hdnEstimateId.Value) && ep.me_pricing_id == Convert.ToInt32(hdnPricingId.Value) && ep.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])).SingleOrDefault() != null)
                    {
                        model_estimate_pricing pd = _db.model_estimate_pricings.Single(p => p.item_id == Convert.ToInt32(diitem2.Cells[0].Text) && p.me_pricing_id == Convert.ToInt32(hdnPricingId.Value) && p.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));
                        LaborId2 = Convert.ToInt32(pd.labor_id);
                        nCost2 = Convert.ToDecimal(pd.item_cost);
                    }
                    if (LaborId2 == 2)
                    {
                        item_price itm = _db.item_prices.Single(it => it.item_id == Convert.ToInt32(diitem2.Cells[0].Text) && it.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));
                        nLaborRate2 = (decimal)itm.labor_rate * (decimal)itm.retail_multiplier;
                        nLaborRate2 = nLaborRate2 * nQty2;
                    }

                    nTotalPrice2 = nCost2 * nQty2 + nLaborRate2;
                    lblTotal_price2.Text = nTotalPrice2.ToString("c");

                }

                if (nIndex > -1)
                    break;
            }
            if (nIndex > -1)
                break;
        }
        GridRowUpdatingDirect(grvFind, nIndex);
    }

    protected void grdGrouping_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Move")
        {
            DataClassesDataContext _db = new DataClassesDataContext();

            int index = Convert.ToInt32(e.CommandArgument);
            foreach (GridViewRow dimaster1 in grdGrouping.Rows)
            {
                int serial = dimaster1.RowIndex + 1;
                int LocationId = Convert.ToInt32(grdGrouping.DataKeys[dimaster1.RowIndex].Values[0]);
                string strUpdate = " UPDATE model_estimate_pricing SET sort_id =" + serial + "  WHERE  model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND location_id=" + LocationId + " AND is_direct=1";
                _db.ExecuteCommand(strUpdate, string.Empty);

            }
            int nLocId = Convert.ToInt32(grdGrouping.DataKeys[index].Values[0]);
            string Strq = " UPDATE model_estimate_pricing SET sort_id = 0  WHERE  model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND location_id=" + nLocId + " AND is_direct=1";
            _db.ExecuteCommand(Strq, string.Empty);
            hdnSortDesc.Value = "0";
            BindSelectedItemGrid();
        }

    }
    protected void grdGroupingDirect_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Move1")
        {
            DataClassesDataContext _db = new DataClassesDataContext();

            int index = Convert.ToInt32(e.CommandArgument);
            foreach (GridViewRow dimaster2 in grdGroupingDirect.Rows)
            {
                int serial = dimaster2.RowIndex + 1;
                int LocationId = Convert.ToInt32(grdGroupingDirect.DataKeys[dimaster2.RowIndex].Values[0]);
                string strUpdate = " UPDATE model_estimate_pricing SET sort_id =" + serial + "  WHERE  model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND location_id=" + LocationId + " AND is_direct=2";
                _db.ExecuteCommand(strUpdate, string.Empty);

            }
            int nLocId = Convert.ToInt32(grdGroupingDirect.DataKeys[index].Values[0]);
            string Strq = " UPDATE model_estimate_pricing SET sort_id = 0  WHERE model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND location_id=" + nLocId + " AND is_direct=2";
            _db.ExecuteCommand(Strq, string.Empty);
            hdnSortDesc.Value = "0";
            BindSelectedItemGrid_Direct();
        }

    }
    protected void ddlCustomerLocations_SelectedIndexChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, ddlCustomerLocations.ID, ddlCustomerLocations.GetType().Name, "SelectedIndexChanged"); 
        if (ddlCustomerLocations.SelectedItem.Text != "Select Location")
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            foreach (GridViewRow dimaster1 in grdGrouping.Rows)
            {
                int serial = dimaster1.RowIndex + 1;
                int LocationId = Convert.ToInt32(grdGrouping.DataKeys[dimaster1.RowIndex].Values[0]);
                string strUpdate = " UPDATE model_estimate_pricing SET sort_id =" + serial + "  WHERE  model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND location_id=" + LocationId + " AND is_direct=1";
                _db.ExecuteCommand(strUpdate, string.Empty);

            }
            foreach (GridViewRow dimaster2 in grdGroupingDirect.Rows)
            {
                int serial = dimaster2.RowIndex + 1;
                int LocationId = Convert.ToInt32(grdGroupingDirect.DataKeys[dimaster2.RowIndex].Values[0]);
                string strUpdate = " UPDATE model_estimate_pricing SET sort_id =" + serial + "  WHERE  model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + "  AND sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND location_id=" + LocationId + " AND is_direct=2";
                _db.ExecuteCommand(strUpdate, string.Empty);

            }
            int nLocId = Convert.ToInt32(ddlCustomerLocations.SelectedValue);
            string Strq = " UPDATE model_estimate_pricing SET sort_id = 0  WHERE  model_estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND sales_person_id =" + Convert.ToInt32(hdnSalesPersonId.Value) + " AND location_id=" + nLocId;
            _db.ExecuteCommand(Strq, string.Empty);
            hdnSortDesc.Value = "0";
            BindSelectedItemGrid();
            BindSelectedItemGrid_Direct();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSearch.ID, btnSearch.GetType().Name, "Click"); 
        BindGrid();

    }
    protected void lnkViewAll_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, LinkButton1.ID, LinkButton1.GetType().Name, "Click"); 
        txtSearchItemName.Text = string.Empty;
        BindGrid();

    }


    protected void btnSearchAll_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSearchAll.ID, btnSearchAll.GetType().Name, "Click"); 
        BindGridAllPrice();
    }
    protected void lnkAllViewA_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, lnkAllViewA.ID, lnkAllViewA.GetType().Name, "Click"); 
        txtSearchAll.Text = string.Empty;
        BindGridAllPrice();

    }
    public void BindGridAllPrice()
    {

        lblResult1.Text = "";
        lblAdd.Text = "";
        lblSelectLocation.Text = "";
        DataTable dtMainList = new DataTable();

        DataClassesDataContext _db = new DataClassesDataContext();
        string strCondition = string.Empty;
        if (txtSearchAll.Text.Trim() != "")
        {
            string str = txtSearchAll.Text.Trim();

            if (str.IndexOf(">>") != -1)
            {
                var corrected = str.Substring(0, str.Length - 2);
                str = corrected.Substring(corrected.LastIndexOf(">>") + 2);
            }
            strCondition = "  AND si.section_name LIKE '%" + str.Trim() + "%'";

            string StrQ = " SELECT it.client_id,it.item_id,si.section_name,si.is_mandatory,it.measure_unit,it.item_cost*it.retail_multiplier AS item_cost,it.minimum_qty,it.retail_multiplier,it.labor_rate,it.labor_rate* it.retail_multiplier as LaborUnitCost, it.update_time,si.section_serial,si.section_level,si.parent_id,it.labor_id,((it.item_cost + it.labor_rate) * it.retail_multiplier) * it.minimum_qty AS ext_item_cost " +
                         " FROM item_price it " +
                         " INNER JOIN sectioninfo si on si.section_id =  it.item_id " +
                         " WHERE si.is_active = 1   " + strCondition + " AND si.is_disable = 0";
            dtMainList = csCommonUtility.GetDataTable(StrQ);
            grdItemPriceAll.DataSource = dtMainList;
            grdItemPriceAll.DataKeyNames = new string[] { "item_id", "labor_rate", "retail_multiplier", "is_mandatory", "section_level", "parent_id", "section_serial" };
            grdItemPriceAll.DataBind();
            if (grdItemPriceAll.Rows.Count == 0)
            {
                btnAddMultiple.Visible = false;
                lblAdd.Text = csCommonUtility.GetSystemErrorMessage("No Record Found");
                lblResult1.Text = csCommonUtility.GetSystemErrorMessage("No Record Found");
            }
            else
            {
                btnAddMultiple.Visible = true;
            }
        }
        else
        {
            grdItemPriceAll.DataSource = dtMainList;
            grdItemPriceAll.DataBind();
            btnAddMultiple.Visible = false;
        }



    }

    public static DataTable LoadFullSection()
    {
        DataTable dt = new DataTable();
        try
        {
            int nClientId = Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
            DataClassesDataContext _db = new DataClassesDataContext();
            int[] ids = (int[])HttpContext.Current.Session["myIds"];

            string strId = "";


            for (int i = 0; i < ids.Length; i++)
                strId += ids[i] + ",";

            strId = strId.TrimEnd(',');

            string sSQL = "select isnull(s5.section_id,isnull(s4.section_id,isnull(s3.section_id,isnull(s2.section_id,0)))) as [section_auto_id], isnull(s5.section_id,isnull(s4.section_id,isnull(s3.section_id,isnull(s2.section_id,0)))) as section_id, " +
                       " s1.section_name+'>>'+s2.section_name+isnull('>>'+s3.section_name,'')+isnull('>>'+s4.section_name,'')+isnull('>>'+s5.section_name,'') as section_name " +
                       " from sectioninfo s1 " +
                       " left outer join sectioninfo s2 on s2.parent_id = s1.section_id and s2.client_id =" + nClientId +
                       " left outer join sectioninfo s3 on s3.parent_id = s2.section_id and s3.client_id =" + nClientId +
                       " left outer join sectioninfo s4 on s4.parent_id = s3.section_id and s4.client_id =" + nClientId +
                       " left outer join sectioninfo s5 on s5.parent_id = s4.section_id and s5.client_id =" + nClientId +
                       " where s1.client_id =" + nClientId + " and s1.section_id in( " + strId + ")";

            var result = _db.ExecuteQuery<sectioninfo>(sSQL);

            dt = csCommonUtility.LINQToDataTable(result);

            HttpContext.Current.Session["gspSearch"] = dt;

        }
        catch (Exception ex)
        {
            string ss = ex.Message;
        }

        return dt;
    }

    public static string GetItemDetialsFull(int SectionId)
    {
        string strSection = string.Empty;

        //&& c.parent_id > 0 
        DataClassesDataContext _db = new DataClassesDataContext();
        List<sectioninfo> list = _db.sectioninfos.Where(c => c.section_id == SectionId && c.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])).ToList();
        foreach (sectioninfo sec1 in list)
        {
            strDetailsFull = sec1.section_name + " >> " + strDetailsFull;
            GetItemDetialsFull(Convert.ToInt32(sec1.parent_id));
        }
        return strDetailsFull;

    }



    protected void grdItemPriceAll_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            strDetails = "";

            int nsectionId = Convert.ToInt32(grdItemPriceAll.DataKeys[Convert.ToInt32(e.Row.RowIndex)].Values[0]);
            int nsectionLevelId = Convert.ToInt32(grdItemPriceAll.DataKeys[Convert.ToInt32(e.Row.RowIndex)].Values[4]);
            string strItem = GetItemDetialsForUpdateItem(nsectionId).ToString();
            e.Row.Cells[2].Text = strItem;
            string sectionName = GetSectionName(nsectionLevelId);
            e.Row.Cells[1].Text = sectionName;

        }

    }
    protected void btnAddMultiple_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnAddMultiple.ID, btnAddMultiple.GetType().Name, "Click"); 
        lblSelectLocation.Text = "";
        lblResult1.Text = "";
        lblMessage.Text = "";
        lblAdd.Text = "";


        DataClassesDataContext _db = new DataClassesDataContext();
        foreach (GridViewRow di in grdItemPriceAll.Rows)
        {
            CheckBox chkIsSelected = (CheckBox)di.FindControl("chkIsSelected");
            int index = Convert.ToInt32(di.RowIndex);
            if (chkIsSelected.Checked)
            {
                strDetails = string.Empty;

                decimal ditem_cost = 0;
                decimal dlabor_rate = 0;
                decimal dretail_multiplier = 0;
                string smeasure_unit = "";
                decimal dminimum_qty = 0;
                decimal nQty = 0;


                int nSectionId = Convert.ToInt32(grdItemPriceAll.Rows[index].Cells[0].Text);
                TextBox txtQty = (TextBox)di.FindControl("txtQty");
                TextBox txtShortNote = (TextBox)di.FindControl("txtShortNote");
                //Label lblTotalPrice = (Label)di.FindControl("lblTotalPrice");
                DropDownList ddlLabor = (DropDownList)di.FindControl("ddlLabor");
                DropDownList ddlDirect = (DropDownList)di.FindControl("ddlDirect");



                dlabor_rate = Convert.ToDecimal(grdItemPriceAll.DataKeys[index].Values[1].ToString());
                dretail_multiplier = Convert.ToDecimal(grdItemPriceAll.DataKeys[index].Values[2].ToString());
                smeasure_unit = di.Cells[6].Text;
                ditem_cost = Convert.ToDecimal(di.Cells[7].Text.Replace("$", "").Replace("(", "-").Replace(")", ""));
                dminimum_qty = Convert.ToDecimal(di.Cells[9].Text);
                nQty = Convert.ToDecimal(txtQty.Text);
                if (dminimum_qty > nQty)
                {
                    lblResult1.Text = csCommonUtility.GetSystemRequiredMessage("Qty should be greater than minimum value for Item Id=" + nSectionId + "");
                    lblAdd.Text = csCommonUtility.GetSystemRequiredMessage("Qty should be greater than minimum value for Item Id=" + nSectionId + "");
                    return;
                }

                decimal nLaborRate = dlabor_rate * dretail_multiplier;
                nLaborRate = nLaborRate * nQty;
                decimal nTotalPrice = 0;
                if (ddlLabor.SelectedValue == "2")
                    nTotalPrice = ditem_cost * nQty + nLaborRate;
                else
                    nTotalPrice = ditem_cost * nQty;

                if (ddlCustomerLocations.SelectedItem.Text == "Select Location")
                {
                    lblAdd.Text = csCommonUtility.GetSystemErrorMessage("Please select location.");
                    lblSelectLocation.Text = csCommonUtility.GetSystemErrorMessage("Please select location.");
                    ddlCustomerLocations.Focus();
                    return;
                }


                sectioninfo sinfo = _db.sectioninfos.Single(s => s.section_id == nSectionId && s.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));

                bool IsCommissionExclude = Convert.ToBoolean(sinfo.is_CommissionExclude);
                userinfo obj = (userinfo)Session["oUser"];
                model_estimate_pricing price_detail = new model_estimate_pricing();


                price_detail.item_name = GetItemDetialsForUpdateItem(Convert.ToInt32(hdnSectionId.Value)).ToString();
                price_detail.measure_unit = smeasure_unit;
                price_detail.minimum_qty = dminimum_qty;
                price_detail.retail_multiplier = dretail_multiplier;
                price_detail.labor_rate = dlabor_rate;
                price_detail.item_cost = ditem_cost;

                price_detail.client_id = Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
                if (Convert.ToInt32(hdnSalesPersonId.Value) > 0)
                    price_detail.sales_person_id = Convert.ToInt32(hdnSalesPersonId.Value);
                else
                    price_detail.sales_person_id = obj.sales_person_id;
                price_detail.model_estimate_id = Convert.ToInt32(hdnEstimateId.Value);
                price_detail.location_id = Convert.ToInt32(ddlCustomerLocations.SelectedValue);
                price_detail.section_level = sinfo.section_level;
                price_detail.item_id = nSectionId;
                price_detail.location_id = Convert.ToInt32(ddlCustomerLocations.SelectedValue);
                price_detail.section_name = GetSectionName(Convert.ToInt32(sinfo.section_level));
                price_detail.quantity = Convert.ToDecimal(txtQty.Text);
                price_detail.labor_id = Convert.ToInt32(ddlLabor.SelectedValue);
                price_detail.is_direct = Convert.ToInt32(ddlDirect.SelectedValue);

                if (Convert.ToInt32(ddlDirect.SelectedValue) == 1)
                {
                    price_detail.total_retail_price = nTotalPrice;
                    price_detail.total_direct_price = 0;
                }
                else
                {
                    price_detail.total_retail_price = 0;
                    price_detail.total_direct_price = nTotalPrice;
                }

                if (Convert.ToInt32(ddlLabor.SelectedValue) == 1)
                    price_detail.labor_rate = 0;
                price_detail.section_serial = Convert.ToDecimal(sinfo.section_serial);
                int serial = GetSerialMultiple(Convert.ToInt32(sinfo.section_level));
                price_detail.item_cnt = serial;
                price_detail.pricing_type = "A";
                price_detail.short_notes = txtShortNote.Text;
                price_detail.create_date = DateTime.Now;
                price_detail.last_updated_date = DateTime.Now;
                price_detail.sort_id = 0;

                _db.model_estimate_pricings.InsertOnSubmit(price_detail);
                _db.SubmitChanges();
            }

        }


        lblAdd.Text = csCommonUtility.GetSystemMessage("Selected Item(s) added to estimate pricing list");
        lblResult1.Text = csCommonUtility.GetSystemMessage("Selected Item(s) added to estimate pricing list");
        hdnSortDesc.Value = "1";
        BindSelectedItemGrid();
        BindSelectedItemGrid_Direct();
        Calculate_Total();

        if (grdGrouping.Rows.Count == 0)
        {
            lblRetailPricingHeader.Visible = false;
        }
        else
        {
            lblRetailPricingHeader.Visible = true;
        }
        if (grdGroupingDirect.Rows.Count == 0)
        {
            lblDirectPricingHeader.Visible = false;
        }
        else
        {
            lblDirectPricingHeader.Visible = true;
        }
    }

}
