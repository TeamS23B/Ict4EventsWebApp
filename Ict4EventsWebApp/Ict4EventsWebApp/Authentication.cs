using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;

namespace Ict4EventsWebApp
{
    public class Authentication
    {
        private readonly string _domain = ConfigurationManager.AppSettings.Get("ip");
        private readonly string _username = ConfigurationManager.AppSettings.Get("Username");
        private readonly string _password = ConfigurationManager.AppSettings.Get("Password");

        public static Authentication Instance
        {
            get { return _instance ?? (_instance = new Authentication()); }
        }

        private static Authentication _instance;

        private PrincipalContext context;
        
        private Authentication()
        {
            Console.WriteLine("Ip: {0}, User: {1}, Pass: {2}",_domain,_username,_password);
            context = context = new PrincipalContext(ContextType.Domain, _domain, _username, _password);
            context.ValidateCredentials(_username, _password, ContextOptions.ServerBind|ContextOptions.SimpleBind); ;
        }

        public bool IsAuthenticated(String userame, String password)
        {
            return context.ValidateCredentials(userame, password);
        }

        public void AddUser(String username, String password)
        {
#if DEBUG
            return;
#endif
            using (var up = new UserPrincipal(context))
            {

                up.SamAccountName = username;
                up.SetPassword(password);
                up.Enabled = true;
                up.Save();
            }
        }
    }
}