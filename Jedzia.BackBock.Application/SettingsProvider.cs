using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.ViewModel;
using Jedzia.BackBock.Application.Properties;

namespace Jedzia.BackBock.Application
{
    class SettingsProvider : ISettingsProvider, IDisposable
    {
        private Settings settings;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SettingsProvider"/> class.
        /// </summary>
        public SettingsProvider(Settings settings)
        {
            this.settings = settings;
        }

        #region ISettingsProvider Members

        public string GetStartupDataFile()
        {
            return settings.StartupData;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.settings.Save();
            }
        }
    }
}
