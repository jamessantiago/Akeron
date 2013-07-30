using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Modules
{
    public class Akeron : IHttpModule, IDisposable
    {
        //TODO: inject running config model (ADO.NET/dataset)

        #region IHttpModule Members

        public void Init(HttpApplication application)
        {
            //see http://support.microsoft.com/kb/307985 for event descriptions

            application.PreRequestHandlerExecute += new EventHandler(OnPreRequestHandlerExecute);
            
            // TODO: add additional application event handlers here
        }
        
        #endregion

        #region Module Event Handlers

        public void OnPreRequestHandlerExecute(Object source, EventArgs e)
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
}
