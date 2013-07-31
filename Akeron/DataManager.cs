using System;
using System.Collections.Generic;
using System.Data;
using Styx.Models;
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

        #region private properties

        public ConfigSet configSet = new ConfigSet();
        public WorkingSet workingSet = new WorkingSet();
        private Logger logger = LogManager.GetLogger("DataManager");

        #endregion

        private DataManager() 
        {
            //read xml files

            //track xml files
        }

        #region Configuration Repository

        public bool IsActiveIds()
        {
            return (bool)configSet.Global.Rows[0][configSet.Global.IsActiveIdsColumn];
        }

        public string GetAclAction(string ClientIp, string RequestedLocation)
        {
            return "Deny";
        }

        #endregion
    }

    
}
