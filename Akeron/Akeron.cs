using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Data;
using Styx.Guards;
using NLog;

namespace Styx
{
    public class Akeron : IHttpModule, IDisposable
    {
        #region Private Properties

        private Logger logger = LogManager.GetLogger("Akeron");        

        private delegate void RequestVerifier(ref ContextWrapper wrapper);

        #endregion        

        #region IHttpModule Members

        public void Init(HttpApplication application)
        {
            //see http://support.microsoft.com/kb/307985 for event descriptions

            application.BeginRequest += new EventHandler(OnBeginRequest);
            application.EndRequest += new EventHandler(OnEndRequest);
        }
        
        #endregion

        #region Module Event Handlers

        public void OnBeginRequest(Object source, EventArgs e)
        {

            HttpApplication app = (HttpApplication)source;
            ContextWrapper wrapper = new ContextWrapper()
            {
                context = app.Context,
                IsCompleted = false
            };

            RequestVerifier checkClientIp = new RequestVerifier(AclGuard.CheckClientIp);

            //TODO: add full set of guard checks
        }

        public void OnEndRequest(Object source, EventArgs e)
        {
            HttpApplication app = (HttpApplication)source;
            HttpContext context = app.Context;

            //TODO: add post response checks
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Akeron()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //nothing to dispose so far
            }
        }

        #endregion Dispose
    }

    public static class SyncLocks
    {
        public static object ContextSync = new object();
        public static object ConfigSetSync = new object();
        public static object WorkingSetSync = new object();
    }

    public class ContextWrapper
    {
        public HttpContext context {get; set;}
        public bool IsCompleted { get; set; }
    }
}
