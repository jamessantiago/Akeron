using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using NLog;

namespace Styx.Guards
{
    public static class AclGuard
    {
        private static Logger logger = LogManager.GetLogger("AclGuard");

        public static void CheckClientIp(ref ContextWrapper wrapper)
        {
            string action = DataManager.Instance.GetAclAction(wrapper.context.Request.UserHostAddress, wrapper.context.Request.Path);
            switch (action)
            {
                case "Permit":
                    break;
                case "Deny":
                    lock (SyncLocks.ContextSync)
                    {
                        wrapper.context.Response.Clear();
                        wrapper.context.Response.End();
                        wrapper.IsCompleted = true;
                    }
                    break;
                default: //Log
                    break;
            }
        }

        //TODO: detect http splits
        
    }
}
