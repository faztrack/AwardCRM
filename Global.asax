<%@ Application Language="C#" %>
<%@ Import Namespace="System.Security.Principal" %>
<%@ Import Namespace="Autofac" %>
<%@ Import Namespace="elFinder.Connector.Integration.Autofac" %>

<script RunAt="server">
    private static IContainer _container;

    void Application_Start(object sender, EventArgs e)
    {
        try
        {
            // Code that runs on application startup
            var builder = new ContainerBuilder();
            builder.RegisterElFinderConnectorDefault();
            _container = builder.Build();
            // need also to set container in elFinder module
            _container.SetAsElFinderDependencyResolver();
        }
        catch (System.Reflection.ReflectionTypeLoadException ex)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Exception exSub in ex.LoaderExceptions)
            {
                sb.AppendLine(exSub.Message);
                System.IO.FileNotFoundException exFileNotFound = exSub as System.IO.FileNotFoundException;
                if (exFileNotFound != null)
                {
                    if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                    {
                        sb.AppendLine("Fusion Log:");
                        sb.AppendLine(exFileNotFound.FusionLog);
                    }
                }
                sb.AppendLine();
            }
            string errorMessage = sb.ToString();
            //Display or log the error based on your application.
        }
    }
    protected void Application_AuthenticateRequest(Object sender, EventArgs e)
    {
        // Extract the forms authentication cookie
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = Context.Request.Cookies[cookieName];

        if (null == authCookie)
        {
            // There is no authentication cookie.
            return;
        }

        FormsAuthenticationTicket authTicket = null;
        try
        {
            authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        }
        catch //(Exception ex)
        {
            // Log exception details (omitted for simplicity)
            return;
        }

        if (null == authTicket)
        {
            // Cookie failed to decrypt.
            return;
        }

        // When the ticket was created, the UserData property was assigned a
        // pipe delimited string of role names.
        string[] roles = authTicket.UserData.Split(new char[] { '|' });


        // Create an Identity object
        FormsIdentity id = new FormsIdentity(authTicket);


        GenericPrincipal principal = new GenericPrincipal(id, roles);
        Context.User = principal;


    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
       

        string strRequestedURLWithQueryString = ((System.Web.HttpApplication)(sender)).Request.Url.AbsoluteUri;
        
        //if (strRequestedURLWithQueryString.ToLower().IndexOf("commonscript") == -1)
       // {
          //  Server.Transfer("~/error.aspx?RequestedURL=" + Server.UrlEncode(strRequestedURLWithQueryString));
        //}
        /*
        //if (Session["oUser"] != null)
        //{
        //    strRequestedURLWithQueryString = ((System.Web.HttpApplication)(sender)).Request.Url.AbsoluteUri;
        //    Server.Transfer("~/error.aspx?RequestedURL=" + Server.UrlEncode(strRequestedURLWithQueryString));
        //}
        //else
        //{
        //    Server.Transfer("~/AwardCRMLogin.aspx");
        //}
         * */
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
	

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
