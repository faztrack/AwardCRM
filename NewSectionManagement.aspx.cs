﻿using System;
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
using System.Reflection;


public partial class NewSectionManagement : System.Web.UI.Page
{
    string strDetails = "";
    string strTest = String.Empty;
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        DateTime end = (DateTime)Session["loadstarttime"];
        TimeSpan loadtime = DateTime.Now - end;
        lblLoadTime.Text = (Math.Round(Convert.ToDecimal(loadtime.TotalSeconds), 3).ToString()) + " Sec";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Add("loadstarttime", DateTime.Now);
        LoadEvent();
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
 
            if (Session["oUser"] == null)
            {
                Response.Redirect("AwardCRMLogin.aspx");
            }
            if (Page.User.IsInRole("admin004") == false)
            {
                // No Permission Page.
                Response.Redirect("nopermission.aspx");
            }

            DataClassesDataContext _db = new DataClassesDataContext();
            company_profile objCp = _db.company_profiles.SingleOrDefault(cp => cp.client_id == 1);
            hdnMultiplier.Value = objCp.markup.ToString();
            LoadTree();
           // test.SaveText(strTest);
            LoadMainSectionInfo();
            lblTree.Visible = false;
            btnAddnewRow.Visible = true;
            if (Convert.ToInt32(hdnParentId.Value) > 0)
            {
                lblTree.Visible = true;
                lblSubSection.Visible = true;
                lblItemList.Visible = true;
                grdSubSection.Visible = true;
                grdItem_Price.Visible = true;
                LoadSubSectionInfo();
                LoadItemInfo();

            }
           
        }
    }


    private void LoadEvent()
    {
        string s2 = @"var elem = document.getElementById('{0}_SelectedNode');
                          if(elem != null )
                          {
                                var node = document.getElementById(elem.value);
                                if(node != null)
                                {
                                     node.scrollIntoView(true);
                                }
                          }
                        ";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "myscript", s2.Replace("{0}", trvSection.ClientID), true);
    }

    private void LoadTree()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        //string strQ = " SELECT * FROM sectioninfo WHERE  client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + " AND section_id NOT IN (SELECT item_id FROM item_price WHERE client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + ")";
        //IEnumerable<sectioninfo> list = _db.ExecuteQuery<sectioninfo>(strQ, string.Empty);

        List<sectioninfo> list = (from s in _db.sectioninfos
                                  where s.client_id == 1 && s.is_disable == false 
                                  && !(from i in _db.item_prices where i.client_id == 1
                                  select i.item_id).Contains(s.section_id)
                                  select s).ToList();

        trvSection.Nodes.Clear();

        foreach (sectioninfo sec in list)
        {
            string name = sec.section_name;
            if (sec.parent_id == 0)
            {
                TreeNode node = new TreeNode(sec.section_name, sec.section_id.ToString());
                trvSection.Nodes.Add(node);
                AddChildMenu(node, sec);

                //Test
                //if (!_db.sectioninfos.Any(s => s.parent_id == sec.section_id && s.parent_id != 0))
                //{
                //    strTest += sec.section_id.ToString() + ", ";
                //}
            }
        }

    }
    //bool nBlock = false;
    //public void LoadNewTreeNode()
    //{
    //    LoadTree();
    //    DataClassesDataContext _db = new DataClassesDataContext();

    //    //after new Node Add----------------------------------------------------------------
    //    var sMaxID = _db.sectioninfos.Select(s => s.section_auto_id).Max();
    //    var sName = _db.sectioninfos.Where(s => s.section_auto_id == sMaxID).SingleOrDefault();

    //    if (nBlock == true)  // stop recursive re-entrancy, For Expand
    //        return;
    //    nBlock = true;
    //    trvSection.CollapseAll();        

    //    TreeNode nNode = FindNode(trvSection.Nodes, sName.section_name);

    //    while (nNode != null) // For Expand
    //    {
    //        nNode.Expand();
    //        nNode = nNode.Parent;
    //    }
    //    nBlock = false;
    //}

    //TreeNode n_found_node = null;
    //bool b_node_found = false;
    //bool b_cNode_found = false;
    //public TreeNode FindNode(TreeNodeCollection nCollection, string sectionName)
    //{        

    //    foreach (TreeNode node in nCollection)
    //    {
    //        if (node.Text.ToString() == sectionName)
    //        {
    //            b_node_found = true;
    //            n_found_node = node;                
    //            break;
    //        }
    //        if (!b_node_found)
    //        {
    //            //n_found_node = FindNode(node.ChildNodes, sectionName);
    //            foreach (TreeNode cNode in node.ChildNodes)
    //            {
    //                if (cNode.Text.ToString() == sectionName)
    //                {
    //                    b_cNode_found = true;
    //                    n_found_node = node;                        
    //                    break;
    //                }
    //                if (!b_cNode_found)
    //                {
    //                    n_found_node = FindNode(cNode.ChildNodes, sectionName);
    //                }
    //            }
    //        }
    //    }
    //    return n_found_node;
    //}

    private void AddChildMenu(TreeNode parentNode, sectioninfo sec)
    {
       
        DataClassesDataContext _db = new DataClassesDataContext();
        string strQ = " SELECT * FROM sectioninfo WHERE is_disable = 0 AND client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + " AND section_id NOT IN (SELECT item_id FROM item_price WHERE client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + ")";
        IEnumerable<sectioninfo> list = _db.ExecuteQuery<sectioninfo>(strQ, string.Empty);
        foreach (sectioninfo subsec in list)
        {
            if (subsec.parent_id.ToString() == parentNode.Value)
            {
                TreeNode node = new TreeNode(subsec.section_name, subsec.section_id.ToString());
                parentNode.ChildNodes.Add(node);
                AddChildMenu(node, subsec);

                ////Test
                //if (!_db.sectioninfos.Any(s => s.parent_id == subsec.section_id))
                //{
                //    strTest += subsec.section_id.ToString() + ", ";
                //}
            }
        }
    }



    protected void btnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("customerlist.aspx");
    }


    protected void trvSection_SelectedNodeChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, trvSection.ID, trvSection.GetType().Name, "Change");

        hdnSectionId.Value = trvSection.SelectedValue;
        hdnTrvSelectedValue.Value = trvSection.SelectedValue;
        hdnSubItemParentId.Value = trvSection.SelectedValue;
        lblResult.Text = "";
        lblItemResult.Text = "";
        lblMainSecResult.Text = "";
        lblSubSecResult.Text = "";

        btnAddSubnewRow.Visible = true;
        btnAddItem.Visible = true;
        lblTree.Visible = true;
        lblMainSection.Visible = false;
        btnAddnewRow.Visible = false;
        grdMainSection.Visible = false;
        BindGrid_SubSection_Item();
    }
    public string GetItemDetialsForUpdateItem(int SectionId)
    {

        DataClassesDataContext _db = new DataClassesDataContext();
        List<sectioninfo> list = _db.sectioninfos.Where(c => c.section_id == SectionId && c.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])).ToList();
        foreach (sectioninfo sec1 in list)
        {
            strDetails = sec1.section_name + " >> " + strDetails;
            GetItemDetialsForUpdateItem(Convert.ToInt32(sec1.parent_id));
        }
        return strDetails;
    }
    public void BindGrid_SubSection_Item()
    {

        DataClassesDataContext _db = new DataClassesDataContext();
        sectioninfo sinfo = new sectioninfo();
        sinfo = _db.sectioninfos.SingleOrDefault(c => c.section_id == Convert.ToInt32(hdnTrvSelectedValue.Value) && c.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));
        lblParent.Text = GetItemDetialsForUpdateItem(Convert.ToInt32(hdnTrvSelectedValue.Value));

        hdnSectionLevel.Value = Convert.ToInt32(sinfo.section_level).ToString();
        hdnParentId.Value = Convert.ToInt32(sinfo.parent_id).ToString();
        hdnSectionSerial.Value = Convert.ToDecimal(sinfo.section_serial).ToString();
        hdnSubItemParentId.Value = Convert.ToInt32(hdnTrvSelectedValue.Value).ToString();
        lblParent.ForeColor = Color.Blue;
        if (Convert.ToInt32(hdnSubItemParentId.Value) > 0)
        {
            lblSubSection.Visible = true;
            lblItemList.Visible = true;
            grdSubSection.Visible = true;
            grdItem_Price.Visible = true;
            LoadSubSectionInfo();
            LoadItemInfo();
        }


    }


    bool Block = false;
    protected void trvSection_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, trvSection.ID, trvSection.GetType().Name, "Change");
        
        if (Block == true)  // stop recursive re-entrancy
            return;
        Block = true;

        trvSection.CollapseAll();

        // expand current node and all parent nodes
        TreeNode Node = e.Node;
        while (Node != null)
        {
            Node.Expand();
            Node = Node.Parent;
        }
        Block = false;

    }


    #region  add new Sectiom

    private void LoadMainSectionInfo()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        DataTable tmpTable = LoadSectionTable();

        var item = from sin in _db.sectioninfos
                   where sin.client_id == 1 && sin.parent_id == 0 
                   select new SectionInfo()
                   {
                       section_id = (int)sin.section_id,
                       client_id = (int)sin.client_id,
                       section_name = sin.section_name,
                       parent_id = (int)sin.parent_id,
                       section_notes = sin.section_notes,
                       section_level = (int)sin.section_level,
                       section_serial = (decimal)sin.section_serial,
                       is_active = (bool)sin.is_active,
                       create_date = (DateTime)sin.create_date,
                       cssClassName = sin.cssClassName,
                       is_CommissionExclude = (bool)sin.is_CommissionExclude
                   };
        foreach (SectionInfo Sinfo in item)
        {

            DataRow drNew = tmpTable.NewRow();
            drNew["section_id"] = Sinfo.section_id;
            drNew["client_id"] = Sinfo.client_id;
            drNew["section_name"] = Sinfo.section_name;
            drNew["parent_id"] = Sinfo.parent_id;
            drNew["section_notes"] = Sinfo.section_notes;
            drNew["section_level"] = Sinfo.section_level;
            drNew["section_serial"] = Sinfo.section_serial;
            drNew["is_active"] = Sinfo.is_active;
            drNew["create_date"] = Sinfo.create_date;
            drNew["cssClassName"] = Sinfo.cssClassName;
            drNew["is_CommissionExclude"] = Sinfo.is_CommissionExclude;

            tmpTable.Rows.Add(drNew);
        }


        var result = (from sin in _db.sectioninfos
                      where sin.parent_id == 0 && sin.client_id == 1 
                      select sin.section_id);
        int nsectionId = 0;
        int n = result.Count();
        if (result != null && n > 0)
            nsectionId = result.Max();

        nsectionId = nsectionId + 1000;
        hdnSectionId.Value = nsectionId.ToString();
        hdnSectionLevel.Value = nsectionId.ToString();
        hdnSectionSerial.Value = nsectionId.ToString();

        if (item.Count() == 0)
        {
            DataRow drNew1 = tmpTable.NewRow();
            drNew1["section_id"] = Convert.ToInt32(hdnSectionId.Value);
            drNew1["client_id"] = 1;
            drNew1["section_name"] = "";
            drNew1["parent_id"] = 0;
            drNew1["section_notes"] = "";
            drNew1["section_level"] = Convert.ToInt32(hdnSectionLevel.Value);
            drNew1["section_serial"] = Convert.ToDecimal(hdnSectionSerial.Value);
            drNew1["is_active"] = true;
            drNew1["create_date"] = DateTime.Now;
            drNew1["cssClassName"] = "";
            drNew1["is_CommissionExclude"] = false;

            tmpTable.Rows.InsertAt(drNew1, 0);
        }

        Session.Add("MainSection", tmpTable);
        grdMainSection.DataSource = tmpTable;
        grdMainSection.DataKeyNames = new string[] { "section_id", "parent_id", "section_level", "section_serial", "cssClassName" };
        grdMainSection.DataBind();

    }
    private DataTable LoadSectionTable()
    {
        DataTable table = new DataTable();
        table.Columns.Add("section_id", typeof(int));
        table.Columns.Add("client_id", typeof(int));
        table.Columns.Add("section_name", typeof(string));
        table.Columns.Add("parent_id", typeof(int));
        table.Columns.Add("section_notes", typeof(string));
        table.Columns.Add("section_level", typeof(int));
        table.Columns.Add("section_serial", typeof(decimal));
        table.Columns.Add("is_active", typeof(bool));
        table.Columns.Add("create_date", typeof(DateTime));
        table.Columns.Add("cssClassName", typeof(string));
        table.Columns.Add("is_CommissionExclude", typeof(bool));

        

        return table;
    }

    protected void grdMainSection_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "Save")
        {
            DataClassesDataContext _db = new DataClassesDataContext();

            DataTable table = (DataTable)Session["MainSection"];

            foreach (GridViewRow di in grdMainSection.Rows)
            {
                {
                    CheckBox chkIsActive = (CheckBox)di.FindControl("chkIsActive");
                    TextBox txtSectionName = (TextBox)di.FindControl("txtSectionName");
                    Label lblSectionName = (Label)di.FindControl("lblSectionName");
                    DataRow dr = table.Rows[di.RowIndex];
                    DropDownList ddlcssClassName = (DropDownList)di.FindControl("ddlcssClassName");

                    CheckBox chkIsExcludeCom0 = (CheckBox)di.FindControl("chkIsExcludeCom0");
                    Label lblAExcludeCom0 = (Label)di.FindControl("lblAExcludeCom0");

                    dr["section_id"] = Convert.ToInt32(grdMainSection.DataKeys[di.RowIndex].Values[0]);
                    dr["client_id"] = 1;
                    dr["section_name"] = txtSectionName.Text;
                    dr["parent_id"] = Convert.ToInt32(grdMainSection.DataKeys[di.RowIndex].Values[1]);
                    dr["section_notes"] = "";
                    dr["section_level"] = Convert.ToInt32(grdMainSection.DataKeys[di.RowIndex].Values[2]);
                    dr["section_serial"] = Convert.ToDecimal(grdMainSection.DataKeys[di.RowIndex].Values[3]);
                    dr["is_active"] = Convert.ToBoolean(chkIsActive.Checked);                  
                    dr["create_date"] = DateTime.Now;
                    dr["cssClassName"] = ddlcssClassName.SelectedValue;// grdMainSection.DataKeys[di.RowIndex].Values[4].ToString();
                    dr["is_CommissionExclude"] = Convert.ToBoolean(chkIsExcludeCom0.Checked);     
                }

            }
            foreach (DataRow dr in table.Rows)
            {
                bool bFlagNew = false;

                sectioninfo SecInfo = _db.sectioninfos.SingleOrDefault(l => l.section_id == Convert.ToInt32(dr["section_id"]));
                if (SecInfo == null)
                {
                    SecInfo = new sectioninfo();
                    bFlagNew = true;
                    if (_db.sectioninfos.Where(l => l.client_id == 1 && l.parent_id == 0 && l.is_disable == false && l.section_name == dr["section_name"].ToString()).SingleOrDefault() != null)
                    {
                        lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Section name already exist. Please try another name.");
                        lblMainSecResult.Text = csCommonUtility.GetSystemRequiredMessage("Section name already exist. Please try another name.");
                        return;
                    }
                }

                string str = dr["section_name"].ToString().Trim();
                if (str.Length > 0)
                {
                    SecInfo.section_id = Convert.ToInt32(dr["section_id"]);
                    SecInfo.client_id = Convert.ToInt32(dr["client_id"]);
                    SecInfo.section_name = dr["section_name"].ToString();
                    SecInfo.parent_id = Convert.ToInt32(dr["parent_id"]);
                    SecInfo.section_notes = dr["section_notes"].ToString();
                    SecInfo.section_level = Convert.ToInt32(dr["section_level"]);
                    SecInfo.section_serial = Convert.ToDecimal(dr["section_serial"]);
                    SecInfo.is_active = Convert.ToBoolean(dr["is_active"]);
                    SecInfo.is_disable = false;
                    SecInfo.create_date = DateTime.Now;
                    SecInfo.is_mandatory = false;
                    SecInfo.is_CommissionExclude = Convert.ToBoolean(dr["is_CommissionExclude"]);
                    SecInfo.cssClassName = dr["cssClassName"].ToString();
                    if (bFlagNew)
                    {
                        _db.sectioninfos.InsertOnSubmit(SecInfo);
                    }
                   
                }
                else
                {
                    lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Section Name is a required field");
                    lblMainSecResult.Text = csCommonUtility.GetSystemRequiredMessage("Section Name is a required field");
                    return;
                }
               
            }
            lblResult.Text = csCommonUtility.GetSystemMessage("Data saved successfully");
            lblMainSecResult.Text = csCommonUtility.GetSystemMessage("Data saved successfully");
            _db.SubmitChanges();
            LoadMainSectionInfo();
            LoadTree();


        }
    }

    protected void grdMainSection_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        sectioninfo objSI = new sectioninfo();

        int nSectionId = Convert.ToInt32(grdMainSection.DataKeys[Convert.ToInt32(e.NewEditIndex)].Values[0]);


        CheckBox chkIsActive = (CheckBox)grdMainSection.Rows[e.NewEditIndex].FindControl("chkIsActive");
        TextBox txtSectionName = (TextBox)grdMainSection.Rows[e.NewEditIndex].FindControl("txtSectionName");
        Label lblSectionName = (Label)grdMainSection.Rows[e.NewEditIndex].FindControl("lblSectionName");

        Label lblcssClassName = (Label)grdMainSection.Rows[e.NewEditIndex].FindControl("lblcssClassName");
        DropDownList ddlcssClassName = (DropDownList)grdMainSection.Rows[e.NewEditIndex].FindControl("ddlcssClassName");

        CheckBox chkIsExcludeCom0 = (CheckBox)grdMainSection.Rows[e.NewEditIndex].FindControl("chkIsExcludeCom0");
        Label lblAExcludeCom0 = (Label)grdMainSection.Rows[e.NewEditIndex].FindControl("lblAExcludeCom0");

        if (_db.sectioninfos.Where(si => si.section_id == nSectionId).Count() > 0)
        {
            objSI = _db.sectioninfos.Single(si => si.section_id == nSectionId);
            ddlcssClassName.SelectedValue = objSI.cssClassName.ToString();
            ddlcssClassName.CssClass = ddlcssClassName.SelectedValue;
        }

        lblcssClassName.Visible = false;
        ddlcssClassName.Visible = true;

        lblAExcludeCom0.Visible = false;
        chkIsExcludeCom0.Visible = true;

        txtSectionName.Visible = true;
        lblSectionName.Visible = false;
        chkIsActive.Enabled = true;
        LinkButton btn = (LinkButton)grdMainSection.Rows[e.NewEditIndex].Cells[4].Controls[0];
        btn.Text = "Update";
        btn.CommandName = "Update";

    }

    protected void GetCssClassName(object sender, EventArgs e)
    {

        string strSender = sender.ToString();
        int i = 0;

        DropDownList ddlcssClassName1 = (DropDownList)grdMainSection.FindControl("ddlcssClassName");
        ddlcssClassName1 = (DropDownList)sender;
        GridViewRow gvr = (GridViewRow)ddlcssClassName1.NamingContainer;
        i = gvr.RowIndex;

        DropDownList ddlcssClassName = (DropDownList)grdMainSection.Rows[i].FindControl("ddlcssClassName");

        ddlcssClassName.CssClass = ddlcssClassName.SelectedValue;
       
        ddlcssClassName.Items.FindByValue(ddlcssClassName.SelectedValue).Selected = true;
    }

    protected void grdMainSection_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        CheckBox chkIsActive = (CheckBox)grdMainSection.Rows[e.RowIndex].FindControl("chkIsActive");
        TextBox txtSectionName = (TextBox)grdMainSection.Rows[e.RowIndex].FindControl("txtSectionName");
        Label lblSectionName = (Label)grdMainSection.Rows[e.RowIndex].FindControl("lblSectionName");

        DropDownList ddlcssClassName = (DropDownList)grdMainSection.Rows[e.RowIndex].FindControl("ddlcssClassName");
        string strClassName = ddlcssClassName.SelectedItem.Value;

        CheckBox chkIsExcludeCom0 = (CheckBox)grdMainSection.Rows[e.RowIndex].FindControl("chkIsExcludeCom0");
        Label lblAExcludeCom0 = (Label)grdMainSection.Rows[e.RowIndex].FindControl("lblAExcludeCom0");

        int nSectionId = Convert.ToInt32(grdMainSection.DataKeys[Convert.ToInt32(e.RowIndex)].Values[0]);
        string strMainName = txtSectionName.Text.Replace("'", "''");
        if (_db.sectioninfos.Where(l => l.client_id == 1 && l.parent_id == 0 && l.is_disable == false && l.section_name == strMainName).SingleOrDefault() != null)
        {
            List<sectioninfo> sList = _db.sectioninfos.Where(l => l.client_id == 1 && l.parent_id == 0 && l.is_disable == false && l.section_name == strMainName).ToList();
            foreach (sectioninfo objsec in sList)
            {
                if (objsec.section_id != nSectionId)
                {
                    lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Section name is already exist. Please try another name to update");
                    lblMainSecResult.Text = csCommonUtility.GetSystemRequiredMessage("Section name is already exist. Please try another name to update");
                    return;
                }
            }

        }
        string strQ = "UPDATE sectioninfo SET section_name='" + txtSectionName.Text.Replace("'", "''") + "' , is_active='" + Convert.ToBoolean(chkIsActive.Checked) + "',is_CommissionExclude='" + Convert.ToBoolean(chkIsExcludeCom0.Checked) + "', cssClassName='" + strClassName + "'  WHERE section_id=" + nSectionId + "  AND client_id=1";
        _db.ExecuteCommand(strQ, string.Empty);

        string strItemQ = "UPDATE sectioninfo SET is_CommissionExclude ='" + Convert.ToBoolean(chkIsExcludeCom0.Checked) + "'  WHERE section_level  =" + nSectionId + "  AND client_id=1";
        _db.ExecuteCommand(strItemQ, string.Empty);

        LoadMainSectionInfo();
        lblResult.Text = csCommonUtility.GetSystemMessage("Data updated successfully");
        lblMainSecResult.Text = csCommonUtility.GetSystemMessage("Data updated successfully");
        LoadTree();

    }

    protected void grdMainSection_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            CheckBox chkIsExcludeCom0 = (CheckBox)e.Row.FindControl("chkIsExcludeCom0");
            Label lblAExcludeCom0 = (Label)e.Row.FindControl("lblAExcludeCom0");

            TextBox txtSectionName = (TextBox)e.Row.FindControl("txtSectionName");
            Label lblSectionName = (Label)e.Row.FindControl("lblSectionName");
            DropDownList ddlcssClassName = (DropDownList)e.Row.FindControl("ddlcssClassName");
            string strClassName = grdMainSection.DataKeys[e.Row.RowIndex].Values[4].ToString();
            Label lblcssClassName = (Label)e.Row.FindControl("lblcssClassName");

            ddlcssClassName.SelectedValue = strClassName;

            lblcssClassName.CssClass = strClassName;
            lblcssClassName.Text = strClassName.Replace("fc-","");

            if (chkIsExcludeCom0.Checked)
            {
                lblAExcludeCom0.Text = "Yes";
            }
            else
            {
                lblAExcludeCom0.Text = "No";
            }


            string str = txtSectionName.Text.Replace("&nbsp;", "");
            if (str == "" || Convert.ToInt32(grdMainSection.DataKeys[Convert.ToInt32(e.Row.RowIndex)].Values[0]) == 0)
            {
                txtSectionName.Visible = true;
                lblSectionName.Visible = false;
                ddlcssClassName.Visible = true;
                lblcssClassName.Visible = false;

                chkIsExcludeCom0.Visible = true;
                lblAExcludeCom0.Visible = false;

                if (strClassName == "")
                {
                    ddlcssClassName.SelectedValue = "fc-RoyalBlue";
                    ddlcssClassName.CssClass = "fc-RoyalBlue";
                }

                LinkButton btn = (LinkButton)e.Row.Cells[4].Controls[0];
                btn.Text = "Save";
                btn.CommandName = "Save";

            }
        }

    }


    protected void btnAddnewRow_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnAddnewRow.ID, btnAddnewRow.GetType().Name, "Click"); 
        DataClassesDataContext _db = new DataClassesDataContext();
        var result = (from sin in _db.sectioninfos
                      where sin.parent_id == 0 && sin.client_id == 1
                      select sin.section_id);
        int nsectionId = 0;
        int n = result.Count();
        if (result != null && n > 0)
            nsectionId = result.Max();

        nsectionId = nsectionId + 1000;
        hdnSectionId.Value = nsectionId.ToString();
        hdnSectionLevel.Value = nsectionId.ToString();
        hdnSectionSerial.Value = nsectionId.ToString();

        DataTable table = (DataTable)Session["MainSection"];

        int nSecId = Convert.ToInt32(hdnSectionId.Value);
        bool contains = table.AsEnumerable().Any(row => nSecId == row.Field<int>("section_id"));
        if (contains)
        {
            lblResult.Text = csCommonUtility.GetSystemRequiredMessage("You have already a pending item to save.");
            lblMainSecResult.Text = csCommonUtility.GetSystemRequiredMessage("You have already a pending item to save.");
            return;
        }

        foreach (GridViewRow di in grdMainSection.Rows)
        {
            {
                CheckBox chkIsActive = (CheckBox)di.FindControl("chkIsActive");
                TextBox txtSectionName = (TextBox)di.FindControl("txtSectionName");
                Label lblSectionName = (Label)di.FindControl("lblSectionName");
                DropDownList ddlcssClassName = (DropDownList)di.FindControl("ddlcssClassName");
                DataRow dr = table.Rows[di.RowIndex];

                CheckBox chkIsExcludeCom0 = (CheckBox)di.FindControl("chkIsExcludeCom0");
                Label lblAExcludeCom0 = (Label)di.FindControl("lblAExcludeCom0");
              //  ddlcssClassName.Visible = true;

                dr["section_id"] = Convert.ToInt32(grdMainSection.DataKeys[di.RowIndex].Values[0]);
                dr["client_id"] = 1;
                dr["section_name"] = txtSectionName.Text;
                dr["parent_id"] = Convert.ToInt32(grdMainSection.DataKeys[di.RowIndex].Values[1]);
                dr["section_notes"] = "";
                dr["section_level"] = Convert.ToInt32(grdMainSection.DataKeys[di.RowIndex].Values[2]);
                dr["section_serial"] = Convert.ToDecimal(grdMainSection.DataKeys[di.RowIndex].Values[3]);
                dr["is_active"] = Convert.ToBoolean(chkIsActive.Checked);
                dr["create_date"] = DateTime.Now;
                dr["cssClassName"] = grdMainSection.DataKeys[di.RowIndex].Values[4].ToString();
                dr["is_CommissionExclude"] = Convert.ToBoolean(chkIsExcludeCom0.Checked);


            }

        }

       

        DataRow drNew = table.NewRow();
        drNew["section_id"] = Convert.ToInt32(hdnSectionId.Value);
        drNew["client_id"] = 1;
        drNew["section_name"] = "";
        drNew["parent_id"] = 0;
        drNew["section_notes"] = "";
        drNew["section_level"] = Convert.ToInt32(hdnSectionLevel.Value);
        drNew["section_serial"] = Convert.ToDecimal(hdnSectionSerial.Value);
        drNew["is_active"] = true;
        drNew["create_date"] = DateTime.Now;
        drNew["cssClassName"] = "";
        drNew["is_CommissionExclude"] = false;
        //table.Rows.Add(drNew);
        table.Rows.InsertAt(drNew, 0);

        Session.Add("MainSection", table);
        grdMainSection.DataSource = table;
        grdMainSection.DataKeyNames = new string[] { "section_id", "parent_id", "section_level", "section_serial", "cssClassName" };
        grdMainSection.DataBind();
        lblResult.Text = "";
        lblMainSecResult.Text = "";
        lblItemResult.Text = "";
        lblSubSecResult.Text = "";

    }
    #endregion



    #region  add new Sub Section

    private void LoadSubSectionInfo()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        DataTable tmpTable = LoadSectionTable();
        sectioninfo sinfo = new sectioninfo();
        sinfo = _db.sectioninfos.SingleOrDefault(c => c.section_id == Convert.ToInt32(hdnSubItemParentId.Value) && c.client_id == 1);
        hdnSectionLevel.Value = sinfo.section_level.ToString();
        string strQ = "";
        if (Convert.ToInt32(hdnSectionLevel.Value) == 47000)
        {
            strQ = " SELECT * FROM sectioninfo WHERE parent_id = " + Convert.ToInt32(hdnSubItemParentId.Value) + " AND section_level = " + Convert.ToInt32(hdnSectionLevel.Value) + " AND client_id = 1 AND section_id < " + Convert.ToInt32(hdnSectionLevel.Value) + " + 100 AND section_id NOT IN ( SELECT item_id FROM item_price where item_id IN(Select section_id from sectioninfo WHERE section_level = 47000))";
        }
        else
        {
            strQ = " SELECT * FROM sectioninfo WHERE parent_id = " + Convert.ToInt32(hdnSubItemParentId.Value) + " AND section_level = " + Convert.ToInt32(hdnSectionLevel.Value) + " AND client_id = 1 AND section_id < " + Convert.ToInt32(hdnSectionLevel.Value) + " + 100 ";
        }
        IEnumerable<sectioninfo> list = _db.ExecuteQuery<sectioninfo>(strQ, string.Empty);
        //var item = from sin in _db.sectioninfos
        //           where sin.parent_id == Convert.ToInt32(hdnSubItemParentId.Value) && sin.section_level == Convert.ToInt32(hdnSectionLevel.Value) && sin.client_id == 1 && sin.section_id < Convert.ToInt32(hdnSectionLevel.Value) + 100
        //           select new SectionInfo()
        //           {
        //               section_id = (int)sin.section_id,
        //               client_id = (int)sin.client_id,
        //               section_name = sin.section_name,
        //               parent_id = (int)sin.parent_id,
        //               section_notes = sin.section_notes,
        //               section_level = (int)sin.section_level,
        //               section_serial = (decimal)sin.section_serial,
        //               is_active = (bool)sin.is_active,
        //               create_date = (DateTime)sin.create_date
        //           };
        foreach (sectioninfo Sinfo in list)
        {

            DataRow drNew = tmpTable.NewRow();
            drNew["section_id"] = Sinfo.section_id;
            drNew["client_id"] = Sinfo.client_id;
            drNew["section_name"] = Sinfo.section_name;
            drNew["parent_id"] = Sinfo.parent_id;
            drNew["section_notes"] = Sinfo.section_notes;
            drNew["section_level"] = Sinfo.section_level;
            drNew["section_serial"] = Sinfo.section_serial;
            drNew["is_active"] = Sinfo.is_active;
            drNew["create_date"] = Sinfo.create_date;
            drNew["is_CommissionExclude"] = Sinfo.is_CommissionExclude;
            tmpTable.Rows.Add(drNew);
        }



        var result = (from sin in _db.sectioninfos
                      where sin.section_level == Convert.ToInt32(hdnSectionLevel.Value) && sin.client_id == 1 && sin.section_id < Convert.ToInt32(hdnSectionLevel.Value) + 100
                      select sin.section_id);
        int nsectionId = 0;
        int n = result.Count();
        if (result != null && n > 0)
            nsectionId = result.Max();

        if (nsectionId == 0)
        {
            nsectionId = Convert.ToInt32(hdnSectionLevel.Value) + 1;
        }
        else
        {
            nsectionId = nsectionId + 1;
        }
        hdnSectionId.Value = nsectionId.ToString();
        hdnSectionSerial.Value = nsectionId.ToString();
        lblSerial.Text = hdnSectionSerial.Value;


         if (tmpTable.Rows.Count == 0)//if (list.Count() == 0)
         {

            DataRow drNew1 = tmpTable.NewRow();
            drNew1["section_id"] = Convert.ToInt32(hdnSectionId.Value);
            drNew1["client_id"] = 1;
            drNew1["section_name"] = "";
            drNew1["parent_id"] = Convert.ToInt32(hdnSubItemParentId.Value);
            drNew1["section_notes"] = "";
            drNew1["section_level"] = Convert.ToInt32(hdnSectionLevel.Value);
            drNew1["section_serial"] = Convert.ToDecimal(hdnSectionSerial.Value);
            drNew1["is_active"] = true;
            drNew1["create_date"] = DateTime.Now;
            drNew1["is_CommissionExclude"] = true;
             
            //tmpTable.Rows.Add(drNew);
            tmpTable.Rows.InsertAt(drNew1, 0);
        }
        Session.Add("SubSection", tmpTable);
        grdSubSection.DataSource = tmpTable;
        grdSubSection.DataKeyNames = new string[] { "section_id", "parent_id", "section_level", "section_serial" };
        grdSubSection.DataBind();

    }


    protected void grdSubSection_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "Save")
        {
            DataClassesDataContext _db = new DataClassesDataContext();

            DataTable table = (DataTable)Session["SubSection"];

            foreach (GridViewRow di in grdSubSection.Rows)
            {
                {
                    CheckBox chkIsActive1 = (CheckBox)di.FindControl("chkIsActive1");
                    TextBox txtSubSectionName = (TextBox)di.FindControl("txtSubSectionName");
                    Label lblSubSectionName = (Label)di.FindControl("lblSubSectionName");
                    CheckBox chkIsExcludeCom1 = (CheckBox)di.FindControl("chkIsExcludeCom1");
                    Label lblAExcludeCom1 = (Label)di.FindControl("lblAExcludeCom1");
                    DataRow dr = table.Rows[di.RowIndex];

                    dr["section_id"] = Convert.ToInt32(grdSubSection.DataKeys[di.RowIndex].Values[0]);
                    dr["client_id"] = 1;
                    dr["section_name"] = txtSubSectionName.Text;
                    dr["parent_id"] = Convert.ToInt32(grdSubSection.DataKeys[di.RowIndex].Values[1]);
                    dr["section_notes"] = "";
                    dr["section_level"] = Convert.ToInt32(grdSubSection.DataKeys[di.RowIndex].Values[2]);
                    dr["section_serial"] = Convert.ToDecimal(grdSubSection.DataKeys[di.RowIndex].Values[3]);
                    dr["is_active"] = Convert.ToBoolean(chkIsActive1.Checked);
                    dr["is_CommissionExclude"] = Convert.ToBoolean(chkIsExcludeCom1.Checked);
                    dr["create_date"] = DateTime.Now;

                }

            }
            foreach (DataRow dr in table.Rows)
            {
                bool bFlagNew = false;

                sectioninfo SecInfo = _db.sectioninfos.SingleOrDefault(l => l.section_id == Convert.ToInt32(dr["section_id"]));
                if (SecInfo == null)
                {
                    //sectioninfo sinfo = new sectioninfo();
                    //sinfo = _db.sectioninfos.Single(c => c.section_id == Convert.ToInt32(hdnParentId.Value) && c.client_id == 1);
                    //hdnSectionLevel.Value = sinfo.section_level.ToString();
                    SecInfo = new sectioninfo();
                    bFlagNew = true;
                    if (_db.sectioninfos.Where(l => l.client_id == 1 && l.parent_id == Convert.ToInt32(hdnSubItemParentId.Value) && l.is_disable == false && l.section_name == dr["section_name"].ToString()).SingleOrDefault() != null)
                    {
                        lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Section name already exist. Please try another name.");
                        lblSubSecResult.Text = csCommonUtility.GetSystemRequiredMessage("Section name already exist. Please try another name.");
                        return;
                    }
                }

                string str = dr["section_name"].ToString().Trim();
                if (str.Length > 0)
                {
                    SecInfo.section_id = Convert.ToInt32(dr["section_id"]);
                    SecInfo.client_id = Convert.ToInt32(dr["client_id"]);
                    SecInfo.section_name = dr["section_name"].ToString();
                    SecInfo.parent_id = Convert.ToInt32(dr["parent_id"]);
                    SecInfo.section_notes = dr["section_notes"].ToString();
                    SecInfo.section_level = Convert.ToInt32(dr["section_level"]);
                    SecInfo.section_serial = Convert.ToDecimal(dr["section_serial"]);
                    SecInfo.is_active = Convert.ToBoolean(dr["is_active"]);
                    SecInfo.create_date = DateTime.Now;
                    SecInfo.is_mandatory = false;
                    SecInfo.is_CommissionExclude = Convert.ToBoolean(dr["is_CommissionExclude"]); //false;
                    SecInfo.is_disable = false;
                    if (bFlagNew)
                    {
                        _db.sectioninfos.InsertOnSubmit(SecInfo);
                    }
                }
                else
                {
                    lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Section Name is a required field");
                    lblSubSecResult.Text = csCommonUtility.GetSystemRequiredMessage("Section Name is a required field");
                    return;
                }
            }

            lblResult.Text = csCommonUtility.GetSystemMessage("Data saved successfully");
            lblSubSecResult.Text = csCommonUtility.GetSystemMessage("Data saved successfully");
            _db.SubmitChanges();
            LoadSubSectionInfo();
            LoadTree();

        }
    }

    protected void grdSubSection_RowEditing(object sender, GridViewEditEventArgs e)
    {
        CheckBox chkIsActive1 = (CheckBox)grdSubSection.Rows[e.NewEditIndex].FindControl("chkIsActive1");
        TextBox txtSubSectionName = (TextBox)grdSubSection.Rows[e.NewEditIndex].FindControl("txtSubSectionName");
        Label lblSubSectionName = (Label)grdSubSection.Rows[e.NewEditIndex].FindControl("lblSubSectionName");

        CheckBox chkIsExcludeCom1 = (CheckBox)grdSubSection.Rows[e.NewEditIndex].FindControl("chkIsExcludeCom1");
        Label lblAExcludeCom1 = (Label)grdSubSection.Rows[e.NewEditIndex].FindControl("lblAExcludeCom1");


        chkIsExcludeCom1.Visible = true;
        lblAExcludeCom1.Visible = false;

        txtSubSectionName.Visible = true;
        lblSubSectionName.Visible = false;
        chkIsActive1.Enabled = true;
        LinkButton btn = (LinkButton)grdSubSection.Rows[e.NewEditIndex].Cells[3].Controls[0];
        btn.Text = "Update";
        btn.CommandName = "Update";

    }
    protected void grdSubSection_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        CheckBox chkIsActive1 = (CheckBox)grdSubSection.Rows[e.RowIndex].FindControl("chkIsActive1");
        TextBox txtSubSectionName = (TextBox)grdSubSection.Rows[e.RowIndex].FindControl("txtSubSectionName");
        Label lblSubSectionName = (Label)grdSubSection.Rows[e.RowIndex].FindControl("lblSubSectionName");

        CheckBox chkIsExcludeCom1 = (CheckBox)grdSubSection.Rows[e.RowIndex].FindControl("chkIsExcludeCom1");
        Label lblAExcludeCom1 = (Label)grdSubSection.Rows[e.RowIndex].FindControl("lblAExcludeCom1");

        int nSectionId = Convert.ToInt32(grdSubSection.DataKeys[Convert.ToInt32(e.RowIndex)].Values[0]);
        int nParentId = Convert.ToInt32(grdSubSection.DataKeys[Convert.ToInt32(e.RowIndex)].Values[1]);
        string strSubName = txtSubSectionName.Text.Replace("'", "''");
        if (_db.sectioninfos.Where(l => l.client_id == 1 && l.parent_id == nParentId && l.is_disable == false && l.section_name == strSubName).SingleOrDefault() != null)
        {
            List<sectioninfo> sList = _db.sectioninfos.Where(l => l.client_id == 1 && l.parent_id == nParentId && l.is_disable == false && l.section_name == strSubName).ToList();
            foreach (sectioninfo objsec in sList)
            {
                if (objsec.section_id != nSectionId)
                {
                    lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Section name is already exist. Please try another name to update");
                    lblSubSecResult.Text = csCommonUtility.GetSystemRequiredMessage("Section name is already exist. Please try another name to update");
                    return;
                }
            }

        }

        string strQ = "UPDATE sectioninfo SET section_name='" + txtSubSectionName.Text.Replace("'", "''") + "' , is_active='" + Convert.ToBoolean(chkIsActive1.Checked) + "',is_CommissionExclude ='" + Convert.ToBoolean(chkIsExcludeCom1.Checked) + "'  WHERE section_id=" + nSectionId + "  AND client_id=1";
        _db.ExecuteCommand(strQ, string.Empty);

        string strItemQ = "UPDATE sectioninfo SET is_CommissionExclude ='" + Convert.ToBoolean(chkIsExcludeCom1.Checked) + "'  WHERE parent_id =" + nSectionId + "  AND client_id=1";
        _db.ExecuteCommand(strItemQ, string.Empty);

        LoadSubSectionInfo();

        lblResult.Text = csCommonUtility.GetSystemMessage("Data updated successfully");
        lblSubSecResult.Text = csCommonUtility.GetSystemMessage("Data updated successfully");

    }

    protected void grdSubSection_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {



            TextBox txtSubSectionName = (TextBox)e.Row.FindControl("txtSubSectionName");
            Label lblSubSectionName = (Label)e.Row.FindControl("lblSubSectionName");

            CheckBox chkIsExcludeCom1 = (CheckBox)e.Row.FindControl("chkIsExcludeCom1");
            Label lblAExcludeCom1 = (Label)e.Row.FindControl("lblAExcludeCom1");
            if (chkIsExcludeCom1.Checked)
            {
                lblAExcludeCom1.Text = "Yes";
            }
            else
            {
                lblAExcludeCom1.Text = "No";
            }

            string str = txtSubSectionName.Text.Replace("&nbsp;", "");
            if (str == "" || Convert.ToInt32(grdSubSection.DataKeys[Convert.ToInt32(e.Row.RowIndex)].Values[0]) == 0)
            {
                txtSubSectionName.Visible = true;
                lblSubSectionName.Visible = false;

                chkIsExcludeCom1.Visible = true;
                lblAExcludeCom1.Visible = false;

                LinkButton btn = (LinkButton)e.Row.Cells[3].Controls[0];
                btn.Text = "Save";
                btn.CommandName = "Save";

            }


        }

    }
    protected void btnAddSubnewRow_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnAddSubnewRow.ID, btnAddSubnewRow.GetType().Name, "Click"); 
        DataClassesDataContext _db = new DataClassesDataContext();
        sectioninfo sinfo = new sectioninfo();
        sinfo = _db.sectioninfos.SingleOrDefault(c => c.section_id == Convert.ToInt32(hdnSubItemParentId.Value) && c.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));
        hdnSectionLevel.Value = sinfo.section_level.ToString();
        var result = (from sin in _db.sectioninfos
                      where sin.section_level == Convert.ToInt32(hdnSectionLevel.Value) && sin.client_id == 1 && sin.section_id < Convert.ToInt32(hdnSectionLevel.Value) + 100
                      select sin.section_id);
        int nsectionId = 0;
        int n = result.Count();
        if (result != null && n > 0)
            nsectionId = result.Max();


        if (nsectionId == 0)
        {
            nsectionId = Convert.ToInt32(hdnSectionLevel.Value) + 1;
        }
        else
        {
            nsectionId = nsectionId + 1;
        }

        hdnSectionId.Value = nsectionId.ToString();
        hdnSectionSerial.Value = nsectionId.ToString();
        lblSerial.Text = hdnSectionSerial.Value;

        DataTable table = (DataTable)Session["SubSection"];
        int nSecId = Convert.ToInt32(hdnSectionId.Value);
        bool contains = table.AsEnumerable().Any(row => nSecId == row.Field<int>("section_id"));
        if (contains)
        {
            lblResult.Text =csCommonUtility.GetSystemRequiredMessage("You have already a pending item to save.");
            lblSubSecResult.Text = csCommonUtility.GetSystemRequiredMessage("You have already a pending item to save.");
            return;
        }

        foreach (GridViewRow di in grdSubSection.Rows)
        {
            {
                CheckBox chkIsActive1 = (CheckBox)di.FindControl("chkIsActive1");
                CheckBox chkIsExcludeCom1 = (CheckBox)di.FindControl("chkIsExcludeCom1");
                Label lblAExcludeCom1 = (Label)di.FindControl("lblAExcludeCom1");
                TextBox txtSubSectionName = (TextBox)di.FindControl("txtSubSectionName");
                Label lblSubSectionName = (Label)di.FindControl("lblSubSectionName");
                DataRow dr = table.Rows[di.RowIndex];

                dr["section_id"] = Convert.ToInt32(grdSubSection.DataKeys[di.RowIndex].Values[0]);
                dr["client_id"] = 1;
                dr["section_name"] = txtSubSectionName.Text;
                dr["parent_id"] = Convert.ToInt32(grdSubSection.DataKeys[di.RowIndex].Values[1]);
                dr["section_notes"] = "";
                dr["section_level"] = Convert.ToInt32(grdSubSection.DataKeys[di.RowIndex].Values[2]);
                dr["section_serial"] = Convert.ToDecimal(grdSubSection.DataKeys[di.RowIndex].Values[3]);
                dr["is_active"] = Convert.ToBoolean(chkIsActive1.Checked);
                dr["is_CommissionExclude"] = Convert.ToBoolean(chkIsExcludeCom1.Checked);
                dr["create_date"] = DateTime.Now;


            }

        }

        DataRow drNew = table.NewRow();
        drNew["section_id"] = Convert.ToInt32(hdnSectionId.Value);
        drNew["client_id"] = 1;
        drNew["section_name"] = "";
        drNew["parent_id"] = Convert.ToInt32(hdnSubItemParentId.Value);
        drNew["section_notes"] = "";
        drNew["section_level"] = Convert.ToInt32(hdnSectionLevel.Value);
        drNew["section_serial"] = Convert.ToDecimal(hdnSectionSerial.Value);
        drNew["is_active"] = true;
        drNew["is_CommissionExclude"] = false;
        drNew["create_date"] = DateTime.Now;
      
        table.Rows.InsertAt(drNew, 0);

        Session.Add("SubSection", table);
        grdSubSection.DataSource = table;
        grdSubSection.DataKeyNames = new string[] { "section_id", "parent_id", "section_level", "section_serial" };
        grdSubSection.DataBind();
        lblResult.Text = "";
        lblMainSecResult.Text = "";
        lblItemResult.Text = "";
        lblSubSecResult.Text = "";
    }
    #endregion


    #region  add new Items
    private DataTable LoadItemPriceTable()
    {
        DataTable table = new DataTable();
        table.Columns.Add("section_id", typeof(int));
        table.Columns.Add("client_id", typeof(int));
        table.Columns.Add("section_name", typeof(string));
        table.Columns.Add("parent_id", typeof(int));
        table.Columns.Add("section_notes", typeof(string));
        table.Columns.Add("section_level", typeof(int));
        table.Columns.Add("section_serial", typeof(decimal));
        table.Columns.Add("is_active", typeof(bool));      
        table.Columns.Add("create_date", typeof(DateTime));

        table.Columns.Add("item_id", typeof(int));
        table.Columns.Add("measure_unit", typeof(string));
        table.Columns.Add("item_cost", typeof(decimal));
        table.Columns.Add("minimum_qty", typeof(decimal));
        table.Columns.Add("retail_multiplier", typeof(decimal));
        table.Columns.Add("labor_rate", typeof(decimal));
        table.Columns.Add("update_time", typeof(DateTime));
        table.Columns.Add("labor_id", typeof(int));
        table.Columns.Add("is_mandatory", typeof(bool));
        table.Columns.Add("is_CommissionExclude", typeof(bool));

        return table;
    }


    private void LoadItemInfo()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        DataTable tmpTable = LoadItemPriceTable();
        sectioninfo sinfo = new sectioninfo();
        sinfo = _db.sectioninfos.SingleOrDefault(c => c.section_id == Convert.ToInt32(hdnSubItemParentId.Value) && c.client_id == 1);
        hdnSectionLevel.Value = sinfo.section_level.ToString();
        var item = from it in _db.item_prices
                   join si in _db.sectioninfos on it.item_id equals si.section_id
                   where si.parent_id == Convert.ToInt32(hdnSubItemParentId.Value) && si.section_level == Convert.ToInt32(hdnSectionLevel.Value)
                   && si.client_id == 1 && si.is_disable == false
                   //var item = from sin in _db.sectioninfos
                   //           where sin.parent_id == Convert.ToInt32(hdnSubItemParentId.Value) && sin.section_level == Convert.ToInt32(hdnSectionLevel.Value) && sin.client_id == 1 && sin.section_id > Convert.ToInt32(hdnSectionLevel.Value) + 100
                   select new SectionItemPriceModel()
                  {
                      section_id = (int)si.section_id,
                      client_id = (int)si.client_id,
                      section_name = si.section_name,
                      parent_id = (int)si.parent_id,
                      section_notes = si.section_notes,
                      section_level = (int)si.section_level,
                      section_serial = (decimal)si.section_serial,
                      is_active = (bool)si.is_active,
                      create_date = (DateTime)si.create_date,

                      item_id = (int)it.item_id,
                      measure_unit = it.measure_unit,
                      item_cost = (decimal)it.item_cost,
                      minimum_qty = (decimal)it.minimum_qty,
                      retail_multiplier = (decimal)it.retail_multiplier,
                      labor_rate = (decimal)it.labor_rate,
                      labor_id = (int)it.labor_id,
                      is_mandatory = (bool)si.is_mandatory,
                      is_CommissionExclude = (bool)si.is_CommissionExclude
                  };
        foreach (SectionItemPriceModel Sinfo in item)
        {

            DataRow drNew = tmpTable.NewRow();
            drNew["section_id"] = Sinfo.section_id;
            drNew["client_id"] = Sinfo.client_id;
            drNew["section_name"] = Sinfo.section_name;
            drNew["parent_id"] = Sinfo.parent_id;
            drNew["section_notes"] = Sinfo.section_notes;
            drNew["section_level"] = Sinfo.section_level;
            drNew["section_serial"] = Sinfo.section_serial;
            drNew["is_active"] = Sinfo.is_active;
            drNew["create_date"] = Sinfo.create_date;

            drNew["item_id"] = Sinfo.item_id;
            drNew["measure_unit"] = Sinfo.measure_unit;
            drNew["item_cost"] = Sinfo.item_cost;
            drNew["minimum_qty"] = Sinfo.minimum_qty;
            drNew["retail_multiplier"] = Sinfo.retail_multiplier;
            drNew["labor_rate"] = Sinfo.labor_rate;
            drNew["labor_id"] = Sinfo.labor_id;
            drNew["is_mandatory"] = Sinfo.is_mandatory;
            drNew["is_CommissionExclude"] = Sinfo.is_CommissionExclude;
            tmpTable.Rows.Add(drNew);


        }



        var result = (from sin in _db.sectioninfos
                      where sin.section_level == Convert.ToInt32(hdnSectionLevel.Value) && sin.client_id == 1
                      && sin.section_id > Convert.ToInt32(hdnSectionLevel.Value) + 100 
                      select sin.section_id);
        int nsectionId = 0;
        int n = result.Count();
        if (result != null && n > 0)
            nsectionId = result.Max();

        if (nsectionId == 0)
        {
            nsectionId = Convert.ToInt32(hdnSectionLevel.Value) + 100 + 1;
        }
        else
        {
            nsectionId = nsectionId + 1;
        }
        hdnSectionId.Value = nsectionId.ToString();
        string strSerial = nsectionId.ToString();
        string str = "";
        if (strSerial.Length < 5)
        {
            str = strSerial.Substring(2);
        }
        hdnSectionSerial.Value = hdnSectionLevel.Value + "." + str;
        lblSerial.Text = hdnSectionSerial.Value;

        //if (tmpTable.Rows.Count() == 0)
        if (item.Count() == 0)
        {

            DataRow drNew1 = tmpTable.NewRow();
            drNew1["section_id"] = Convert.ToInt32(hdnSectionId.Value);
            drNew1["client_id"] = 1;
            drNew1["section_name"] = "";
            drNew1["parent_id"] = Convert.ToInt32(hdnSubItemParentId.Value);
            drNew1["section_notes"] = "";
            drNew1["section_level"] = Convert.ToInt32(hdnSectionLevel.Value);
            drNew1["section_serial"] = Convert.ToDecimal(hdnSectionSerial.Value);
            drNew1["is_active"] = true;
            drNew1["create_date"] = DateTime.Now;

            drNew1["item_id"] = Convert.ToInt32(hdnSectionId.Value);
            drNew1["measure_unit"] = "";
            drNew1["item_cost"] = 0;
            drNew1["minimum_qty"] = 0;
            drNew1["retail_multiplier"] = Convert.ToDecimal(hdnMultiplier.Value);
            drNew1["labor_rate"] = 0;
            drNew1["labor_id"] = 2;
            drNew1["is_mandatory"] = false;
            drNew1["is_CommissionExclude"] = false;
            
            //tmpTable.Rows.Add(drNew);
            tmpTable.Rows.InsertAt(drNew1, 0);
        }
        Session.Add("NewItem", tmpTable);
        grdItem_Price.DataSource = tmpTable;
        grdItem_Price.DataKeyNames = new string[] { "section_id", "parent_id", "section_level", "section_serial", "labor_id","is_mandatory","is_CommissionExclude" };
        grdItem_Price.DataBind();

    }


    protected void grdItem_Price_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "Save")
        {
            DataClassesDataContext _db = new DataClassesDataContext();

            DataTable table = (DataTable)Session["NewItem"];

            foreach (GridViewRow di in grdItem_Price.Rows)
            {
                {
                    CheckBox chkIsActiveItem = (CheckBox)di.FindControl("chkIsActiveItem");
                    CheckBox chkIsMandatory = (CheckBox)di.FindControl("chkIsMandatory");
                    CheckBox chkIsExcludeCom = (CheckBox)di.FindControl("chkIsExcludeCom");
                    
                    TextBox txtItemName = (TextBox)di.FindControl("txtItemName");
                    Label lblItemnName = (Label)di.FindControl("lblItemnName");

                    TextBox txtMeasureUnit = (TextBox)di.FindControl("txtMeasureUnit");
                    Label lblMeasureUnit = (Label)di.FindControl("lblMeasureUnit");

                    TextBox txtCost = (TextBox)di.FindControl("txtCost");
                    Label lblCost = (Label)di.FindControl("lblCost");

                    TextBox txtMinQty = (TextBox)di.FindControl("txtMinQty");
                    Label lblMinQty = (Label)di.FindControl("lblMinQty");

                    TextBox txtRetailMulti = (TextBox)di.FindControl("txtRetailMulti");
                    Label lblRetailMulti = (Label)di.FindControl("lblRetailMulti");

                    TextBox txtLabor = (TextBox)di.FindControl("txtLabor");
                    Label lblLabor = (Label)di.FindControl("lblLabor");
                    DataRow dr = table.Rows[di.RowIndex];

                    dr["section_id"] = Convert.ToInt32(grdItem_Price.DataKeys[di.RowIndex].Values[0]);
                    dr["client_id"] = 1;
                    dr["section_name"] = txtItemName.Text;
                    dr["parent_id"] = Convert.ToInt32(grdItem_Price.DataKeys[di.RowIndex].Values[1]);
                    dr["section_notes"] = "";
                    dr["section_level"] = Convert.ToInt32(grdItem_Price.DataKeys[di.RowIndex].Values[2]);
                    dr["section_serial"] = Convert.ToDecimal(grdItem_Price.DataKeys[di.RowIndex].Values[3]);
                    dr["is_active"] = Convert.ToBoolean(chkIsActiveItem.Checked);                   
                    dr["is_mandatory"] = Convert.ToBoolean(chkIsMandatory.Checked);
                    dr["is_CommissionExclude"] = Convert.ToBoolean(chkIsExcludeCom.Checked);
                    dr["create_date"] = DateTime.Now;

                    dr["item_id"] = Convert.ToInt32(grdItem_Price.DataKeys[di.RowIndex].Values[0]);
                    dr["measure_unit"] = txtMeasureUnit.Text;
                    dr["item_cost"] = Convert.ToDecimal(txtCost.Text);
                    dr["minimum_qty"] = Convert.ToDecimal(txtMinQty.Text);
                    dr["retail_multiplier"] = Convert.ToDecimal(txtRetailMulti.Text);
                    dr["labor_rate"] = Convert.ToDecimal(txtLabor.Text);
                    if (Convert.ToDecimal(txtLabor.Text) > 0)
                        dr["labor_id"] = 2;
                    else
                        dr["labor_id"] = 1;

                }

            }
            foreach (DataRow dr in table.Rows)
            {
                bool bFlagNew = false;
                bool bFlagNewIt = false;

                sectioninfo SecInfo = _db.sectioninfos.SingleOrDefault(l => l.section_id == Convert.ToInt32(dr["section_id"]));
                item_price itm = _db.item_prices.SingleOrDefault(l => l.item_id == Convert.ToInt32(dr["item_id"]));

                if (SecInfo == null)
                {
                    SecInfo = new sectioninfo();
                    bFlagNew = true;
                    if (_db.sectioninfos.Where(l => l.client_id == 1 && l.parent_id == Convert.ToInt32(hdnSubItemParentId.Value) && l.is_disable == false && l.section_name == dr["section_name"].ToString()).SingleOrDefault() != null)
                    {
                        lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Section name already exist. Please try another name.");
                        lblItemResult.Text = csCommonUtility.GetSystemRequiredMessage("Section name already exist. Please try another name.");
                        return;
                    }
                }
                if (itm == null)
                {
                    itm = new item_price();
                    bFlagNewIt = true;
                }

                string str = dr["section_name"].ToString().Trim();
                if (str.Length > 0)
                {
                    SecInfo.section_id = Convert.ToInt32(dr["section_id"]);
                    SecInfo.client_id = Convert.ToInt32(dr["client_id"]);
                    SecInfo.section_name = dr["section_name"].ToString();
                    SecInfo.parent_id = Convert.ToInt32(dr["parent_id"]);
                    SecInfo.section_notes = dr["section_notes"].ToString();
                    SecInfo.section_level = Convert.ToInt32(dr["section_level"]);
                    SecInfo.section_serial = Convert.ToDecimal(dr["section_serial"]);
                    SecInfo.is_active = Convert.ToBoolean(dr["is_active"]);
                    SecInfo.is_mandatory = Convert.ToBoolean(dr["is_mandatory"]);
                    SecInfo.is_CommissionExclude = Convert.ToBoolean(dr["is_CommissionExclude"]);
                    SecInfo.is_disable = false;
                    SecInfo.create_date = DateTime.Now;
                    itm.item_id = Convert.ToInt32(dr["item_id"]);
                    itm.measure_unit = dr["measure_unit"].ToString();
                    itm.client_id = Convert.ToInt32(dr["client_id"]);
                    itm.item_cost = Convert.ToDecimal(dr["item_cost"]);
                    itm.minimum_qty = Convert.ToDecimal(dr["minimum_qty"]);
                    itm.retail_multiplier = Convert.ToDecimal(dr["retail_multiplier"]);
                    itm.labor_rate = Convert.ToDecimal(dr["labor_rate"]);
                    itm.labor_id = Convert.ToInt32(dr["labor_id"]);
                    itm.update_time = DateTime.Now;

                    if (bFlagNew)
                    {
                        _db.sectioninfos.InsertOnSubmit(SecInfo);
                    }
                    if (bFlagNewIt)
                    {
                        _db.item_prices.InsertOnSubmit(itm);
                    }
                   

                }
                else
                {
                    lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Item Name is a required field");
                    lblItemResult.Text = csCommonUtility.GetSystemRequiredMessage("Item Name is a required field");
                    return;
                }
               
            }
            lblResult.Text = csCommonUtility.GetSystemMessage("Data saved successfully");
            lblItemResult.Text = csCommonUtility.GetSystemMessage("Data saved successfully");
            _db.SubmitChanges();
           
            LoadItemInfo();


        }
    }

    protected void grdItem_Price_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Label lblActive = (Label)grdItem_Price.Rows[e.NewEditIndex].FindControl("lblActive");
        CheckBox chkIsActiveItem = (CheckBox)grdItem_Price.Rows[e.NewEditIndex].FindControl("chkIsActiveItem");

        Label lblAMandatory = (Label)grdItem_Price.Rows[e.NewEditIndex].FindControl("lblAMandatory");
        CheckBox chkIsMandatory = (CheckBox)grdItem_Price.Rows[e.NewEditIndex].FindControl("chkIsMandatory");

        Label lblAExcludeCom = (Label)grdItem_Price.Rows[e.NewEditIndex].FindControl("lblAExcludeCom");
        CheckBox chkIsExcludeCom = (CheckBox)grdItem_Price.Rows[e.NewEditIndex].FindControl("chkIsExcludeCom");

        
        
        TextBox txtItemName = (TextBox)grdItem_Price.Rows[e.NewEditIndex].FindControl("txtItemName");
        Label lblItemnName = (Label)grdItem_Price.Rows[e.NewEditIndex].FindControl("lblItemnName");
        TextBox txtMeasureUnit = (TextBox)grdItem_Price.Rows[e.NewEditIndex].FindControl("txtMeasureUnit");
        Label lblMeasureUnit = (Label)grdItem_Price.Rows[e.NewEditIndex].FindControl("lblMeasureUnit");

        TextBox txtCost = (TextBox)grdItem_Price.Rows[e.NewEditIndex].FindControl("txtCost");
        Label lblCost = (Label)grdItem_Price.Rows[e.NewEditIndex].FindControl("lblCost");

        TextBox txtMinQty = (TextBox)grdItem_Price.Rows[e.NewEditIndex].FindControl("txtMinQty");
        Label lblMinQty = (Label)grdItem_Price.Rows[e.NewEditIndex].FindControl("lblMinQty");

        TextBox txtRetailMulti = (TextBox)grdItem_Price.Rows[e.NewEditIndex].FindControl("txtRetailMulti");
        Label lblRetailMulti = (Label)grdItem_Price.Rows[e.NewEditIndex].FindControl("lblRetailMulti");

        TextBox txtLabor = (TextBox)grdItem_Price.Rows[e.NewEditIndex].FindControl("txtLabor");
        Label lblLabor = (Label)grdItem_Price.Rows[e.NewEditIndex].FindControl("lblLabor");


        chkIsActiveItem.Visible = true;
        lblActive.Visible = false;

        chkIsMandatory.Visible = true;
        lblAMandatory.Visible = false;

        chkIsExcludeCom.Visible = true;
        lblAExcludeCom.Visible = false;

      
        

        txtItemName.Visible = true;
        lblItemnName.Visible = false;
       

        txtItemName.Visible = true;
        lblItemnName.Visible = false;

        txtMeasureUnit.Visible = true;
        lblMeasureUnit.Visible = false;

        txtCost.Visible = true;
        lblCost.Visible = false;

        txtMinQty.Visible = true;
        lblMinQty.Visible = false;

        txtRetailMulti.Visible = true;
        lblRetailMulti.Visible = false;

        txtLabor.Visible = true;
        lblLabor.Visible = false;

        LinkButton btn = (LinkButton)grdItem_Price.Rows[e.NewEditIndex].Cells[9].Controls[0];
        btn.Text = "Update";
        btn.CommandName = "Update";

    }
    protected void grdItem_Price_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        CheckBox chkIsActiveItem = (CheckBox)grdItem_Price.Rows[e.RowIndex].FindControl("chkIsActiveItem");
        CheckBox chkIsMandatory = (CheckBox)grdItem_Price.Rows[e.RowIndex].FindControl("chkIsMandatory");
        CheckBox chkIsExcludeCom = (CheckBox)grdItem_Price.Rows[e.RowIndex].FindControl("chkIsExcludeCom");

        
        
        TextBox txtItemName = (TextBox)grdItem_Price.Rows[e.RowIndex].FindControl("txtItemName");
        Label lblItemnName = (Label)grdItem_Price.Rows[e.RowIndex].FindControl("lblItemnName");

        TextBox txtMeasureUnit = (TextBox)grdItem_Price.Rows[e.RowIndex].FindControl("txtMeasureUnit");
        Label lblMeasureUnit = (Label)grdItem_Price.Rows[e.RowIndex].FindControl("lblMeasureUnit");

        TextBox txtCost = (TextBox)grdItem_Price.Rows[e.RowIndex].FindControl("txtCost");
        Label lblCost = (Label)grdItem_Price.Rows[e.RowIndex].FindControl("lblCost");

        TextBox txtMinQty = (TextBox)grdItem_Price.Rows[e.RowIndex].FindControl("txtMinQty");
        Label lblMinQty = (Label)grdItem_Price.Rows[e.RowIndex].FindControl("lblMinQty");

        TextBox txtRetailMulti = (TextBox)grdItem_Price.Rows[e.RowIndex].FindControl("txtRetailMulti");
        Label lblRetailMulti = (Label)grdItem_Price.Rows[e.RowIndex].FindControl("lblRetailMulti");

        TextBox txtLabor = (TextBox)grdItem_Price.Rows[e.RowIndex].FindControl("txtLabor");
        Label lblLabor = (Label)grdItem_Price.Rows[e.RowIndex].FindControl("lblLabor");
        int nLaborId = 1;
        if (Convert.ToDecimal(txtLabor.Text) > 0)
            nLaborId = 2;



        int nSectionId = Convert.ToInt32(grdItem_Price.DataKeys[Convert.ToInt32(e.RowIndex)].Values[0]);
        int nParentId = Convert.ToInt32(grdItem_Price.DataKeys[Convert.ToInt32(e.RowIndex)].Values[1]);
        string strItemName = txtItemName.Text.Replace("'", "''");
        if (_db.sectioninfos.Where(l => l.client_id == 1 && l.parent_id == nParentId && l.is_disable == false && l.section_name == strItemName).SingleOrDefault() != null)
        {
            List<sectioninfo> sList = _db.sectioninfos.Where(l => l.client_id == 1 && l.parent_id == nParentId && l.is_disable == false && l.section_name == strItemName).ToList();
            foreach (sectioninfo objsec in sList)
            {
                if (objsec.section_id != nSectionId)
                {
                    lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Item name is already exist. Please try another name to update");
                    lblItemResult.Text = csCommonUtility.GetSystemRequiredMessage("Item name is already exist. Please try another name to update");
                    return;
                }
            }
          
        }

        string strQ = "UPDATE sectioninfo SET section_name='" + txtItemName.Text.Replace("'", "''") + "' , is_mandatory ='" + Convert.ToBoolean(chkIsMandatory.Checked) + "',is_CommissionExclude ='" + Convert.ToBoolean(chkIsExcludeCom.Checked) + "',  is_active='" + Convert.ToBoolean(chkIsActiveItem.Checked) + "'  WHERE section_id=" + nSectionId + "  AND client_id=1";
        _db.ExecuteCommand(strQ, string.Empty);
        string strQItem = "UPDATE item_price SET measure_unit='" + txtMeasureUnit.Text + "', item_cost=" + Convert.ToDecimal(txtCost.Text) + ", minimum_qty=" + Convert.ToDecimal(txtMinQty.Text) + ", retail_multiplier=" + Convert.ToDecimal(txtRetailMulti.Text) + ", labor_rate=" + Convert.ToDecimal(txtLabor.Text) + ", update_time='" + DateTime.Now + "',labor_id=" + nLaborId + " WHERE item_id =" + nSectionId + " AND client_id=1";
        _db.ExecuteCommand(strQItem, string.Empty);

        LoadItemInfo();

        lblResult.Text = csCommonUtility.GetSystemMessage("Data updated successfully");
        lblItemResult.Text = csCommonUtility.GetSystemMessage("Data updated successfully");


    }

    protected void grdItem_Price_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            CheckBox chkIsActiveItem = (CheckBox)e.Row.FindControl("chkIsActiveItem");
            CheckBox chkIsMandatory = (CheckBox)e.Row.FindControl("chkIsMandatory");
            CheckBox chkIsExcludeCom = (CheckBox)e.Row.FindControl("chkIsExcludeCom");

            Label lblAMandatory = (Label)e.Row.FindControl("lblAMandatory");
            Label lblActive = (Label)e.Row.FindControl("lblActive");

            Label lblAExcludeCom = (Label)e.Row.FindControl("lblAExcludeCom");

          


            TextBox txtItemName = (TextBox)e.Row.FindControl("txtItemName");
            Label lblItemnName = (Label)e.Row.FindControl("lblItemnName");


            TextBox txtMeasureUnit = (TextBox)e.Row.FindControl("txtMeasureUnit");
            Label lblMeasureUnit = (Label)e.Row.FindControl("lblMeasureUnit");

            TextBox txtCost = (TextBox)e.Row.FindControl("txtCost");
            Label lblCost = (Label)e.Row.FindControl("lblCost");

            TextBox txtMinQty = (TextBox)e.Row.FindControl("txtMinQty");
            Label lblMinQty = (Label)e.Row.FindControl("lblMinQty");

            TextBox txtRetailMulti = (TextBox)e.Row.FindControl("txtRetailMulti");
            Label lblRetailMulti = (Label)e.Row.FindControl("lblRetailMulti");

            TextBox txtLabor = (TextBox)e.Row.FindControl("txtLabor");
            Label lblLabor = (Label)e.Row.FindControl("lblLabor");


            if (chkIsMandatory.Checked)
            {
                e.Row.Attributes.CssStyle.Add("color", "Violet");
                chkIsMandatory.Attributes.CssStyle.Add("color", "Violet");
                lblAMandatory.Text = "Yes";

            }
            else
            {
                lblAMandatory.Text = "No";
            }

            if (chkIsActiveItem.Checked)
            {
                lblActive.Text = "Yes";
            }
            else
            {
                lblActive.Text = "No";
            }

            if (chkIsExcludeCom.Checked)
            {
                lblAExcludeCom.Text = "Yes";
            }
            else
            {
                lblAExcludeCom.Text = "No";
            }

            string str = txtItemName.Text.Replace("&nbsp;", "");
            if (str == "" || Convert.ToInt32(grdItem_Price.DataKeys[Convert.ToInt32(e.Row.RowIndex)].Values[0]) == 0)
            {
                chkIsActiveItem.Visible = true;
                lblActive.Visible = false;

                chkIsMandatory.Visible = true;
                lblAMandatory.Visible = false;

                chkIsExcludeCom.Visible = true;
                lblAExcludeCom.Visible = false;

                txtItemName.Visible = true;
                lblItemnName.Visible = false;

                txtMeasureUnit.Visible = true;
                lblMeasureUnit.Visible = false;

                txtCost.Visible = true;
                lblCost.Visible = false;

                txtMinQty.Visible = true;
                lblMinQty.Visible = false;

                txtRetailMulti.Visible = true;
                lblRetailMulti.Visible = false;

                txtLabor.Visible = true;
                lblLabor.Visible = false;

                LinkButton btn = (LinkButton)e.Row.Cells[9].Controls[0];
                btn.Text = "Save";
                btn.CommandName = "Save";

            }


        }

    }
    protected void btnAddItem_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnAddItem.ID, btnAddItem.GetType().Name, "Click"); 
        DataClassesDataContext _db = new DataClassesDataContext();

        sectioninfo sinfo = new sectioninfo();
        sinfo = _db.sectioninfos.SingleOrDefault(c => c.section_id == Convert.ToInt32(hdnSubItemParentId.Value) && c.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));
        hdnSectionLevel.Value = sinfo.section_level.ToString();
        var result = (from sin in _db.sectioninfos
                      where sin.section_level == Convert.ToInt32(hdnSectionLevel.Value) && sin.client_id == 1 && sin.section_id > Convert.ToInt32(hdnSectionLevel.Value) + 100
                      select sin.section_id);
        int nsectionId = 0;
        int n = result.Count();
        if (result != null && n > 0)
            nsectionId = result.Max();

        if (nsectionId == 0)
        {
            nsectionId = Convert.ToInt32(hdnSectionLevel.Value) + 100 + 1;
        }
        else
        {
            nsectionId = nsectionId + 1;
        }
        hdnSectionId.Value = nsectionId.ToString();
        string strSerial = nsectionId.ToString();
        string str = "";
        if (strSerial.Length < 5)
        {
            str = strSerial.Substring(2);
        }
        hdnSectionSerial.Value = hdnSectionLevel.Value + "." + str;
        lblSerial.Text = hdnSectionSerial.Value;

        DataTable table = (DataTable)Session["NewItem"];

        int nSecId = Convert.ToInt32(hdnSectionId.Value);
        bool contains = table.AsEnumerable().Any(row => nSecId == row.Field<int>("section_id"));
        if (contains)
        {
            lblResult.Text = csCommonUtility.GetSystemRequiredMessage("You have already a pending item to save.");
            lblItemResult.Text = csCommonUtility.GetSystemRequiredMessage("You have already a pending item to save.");
            return;
        }
        foreach (GridViewRow di in grdItem_Price.Rows)
        {
            {
                CheckBox chkIsActiveItem = (CheckBox)di.FindControl("chkIsActiveItem");
                CheckBox chkIsMandatory = (CheckBox)di.FindControl("chkIsMandatory");
                CheckBox chkIsExcludeCom = (CheckBox)di.FindControl("chkIsExcludeCom");
                
                TextBox txtItemName = (TextBox)di.FindControl("txtItemName");
                Label lblItemnName = (Label)di.FindControl("lblItemnName");
                TextBox txtMeasureUnit = (TextBox)di.FindControl("txtMeasureUnit");
                Label lblMeasureUnit = (Label)di.FindControl("lblMeasureUnit");

                TextBox txtCost = (TextBox)di.FindControl("txtCost");
                Label lblCost = (Label)di.FindControl("lblCost");

                TextBox txtMinQty = (TextBox)di.FindControl("txtMinQty");
                Label lblMinQty = (Label)di.FindControl("lblMinQty");

                TextBox txtRetailMulti = (TextBox)di.FindControl("txtRetailMulti");
                Label lblRetailMulti = (Label)di.FindControl("lblRetailMulti");

                TextBox txtLabor = (TextBox)di.FindControl("txtLabor");
                Label lblLabor = (Label)di.FindControl("lblLabor");
                DataRow dr = table.Rows[di.RowIndex];

                dr["section_id"] = Convert.ToInt32(grdItem_Price.DataKeys[di.RowIndex].Values[0]);
                dr["client_id"] = 1;
                dr["section_name"] = txtItemName.Text;
                dr["parent_id"] = Convert.ToInt32(grdItem_Price.DataKeys[di.RowIndex].Values[1]);
                dr["section_notes"] = "";
                dr["section_level"] = Convert.ToInt32(grdItem_Price.DataKeys[di.RowIndex].Values[2]);
                dr["section_serial"] = Convert.ToDecimal(grdItem_Price.DataKeys[di.RowIndex].Values[3]);
                dr["is_active"] = Convert.ToBoolean(chkIsActiveItem.Checked);                
                dr["is_mandatory"] = Convert.ToBoolean(chkIsMandatory.Checked);
                dr["is_CommissionExclude"] = Convert.ToBoolean(chkIsExcludeCom.Checked);
                dr["create_date"] = DateTime.Now;

                dr["item_id"] = Convert.ToInt32(grdItem_Price.DataKeys[di.RowIndex].Values[0]);
                dr["measure_unit"] = txtMeasureUnit.Text;
                dr["item_cost"] = Convert.ToDecimal(txtCost.Text);
                dr["minimum_qty"] = Convert.ToDecimal(txtMinQty.Text);
                dr["retail_multiplier"] = Convert.ToDecimal(txtRetailMulti.Text);
                dr["labor_rate"] = Convert.ToDecimal(txtLabor.Text);
                if (Convert.ToDecimal(txtLabor.Text) > 0)
                    dr["labor_id"] = 2;
                else
                    dr["labor_id"] = 1;
              


            }

        }

       
        DataRow drNew = table.NewRow();
        drNew["section_id"] = Convert.ToInt32(hdnSectionId.Value);
        drNew["client_id"] = 1;
        drNew["section_name"] = "";
        drNew["parent_id"] = Convert.ToInt32(hdnSubItemParentId.Value);
        drNew["section_notes"] = "";
        drNew["section_level"] = Convert.ToInt32(hdnSectionLevel.Value);
        drNew["section_serial"] = Convert.ToDecimal(hdnSectionSerial.Value);
        drNew["is_active"] = true;
        drNew["create_date"] = DateTime.Now;

        drNew["item_id"] = Convert.ToInt32(hdnSectionId.Value);
        drNew["measure_unit"] = "";
        drNew["item_cost"] = 0;
        drNew["minimum_qty"] = 0;
        drNew["retail_multiplier"] = Convert.ToDecimal(hdnMultiplier.Value);
        drNew["labor_rate"] = 0;
        drNew["labor_id"] = 2;
        drNew["is_mandatory"] = false;
        drNew["is_CommissionExclude"] = false;
        
        
        //table.Rows.Add(drNew);
        table.Rows.InsertAt(drNew, 0);

        Session.Add("NewItem", table);
        grdItem_Price.DataSource = table;
        grdItem_Price.DataKeyNames = new string[] { "section_id", "parent_id", "section_level", "section_serial", "labor_id","is_mandatory","is_CommissionExclude" };
        grdItem_Price.DataBind();
        lblResult.Text = "";
        lblMainSecResult.Text = "";
        lblItemResult.Text = "";
        lblSubSecResult.Text = "";
    }
    #endregion
    protected void btnDisable_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnDisable.ID, btnDisable.GetType().Name, "Click");
        
        DataClassesDataContext _db = new DataClassesDataContext();
        bool ischecked = false;
        try
        {
            foreach (GridViewRow di in grdItem_Price.Rows)
            {
                {
                    CheckBox chkIsDisable = (CheckBox)di.FindControl("chkIsDisable");

                    int nSectionId = Convert.ToInt32(grdItem_Price.DataKeys[Convert.ToInt32(di.RowIndex)].Values[0]);
                    if (chkIsDisable.Checked)
                    {
                        ischecked = true;
                        string strQ = "UPDATE sectioninfo SET is_disable='" + Convert.ToBoolean(chkIsDisable.Checked) + "'  WHERE section_id=" + nSectionId + "  AND client_id=1";
                        _db.ExecuteCommand(strQ, string.Empty);
                    }
                }
            }
            if (ischecked)
            {
                LoadItemInfo();
                lblResult.Text = csCommonUtility.GetSystemMessage("Items has been Disabled Successfully");
            }
            else
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Please select Item(s)");
            }
        }
        catch (Exception ex)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage(ex.Message);
        }
    }

    protected void btnHome_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnHome.ID, btnHome.GetType().Name, "Click"); 
        lblMainSection.Visible = true;
        btnAddnewRow.Visible = true;
        grdMainSection.Visible = true;
        btnAddnewRow.Visible = true;
        hdnParentId.Value = "0";
        hdnSectionId.Value = "0";
        hdnSectionSerial.Value = "0";
        lblParent.Text = "";
        btnAddSubnewRow.Visible = false;
        btnAddItem.Visible = false;
        lblTree.Visible = false;
        lblTree.Visible = false;
        lblSubSection.Visible = false;
        lblItemList.Visible = false;
        grdSubSection.Visible = false;
        grdItem_Price.Visible = false;

        LoadTree();



    }
    protected void grdItem_Price_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dtItems = (DataTable)Session["NewItem"];

        string strShort = e.SortExpression + " " + hdnOrder.Value;

        if (hdnOrder.Value == "DESC")
        {
            hdnOrder.Value = "ASC";
        }
        else
        {
            hdnOrder.Value = "DESC";
        }
        strShort = e.SortExpression + " " + hdnOrder.Value;
        DataView dv = dtItems.DefaultView;
        dv.Sort = strShort;
        Session["NewItem"] = dv.ToTable();
        dtItems = (DataTable)Session["NewItem"];
        grdItem_Price.DataSource = dtItems;
        grdItem_Price.DataKeyNames = new string[] { "section_id", "parent_id", "section_level", "section_serial", "labor_id","is_mandatory","is_CommissionExclude" };
        grdItem_Price.DataBind();

    }
    protected void grdSubSection_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dtSubSection = (DataTable)Session["SubSection"];

        string strShort = e.SortExpression + " " + hdnOrder.Value;

        if (hdnOrder.Value == "DESC")
        {
            hdnOrder.Value = "ASC";
        }
        else
        {
            hdnOrder.Value = "DESC";
        }
        strShort = e.SortExpression + " " + hdnOrder.Value;
        DataView dv = dtSubSection.DefaultView;
        dv.Sort = strShort;
        Session["SubSection"] = dv.ToTable();
        dtSubSection = (DataTable)Session["SubSection"];
        grdSubSection.DataSource = dtSubSection;
        grdSubSection.DataKeyNames = new string[] { "section_id", "parent_id", "section_level", "section_serial" };
        grdSubSection.DataBind();

    }
    protected void grdMainSection_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dtMainSection = (DataTable)Session["MainSection"];

        string strShort = e.SortExpression + " " + hdnOrder.Value;

        if (hdnOrder.Value == "DESC")
        {
            hdnOrder.Value = "ASC";
        }
        else
        {
            hdnOrder.Value = "DESC";
        }
        strShort = e.SortExpression + " " + hdnOrder.Value;
        DataView dv = dtMainSection.DefaultView;
        dv.Sort = strShort;
        Session["MainSection"] = dv.ToTable();
        dtMainSection = (DataTable)Session["MainSection"];

        grdMainSection.DataSource = dtMainSection;
        grdMainSection.DataKeyNames = new string[] { "section_id", "parent_id", "section_level", "section_serial" };
        grdMainSection.DataBind();

    }
}