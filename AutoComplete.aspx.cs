using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class AutoComplete : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
        }
    }
    [System.Web.Services.WebMethod]
    public static List<string> GetData()
    {
        string[] array = new string[] { "Art", "Aerobics", "Asia", "Assistant", "AssistantManager", "Bakery", "Bakes", "Barbaq", "Bar", "Cafeteria Manager", "Cofee", "Costarica", "Casino" };
        // Here I am returning as a simple list, u can return any
        return array.ToList<string>();
    }
    [System.Web.Services.WebMethod]
    public static List<csCustomer> GetCustomer(string keyword)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        customer objCust = new customer();

        //  IEnumerable<csCustomer> mList

        var item = from c in _db.customers
                   where c.first_name1.ToUpper().StartsWith(keyword.ToUpper())
                   select new csCustomer
                   {
                       customer_id = c.customer_id,
                       first_name1 = c.first_name1
                   };
       
        DataTable dtMain = SessionInfo.LINQToDataTable(item);
        return item.ToList();
    }
}
public class CustomerDTO
{
    public string firstname { get; set; }
    public string lastname { get; set; }
}