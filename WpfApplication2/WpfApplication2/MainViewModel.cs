using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Components.DictionaryAdapter;

namespace WpfApplication2
{
    public class MainViewModel : ViewModelBase
    {

        /// <summary>
        /// The summary.
        /// </summary>
        private IDepp mainWindow;

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        //[DoNotWire]
        public IDepp MainWindow
        {
            get
            {
                return this.mainWindow;
            }

            set
            {
                this.mainWindow = value;
            }
        }

        private readonly IDataProvider dataprovider;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MainViewModel"/> class.
        /// </summary>
        public MainViewModel(IDataProvider dataprovider)
        {
            this.dataprovider = dataprovider;
        }
        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private string _title = null;

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                if (_title == null)
                {
                    _title = dataprovider.GetTitle();
                    RaisePropertyChanged(() => Title);
                }
                return _title;
            }
            set
            {
                Set(() => Title, ref _title, value);
            }
        }

        private RelayCommand _myCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand MyCommand
        {
            get
            {
                return _myCommand
                    ?? (_myCommand = new RelayCommand(
                                          () =>
                                          {
                                              Fuck();
                                          }));
            }
        }

        public void Fuck()
        {
            var aa = this;
        }
    }
}
