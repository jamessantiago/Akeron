using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Data;
using NLog;

namespace Styx
{
    public class Akeron : IHttpModule, IDisposable
    {
        #region Private Properties

        private Logger logger = LogManager.GetLogger("Akeron");        

        private delegate void RequestVerifier(HttpContext context);

        //TODO: inject running config model (ADO.NET/dataset)        

        #endregion        

        #region IHttpModule Members

        public void Init(HttpApplication application)
        {
            //see http://support.microsoft.com/kb/307985 for event descriptions

            application.BeginRequest += new EventHandler(OnBeginRequest);
            // TODO: add additional application event handlers here
        }
        
        #endregion

        #region Module Event Handlers

        public void OnBeginRequest(Object source, EventArgs e)
        {

            HttpApplication app = (HttpApplication)source;
            HttpContext context = app.Context;


            // TODO: implement module functionality here
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
                //TODO track objects requiring disposal
            }
        }

        #endregion Dispose
    }

    public static class SyncLocks
    {
        public static object ContextSync = new object();
    }

    public class ContextWrapper
    {
        public HttpContext context {get; set;}
        public bool IsCompleted { get; set; }
    }
}
