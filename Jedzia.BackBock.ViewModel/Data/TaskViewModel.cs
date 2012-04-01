using System;
using System.ComponentModel;

namespace Jedzia.BackBock.ViewModel.Data
{
    /// <summary>
    /// Test. DeleteME
    /// </summary>
    public class MyStructure
    {
        private int valueOne;

        /// <summary>
        /// Gets or sets the value one.
        /// </summary>
        /// <value>
        /// The value one.
        /// </value>
        public int ValueOne
        {
            get { return valueOne; }
            set { valueOne = value; }
        }
        private Boolean valueTwo;

        /// <summary>
        /// Gets or sets a value indicating whether [value two].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [value two]; otherwise, <c>false</c>.
        /// </value>
        public Boolean ValueTwo
        {
            get { return valueTwo; }
            set { valueTwo = value; }
        }
    }

    /// <summary>
    /// DataViewModel representation of the <see cref="TaskType"/> data.
    /// </summary>
    public partial class TaskViewModel
    {
        private MyStructure myProperty = new MyStructure();

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskViewModel"/> class.
        /// </summary>
        public TaskViewModel()
        {
            this.data = new Jedzia.BackBock.Model.Data.TaskType();
            this.data.TypeName = "NasenBock";
        }

        /*/// <summary>
        /// Gets or sets 
        /// </summary>
        public List<System.Xml.XmlAttribute> Attributes
        {
            get
            {
                return this.data.AnyAttr;
            }

            set
            {
                this.data.AnyAttr = value;
            }
        }*/

        /// <summary>
        /// Backing field of the TaskInstance property.
        /// </summary>
        [NonSerialized]
        private object taskInstance;

        /// <summary>
        /// Gets or sets the task instance used to display task data editors.
        /// </summary>
        /// <value>
        /// The task instance.
        /// </value>
        [Browsable(false)]
        public object TaskInstance
        {
            get
            {
                return this.taskInstance;
            }

            set
            {
                if (this.taskInstance == value)
                {
                    return;
                }
                this.taskInstance = value;
                RaisePropertyChanged("TaskInstance");
            }
        }

        /// <summary>
        /// Gets or sets my property. DeleteMe
        /// </summary>
        /// <value>
        /// My property.
        /// </value>
        [DisplayNameAttribute("Arschloch")]
        public MyStructure MyProperty
        {
            get
            {
                return this.myProperty;
            }

            set
            {
                this.myProperty = value;
            }
        }
    }
}
