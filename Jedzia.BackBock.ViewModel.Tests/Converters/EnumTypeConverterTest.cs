using Jedzia.BackBock.ViewModel.Converters;
using MbUnit.Framework;
using System;
using System.Globalization;

namespace Jedzia.BackBock.ViewModel.Tests
{
    
    
    /// <summary>
    ///This is a test class for EnumTypeConverterTest and is intended
    ///to contain all EnumTypeConverterTest Unit Tests
    ///</summary>
    [TestFixture]
    public class EnumTypeConverterTest
    {
        enum MyEnum { ValueOne, ValueTwo }

        /// <summary>
        ///A test for ConvertBack
        ///</summary>
        [Test]
        public void ConvertBackTest()
        {
            EnumTypeConverter target = new EnumTypeConverter(); 
            object value = null; 
            Type targetType = null; 
            object parameter = null;
            CultureInfo culture = null;
            object expected = null;
            object actual = null;
            Assert.Throws<NotSupportedException>(
                ()=> actual = target.ConvertBack(value, targetType, parameter, culture));
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Convert
        ///</summary>
        [Test]
        public void ConvertTest()
        {
            EnumTypeConverter target = new EnumTypeConverter(); 
            object value = MyEnum.ValueOne; 
            Type targetType = null;
            object parameter = null;
            CultureInfo culture = null;
            object expected = new[] { MyEnum.ValueOne, MyEnum.ValueTwo };
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for EnumTypeConverter Constructor
        ///</summary>
        [Test]
        public void EnumTypeConverterConstructorTest()
        {
            EnumTypeConverter target = new EnumTypeConverter();
        }
    }
}
