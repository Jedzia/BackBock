using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using Jedzia.BackBock.ViewModel.Data;
using Jedzia.BackBock.Model.Data;

namespace Jedzia.BackBock.ViewModel.MainWindow
{
    // Todo: Move this to converters namespace.
    public sealed class DisplayModeToVisibility : DependencyObject, IValueConverter
    {
        #region Fields

        public static readonly DependencyProperty InvertProperty =
            DependencyProperty.Register(
                "Invert",
                typeof(bool),
                typeof(DisplayModeToVisibility),
                new FrameworkPropertyMetadata(false));

        #endregion

        #region Properties

        public bool Invert
        {
            get
            {
                return (bool)GetValue(InvertProperty);
            }
            set
            {
                SetValue(InvertProperty, value);
            }
        }

        #endregion

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value != null) ^ this.Invert)
            {
                return Visibility.Visible.ToString();
            }
            //return "";
            return Visibility.Collapsed.ToString();
            //return "Class";
        }

        // No need to implement converting back on a one-way binding 
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DisplayModeViewModel : ViewModelBase
    {
        /// <summary>
        /// The summary.
        /// </summary>
        private DisplayMode displayMode = Jedzia.BackBock.ViewModel.DisplayMode.Standard;

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public DisplayMode DisplayMode
        {
            get
            {
                return this.displayMode;
            }

            set
            {
                if (this.displayMode == value)
                {
                    return;
                }
                this.displayMode = value;
                RaisePropertyChanged("DisplayMode");
                // Todo: Transition Logic
                RaisePropertyChanged("DisplayExpert");
                RaisePropertyChanged("DisplayAll");
            }
        }

        public string DisplayExpert
        {
            get
            {
                return this.IsExpertDisplayMode ? "Visible" : "Collapsed";
            }
        }

        public string DisplayAll
        {
            get
            {
                return this.IsAllDisplayMode ? "Visible" : "Collapsed";
            }
        }

        public bool IsStandardDisplayMode
        {
            get
            {
                return this.DisplayMode == DisplayMode.Standard |
                    this.DisplayMode == DisplayMode.Expert |
                    this.DisplayMode == DisplayMode.All;
            }
        }

        public bool IsExpertDisplayMode
        {
            get
            {
                return this.DisplayMode == DisplayMode.Expert |
                    this.DisplayMode == DisplayMode.All;
            }
        }

        public bool IsAllDisplayMode
        {
            get
            {
                return this.DisplayMode == DisplayMode.All;
            }
        }

    }

   /* public sealed class ClassGeneralOptViewModel : DisplayModeViewModel
    {
        ClassDataItemListViewModel origClassData;
        ClassDataItemListViewModel workClassData;
        public ClassGeneralOptViewModel(ClassDataItemListViewModel classData)
        {
            this.workClassData = new ClassDataItemListViewModel(classData.classdataitemlist.Clone());
            this.origClassData = classData;
        }
        /// <summary>
        /// The summary.
        /// </summary>

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public string ClassName
        {
            get
            {
                return this.workClassData.Name;
            }

            set
            {
                if (this.workClassData.Name == value)
                {
                    return;
                }
                this.workClassData.Name = value;
                RaisePropertyChanged("ClassName");
            }
        }

        internal void Accept()
        {
            origClassData.Name = workClassData.Name;
            origClassData.Type = workClassData.Type;
            origClassData.Color = workClassData.Color;
        }
    }

    public sealed class ClassFieldOptViewModel : DisplayModeViewModel
    {
        // Todo: Alle FeldEigenschaften abbilden. (oder autogen?)
        ClassDataItemViewModel origMemberData;
        ClassDataItemViewModel workMemberData;
        public ClassFieldOptViewModel(ClassDataItemViewModel classData)
        {
            this.workMemberData = classData;
            // this.origMemberData = classData;
            //this.workMemberData = new ClassDataItemViewModel(classData.ClassDataItem);
        }

        // ItemType. HasFacets: False AttrQName: 
        //                   propertyType: System.String 
        /// <summary>
        /// Gets or sets the ItemType. HasFacets: False AttrQName: 
        /// </summary>
        /// <value>The ItemType.</value>
        public System.String ItemType
        {
            get
            {
                return this.workMemberData.ItemType;
            }

            set
            {
                if (this.workMemberData.ItemType == value)
                {
                    return;
                }
                this.workMemberData.ItemType = value;
                RaisePropertyChanged("ItemType");
            }
        }

        // Name. HasFacets: False AttrQName: 
        //                   propertyType: System.String 
        /// <summary>
        /// Gets or sets the Name. HasFacets: False AttrQName: 
        /// </summary>
        /// <value>The Name.</value>
        public System.String Name
        {
            get
            {
                return this.workMemberData.Name;
            }

            set
            {
                if (this.workMemberData.Name == value)
                {
                    return;
                }
                this.workMemberData.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        // Color. HasFacets: False AttrQName: 
        //                   propertyType: System.String 
        /// <summary>
        /// Gets or sets the Color. HasFacets: False AttrQName: 
        /// </summary>
        /// <value>The Color.</value>
        public System.String Color
        {
            get
            {
                return this.workMemberData.Color;
            }

            set
            {
                if (this.workMemberData.Color == value)
                {
                    return;
                }
                this.workMemberData.Color = value;
                RaisePropertyChanged("Color");
            }
        }

        // Scope. HasFacets: False AttrQName: 
        //                   propertyType: System.String 
        /// <summary>
        /// Gets or sets the Scope. HasFacets: False AttrQName: 
        /// </summary>
        /// <value>The Scope.</value>
        public System.String Scope
        {
            get
            {
                return this.workMemberData.Scope;
            }

            set
            {
                if (this.workMemberData.Scope == value)
                {
                    return;
                }
                this.workMemberData.Scope = value;
                RaisePropertyChanged("Scope");
            }
        }

        // AccessSpecifier. HasFacets: False AttrQName: 
        //                   propertyType: System.String 
        /// <summary>
        /// Gets or sets the AccessSpecifier. HasFacets: False AttrQName: 
        /// </summary>
        /// <value>The AccessSpecifier.</value>
        public System.String AccessSpecifier
        {
            get
            {
                return this.workMemberData.AccessSpecifier;
            }

            set
            {
                if (this.workMemberData.AccessSpecifier == value)
                {
                    return;
                }
                this.workMemberData.AccessSpecifier = value;
                RaisePropertyChanged("AccessSpecifier");
            }
        }

        // Static. HasFacets: False AttrQName: 
        //                   propertyType: System.Boolean 
        /// <summary>
        /// Gets or sets the Static. HasFacets: False AttrQName: 
        /// </summary>
        /// <value>The Static.</value>
        public System.Boolean Static
        {
            get
            {
                return this.workMemberData.Static;
            }

            set
            {
                if (this.workMemberData.Static == value)
                {
                    return;
                }
                this.workMemberData.Static = value;
                RaisePropertyChanged("Static");
            }
        }

        // Signature. HasFacets: False AttrQName: 
        //                   propertyType: System.String 
        /// <summary>
        /// Gets or sets the Signature. HasFacets: False AttrQName: 
        /// </summary>
        /// <value>The Signature.</value>
        public System.String Signature
        {
            get
            {
                return this.workMemberData.Signature;
            }

            set
            {
                if (this.workMemberData.Signature == value)
                {
                    return;
                }
                this.workMemberData.Signature = value;
                RaisePropertyChanged("Signature");
            }
        }

        // Parameternames. HasFacets: False AttrQName: 
        //                   propertyType: System.String 
        /// <summary>
        /// Gets or sets the Parameternames. HasFacets: False AttrQName: 
        /// </summary>
        /// <value>The Parameternames.</value>
        public System.String Parameternames
        {
            get
            {
                return this.workMemberData.Parameternames;
            }

            set
            {
                if (this.workMemberData.Parameternames == value)
                {
                    return;
                }
                this.workMemberData.Parameternames = value;
                RaisePropertyChanged("Parameternames");
            }
        }

        // ReturnType. HasFacets: False AttrQName: 
        //                   propertyType: System.String 
        /// <summary>
        /// Gets or sets the ReturnType. HasFacets: False AttrQName: 
        /// </summary>
        /// <value>The ReturnType.</value>
        public System.String ReturnType
        {
            get
            {
                return this.workMemberData.ReturnType;
            }

            set
            {
                if (this.workMemberData.ReturnType == value)
                {
                    return;
                }
                this.workMemberData.ReturnType = value;
                RaisePropertyChanged("ReturnType");
            }
        }

        // IsConstructor. HasFacets: False AttrQName: 
        //                   propertyType: System.Boolean 
        /// <summary>
        /// Gets or sets the IsConstructor. HasFacets: False AttrQName: 
        /// </summary>
        /// <value>The IsConstructor.</value>
        public System.Boolean IsConstructor
        {
            get
            {
                return this.workMemberData.IsConstructor;
            }

            set
            {
                if (this.workMemberData.IsConstructor == value)
                {
                    return;
                }
                this.workMemberData.IsConstructor = value;
                RaisePropertyChanged("IsConstructor");
            }
        }

        // DisplayData. HasFacets: False AttrQName: 
        //                   propertyType: System.String 
        /// <summary>
        /// Gets or sets the DisplayData. HasFacets: False AttrQName: 
        /// </summary>
        /// <value>The DisplayData.</value>
        public System.String DisplayData
        {
            get
            {
                return this.workMemberData.DisplayData;
            }

            set
            {
                if (this.workMemberData.DisplayData == value)
                {
                    return;
                }
                this.workMemberData.DisplayData = value;
                RaisePropertyChanged("DisplayData");
            }
        }


        internal void Accept()
        {
            //origClassData.Name = workClassData.Name;
            //origClassData.Type = workClassData.Type;
            //origClassData.Color = workClassData.Color;
        }
    }*/

}
