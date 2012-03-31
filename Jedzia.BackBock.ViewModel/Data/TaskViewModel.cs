using System;
using System.ComponentModel;

namespace Jedzia.BackBock.ViewModel.Data
{
    public class MyStructure
    {
        private int valueOne;

        public int ValueOne
        {
            get { return valueOne; }
            set { valueOne = value; }
        }
        private Boolean valueTwo;

        public Boolean ValueTwo
        {
            get { return valueTwo; }
            set { valueTwo = value; }
        }
    }

    public partial class TaskViewModel
    {
        private MyStructure myProperty = new MyStructure();

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
        /// The summary.
        /// </summary>
        [NonSerialized]
        private object taskInstance;

        /// <summary>
        /// Gets or sets the Task.
        /// </summary>
        /// <value>The Task.</value>
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
        /// Gets or sets 
        /// </summary>
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
