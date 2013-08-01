using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
using Styx.Models;
using Styx.Util;
using NLog;

namespace Styx
{
    public sealed class DataManager : IDisposable
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
            configSet.ReadXml("Configuration.xml");
            workingSet.ReadXml("WorkingData.xml");

            FileSystemWatcher configWatcher = new FileSystemWatcher();
            configWatcher.NotifyFilter = NotifyFilters.LastWrite;
            configWatcher.Filter = "Configuration.xml";
            configWatcher.Changed += new FileSystemEventHandler(OnConfigChanged);
            configWatcher.EnableRaisingEvents = true;            
        }

        #region File Watcher Events

        private void OnConfigChanged(object source, FileSystemEventArgs e)
        {
            lock (SyncLocks.ConfigSetSync)
            {
                configSet.ReadXml("Configuration.xml");
            }
        }        

        #endregion

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
        #endregion

        #region Dispose

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DataManager()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                configSet.WriteXml("Configuration.xml");
                configSet.Dispose();
                configSet = null;

                workingSet.WriteXml("Configuration.xml");
                workingSet.Dispose();
                workingSet = null;
            }
        }

        #endregion Dispose
    }

    
}
