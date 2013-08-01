using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using Styx.Models;
using Styx.Util;
using NLog;

namespace Styx
{
    public sealed class DataManager
    {

        #region Singleton Constructor

        private static volatile DataManager instance;
        private static object syncRoot = new Object();

        public static DataManager Instance
        {
            get
            {
                if (instance != null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DataManager();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Private Properties
        private Logger logger = LogManager.GetLogger("DataManager");
        #endregion

        #region Public Properties
        public ConfigSet configSet = new ConfigSet();
        public WorkingSet workingSet = new WorkingSet();
        #endregion

        private DataManager() 
        {
            //TODO: read xml files

            //TODO: track xml files
        }

        #region Configuration Repository

        public string GetAclAction(string ClientIp, string RequestedLocation)
        {
            //TODO: add time limitation to acls
            var acls = configSet.IpAccessList.Select("", "OrderId ASC");
            foreach (var acl in acls)
            {
                string ip = acl[configSet.IpAccessList.IPColumn] as string;
                string mask = acl[configSet.IpAccessList.MaskColumn] as string;
                string location = acl[configSet.IpAccessList.LocationColumn] as string;
                string action = acl[configSet.IpAccessList.ActionColumn] as string;

                if (StringUtil.IsNotNullOrEmpty(ip, mask, location, action))
                {
                    bool ipMatches = IpUtil.IsInIpRange(ClientIp, ip, mask);
                    bool locationMatches = Regex.IsMatch(RequestedLocation, location);

                    if (ipMatches && locationMatches)
                    {
                        return action;
                    }
                }
            }
            return "Permit";
        }

        //TODO: add dispose method to write to xml files (research)

        #endregion
    }

    
}
