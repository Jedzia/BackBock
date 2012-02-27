using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Jedzia.BackBock.ViewModel.Data
{
    public class MyStructure
    {
        public int ValueOne;
        public Boolean ValueTwo;
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
