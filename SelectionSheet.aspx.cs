using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SelectionSheet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);

            if (Session["oUser"] == null)
            {
                Response.Redirect("AwardCRMLogin.aspx");
            }
            if (Page.User.IsInRole("admin032") == false)
            {
                // No Permission Page.
                Response.Redirect("nopermission.aspx");
            }
            DataClassesDataContext _db = new DataClassesDataContext();
            int nEstimateId = Convert.ToInt32(Request.QueryString.Get("eid"));
            hdnEstimateId.Value = nEstimateId.ToString();
            int nCustomerId = Convert.ToInt32(Request.QueryString.Get("cid"));
            hdnCustomerId.Value = nCustomerId.ToString();

            LoadSelectionMaster();

            GetCabinetSheet();
            //Kitchen/Shower/Tub
            loadKitchenSheetSelection();
            loadShowerSheetSelection();
            loadTUBSheetSelection();
            loadBathSheetSelection();
            loadKitchen2SheetSelection();
        }
    }
    protected void LoadSelectionMaster()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        SelectionSheetMaster objSSM = new SelectionSheetMaster();

        if (_db.SelectionSheetMasters.Any(ss => ss.customer_id == Convert.ToInt32(hdnCustomerId.Value) && ss.estimate_id == Convert.ToInt32(hdnEstimateId.Value)))
        {
            objSSM = _db.SelectionSheetMasters.SingleOrDefault(ss => ss.customer_id == Convert.ToInt32(hdnCustomerId.Value) && ss.estimate_id == Convert.ToInt32(hdnEstimateId.Value));

            chkCabinet.Checked = Convert.ToBoolean(objSSM.IsCabinet);
            chkKitchen.Checked = Convert.ToBoolean(objSSM.IsKitchen);
            chkShower.Checked = Convert.ToBoolean(objSSM.IsShower);
            chkTub.Checked = Convert.ToBoolean(objSSM.IsTub);
            chkBathroom.Checked = Convert.ToBoolean(objSSM.IsBath);
            chkitchen2.Checked = Convert.ToBoolean(objSSM.IsKitchen2);

            if (Convert.ToBoolean(objSSM.IsCabinet))
                cpnlExtCabinet.ClientState = "false";
            else
                cpnlExtCabinet.ClientState = "true";

            if (Convert.ToBoolean(objSSM.IsKitchen))
                cpnlExtKitchen.ClientState = "false";
            else
                cpnlExtKitchen.ClientState = "true";

            if (Convert.ToBoolean(objSSM.IsShower))
                cpnlExtShower.ClientState = "false";
            else
                cpnlExtShower.ClientState = "true";

            if (Convert.ToBoolean(objSSM.IsTub))
                cpnlExtTub.ClientState = "false";
            else
                cpnlExtTub.ClientState = "true";

            if (Convert.ToBoolean(objSSM.IsBath))
                cpnlExtBathroom.ClientState = "false";
            else
                cpnlExtBathroom.ClientState = "true";

            if (Convert.ToBoolean(objSSM.IsKitchen2))
                cpnlExtkitchen2.ClientState = "false";
            else
                cpnlExtkitchen2.ClientState = "true";
        }
    }

    protected void GetCabinetSheet()
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        DataTable tmpTable = LoadSectionTable();

        var objCabSSList = _db.CabinetSheetSelections.Where(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value)).ToList();

        foreach (CabinetSheetSelection Cabinfo in objCabSSList)
        {

            DataRow drNew = tmpTable.NewRow();

            drNew["CabinetSheetID"] = Cabinfo.CabinetSheetID;
            drNew["customer_id"] = Cabinfo.customer_id;
            drNew["estimate_id"] = Cabinfo.estimate_id;
            drNew["CabinetSheetName"] = Cabinfo.CabinetSheetName;
            drNew["UpperWallDoor"] = Cabinfo.UpperWallDoor;
            drNew["UpperWallWood"] = Cabinfo.UpperWallWood;
            drNew["UpperWallStain"] = Cabinfo.UpperWallStain;
            drNew["UpperWallExterior"] = Cabinfo.UpperWallExterior;
            drNew["UpperWallInterior"] = Cabinfo.UpperWallInterior;
            drNew["UpperWallOther"] = Cabinfo.UpperWallOther;
            drNew["BaseDoor"] = Cabinfo.BaseDoor;
            drNew["BaseWood"] = Cabinfo.BaseWood;
            drNew["BaseStain"] = Cabinfo.BaseStain;
            drNew["BaseExterior"] = Cabinfo.BaseExterior;
            drNew["BaseInterior"] = Cabinfo.BaseInterior;
            drNew["BaseOther"] = Cabinfo.BaseOther;
            drNew["MiscDoor"] = Cabinfo.MiscDoor;
            drNew["MiscWood"] = Cabinfo.MiscWood;
            drNew["MiscStain"] = Cabinfo.MiscStain;
            drNew["MiscExterior"] = Cabinfo.MiscExterior;
            drNew["MiscInterior"] = Cabinfo.MiscInterior;
            drNew["MiscOther"] = Cabinfo.MiscOther;
            drNew["LastUpdateDate"] = Cabinfo.LastUpdateDate;
            drNew["UpdateBy"] = Cabinfo.UpdateBy;

            tmpTable.Rows.Add(drNew);
        }

        if (objCabSSList.Count() == 0)
        {
            DataRow drNew1 = tmpTable.NewRow();

            drNew1["CabinetSheetID"] = 0;
            drNew1["customer_id"] = Convert.ToInt32(hdnCustomerId.Value);
            drNew1["estimate_id"] = Convert.ToInt32(hdnEstimateId.Value);
            drNew1["CabinetSheetName"] = "";
            drNew1["UpperWallDoor"] = "";
            drNew1["UpperWallWood"] = "";
            drNew1["UpperWallStain"] = "";
            drNew1["UpperWallExterior"] = "";
            drNew1["UpperWallInterior"] = "";
            drNew1["UpperWallOther"] = "";
            drNew1["BaseDoor"] = "";
            drNew1["BaseWood"] = "";
            drNew1["BaseStain"] = "";
            drNew1["BaseExterior"] = "";
            drNew1["BaseInterior"] = "";
            drNew1["BaseOther"] = "";
            drNew1["MiscDoor"] = "";
            drNew1["MiscWood"] = "";
            drNew1["MiscStain"] = "";
            drNew1["MiscExterior"] = "";
            drNew1["MiscInterior"] = "";
            drNew1["MiscOther"] = "";
            drNew1["LastUpdateDate"] = DateTime.Now;
            drNew1["UpdateBy"] = User.Identity.Name;


            tmpTable.Rows.InsertAt(drNew1, 0);
        }

        Session.Add("sCabinetSection", tmpTable);

        grdCabinetSelectionSheet.DataSource = tmpTable;
        grdCabinetSelectionSheet.DataKeyNames = new string[] { "CabinetSheetID", "customer_id", "estimate_id" };
        grdCabinetSelectionSheet.DataBind();



    }

    protected void grdCabinetSelectionSheet_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int nCabinetSheetID = Convert.ToInt32(grdCabinetSelectionSheet.DataKeys[e.Row.RowIndex].Values[0].ToString());

            LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");

            lnkDelete.Attributes["CommandArgument"] = string.Format("{0}", nCabinetSheetID);

            if (nCabinetSheetID > 0)
                lnkDelete.Visible = true;

        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSave.ID, btnSave.GetType().Name, "Click");
        lblResult.Text = "";
        DataClassesDataContext _db = new DataClassesDataContext();
        string strRequired = string.Empty;
        try
        {
            DataTable table = (DataTable)Session["sCabinetSection"];



            foreach (GridViewRow row in grdCabinetSelectionSheet.Rows)
            {
                TextBox txtCabinetSheetName = (TextBox)row.FindControl("txtCabinetSheetName");

                TextBox txtUpperWallDoor = (TextBox)row.FindControl("txtUpperWallDoor");
                TextBox txtUpperWallWood = (TextBox)row.FindControl("txtUpperWallWood");
                TextBox txtUpperWallStain = (TextBox)row.FindControl("txtUpperWallStain");
                TextBox txtUpperWallExterior = (TextBox)row.FindControl("txtUpperWallExterior");
                TextBox txtUpperWallInterior = (TextBox)row.FindControl("txtUpperWallInterior");
                TextBox txtUpperWallOther = (TextBox)row.FindControl("txtUpperWallOther");

                TextBox txtBaseDoor = (TextBox)row.FindControl("txtBaseDoor");
                TextBox txtBaseWood = (TextBox)row.FindControl("txtBaseWood");
                TextBox txtBaseStain = (TextBox)row.FindControl("txtBaseStain");
                TextBox txtBaseExterior = (TextBox)row.FindControl("txtBaseExterior");
                TextBox txtBaseInterior = (TextBox)row.FindControl("txtBaseInterior");
                TextBox txtBaseOther = (TextBox)row.FindControl("txtBaseOther");

                TextBox txtMiscDoor = (TextBox)row.FindControl("txtMiscDoor");
                TextBox txtMiscWood = (TextBox)row.FindControl("txtMiscWood");
                TextBox txtMiscStain = (TextBox)row.FindControl("txtMiscStain");
                TextBox txtMiscExterior = (TextBox)row.FindControl("txtMiscExterior");
                TextBox txtMiscInterior = (TextBox)row.FindControl("txtMiscInterior");
                TextBox txtMiscOther = (TextBox)row.FindControl("txtMiscOther");

                Label lblCabinetSheetName = (Label)row.FindControl("lblCabinetSheetName");

                Label lblUpperWallDoor = (Label)row.FindControl("lblUpperWallDoor");
                Label lblUpperWallWood = (Label)row.FindControl("lblUpperWallWood");
                Label lblUpperWallStain = (Label)row.FindControl("lblUpperWallStain");
                Label lblUpperWallExterior = (Label)row.FindControl("lblUpperWallExterior");
                Label lblUpperWallInterior = (Label)row.FindControl("lblUpperWallInterior");
                Label lblUpperWallOther = (Label)row.FindControl("lblUpperWallOther");

                Label lblBaseDoor = (Label)row.FindControl("lblBaseDoor");
                Label lblBaseWood = (Label)row.FindControl("lblBaseWood");
                Label lblBaseStain = (Label)row.FindControl("lblBaseStain");
                Label lblBaseExterior = (Label)row.FindControl("lblBaseExterior");
                Label lblBaseInterior = (Label)row.FindControl("lblBaseInterior");
                Label lblBaseOther = (Label)row.FindControl("lblBaseOther");

                Label lblMiscDoor = (Label)row.FindControl("lblMiscDoor");
                Label lblMiscWood = (Label)row.FindControl("lblMiscWood");
                Label lblMiscStain = (Label)row.FindControl("lblMiscStain");
                Label lblMiscExterior = (Label)row.FindControl("lblMiscExterior");
                Label lblMiscInterior = (Label)row.FindControl("lblMiscInterior");
                Label lblMiscOther = (Label)row.FindControl("lblMiscOther");

                if (txtCabinetSheetName.Text.Trim() == "")
                {
                    lblCabinetSheetName.Text = csCommonUtility.GetSystemRequiredMessage2("Name of Cabinet is required.");
                    strRequired = "required";
                }
                else
                    lblCabinetSheetName.Text = "";

                if (txtUpperWallDoor.Text.Trim() == "")
                {
                    lblUpperWallDoor.Text = csCommonUtility.GetSystemRequiredMessage2("Upper Wall Door is required.");
                    strRequired = "required";
                }
                else
                    lblUpperWallDoor.Text = "";

                if (txtUpperWallWood.Text.Trim() == "")
                {
                    lblUpperWallWood.Text = csCommonUtility.GetSystemRequiredMessage2("Upper Wall Wood is required.");
                    strRequired = "required";
                }
                else
                    lblUpperWallWood.Text = "";

                if (txtUpperWallStain.Text.Trim() == "")
                {
                    lblUpperWallStain.Text = csCommonUtility.GetSystemRequiredMessage2("Upper Wall Stain is required.");
                    strRequired = "required";
                }
                else
                    lblUpperWallStain.Text = "";

                if (txtUpperWallExterior.Text.Trim() == "")
                {
                    lblUpperWallExterior.Text = csCommonUtility.GetSystemRequiredMessage2("Upper Wall Exterior is required.");
                    strRequired = "required";
                }
                else
                    lblUpperWallExterior.Text = "";

                if (txtUpperWallInterior.Text.Trim() == "")
                {
                    lblUpperWallInterior.Text = csCommonUtility.GetSystemRequiredMessage2("Upper Wall Interior is required.");
                    strRequired = "required";
                }
                else
                    lblUpperWallInterior.Text = "";




                if (txtBaseDoor.Text.Trim() == "")
                {
                    lblBaseDoor.Text = csCommonUtility.GetSystemRequiredMessage2("Base Door is required.");
                    strRequired = "required";
                }
                else
                    lblBaseDoor.Text = "";

                if (txtBaseWood.Text.Trim() == "")
                {
                    lblBaseWood.Text = csCommonUtility.GetSystemRequiredMessage2("Base Wood is required.");
                    strRequired = "required";
                }
                else
                    lblBaseWood.Text = "";

                if (txtBaseStain.Text.Trim() == "")
                {
                    lblBaseStain.Text = csCommonUtility.GetSystemRequiredMessage2("Base Stain is required.");
                    strRequired = "required";
                }
                else
                    lblBaseStain.Text = "";

                if (txtBaseExterior.Text.Trim() == "")
                {
                    lblBaseExterior.Text = csCommonUtility.GetSystemRequiredMessage2("Base Exterior is required.");
                    strRequired = "required";
                }
                else
                    lblBaseExterior.Text = "";

                if (txtBaseInterior.Text.Trim() == "")
                {
                    lblBaseInterior.Text = csCommonUtility.GetSystemRequiredMessage2("Base Interior is required.");
                    strRequired = "required";
                }
                else
                    lblBaseInterior.Text = "";


                if (txtMiscDoor.Text.Trim() == "")
                {
                    lblMiscDoor.Text = csCommonUtility.GetSystemRequiredMessage2("Misc Door is required.");
                    strRequired = "required";
                }
                else
                    lblMiscDoor.Text = "";

                if (txtMiscWood.Text.Trim() == "")
                {
                    lblMiscWood.Text = csCommonUtility.GetSystemRequiredMessage2("Misc Wood is required.");
                    strRequired = "required";
                }
                else
                    lblMiscWood.Text = "";

                if (txtMiscStain.Text.Trim() == "")
                {
                    lblMiscStain.Text = csCommonUtility.GetSystemRequiredMessage2("Misc Stain is required.");
                    strRequired = "required";
                }
                else
                    lblMiscStain.Text = "";

                if (txtMiscExterior.Text.Trim() == "")
                {
                    lblMiscExterior.Text = csCommonUtility.GetSystemRequiredMessage2("Misc Exterior is required.");
                    strRequired = "required";
                }
                else
                    lblMiscExterior.Text = "";

                if (txtMiscInterior.Text.Trim() == "")
                {
                    lblMiscInterior.Text = csCommonUtility.GetSystemRequiredMessage2("Misc Interior is required.");
                    strRequired = "required";
                }
                else
                    lblMiscInterior.Text = "";




                if (strRequired.Length == 0)
                {
                    DataRow dr = table.Rows[row.RowIndex];

                    dr["CabinetSheetID"] = Convert.ToInt32(grdCabinetSelectionSheet.DataKeys[row.RowIndex].Values[0]);
                    dr["customer_id"] = Convert.ToInt32(hdnCustomerId.Value);
                    dr["estimate_id"] = Convert.ToInt32(hdnEstimateId.Value);
                    dr["CabinetSheetName"] = txtCabinetSheetName.Text.Trim();
                    dr["UpperWallDoor"] = txtUpperWallDoor.Text.Trim();
                    dr["UpperWallWood"] = txtUpperWallWood.Text.Trim();
                    dr["UpperWallStain"] = txtUpperWallStain.Text.Trim();
                    dr["UpperWallExterior"] = txtUpperWallExterior.Text.Trim();
                    dr["UpperWallInterior"] = txtUpperWallInterior.Text.Trim();
                    dr["UpperWallOther"] = txtUpperWallOther.Text.Trim();
                    dr["BaseDoor"] = txtBaseDoor.Text.Trim();
                    dr["BaseWood"] = txtBaseWood.Text.Trim();
                    dr["BaseStain"] = txtBaseStain.Text.Trim();
                    dr["BaseExterior"] = txtBaseExterior.Text.Trim();
                    dr["BaseInterior"] = txtBaseInterior.Text.Trim();
                    dr["BaseOther"] = txtBaseOther.Text.Trim();
                    dr["MiscDoor"] = txtMiscDoor.Text.Trim();
                    dr["MiscWood"] = txtMiscWood.Text.Trim();
                    dr["MiscStain"] = txtMiscStain.Text.Trim();
                    dr["MiscExterior"] = txtMiscExterior.Text.Trim();
                    dr["MiscInterior"] = txtMiscInterior.Text.Trim();
                    dr["MiscOther"] = txtMiscOther.Text.Trim();
                    dr["LastUpdateDate"] = DateTime.Now;
                    dr["UpdateBy"] = User.Identity.Name;
                }
            }
            if (strRequired.Length == 0)
            {
                foreach (DataRow dr in table.Rows)
                {
                    bool bFlagNew = false;

                    CabinetSheetSelection objCabSS = _db.CabinetSheetSelections.SingleOrDefault(l => l.CabinetSheetID == Convert.ToInt32(dr["CabinetSheetID"]));
                    if (objCabSS == null)
                    {
                        objCabSS = new CabinetSheetSelection();
                        bFlagNew = true;

                    }


                    objCabSS.CabinetSheetID = Convert.ToInt32(dr["CabinetSheetID"]);
                    objCabSS.customer_id = Convert.ToInt32(hdnCustomerId.Value);
                    objCabSS.estimate_id = Convert.ToInt32(hdnEstimateId.Value);
                    objCabSS.CabinetSheetName = dr["CabinetSheetName"].ToString();
                    objCabSS.UpperWallDoor = dr["UpperWallDoor"].ToString();
                    objCabSS.UpperWallWood = dr["UpperWallWood"].ToString();
                    objCabSS.UpperWallStain = dr["UpperWallStain"].ToString();
                    objCabSS.UpperWallExterior = dr["UpperWallExterior"].ToString();
                    objCabSS.UpperWallInterior = dr["UpperWallInterior"].ToString();
                    objCabSS.UpperWallOther = dr["UpperWallOther"].ToString();
                    objCabSS.BaseDoor = dr["BaseDoor"].ToString();
                    objCabSS.BaseWood = dr["BaseWood"].ToString();
                    objCabSS.BaseStain = dr["BaseStain"].ToString();
                    objCabSS.BaseExterior = dr["BaseExterior"].ToString();
                    objCabSS.BaseInterior = dr["BaseInterior"].ToString();
                    objCabSS.BaseOther = dr["BaseOther"].ToString();
                    objCabSS.MiscDoor = dr["MiscDoor"].ToString();
                    objCabSS.MiscWood = dr["MiscWood"].ToString();
                    objCabSS.MiscStain = dr["MiscStain"].ToString();
                    objCabSS.MiscExterior = dr["MiscExterior"].ToString();
                    objCabSS.MiscInterior = dr["MiscInterior"].ToString();
                    objCabSS.MiscOther = dr["MiscOther"].ToString();
                    objCabSS.LastUpdateDate = DateTime.Now;
                    objCabSS.UpdateBy = User.Identity.Name;


                    if (bFlagNew)
                    {
                        _db.CabinetSheetSelections.InsertOnSubmit(objCabSS);
                    }
                }


                lblResult.Text = csCommonUtility.GetSystemMessage("Data has been saved successfully");

                _db.SubmitChanges();
                GetCabinetSheet();
            }
        }
        catch (Exception ex)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage(ex.Message);
        }
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        
        try
        {
          
            DataClassesDataContext _db = new DataClassesDataContext();
            LinkButton lnkDelete = (LinkButton)sender;
            KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, lnkDelete.ID, lnkDelete.GetType().Name, "Click");
            int nCabinetSheetID = Convert.ToInt32(lnkDelete.Attributes["CommandArgument"]);
            string strQ = string.Empty;

            strQ = "Delete FROM CabinetSheetSelection  WHERE CabinetSheetID =" + nCabinetSheetID;
            _db.ExecuteCommand(strQ, string.Empty);
            GetCabinetSheet();
        }
        catch (Exception ex)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage(ex.Message);
        }


    }

    private DataTable LoadSectionTable()
    {
        DataTable table = new DataTable();

        table.Columns.Add("CabinetSheetID", typeof(int));
        table.Columns.Add("customer_id", typeof(int));
        table.Columns.Add("estimate_id", typeof(int));
        table.Columns.Add("CabinetSheetName", typeof(string));
        table.Columns.Add("UpperWallDoor", typeof(string));
        table.Columns.Add("UpperWallWood", typeof(string));
        table.Columns.Add("UpperWallStain", typeof(string));
        table.Columns.Add("UpperWallExterior", typeof(string));
        table.Columns.Add("UpperWallInterior", typeof(string));
        table.Columns.Add("UpperWallOther", typeof(string));
        table.Columns.Add("BaseDoor", typeof(string));
        table.Columns.Add("BaseWood", typeof(string));
        table.Columns.Add("BaseStain", typeof(string));
        table.Columns.Add("BaseExterior", typeof(string));
        table.Columns.Add("BaseInterior", typeof(string));
        table.Columns.Add("BaseOther", typeof(string));
        table.Columns.Add("MiscDoor", typeof(string));
        table.Columns.Add("MiscWood", typeof(string));
        table.Columns.Add("MiscStain", typeof(string));
        table.Columns.Add("MiscExterior", typeof(string));
        table.Columns.Add("MiscInterior", typeof(string));
        table.Columns.Add("MiscOther", typeof(string));
        table.Columns.Add("LastUpdateDate", typeof(DateTime));
        table.Columns.Add("UpdateBy", typeof(string));

        return table;
    }

    protected void btnAddItem_Click(object sender, EventArgs e)
    {

        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnAddItem.ID, btnAddItem.GetType().Name, "Click");
        CabinetSheetSelection objCabSS = new CabinetSheetSelection();
        DataClassesDataContext _db = new DataClassesDataContext();

        DataTable table = (DataTable)Session["sCabinetSection"];

        int nCabinetSheetID = Convert.ToInt32(hdnCabinetSheetID.Value);
        bool contains = table.AsEnumerable().Any(row => nCabinetSheetID == row.Field<int>("CabinetSheetID"));
        if (contains)
        {
            lblResult.Text = csCommonUtility.GetSystemRequiredMessage("You have already a pending item to save.");
            return;
        }

        foreach (GridViewRow row in grdCabinetSelectionSheet.Rows)
        {

            DataRow dr = table.Rows[row.RowIndex];

            TextBox txtCabinetSheetName = (TextBox)row.FindControl("txtCabinetSheetName");

            TextBox txtUpperWallDoor = (TextBox)row.FindControl("txtUpperWallDoor");
            TextBox txtUpperWallWood = (TextBox)row.FindControl("txtUpperWallWood");
            TextBox txtUpperWallStain = (TextBox)row.FindControl("txtUpperWallStain");
            TextBox txtUpperWallExterior = (TextBox)row.FindControl("txtUpperWallExterior");
            TextBox txtUpperWallInterior = (TextBox)row.FindControl("txtUpperWallInterior");
            TextBox txtUpperWallOther = (TextBox)row.FindControl("txtUpperWallOther");

            TextBox txtBaseDoor = (TextBox)row.FindControl("txtBaseDoor");
            TextBox txtBaseWood = (TextBox)row.FindControl("txtBaseWood");
            TextBox txtBaseStain = (TextBox)row.FindControl("txtBaseStain");
            TextBox txtBaseExterior = (TextBox)row.FindControl("txtBaseExterior");
            TextBox txtBaseInterior = (TextBox)row.FindControl("txtBaseInterior");
            TextBox txtBaseOther = (TextBox)row.FindControl("txtBaseOther");

            TextBox txtMiscDoor = (TextBox)row.FindControl("txtMiscDoor");
            TextBox txtMiscWood = (TextBox)row.FindControl("txtMiscWood");
            TextBox txtMiscStain = (TextBox)row.FindControl("txtMiscStain");
            TextBox txtMiscExterior = (TextBox)row.FindControl("txtMiscExterior");
            TextBox txtMiscInterior = (TextBox)row.FindControl("txtMiscInterior");
            TextBox txtMiscOther = (TextBox)row.FindControl("txtMiscOther");

            dr["CabinetSheetID"] = Convert.ToInt32(grdCabinetSelectionSheet.DataKeys[row.RowIndex].Values[0]);
            dr["customer_id"] = Convert.ToInt32(hdnCustomerId.Value);
            dr["estimate_id"] = Convert.ToInt32(hdnEstimateId.Value);
            dr["CabinetSheetName"] = txtCabinetSheetName.Text.Trim();
            dr["UpperWallDoor"] = txtUpperWallDoor.Text.Trim();
            dr["UpperWallWood"] = txtUpperWallWood.Text.Trim();
            dr["UpperWallStain"] = txtUpperWallStain.Text.Trim();
            dr["UpperWallExterior"] = txtUpperWallExterior.Text.Trim();
            dr["UpperWallInterior"] = txtUpperWallInterior.Text.Trim();
            dr["UpperWallOther"] = txtUpperWallOther.Text.Trim();
            dr["BaseDoor"] = txtBaseDoor.Text.Trim();
            dr["BaseWood"] = txtBaseWood.Text.Trim();
            dr["BaseStain"] = txtBaseStain.Text.Trim();
            dr["BaseExterior"] = txtBaseExterior.Text.Trim();
            dr["BaseInterior"] = txtBaseInterior.Text.Trim();
            dr["BaseOther"] = txtBaseOther.Text.Trim();
            dr["MiscDoor"] = txtMiscDoor.Text.Trim();
            dr["MiscWood"] = txtMiscWood.Text.Trim();
            dr["MiscStain"] = txtMiscStain.Text.Trim();
            dr["MiscExterior"] = txtMiscExterior.Text.Trim();
            dr["MiscInterior"] = txtMiscInterior.Text.Trim();
            dr["MiscOther"] = txtMiscOther.Text.Trim();
            dr["LastUpdateDate"] = DateTime.Now;
            dr["UpdateBy"] = User.Identity.Name;
        }

        DataRow drNew = table.NewRow();

        drNew["CabinetSheetID"] = Convert.ToInt32(hdnCabinetSheetID.Value);
        drNew["customer_id"] = Convert.ToInt32(hdnCustomerId.Value);
        drNew["estimate_id"] = Convert.ToInt32(hdnEstimateId.Value);
        drNew["CabinetSheetName"] = "";
        drNew["UpperWallDoor"] = "";
        drNew["UpperWallWood"] = "";
        drNew["UpperWallStain"] = "";
        drNew["UpperWallExterior"] = "";
        drNew["UpperWallInterior"] = "";
        drNew["UpperWallOther"] = "";
        drNew["BaseDoor"] = "";
        drNew["BaseWood"] = "";
        drNew["BaseStain"] = "";
        drNew["BaseExterior"] = "";
        drNew["BaseInterior"] = "";
        drNew["BaseOther"] = "";
        drNew["MiscDoor"] = "";
        drNew["MiscWood"] = "";
        drNew["MiscStain"] = "";
        drNew["MiscExterior"] = "";
        drNew["MiscInterior"] = "";
        drNew["MiscOther"] = "";
        drNew["LastUpdateDate"] = DateTime.Now;
        drNew["UpdateBy"] = User.Identity.Name;

        table.Rows.InsertAt(drNew, 0);

        Session.Add("sCabinetSection", table);
        grdCabinetSelectionSheet.DataSource = table;
        grdCabinetSelectionSheet.DataKeyNames = new string[] { "CabinetSheetID", "customer_id", "estimate_id" };
        grdCabinetSelectionSheet.DataBind();
        lblResult.Text = "";

    }

    #region KITCHEN/TUB/SHOWER

    private void loadKitchenSheetSelection()
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        if (_db.KitchenSheetSelections.Where(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value)).SingleOrDefault() != null)
        {
            KitchenSheetSelection kss = _db.KitchenSheetSelections.SingleOrDefault(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value));
            txtBacksplashQTY1.Text = kss.BacksplashQTY;
            txtBacksplashMOU1.Text = kss.BacksplashMOU;
            txtBacksplashStyle1.Text = kss.BacksplashStyle;
            txtBacksplashColor1.Text = kss.BacksplashColor;
            txtBacksplashSize1.Text = kss.BacksplashSize;
            txtBacksplashVendor1.Text = kss.BacksplashVendor;
            txtBacksplashPattern1.Text = kss.BacksplashPattern;
            txtBacksplashGroutColor1.Text = kss.BacksplashGroutColor;
            txtBBullnoseQTY1.Text = kss.BBullnoseQTY;
            txtBBullnoseMOU1.Text = kss.BBullnoseMOU;
            txtBBullnoseStyle1.Text = kss.BBullnoseStyle;
            txtBBullnoseColor1.Text = kss.BBullnoseColor;
            txtBBullnoseSize1.Text = kss.BBullnoseSize;
            txtBBullnoseVendor1.Text = kss.BBullnoseVendor;
            txtSchluterNOSticks1.Text = kss.SchluterNOSticks;
            txtSchluterColor1.Text = kss.SchluterColor;
            txtSchluterProfile1.Text = kss.SchluterProfile;
            txtSchluterThickness1.Text = kss.SchluterThickness;
            txtFloorQTY1.Text = kss.FloorQTY;
            txtFloorMOU1.Text = kss.FloorMOU;
            txtFloorStyle1.Text = kss.FloorStyle;
            txtFloorColor1.Text = kss.FloorColor;
            txtFloorSize1.Text = kss.FloorSize;
            txtFloorVendor1.Text = kss.FloorVendor;
            txtFloorPattern1.Text = kss.FloorPattern;
            txtFloorDirection1.Text = kss.FloorDirection;
            txtBaseboardQTY1.Text = kss.BaseboardQTY;
            txtBaseboardMOU1.Text = kss.BaseboardMOU;
            txtBaseboardStyle1.Text = kss.BaseboardStyle;
            txtBaseboardColor1.Text = kss.BaseboardColor;
            txtBaseboardSize1.Text = kss.BaseboardSize;
            txtBaseboardVendor1.Text = kss.BaseboardVendor;
            txtFloorGroutColor1.Text = kss.FloorGroutColor;
        }
    }

    private void loadShowerSheetSelection()
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        if (_db.ShowerSheetSelections.Where(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value)).SingleOrDefault() != null)
        {
            ShowerSheetSelection sss = _db.ShowerSheetSelections.SingleOrDefault(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value));
            txtWallQTY1.Text = sss.WallTileQTY;
            txtWallMOU1.Text = sss.WallTileMOU;
            txtWallStyle1.Text = sss.WallTileStyle;
            txtWallColor1.Text = sss.WallTileColor;
            txtWallSize1.Text = sss.WallTileSize;
            txtWallVendor1.Text = sss.WallTileVendor;
            txtWallPattern1.Text = sss.WallTilePattern;
            txtWallGroutColor1.Text = sss.WallTileGroutColor;
            txtSWBullnoseQTY1.Text = sss.WBullnoseQTY;
            txtSWBullnoseMOU1.Text = sss.WBullnoseMOU;
            txtSWBullnoseStyle1.Text = sss.WBullnoseStyle;
            txtSWBullnoseColor1.Text = sss.WBullnoseColor;
            txtSWBullnoseSize1.Text = sss.WBullnoseSize;
            txtSWBullnoseVendor1.Text = sss.WBullnoseVendor;
            txtSchluterNOSticks2.Text = sss.SchluterNOSticks;
            txtSchluterColor2.Text = sss.SchluterColor;
            txtSchluterProfile2.Text = sss.SchluterProfile;
            txtSchluterThicknes2.Text = sss.SchluterThickness;
            txtShowerPanQTY1.Text = sss.ShowerPanQTY;
            txtShowerPanMOU1.Text = sss.ShowerPanMOU;
            txtShowerPanStyle1.Text = sss.ShowerPanStyle;
            txtShowerPanColor1.Text = sss.ShowerPanColor;
            txtShowerPanSize1.Text = sss.ShowerPanSize;
            txtShowerPanVendor1.Text = sss.ShowerPanVendor;
            //sss.ShowerPanPattern = "";
            txtShowerPanGroutColor1.Text = sss.ShowerPanGroutColor;
            txtDecobandQTY1.Text = sss.DecobandQTY;
            txtDecobandMOU1.Text = sss.DecobandMOU;
            txtDecobandStyle1.Text = sss.DecobandStyle;
            txtDecobandColor1.Text = sss.DecobandColor;
            txtDecobandSize1.Text = sss.DecobandSize;
            txtDecobandVendor1.Text = sss.DecobandVendor;
            txtDecobandHeight1.Text = sss.DecobandHeight;
            txtNicheTileQTY1.Text = sss.NicheTileQTY;
            txtNicheTileMOU1.Text = sss.NicheTileMOU;
            txtNicheTileStyle1.Text = sss.NicheTileStyle;
            txtNicheTileColor1.Text = sss.NicheTileColor;
            txtNicheTileSize1.Text = sss.NicheTileSize;
            txtNicheTileVendor1.Text = sss.NicheTileVendor;
            txtNicheLocation1.Text = sss.NicheLocation;
            txtNicheSize1.Text = sss.NicheSize;
            txtBenchTileQTY1.Text = sss.BenchTileQTY;
            txtBenchTileMOU1.Text = sss.BenchTileMOU;
            txtBenchTileStyle1.Text = sss.BenchTileStyle;
            txtBenchTileColor1.Text = sss.BenchTileColor;
            txtBenchTileSize1.Text = sss.BenchTileSize;
            txtBenchTileVendor1.Text = sss.BenchTileVendor;
            txtBenchLocation1.Text = sss.BenchLocation;
            txtBenchSize1.Text = sss.BenchSize;
            txtFloorQTY2.Text = sss.FloorQTY;
            txtFloorMOU2.Text = sss.FloorMOU;
            txtFloorStyle2.Text = sss.FloorStyle;
            txtFloorColor2.Text = sss.FloorColor;
            txtFloorSize2.Text = sss.FloorSize;
            txtFloorVendor2.Text = sss.FloorVendor;
            txtFloorPattern2.Text = sss.FloorPattern;
            txtFloorDirection2.Text = sss.FloorDirection;
            txtBaseboardQTY2.Text = sss.BaseboardQTY;
            txtBaseboardMOU2.Text = sss.BaseboardMOU;
            txtBaseboardStyle2.Text = sss.BaseboardStyle;
            txtBaseboardColor2.Text = sss.BaseboardColor;
            txtBaseboardSize2.Text = sss.BaseboardSize;
            txtBaseboardVendor2.Text = sss.BaseboardVendor;
            txtFloorGroutColor2.Text = sss.FloorGroutColor;
            txtTileto2.Text = sss.TileTo;
        }
    }

    private void loadTUBSheetSelection()
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        if (_db.TubSheetSelections.Where(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value)).SingleOrDefault() != null)
        {
            TubSheetSelection tss = _db.TubSheetSelections.SingleOrDefault(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value));
            txtWallQTY3.Text = tss.WallTileQTY;
            txtWallMOU3.Text = tss.WallTileMOU;
            txtWallStyle3.Text = tss.WallTileStyle;
            txtWallColor3.Text = tss.WallTileColor;
            txtWallSize3.Text = tss.WallTileSize;
            txtWallVendor3.Text = tss.WallTileVendor;
            txtWallPattern3.Text = tss.WallTilePattern;
            txtWallGroutColor3.Text = tss.WallTileGroutColor;
            txtWBullnoseQTY3.Text = tss.WBullnoseQTY;
            txtWBullnoseMOU3.Text = tss.WBullnoseMOU;
            txtWBullnoseStyle3.Text = tss.WBullnoseStyle;
            txtWBullnoseColor3.Text = tss.WBullnoseColor;
            txtWBullnoseSize3.Text = tss.WBullnoseSize;
            txtWBullnoseVendor3.Text = tss.WBullnoseVendor;
            txtSchluterNOSticks3.Text = tss.SchluterNOSticks;
            txtSchluterColor3.Text = tss.SchluterColor;
            txtSchluterProfile3.Text = tss.SchluterProfile;
            txtSchluterThicknes3.Text = tss.SchluterThickness;
            txtDecobandQTY3.Text = tss.DecobandQTY;
            txtDecobandMOU3.Text = tss.DecobandMOU;
            txtDecobandStyle3.Text = tss.DecobandStyle;
            txtDecobandColor3.Text = tss.DecobandColor;
            txtDecobandSize3.Text = tss.DecobandSize;
            txtDecobandVendor3.Text = tss.DecobandVendor;
            txtDecobandHeight3.Text = tss.DecobandHeight;
            txtNicheTileQTY3.Text = tss.NicheTileQTY;
            txtNicheTileMOU3.Text = tss.NicheTileMOU;
            txtNicheTileStyle3.Text = tss.NicheTileStyle;
            txtNicheTileColor3.Text = tss.NicheTileColor;
            txtNicheTileSize3.Text = tss.NicheTileSize;
            txtNicheTileVendor3.Text = tss.NicheTileVendor;
            txtNicheLocation3.Text = tss.NicheLocation;
            txtNicheSize3.Text = tss.NicheSize;
            txtShelfLocation3.Text = tss.ShelfLocation;
            txtFloorQTY3.Text = tss.FloorQTY;
            txtFloorMOU3.Text = tss.FloorMOU;
            txtFloorStyle3.Text = tss.FloorStyle;
            txtFloorColor3.Text = tss.FloorColor;
            txtFloorSize3.Text = tss.FloorSize;
            txtFloorVendor3.Text = tss.FloorVendor;
            txtFloorPattern3.Text = tss.FloorPattern;
            txtFloorDirection3.Text = tss.FloorDirection;
            txtBaseboardQTY3.Text = tss.BaseboardQTY;
            txtBaseboardMOU3.Text = tss.BaseboardMOU;
            txtBaseboardStyle3.Text = tss.BaseboardStyle;
            txtBaseboardColor3.Text = tss.BaseboardColor;
            txtBaseboardSize3.Text = tss.BaseboardSize;
            txtBaseboardVendor3.Text = tss.BaseboardVendor;
            txtFloorGroutColor3.Text = tss.FloorGroutColor;
            txtTileto3.Text = tss.TileTo;
        }
    }


    private void loadBathSheetSelection()
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        BathroomSheetSelection bss = new BathroomSheetSelection();

        if (_db.BathroomSheetSelections.Where(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value)).SingleOrDefault() != null)
        {
            bss = _db.BathroomSheetSelections.SingleOrDefault(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value));


            txtSinkQty.Text = bss.Sink_Qty;
            txtSinkStyle.Text = bss.Sink_Style;
            txtSinkOrder.Text = bss.Sink_WhereToOrder;
            txtSinkFaucentQty.Text = bss.Sink_Fuacet_Qty;
            txtSinkFaucentStyle.Text = bss.Sink_Fuacet_Style;
            txtSinkFaucentOrder.Text = bss.Sink_Fuacet_WhereToOrder;
            txtSinkDrainQty.Text = bss.Sink_Drain_Qty;
            txtSinkDrainStyle.Text = bss.Sink_Drain_Style;
            txtSinkdrainOrder.Text = bss.Sink_Drain_WhereToOrder;
            txtSinkValveQty.Text = bss.Sink_Valve_Qty;
            txtSinkValveStyle.Text = bss.Sink_Valve_Style;
            txtSinkValveOrder.Text = bss.Sink_Valve_WhereToOrder;
            txtBathTubQty.Text = bss.Bathtub_Qty;
            txtBathTubStyle.Text = bss.Bathtub_Style;
            txtBathTubOrder.Text = bss.Bathtub_WhereToOrder;
            txtTubFaucentQty.Text = bss.Tub_Faucet_Qty;
            txtTubFaucentStyle.Text = bss.Tub_Faucet_Style;
            txtTubFaucentOrder.Text = bss.Tub_Faucet_WhereToOrder;
            txtTubValveQty.Text = bss.Tub_Valve_Qty;
            txtTubValveStyle.Text = bss.Tub_Valve_Style;
            txtTubValveOrder.Text = bss.Tub_Valve_WhereToOrder;
            txtTubDrainQty.Text = bss.Tub_Drain_Qty;
            txtTubDrainStyle.Text = bss.Tub_Drain_Style;
            txtTubDrainOrder.Text = bss.Tub_Drain_WhereToOrder;
            txtToiletQty.Text = bss.Tollet_Qty;
            txtToiletStyle.Text = bss.Tollet_Style;
            txtToiletOrder.Text = bss.Tollet_WhereToOrder;
            txtShower_TubSystemQty.Text = bss.Shower_TubSystem_Qty;
            txtShower_TubSystemStyle.Text = bss.Shower_TubSystem_Style;
            txtShower_TubSystemOrder.Text = bss.Shower_TubSystem_WhereToOrder;
            txtShowerValveQty.Text = bss.Shower_Value_Qty;
            txtShowerValveStyle.Text = bss.Shower_Value_Style;

            txtShowerValveOrder.Text = bss.Shower_Value_WhereToOrder;
            txtHandheldShowerQty.Text = bss.Handheld_Shower_Qty;
            txtHandheldShowerStyle.Text = bss.Handheld_Shower_Style;
            txtHandheldShowerOrder.Text = bss.Handheld_Shower_WhereToOrder;
            txtBodySprayQty.Text = bss.Body_Spray_Qty;
            txtBodySprayStyle.Text = bss.Body_Spray_Style;
            txtBodySprayOrder.Text = bss.Body_Spray_WhereToOrder;
            txtBodySprayValveQty.Text = bss.Body_Spray_Valve_Qty;
            txtBodySprayValveStyle.Text = bss.Body_Spray_Valve_Style;
            txtBodySprayValveOrder.Text = bss.Body_Spray_Valve_WhereToOrder;
            txtShowerDrainQty.Text = bss.Shower_Drain_Qty;
            txtShowerDrainStyle.Text = bss.Shower_Drain_Style;
            txtShowerDrainOrder.Text = bss.Shower_Drain_WhereToOrder;
            txtShowerDrainBody_PlugQty.Text = bss.Shower_Drain_Body_Plug_Qty;
            txtShowerDrainBody_PlugStyle.Text = bss.Shower_Drain_Body_Plug_Style;
            txtShowerDrainBody_PlugOrder.Text = bss.Shower_Drain_Body_Plug_WhereToOrder;
            txtShowerDrainCoverQty.Text = bss.Shower_Drain_Cover_Qty;
            txtShowerDrainCoverStyle.Text = bss.Shower_Drain_Cover_Style;
            txtShowerDrainCoverOrder.Text = bss.Shower_Drain_Cover_WhereToOrder;
            txtCounterTopQty.Text = bss.Counter_Top_Qty;
            txtCounterTopStyle.Text = bss.Counter_Top_Style;
            txtCounterTopOrder.Text = bss.Counter_Top_WhereToOrder;
            txtCounterTopEdgeQty.Text = bss.Counter_To_Edge_Qty;
            txtCounterTopEdgeStyle.Text = bss.Counter_To_Edge_Style;
            txtCounterTopEdgeOrder.Text = bss.Counter_To_Edge_WhereToOrder;
            txtCounterTop_OverhangQty.Text = bss.Counter_Top_Overhang_Qty;
            txtCounterTop_OverhangStyle.Text = bss.Counter_Top_Overhang_Style;
            txtCounterTop_OverhangOrder.Text = bss.Counter_Top_Overhang_WhereToOrder;
            txtAdditionalplacesgettingcountertopQty.Text = bss.AdditionalPlacesGettingCountertop_Qty;
            txtAdditionalplacesgettingcountertopStyle.Text = bss.AdditionalPlacesGettingCountertop_Style;
            txtAdditionalplacesgettingcountertopOrder.Text = bss.AdditionalPlacesGettingCountertop_WhereToOrder;
            txtGranite_Quartz_BacksplashQty.Text = bss.Granite_Quartz_Backsplash_Qty;
            txtGranite_Quartz_BacksplashStyle.Text = bss.Granite_Quartz_Backsplash_Style;
            txtGranite_Quartz_BacksplashOrder.Text = bss.Granite_Quartz_Backsplash_WhereToOrder;
            txtTubwalltileQty.Text = bss.Tub_Wall_Tile_Qty;
            txtTubwalltileStyle.Text = bss.Tub_Wall_Tile_Style;
            txtTubwalltileOrder.Text = bss.Tub_Wall_Tile_WhereToOrder;
            txtWallTilelayoutQty.Text = bss.Wall_Tile_Layout_Qty;
            txtWallTilelayoutStyle.Text = bss.Wall_Tile_Layout_Style;
            txtWallTilelayoutOrder.Text = bss.Wall_Tile_Layout_WhereToOrder;
            txtTubskirttileQty.Text = bss.Tub_skirt_tile_Qty;
            txtTubskirttileStyle.Text = bss.Tub_skirt_tile_Style;
            txtTubskirttileOrder.Text = bss.Tub_skirt_tile_WhereToOrder;
            txtShowerWallTileQty.Text = bss.Shower_Wall_Tile_Qty;
            txtShowerWallTileStyle.Text = bss.Shower_Wall_Tile_Style;
            txtShowerWallTileOrder.Text = bss.Shower_Wall_Tile_WhereToOrder;

            txtWall_Tile_layoutQty.Text = bss.Wall_Tile_Layout_Qty;
            txtWall_Tile_layoutStyle.Text = bss.Wall_Tile_Layout_Style;
            txtWall_Tile_layoutOrder.Text = bss.Wall_Tile_Layout_WhereToOrder;

            txtShowerFloorTileQty.Text = bss.Shower_Floor_Tile_Qty;
            txtShowerFloorTileStyle.Text = bss.Shower_Floor_Tile_Style;
            txtShowerFloorTileOrder.Text = bss.Shower_Floor_Tile_WhereToOrder;
            txtShowerTubTileHeightQty.Text = bss.Shower_Tub_Tile_Height_Qty;
            txtShowerTubTileHeightStyle.Text = bss.Shower_Tub_Tile_Height_Style;
            txtShowerTubTileHeightOrder.Text = bss.Shower_Tub_Tile_Height_WhereToOrder;
            txtFloorTiletQty.Text = bss.Floor_Tile_Qty;
            txtFloorTiletstyle.Text = bss.Floor_Tile_Style;
            txtFloorTiletOrder.Text = bss.Floor_Tile_WhereToOrder;
            txtFloorTilelayoutQty.Text = bss.Floor_Tile_layout_Qty;
            txtFloorTilelayoutStyle.Text = bss.Floor_Tile_layout_Style;
            txtFloorTilelayoutOrder.Text = bss.Floor_Tile_layout_WhereToOrder;
            txtBullnoseTileQty.Text = bss.BullnoseTile_Qty;
            txtBullnoseTileStyle.Text = bss.BullnoseTile_Style;

            txtBullnoseTileOrder.Text = bss.BullnoseTile_WhereToOrder;

            txtDecobandQty.Text = bss.Deco_Band_Qty;
            txtDecobandStyle.Text = bss.Deco_Band_Style;
            txtDecobandOrder.Text = bss.Deco_Band_WhereToOrder;
            txtDecobandHeightQty.Text = bss.Deco_Band_Height_Qty;
            txtDecobandHeightStyle.Text = bss.Deco_Band_Height_Style;
            txtDecobandHeightOrder.Text = bss.Deco_Band_Height_WhereToOrder;
            txtTileBaseboardQty.Text = bss.Tile_Baseboard_Qty;
            txtTileBaseboardStyle.Text = bss.Tile_Baseboard_Style;
            txtTileBaseboardOrder.Text = bss.Tile_Baseboard_WhereToOrder;
            txtGroutSelectionQty.Text = bss.Grout_Selection_Qty;
            txtGroutSelectionStyle.Text = bss.Grout_Selection_Style;
            txtGroutSelectionOrder.Text = bss.Grout_Selection_WhereToOrder;
            txtNicheLocationQty.Text = bss.Niche_Location_Qty;
            txtNicheLocationStyle.Text = bss.Niche_Location_Style;
            txtNicheLocationOrder.Text = bss.Niche_Location_WhereToOrder;
            txtNicheSizeQty.Text = bss.Niche_Size_Qty;
            txtNicheSizeStyle.Text = bss.Niche_Size_Style;
            txtNicheSizeOrder.Text = bss.Niche_Size_WhereToOrder;
            txtGlassQty.Text = bss.Glass_Qty;
            txtGlassStyle.Text = bss.Glass_Style;
            txtGlassOrder.Text = bss.Glass_WhereToOrder;
            txtWindowQty.Text = bss.Window_Qty;
            txtWindowStyle.Text = bss.Window_Style;
            txtWindowOrder.Text = bss.Window_WhereToOrder;
            txtDoorQty.Text = bss.Door_Qty;
            txtDoorStyle.Text = bss.Door_Style;
            txtDoorOrder.Text = bss.Door_WhereToOrder;
            txtGrabBarQty.Text = bss.Grab_Bar_Qty;
            txtGrabBarStyle.Text = bss.Grab_Bar_Style;
            txtGrabBarOrder.Text = bss.Grab_Bar_WhereToOrder;
            txtCabinetDoorStyleColorQty.Text = bss.Cabinet_Door_Style_Color_Qty;
            txtCabinetDoorStyleColorStyle.Text = bss.Cabinet_Door_Style_Color_Style;
            txtCabinetDoorStyleColorOrder.Text = bss.Cabinet_Door_Style_Color_WhereToOrder;
            txtMedicineCabinetQty.Text = bss.Medicine_Cabinet_Qty;
            txtMedicineCabinetStyle.Text = bss.Medicine_Cabinet_Style;
            txtMedicineCabinetOrder.Text = bss.Medicine_Cabinet_WhereToOrder;
            txtMirrorQty.Text = bss.Mirror_Qty;
            txtMirrorStyle.Text = bss.Mirror_Style;
            txtMirrorOrder.Text = bss.Mirror_WhereToOrder;
            txtWoodBaseboardQty.Text = bss.Wood_Baseboard_Qty;
            txtWoodBaseboardStyle.Text = bss.Wood_Baseboard_Style;
            txtWoodBaseboardOrder.Text = bss.Wood_Baseboard_WhereToOrder;
            txtPaintColorQty.Text = bss.Paint_Color_Qty;
            txtPaintColorStyle.Text = bss.Paint_Color_Style;
            txtPaintColorOrder.Text = bss.Paint_Color_WhereToOrder;
            txtLightingQty.Text = bss.Lighting_Qty;
            txtLightingStyle.Text = bss.Lighting_Style;
            txtLightingOrder.Text = bss.Lighting_WhereToOrder;
            txtHardwareQty.Text = bss.Hardware_Qty;
            txtHardwareStyle.Text = bss.Hardware_Style;
            txtHardwareOrder.Text = bss.Hardware_WhereToOrder;
            txtBathpecialNotes.Text = bss.Special_Notes;

        }
    }


    private void loadKitchen2SheetSelection()
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        KitchenSelection kss = new KitchenSelection();

        if (_db.KitchenSelections.Where(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value)).SingleOrDefault() != null)
        {
            kss = _db.KitchenSelections.SingleOrDefault(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value));


            txtKitchenSinkQty.Text = kss.Sink_Qty;
            txtKitchenSinkStyle.Text = kss.Sink_Style;
            txtKitchenSinkOrder.Text = kss.Sink_WhereToOrder;
            txtKitchenSinkFaucetQty.Text = kss.Sink_Fuacet_Qty;
            txtKitchenSinkFaucetStyle.Text = kss.Sink_Fuacet_Style;
            txtKitchenSinkFaucetOrder.Text = kss.Sink_Fuacet_WhereToOrder;
            txtKitchenSinkDrainQty.Text = kss.Sink_Drain_Qty;
            txtKitchenSinkDrainStyle.Text = kss.Sink_Drain_Style;
            txtKitchenSinkDrainOrder.Text = kss.Sink_Drain_WhereToOrder;
            txtKitchenCounterTopQty.Text = kss.Counter_Top_Qty;
            txtKitchenCounterTopStyle.Text = kss.Counter_Top_Style;
            txtKitchenCounterTopOrder.Text = kss.Counter_Top_WhereToOrder;
            txtKitchenGraniteQuartzBacksplashQty.Text = kss.Granite_Quartz_Backsplash_Qty;
            txtKitchenGraniteQuartzBacksplashStyle.Text = kss.Granite_Quartz_Backsplash_Style;
            txtKitchenGraniteQuartzBacksplashOrder.Text = kss.Granite_Quartz_Backsplash_WhereToOrder;
            txtKitchenCounterTopOverhangQty.Text = kss.Counter_Top_Overhang_Qty;
            txtKitchenCounterTopOverhangStyle.Text = kss.Counter_Top_Overhang_Style;
            txtKitchenCounterTopOverhangOrder.Text = kss.Counter_Top_Overhang_WhereToOrder;
            txtKitchenAdditionalplacesgettingcountertopQty.Text = kss.AdditionalPlacesGettingCountertop_Qty;
            txtKitchenAdditionalplacesgettingcountertopStyle.Text = kss.AdditionalPlacesGettingCountertop_Style;
            txtKitchenAdditionalplacesgettingcountertopOrder.Text = kss.AdditionalPlacesGettingCountertop_WhereToOrder;
            txtKitchenCounterTopEdgeQty.Text = kss.Counter_To_Edge_Qty;
            txtKitchenCounterTopEdgeStyle.Text = kss.Counter_To_Edge_Style;
            txtKitchenCounterTopEdgeOrder.Text = kss.Counter_To_Edge_WhereToOrder;
            txtKitchenCabinetsQty.Text = kss.Cabinets_Qty;
            txtKitchenCabinetsStyle.Text = kss.Cabinets_Style;
            txtKitchenCabinetsOrder.Text = kss.Cabinets_WhereToOrder;
            txtKitchenDisposalQty.Text = kss.Disposal_Qty;
            txtKitchenDisposalStyle.Text = kss.Disposal_Style;
            txtKitchenDisposalOrder.Text = kss.Disposal_WhereToOrder;
            txtKitchenBaseboardQty.Text = kss.Baseboard_Qty;
            txtKitchenBaseboardStyle.Text = kss.Baseboard_Style;
            txtKitchenBaseboardOrder.Text = kss.Baseboard_WhereToOrder;
            txtKitchenWindowsQty.Text = kss.Window_Qty;
            txtKitchenWindowsStyle.Text = kss.Window_Style;
            txtKitchenWindowsOrder.Text = kss.Window_WhereToOrder;
            txtKitchenDoorsQty.Text = kss.Door_Qty;
            txtKitchenDoorsStyle.Text = kss.Door_Style;
            txtKitchenDoorsOrder.Text = kss.Door_WhereToOrder;
            txtKitchenLightingQty.Text = kss.Lighting_Qty;
            txtKitchenLightingStyle.Text = kss.Lighting_Style;
            txtKitchenLightingOrder.Text = kss.Lighting_WhereToOrder;
            txtKitchenHardwareQty.Text = kss.Hardware_Qty;
            txtKitchenHardwareStyle.Text = kss.Hardware_Style;
            txtKitchenHardwareOrder.Text = kss.Hardware_WhereToOrder;
            txtKitchenSpecialNotes.Text = kss.Special_Notes;

        }
    }

    protected void btnSaveKitchenSheet_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSaveKitchenSheet.ID, btnSaveKitchenSheet.GetType().Name, "Click"); 
        DataClassesDataContext _db = new DataClassesDataContext();

        KitchenSheetSelection kss = new KitchenSheetSelection();
        //string strQ = "DELETE KitchenSheetSelection WHERE customer_id=" + Convert.ToInt32(hdnCustomerId.Value) + " AND estimate_id=" + Convert.ToInt32(hdnEstimateId.Value);
        //_db.ExecuteCommand(strQ, string.Empty);
        //_db.SubmitChanges();

        if (_db.KitchenSheetSelections.Where(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value)).SingleOrDefault() != null)
        {
            kss = _db.KitchenSheetSelections.SingleOrDefault(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value));
        }

        kss.customer_id = Convert.ToInt32(hdnCustomerId.Value);
        kss.estimate_id = Convert.ToInt32(hdnEstimateId.Value);
        kss.BacksplashQTY = txtBacksplashQTY1.Text;
        kss.BacksplashMOU = txtBacksplashMOU1.Text;
        kss.BacksplashStyle = txtBacksplashStyle1.Text;
        kss.BacksplashColor = txtBacksplashColor1.Text;
        kss.BacksplashSize = txtBacksplashSize1.Text;
        kss.BacksplashVendor = txtBacksplashVendor1.Text;
        kss.BacksplashPattern = txtBacksplashPattern1.Text;
        kss.BacksplashGroutColor = txtBacksplashGroutColor1.Text;
        kss.BBullnoseQTY = txtBBullnoseQTY1.Text;
        kss.BBullnoseMOU = txtBBullnoseMOU1.Text;
        kss.BBullnoseStyle = txtBBullnoseStyle1.Text;
        kss.BBullnoseColor = txtBBullnoseColor1.Text;
        kss.BBullnoseSize = txtBBullnoseSize1.Text;
        kss.BBullnoseVendor = txtBBullnoseVendor1.Text;
        kss.SchluterNOSticks = txtSchluterNOSticks1.Text;
        kss.SchluterColor = txtSchluterColor1.Text;
        kss.SchluterProfile = txtSchluterProfile1.Text;
        kss.SchluterThickness = txtSchluterThickness1.Text;
        kss.FloorQTY = txtFloorQTY1.Text;
        kss.FloorMOU = txtFloorMOU1.Text;
        kss.FloorStyle = txtFloorStyle1.Text;
        kss.FloorColor = txtFloorColor1.Text;
        kss.FloorSize = txtFloorSize1.Text;
        kss.FloorVendor = txtFloorVendor1.Text;
        kss.FloorPattern = txtFloorPattern1.Text;
        kss.FloorDirection = txtFloorDirection1.Text;
        kss.BaseboardQTY = txtBaseboardQTY1.Text;
        kss.BaseboardMOU = txtBaseboardMOU1.Text;
        kss.BaseboardStyle = txtBaseboardStyle1.Text;
        kss.BaseboardColor = txtBaseboardColor1.Text;
        kss.BaseboardSize = txtBaseboardSize1.Text;
        kss.BaseboardVendor = txtBaseboardVendor1.Text;
        kss.FloorGroutColor = txtFloorGroutColor1.Text;
        kss.UpdateBy = User.Identity.Name;
        kss.LastUpdateDate = DateTime.Now;

        if (_db.KitchenSheetSelections.Where(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value)).SingleOrDefault() == null)
        {
            _db.KitchenSheetSelections.InsertOnSubmit(kss);
        }
        _db.SubmitChanges();
        lblKitchen.Text = csCommonUtility.GetSystemMessage("Data saved successfully.");
    }
    protected void btnSaveShowerSheet_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSaveShowerSheet.ID, btnSaveShowerSheet.GetType().Name, "Click"); 
        DataClassesDataContext _db = new DataClassesDataContext();

        ShowerSheetSelection sss = new ShowerSheetSelection();
        //string strQ = "DELETE ShowerSheetSelection WHERE customer_id=" + Convert.ToInt32(hdnCustomerId.Value) + " AND estimate_id=" + Convert.ToInt32(hdnEstimateId.Value);
        //_db.ExecuteCommand(strQ, string.Empty);
        //_db.SubmitChanges();
        if (_db.ShowerSheetSelections.Where(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value)).SingleOrDefault() != null)
        {
            sss = _db.ShowerSheetSelections.SingleOrDefault(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value));
        }

        sss.customer_id = Convert.ToInt32(hdnCustomerId.Value);
        sss.estimate_id = Convert.ToInt32(hdnEstimateId.Value);
        sss.WallTileQTY = txtWallQTY1.Text;
        sss.WallTileMOU = txtWallMOU1.Text;
        sss.WallTileStyle = txtWallStyle1.Text;
        sss.WallTileColor = txtWallColor1.Text;
        sss.WallTileSize = txtWallSize1.Text;
        sss.WallTileVendor = txtWallVendor1.Text;
        sss.WallTilePattern = txtWallPattern1.Text;
        sss.WallTileGroutColor = txtWallGroutColor1.Text;
        sss.WBullnoseQTY = txtSWBullnoseQTY1.Text;
        sss.WBullnoseMOU = txtSWBullnoseMOU1.Text;
        sss.WBullnoseStyle = txtSWBullnoseStyle1.Text;
        sss.WBullnoseColor = txtSWBullnoseColor1.Text;
        sss.WBullnoseSize = txtSWBullnoseSize1.Text;
        sss.WBullnoseVendor = txtSWBullnoseVendor1.Text;
        sss.SchluterNOSticks = txtSchluterNOSticks2.Text;
        sss.SchluterColor = txtSchluterColor2.Text;
        sss.SchluterProfile = txtSchluterProfile2.Text;
        sss.SchluterThickness = txtSchluterThicknes2.Text;
        sss.ShowerPanQTY = txtShowerPanQTY1.Text;
        sss.ShowerPanMOU = txtShowerPanMOU1.Text;
        sss.ShowerPanStyle = txtShowerPanStyle1.Text;
        sss.ShowerPanColor = txtShowerPanColor1.Text;
        sss.ShowerPanSize = txtShowerPanSize1.Text;
        sss.ShowerPanVendor = txtShowerPanVendor1.Text;
        sss.ShowerPanPattern = "";
        sss.ShowerPanGroutColor = txtShowerPanGroutColor1.Text;
        sss.DecobandQTY = txtDecobandQTY1.Text;
        sss.DecobandMOU = txtDecobandMOU1.Text;
        sss.DecobandStyle = txtDecobandStyle1.Text;
        sss.DecobandColor = txtDecobandColor1.Text;
        sss.DecobandSize = txtDecobandSize1.Text;
        sss.DecobandVendor = txtDecobandVendor1.Text;
        sss.DecobandHeight = txtDecobandHeight1.Text;
        sss.NicheTileQTY = txtNicheTileQTY1.Text;
        sss.NicheTileMOU = txtNicheTileMOU1.Text;
        sss.NicheTileStyle = txtNicheTileStyle1.Text;
        sss.NicheTileColor = txtNicheTileColor1.Text;
        sss.NicheTileSize = txtNicheTileSize1.Text;
        sss.NicheTileVendor = txtNicheTileVendor1.Text;
        sss.NicheLocation = txtNicheLocation1.Text;
        sss.NicheSize = txtNicheSize1.Text;
        sss.BenchTileQTY = txtBenchTileQTY1.Text;
        sss.BenchTileMOU = txtBenchTileMOU1.Text;
        sss.BenchTileStyle = txtBenchTileStyle1.Text;
        sss.BenchTileColor = txtBenchTileColor1.Text;
        sss.BenchTileSize = txtBenchTileSize1.Text;
        sss.BenchTileVendor = txtBenchTileVendor1.Text;
        sss.BenchLocation = txtBenchLocation1.Text;
        sss.BenchSize = txtBenchSize1.Text;
        sss.FloorQTY = txtFloorQTY2.Text;
        sss.FloorMOU = txtFloorMOU2.Text;
        sss.FloorStyle = txtFloorStyle2.Text;
        sss.FloorColor = txtFloorColor2.Text;
        sss.FloorSize = txtFloorSize2.Text;
        sss.FloorVendor = txtFloorVendor2.Text;
        sss.FloorPattern = txtFloorPattern2.Text;
        sss.FloorDirection = txtFloorDirection2.Text;
        sss.BaseboardQTY = txtBaseboardQTY2.Text;
        sss.BaseboardMOU = txtBaseboardMOU2.Text;
        sss.BaseboardStyle = txtBaseboardStyle2.Text;
        sss.BaseboardColor = txtBaseboardColor2.Text;
        sss.BaseboardSize = txtBaseboardSize2.Text;
        sss.BaseboardVendor = txtBaseboardVendor2.Text;
        sss.FloorGroutColor = txtFloorGroutColor2.Text;
        sss.TileTo = txtTileto2.Text;
        sss.UpdateBy = User.Identity.Name;
        sss.LastUpdateDate = DateTime.Now;
        if (_db.ShowerSheetSelections.Where(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value)).SingleOrDefault() == null)
        {
            _db.ShowerSheetSelections.InsertOnSubmit(sss);
        }
        _db.SubmitChanges();
        lblShower.Text = csCommonUtility.GetSystemMessage("Data saved successfully.");


    }
    protected void btnSaveTubSheet_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSaveTubSheet.ID, btnSaveTubSheet.GetType().Name, "Click"); 
        DataClassesDataContext _db = new DataClassesDataContext();

        TubSheetSelection tss = new TubSheetSelection();
        //string strQ = "DELETE TubSheetSelection WHERE customer_id=" + Convert.ToInt32(hdnCustomerId.Value) + " AND estimate_id=" + Convert.ToInt32(hdnEstimateId.Value);
        //_db.ExecuteCommand(strQ, string.Empty);
        //_db.SubmitChanges();
        if (_db.TubSheetSelections.Where(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value)).SingleOrDefault() != null)
        {
            tss = _db.TubSheetSelections.Single(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value));
        }

        tss.customer_id = Convert.ToInt32(hdnCustomerId.Value);
        tss.estimate_id = Convert.ToInt32(hdnEstimateId.Value);
        tss.WallTileQTY = txtWallQTY3.Text;
        tss.WallTileMOU = txtWallMOU3.Text;
        tss.WallTileStyle = txtWallStyle3.Text;
        tss.WallTileColor = txtWallColor3.Text;
        tss.WallTileSize = txtWallSize3.Text;
        tss.WallTileVendor = txtWallVendor3.Text;
        tss.WallTilePattern = txtWallPattern3.Text;
        tss.WallTileGroutColor = txtWallGroutColor3.Text;
        tss.WBullnoseQTY = txtWBullnoseQTY3.Text;
        tss.WBullnoseMOU = txtWBullnoseMOU3.Text;
        tss.WBullnoseStyle = txtWBullnoseStyle3.Text;
        tss.WBullnoseColor = txtWBullnoseColor3.Text;
        tss.WBullnoseSize = txtWBullnoseSize3.Text;
        tss.WBullnoseVendor = txtWBullnoseVendor3.Text;
        tss.SchluterNOSticks = txtSchluterNOSticks3.Text;
        tss.SchluterColor = txtSchluterColor3.Text;
        tss.SchluterProfile = txtSchluterProfile3.Text;
        tss.SchluterThickness = txtSchluterThicknes3.Text;
        tss.DecobandQTY = txtDecobandQTY3.Text;
        tss.DecobandMOU = txtDecobandMOU3.Text;
        tss.DecobandStyle = txtDecobandStyle3.Text;
        tss.DecobandColor = txtDecobandColor3.Text;
        tss.DecobandSize = txtDecobandSize3.Text;
        tss.DecobandVendor = txtDecobandVendor3.Text;
        tss.DecobandHeight = txtDecobandHeight3.Text;
        tss.NicheTileQTY = txtNicheTileQTY3.Text;
        tss.NicheTileMOU = txtNicheTileMOU3.Text;
        tss.NicheTileStyle = txtNicheTileStyle3.Text;
        tss.NicheTileColor = txtNicheTileColor3.Text;
        tss.NicheTileSize = txtNicheTileSize3.Text;
        tss.NicheTileVendor = txtNicheTileVendor3.Text;
        tss.NicheLocation = txtNicheLocation3.Text;
        tss.NicheSize = txtNicheSize3.Text;
        tss.ShelfLocation = txtShelfLocation3.Text;
        tss.FloorQTY = txtFloorQTY3.Text;
        tss.FloorMOU = txtFloorMOU3.Text;
        tss.FloorStyle = txtFloorStyle3.Text;
        tss.FloorColor = txtFloorColor3.Text;
        tss.FloorSize = txtFloorSize3.Text;
        tss.FloorVendor = txtFloorVendor3.Text;
        tss.FloorPattern = txtFloorPattern3.Text;
        tss.FloorDirection = txtFloorDirection3.Text;
        tss.BaseboardQTY = txtBaseboardQTY3.Text;
        tss.BaseboardMOU = txtBaseboardMOU3.Text;
        tss.BaseboardStyle = txtBaseboardStyle3.Text;
        tss.BaseboardColor = txtBaseboardColor3.Text;
        tss.BaseboardSize = txtBaseboardSize3.Text;
        tss.BaseboardVendor = txtBaseboardVendor3.Text;
        tss.FloorGroutColor = txtFloorGroutColor3.Text;
        tss.TileTo = txtTileto3.Text;
        tss.UpdateBy = User.Identity.Name;
        tss.LastUpdateDate = DateTime.Now;
        if (_db.TubSheetSelections.Where(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value)).SingleOrDefault() == null)
        {
            _db.TubSheetSelections.InsertOnSubmit(tss);
        }
        _db.SubmitChanges();
        lblTub.Text = csCommonUtility.GetSystemMessage("Data saved successfully.");


    }


    protected void btnSaveBathroom_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSaveBathroom.ID, btnSaveBathroom.GetType().Name, "Click"); 
        DataClassesDataContext _db = new DataClassesDataContext();

        BathroomSheetSelection bss = new BathroomSheetSelection();

        if (_db.BathroomSheetSelections.Where(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value)).SingleOrDefault() != null)
        {
            bss = _db.BathroomSheetSelections.Single(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value));
        }

        bss.customer_id = Convert.ToInt32(hdnCustomerId.Value);
        bss.estimate_id = Convert.ToInt32(hdnEstimateId.Value);
        bss.Sink_Qty = txtSinkQty.Text;
        bss.Sink_Style = txtSinkStyle.Text;
        bss.Sink_WhereToOrder = txtSinkOrder.Text;
        bss.Sink_Fuacet_Qty = txtSinkFaucentQty.Text;
        bss.Sink_Fuacet_Style = txtSinkFaucentStyle.Text;
        bss.Sink_Fuacet_WhereToOrder = txtSinkFaucentOrder.Text;
        bss.Sink_Drain_Qty = txtSinkDrainQty.Text;
        bss.Sink_Drain_Style = txtSinkDrainStyle.Text;
        bss.Sink_Drain_WhereToOrder = txtSinkdrainOrder.Text;
        bss.Sink_Valve_Qty = txtSinkValveQty.Text;
        bss.Sink_Valve_Style = txtSinkValveStyle.Text;
        bss.Sink_Valve_WhereToOrder = txtSinkValveOrder.Text;
        bss.Bathtub_Qty = txtBathTubQty.Text;
        bss.Bathtub_Style = txtBathTubStyle.Text;
        bss.Bathtub_WhereToOrder = txtBathTubOrder.Text;
        bss.Tub_Faucet_Qty = txtTubFaucentQty.Text;
        bss.Tub_Faucet_Style = txtTubFaucentStyle.Text;
        bss.Tub_Faucet_WhereToOrder = txtTubFaucentOrder.Text;
        bss.Tub_Valve_Qty = txtTubValveQty.Text;
        bss.Tub_Valve_Style = txtTubValveStyle.Text;
        bss.Tub_Valve_WhereToOrder = txtTubValveOrder.Text;
        bss.Tub_Drain_Qty = txtTubDrainQty.Text;
        bss.Tub_Drain_Style = txtTubDrainStyle.Text;
        bss.Tub_Drain_WhereToOrder = txtTubDrainOrder.Text;
        bss.Tollet_Qty = txtToiletQty.Text;
        bss.Tollet_Style = txtToiletStyle.Text;
        bss.Tollet_WhereToOrder = txtToiletOrder.Text;
        bss.Shower_TubSystem_Qty = txtShower_TubSystemQty.Text;
        bss.Shower_TubSystem_Style = txtShower_TubSystemStyle.Text;
        bss.Shower_TubSystem_WhereToOrder = txtShower_TubSystemOrder.Text;
        bss.Shower_Value_Qty = txtShowerValveQty.Text;
        bss.Shower_Value_Style = txtShowerValveStyle.Text;
        bss.Shower_Value_WhereToOrder = txtShowerValveOrder.Text;
        bss.Handheld_Shower_Qty = txtHandheldShowerQty.Text;
        bss.Handheld_Shower_Style = txtHandheldShowerStyle.Text;
        bss.Handheld_Shower_WhereToOrder = txtHandheldShowerOrder.Text;
        bss.Body_Spray_Qty = txtBodySprayQty.Text;
        bss.Body_Spray_Style = txtBodySprayStyle.Text;
        bss.Body_Spray_WhereToOrder = txtBodySprayOrder.Text;
        bss.Body_Spray_Valve_Qty = txtBodySprayValveQty.Text;
        bss.Body_Spray_Valve_Style = txtBodySprayValveStyle.Text;
        bss.Body_Spray_Valve_WhereToOrder = txtBodySprayValveOrder.Text;
        bss.Shower_Drain_Qty = txtShowerDrainQty.Text;
        bss.Shower_Drain_Style = txtShowerDrainStyle.Text;
        bss.Shower_Drain_WhereToOrder = txtShowerDrainOrder.Text;
        bss.Shower_Drain_Body_Plug_Qty = txtShowerDrainBody_PlugQty.Text;
        bss.Shower_Drain_Body_Plug_Style = txtShowerDrainBody_PlugStyle.Text;
        bss.Shower_Drain_Body_Plug_WhereToOrder = txtShowerDrainBody_PlugOrder.Text;
        bss.Shower_Drain_Cover_Qty = txtShowerDrainCoverQty.Text;
        bss.Shower_Drain_Cover_Style = txtShowerDrainCoverStyle.Text;
        bss.Shower_Drain_Cover_WhereToOrder = txtShowerDrainCoverOrder.Text;
        bss.Counter_Top_Qty = txtCounterTopQty.Text;
        bss.Counter_Top_Style = txtCounterTopStyle.Text;
        bss.Counter_Top_WhereToOrder = txtCounterTopOrder.Text;
        bss.Counter_To_Edge_Qty = txtCounterTopEdgeQty.Text;
        bss.Counter_To_Edge_Style = txtCounterTopEdgeStyle.Text;
        bss.Counter_To_Edge_WhereToOrder = txtCounterTopEdgeOrder.Text;
        bss.Counter_Top_Overhang_Qty = txtCounterTop_OverhangQty.Text;
        bss.Counter_Top_Overhang_Style = txtCounterTop_OverhangStyle.Text;
        bss.Counter_Top_Overhang_WhereToOrder = txtCounterTop_OverhangOrder.Text;
        bss.AdditionalPlacesGettingCountertop_Qty = txtAdditionalplacesgettingcountertopQty.Text;
        bss.AdditionalPlacesGettingCountertop_Style = txtAdditionalplacesgettingcountertopStyle.Text;
        bss.AdditionalPlacesGettingCountertop_WhereToOrder = txtAdditionalplacesgettingcountertopOrder.Text;
        bss.Granite_Quartz_Backsplash_Qty = txtGranite_Quartz_BacksplashQty.Text;
        bss.Granite_Quartz_Backsplash_Style = txtGranite_Quartz_BacksplashStyle.Text;
        bss.Granite_Quartz_Backsplash_WhereToOrder = txtGranite_Quartz_BacksplashOrder.Text;
        bss.Tub_Wall_Tile_Qty = txtTubwalltileQty.Text;
        bss.Tub_Wall_Tile_Style = txtTubwalltileStyle.Text;
        bss.Tub_Wall_Tile_WhereToOrder = txtTubwalltileOrder.Text;
        bss.Wall_Tile_Layout_Qty = txtWallTilelayoutQty.Text;
        bss.Wall_Tile_Layout_Style = txtWallTilelayoutStyle.Text;
        bss.Wall_Tile_Layout_WhereToOrder = txtWallTilelayoutOrder.Text;
        bss.Tub_skirt_tile_Qty = txtTubskirttileQty.Text;
        bss.Tub_skirt_tile_Style = txtTubskirttileStyle.Text;
        bss.Tub_skirt_tile_WhereToOrder = txtTubskirttileOrder.Text;
        bss.Shower_Wall_Tile_Qty = txtShowerWallTileQty.Text;
        bss.Shower_Wall_Tile_Style = txtShowerWallTileStyle.Text;
        bss.Shower_Wall_Tile_WhereToOrder = txtShowerWallTileOrder.Text;

        bss.Wall_Tile_Layout_Qty = txtWall_Tile_layoutQty.Text;
        bss.Wall_Tile_Layout_Style = txtWall_Tile_layoutStyle.Text;
        bss.Wall_Tile_Layout_WhereToOrder = txtWall_Tile_layoutOrder.Text;

        bss.Shower_Floor_Tile_Qty = txtShowerFloorTileQty.Text;
        bss.Shower_Floor_Tile_Style = txtShowerFloorTileStyle.Text;
        bss.Shower_Floor_Tile_WhereToOrder = txtShowerFloorTileOrder.Text;
        bss.Shower_Tub_Tile_Height_Qty = txtShowerTubTileHeightQty.Text;
        bss.Shower_Tub_Tile_Height_Style = txtShowerTubTileHeightStyle.Text;
        bss.Shower_Tub_Tile_Height_WhereToOrder = txtShowerTubTileHeightOrder.Text;
        bss.Floor_Tile_Qty = txtFloorTiletQty.Text;
        bss.Floor_Tile_Style = txtFloorTiletstyle.Text;
        bss.Floor_Tile_WhereToOrder = txtFloorTiletOrder.Text;
        bss.Floor_Tile_layout_Qty = txtFloorTilelayoutQty.Text;
        bss.Floor_Tile_layout_Style = txtFloorTilelayoutStyle.Text;
        bss.Floor_Tile_layout_WhereToOrder = txtFloorTilelayoutOrder.Text;
        bss.BullnoseTile_Qty = txtBullnoseTileQty.Text;
        bss.BullnoseTile_Style = txtBullnoseTileStyle.Text;
        bss.BullnoseTile_WhereToOrder = txtBullnoseTileOrder.Text;
        bss.Deco_Band_Qty = txtDecobandQty.Text;
        bss.Deco_Band_Style = txtDecobandStyle.Text;
        bss.Deco_Band_WhereToOrder = txtDecobandOrder.Text;
        bss.Deco_Band_Height_Qty = txtDecobandHeightQty.Text;
        bss.Deco_Band_Height_Style = txtDecobandHeightStyle.Text;
        bss.Deco_Band_Height_WhereToOrder = txtDecobandHeightOrder.Text;
        bss.Tile_Baseboard_Qty = txtTileBaseboardQty.Text;
        bss.Tile_Baseboard_Style = txtTileBaseboardStyle.Text;
        bss.Tile_Baseboard_WhereToOrder = txtTileBaseboardOrder.Text;
        bss.Grout_Selection_Qty = txtGroutSelectionQty.Text;
        bss.Grout_Selection_Style = txtGroutSelectionStyle.Text;
        bss.Grout_Selection_WhereToOrder = txtGroutSelectionOrder.Text;
        bss.Niche_Location_Qty = txtNicheLocationQty.Text;
        bss.Niche_Location_Style = txtNicheLocationStyle.Text;
        bss.Niche_Location_WhereToOrder = txtNicheLocationOrder.Text;
        bss.Niche_Size_Qty = txtNicheSizeQty.Text;
        bss.Niche_Size_Style = txtNicheSizeStyle.Text;
        bss.Niche_Size_WhereToOrder = txtNicheSizeOrder.Text;
        bss.Glass_Qty = txtGlassQty.Text;
        bss.Glass_Style = txtGlassStyle.Text;
        bss.Glass_WhereToOrder = txtGlassOrder.Text;
        bss.Window_Qty = txtWindowQty.Text;
        bss.Window_Style = txtWindowStyle.Text;
        bss.Window_WhereToOrder = txtWindowOrder.Text;
        bss.Door_Qty = txtDoorQty.Text;
        bss.Door_Style = txtDoorStyle.Text;
        bss.Door_WhereToOrder = txtDoorOrder.Text;
        bss.Grab_Bar_Qty = txtGrabBarQty.Text;
        bss.Grab_Bar_Style = txtGrabBarStyle.Text;
        bss.Grab_Bar_WhereToOrder = txtGrabBarOrder.Text;
        bss.Cabinet_Door_Style_Color_Qty = txtCabinetDoorStyleColorQty.Text;
        bss.Cabinet_Door_Style_Color_Style = txtCabinetDoorStyleColorStyle.Text;
        bss.Cabinet_Door_Style_Color_WhereToOrder = txtCabinetDoorStyleColorOrder.Text;
        bss.Medicine_Cabinet_Qty = txtMedicineCabinetQty.Text;
        bss.Medicine_Cabinet_Style = txtMedicineCabinetStyle.Text;
        bss.Medicine_Cabinet_WhereToOrder = txtMedicineCabinetOrder.Text;
        bss.Mirror_Qty = txtMirrorQty.Text;
        bss.Mirror_Style = txtMirrorStyle.Text;
        bss.Mirror_WhereToOrder = txtMirrorOrder.Text;
        bss.Wood_Baseboard_Qty = txtWoodBaseboardQty.Text;
        bss.Wood_Baseboard_Style = txtWoodBaseboardStyle.Text;
        bss.Wood_Baseboard_WhereToOrder = txtWoodBaseboardOrder.Text;
        bss.Paint_Color_Qty = txtPaintColorQty.Text;
        bss.Paint_Color_Style = txtPaintColorStyle.Text;
        bss.Paint_Color_WhereToOrder = txtPaintColorOrder.Text;
        bss.Lighting_Qty = txtLightingQty.Text;
        bss.Lighting_Style = txtLightingStyle.Text;
        bss.Lighting_WhereToOrder = txtLightingOrder.Text;
        bss.Hardware_Qty = txtHardwareQty.Text;
        bss.Hardware_Style = txtHardwareStyle.Text;
        bss.Hardware_WhereToOrder = txtHardwareOrder.Text;
        bss.Special_Notes = txtBathpecialNotes.Text;
        bss.UpdateBy = User.Identity.Name;
        bss.LastUpdatedDate = DateTime.Now;
        if (_db.BathroomSheetSelections.Where(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value)).SingleOrDefault() == null)
        {
            _db.BathroomSheetSelections.InsertOnSubmit(bss);
        }
        _db.SubmitChanges();
        lblBathroom.Text = csCommonUtility.GetSystemMessage("Data saved successfully.");


    }

    protected void btnSaveKitchen_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSaveKitchen.ID, btnSaveKitchen.GetType().Name, "Click"); 
        DataClassesDataContext _db = new DataClassesDataContext();

        KitchenSelection kss = new KitchenSelection();

        if (_db.KitchenSelections.Where(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value)).SingleOrDefault() != null)
        {
            kss = _db.KitchenSelections.Single(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value));
        }

        kss.customer_id = Convert.ToInt32(hdnCustomerId.Value);
        kss.estimate_id = Convert.ToInt32(hdnEstimateId.Value);
        kss.Sink_Qty = txtKitchenSinkQty.Text;
        kss.Sink_Style = txtKitchenSinkStyle.Text;
        kss.Sink_WhereToOrder = txtKitchenSinkOrder.Text;
        kss.Sink_Fuacet_Qty = txtKitchenSinkFaucetQty.Text;
        kss.Sink_Fuacet_Style = txtKitchenSinkFaucetStyle.Text;
        kss.Sink_Fuacet_WhereToOrder = txtKitchenSinkFaucetOrder.Text;
        kss.Sink_Drain_Qty = txtKitchenSinkDrainQty.Text;
        kss.Sink_Drain_Style = txtKitchenSinkDrainStyle.Text;
        kss.Sink_Drain_WhereToOrder = txtKitchenSinkDrainOrder.Text;
        kss.Counter_Top_Qty = txtKitchenCounterTopQty.Text;
        kss.Counter_Top_Style = txtKitchenCounterTopStyle.Text;
        kss.Counter_Top_WhereToOrder = txtKitchenCounterTopOrder.Text;
        kss.Granite_Quartz_Backsplash_Qty = txtKitchenGraniteQuartzBacksplashQty.Text;
        kss.Granite_Quartz_Backsplash_Style = txtKitchenGraniteQuartzBacksplashStyle.Text;
        kss.Granite_Quartz_Backsplash_WhereToOrder = txtKitchenGraniteQuartzBacksplashOrder.Text;
        kss.Counter_Top_Overhang_Qty = txtKitchenCounterTopOverhangQty.Text;
        kss.Counter_Top_Overhang_Style = txtKitchenCounterTopOverhangStyle.Text;
        kss.Counter_Top_Overhang_WhereToOrder = txtKitchenCounterTopOverhangOrder.Text;
        kss.AdditionalPlacesGettingCountertop_Qty = txtKitchenAdditionalplacesgettingcountertopQty.Text;
        kss.AdditionalPlacesGettingCountertop_Style = txtKitchenAdditionalplacesgettingcountertopStyle.Text;
        kss.AdditionalPlacesGettingCountertop_WhereToOrder = txtKitchenAdditionalplacesgettingcountertopOrder.Text;
        kss.Counter_To_Edge_Qty = txtKitchenCounterTopEdgeQty.Text;
        kss.Counter_To_Edge_Style = txtKitchenCounterTopEdgeStyle.Text;
        kss.Counter_To_Edge_WhereToOrder = txtKitchenCounterTopEdgeOrder.Text;
        kss.Cabinets_Qty = txtKitchenCabinetsQty.Text;
        kss.Cabinets_Style = txtKitchenCabinetsStyle.Text;
        kss.Cabinets_WhereToOrder = txtKitchenCabinetsOrder.Text;
        kss.Disposal_Qty = txtKitchenDisposalQty.Text;
        kss.Disposal_Style = txtKitchenDisposalStyle.Text;
        kss.Disposal_WhereToOrder = txtKitchenDisposalOrder.Text;
        kss.Baseboard_Qty = txtKitchenBaseboardQty.Text;
        kss.Baseboard_Style = txtKitchenBaseboardStyle.Text;
        kss.Baseboard_WhereToOrder = txtKitchenBaseboardOrder.Text;
        kss.Window_Qty = txtKitchenWindowsQty.Text;
        kss.Window_Style = txtKitchenWindowsStyle.Text;
        kss.Window_WhereToOrder = txtKitchenWindowsOrder.Text;
        kss.Door_Qty = txtKitchenDoorsQty.Text;
        kss.Door_Style = txtKitchenDoorsStyle.Text;
        kss.Door_WhereToOrder = txtKitchenDoorsOrder.Text;
        kss.Lighting_Qty = txtKitchenLightingQty.Text;
        kss.Lighting_Style = txtKitchenLightingStyle.Text;
        kss.Lighting_WhereToOrder = txtKitchenLightingOrder.Text;
        kss.Hardware_Qty = txtKitchenHardwareQty.Text;
        kss.Hardware_Style = txtKitchenHardwareStyle.Text;
        kss.Hardware_WhereToOrder = txtKitchenHardwareOrder.Text;
        kss.Special_Notes = txtKitchenSpecialNotes.Text;
        kss.UpdateBy = User.Identity.Name;
        kss.LastUpdatedDate = DateTime.Now;
        if (_db.KitchenSelections.Where(cp => cp.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cp.estimate_id == Convert.ToInt32(hdnEstimateId.Value)).SingleOrDefault() == null)
        {
            _db.KitchenSelections.InsertOnSubmit(kss);
        }
        _db.SubmitChanges();
        lblKitchen2.Text = csCommonUtility.GetSystemMessage("Data saved successfully.");

    }

    #endregion


    protected void chkSelection_CheckedChanged(object sender, EventArgs e)
    {

        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, chkKitchen.ID, chkKitchen.GetType().Name, "Change");
        bool IsInsert = true;
        DataClassesDataContext _db = new DataClassesDataContext();
        SelectionSheetMaster objSSM = new SelectionSheetMaster();

        if (_db.SelectionSheetMasters.Any(ss => ss.customer_id == Convert.ToInt32(hdnCustomerId.Value) && ss.estimate_id == Convert.ToInt32(hdnEstimateId.Value)))
        {
            objSSM = _db.SelectionSheetMasters.SingleOrDefault(ss => ss.customer_id == Convert.ToInt32(hdnCustomerId.Value) && ss.estimate_id == Convert.ToInt32(hdnEstimateId.Value));
            IsInsert = false;
        }

        objSSM.customer_id = Convert.ToInt32(hdnCustomerId.Value);
        objSSM.estimate_id = Convert.ToInt32(hdnEstimateId.Value);
        objSSM.IsCabinet = chkCabinet.Checked;
        objSSM.IsKitchen = chkKitchen.Checked;
        objSSM.IsShower = chkShower.Checked;
        objSSM.IsTub = chkTub.Checked;
        objSSM.IsBath = chkBathroom.Checked;
        objSSM.IsKitchen2 = chkitchen2.Checked;
        objSSM.LastUpdateDate = DateTime.Now;
        objSSM.UpdateBy = User.Identity.Name;

        if (IsInsert)
        {
            _db.SelectionSheetMasters.InsertOnSubmit(objSSM);
        }
        _db.SubmitChanges();
    }

}