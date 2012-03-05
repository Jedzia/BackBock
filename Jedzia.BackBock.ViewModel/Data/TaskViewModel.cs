using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Jedzia.BackBock.Tasks;

namespace Jedzia.BackBock.ViewModel.Data
{
    using Microsoft.Build.Framework;

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
        private ITask taskInstance;

        /// <summary>
        /// Gets or sets the Task.
        /// </summary>
        /// <value>The Task.</value>
        [Browsable(false)]
        public ITask TaskInstance
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
