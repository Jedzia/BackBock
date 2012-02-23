using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;

using WPG.Data;

namespace WPG
{
	public class PropertyTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			Property property = item as Property;
			if (property == null)
			{
				throw new ArgumentException("item must be of type Property");
			}
			FrameworkElement element = container as FrameworkElement;
			if (element == null)
			{
				return base.SelectTemplate(property.Value, container);
			}
			DataTemplate template = FindDataTemplate(property, element);
			return template;
		}		

		private DataTemplate FindDataTemplate(Property property, FrameworkElement element)
		{
			Type propertyType = property.PropertyType;

            if (property.PropertyType.IsGenericType)
            {
                //var gtype0 = property.PropertyType.GetGenericTypeDefinition();
                //var gtype1 = property.PropertyType.GetElementType();
                //var gtype2 = property.PropertyType.DeclaringType;
                //Type[] typeArguments = gtype0.GetGenericArguments();
                //Type[] typeArguments1 = property.PropertyType.GetGenericArguments();
                //propertyType = typeof(List<string>);
                //propertyType = typeof(List<object>);
            }
            if (property.Name == "Path")
            {

            }

            if (!(property.PropertyType is String) && property.PropertyType is IEnumerable)
                propertyType = typeof(List<object>);
            
			DataTemplate template = TryFindDataTemplate(element, propertyType);
            var key = property.QualifierName;
            DataTemplate template2 = TryFindDataTemplate(element, key);
            if (template2 != null)
            {
                return template2;
            }

    		while (template == null && propertyType.BaseType != null)
			{
				propertyType = propertyType.BaseType;
				template = TryFindDataTemplate(element, propertyType);
			}
			if (template == null)
			{
				template = TryFindDataTemplate(element, "default");
			}
			return template;
		}

		private static DataTemplate TryFindDataTemplate(FrameworkElement element, object dataTemplateKey)
		{
            /*if (dataTemplateKey.ToString() == "Jedzia.BackBock.ViewModel.Data.MyStructure")
            {
                
            }*/
			object dataTemplate = element.TryFindResource(dataTemplateKey);
			if (dataTemplate == null)
			{
				dataTemplateKey = new ComponentResourceKey(typeof(PropertyGrid), dataTemplateKey);
				dataTemplate = element.TryFindResource(dataTemplateKey);
			}
			return dataTemplate as DataTemplate;
		}
	}
}
