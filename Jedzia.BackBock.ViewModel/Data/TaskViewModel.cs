using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
