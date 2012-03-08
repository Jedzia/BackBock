
using System.Windows.Input;
using Jedzia.BackBock.ViewModel.Commands;
namespace Jedzia.BackBock.ViewModel.Wizard
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class TaskWizardViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private string title = "Task Wizard";

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                if (title == value)
                {
                    return;
                }

                title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Path" /> property's name.
        /// </summary>
        public const string PathPropertyName = "Path";

        private string path;

        /// <summary>
        /// Sets and gets the Path property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Path
        {
            get
            {
                return path;
            }

            set
            {
                if (path == value)
                {
                    return;
                }

                path = value;
                RaisePropertyChanged(() => Path);
            }
        }

        public IStateWizard Wizard { get; set; }
        /// <summary>
        /// Initializes a new instance of the TaskWizardViewModel class.
        /// </summary>
        public TaskWizardViewModel(/*IStateWizard instance*/)
        {
            //MessengerInstance.Send(pagecount.ToString());
            baseWizard = new BaseWizard();
        }
        private BaseWizard baseWizard;

        #region Next Command

        private RelayCommand nextCommand;

        public ICommand NextCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.nextCommand == null)
                {
                    this.nextCommand = new RelayCommand(this.NextExecuted, this.NextEnabled);
                }

                return this.nextCommand;
            }
        }


        private void NextExecuted(object o)
        {
            var pgc = this.Wizard.PageCount;
            var pgs = this.Wizard.SelectedPage;
            baseWizard.Fire(Trigger.Next);
            this.Wizard.SelectedPage++;
            //this.Next();
        }

        private bool NextEnabled(object sender)
        {
            bool canExecute = 
                baseWizard.State == State.Initial | 
                baseWizard.State == State.ChooseTaskType | 
                baseWizard.State == State.SelectFolders;
            return canExecute;
        }
        #endregion

                #region Previous Command

        private RelayCommand previousCommand;

        public ICommand PreviousCommand
        {
            get
            {
                // See S.142 Listing 5–18. Using Attached Command Behavior to Add Double-Click Functionality to a List Item
                if (this.previousCommand == null)
                {
                    this.previousCommand = new RelayCommand(this.PreviousExecuted, this.PreviousEnabled);
                }

                return this.previousCommand;
            }
        }


        private void PreviousExecuted(object o)
        {
            //this.Previous();
            baseWizard.Fire(Trigger.Previous);
            this.Wizard.SelectedPage--;
        }

        private bool PreviousEnabled(object sender)
        {
            bool canExecute =
                baseWizard.State == State.ChooseTaskType |
                baseWizard.State == State.SelectFolders |
                baseWizard.State == State.ReadyToAccept;
            return canExecute;
        }
        #endregion

    }
}