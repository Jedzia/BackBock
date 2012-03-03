using Jedzia.BackBock.ViewModel.Converters;
using MbUnit.Framework;
using System;
using System.Globalization;

namespace Jedzia.BackBock.ViewModel.Tests
{
    
    
    /// <summary>
    ///This is a test class for DoubleTypeConverterTest and is intended
    ///to contain all DoubleTypeConverterTest Unit Tests
    ///</summary>
    [TestFixture]
    public class DoubleTypeConverterTest
    {

        /// <summary>
        ///A test for ConvertBack
        ///</summary>
        [Test]
        public void ConvertBackTest()
        {
            DoubleTypeConverter target = new DoubleTypeConverter();
            object value = "-23,351";
            Type targetType = null;
            object parameter = null;
            CultureInfo culture = null;
            object expected = -23.351d;
            object actual;
            actual = target.ConvertBack(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            value = null;
            expected = 0.0d;
            actual = target.ConvertBack(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Convert
        ///</summary>
        [Test]
        public void ConvertTest()
        {
            DoubleTypeConverter target = new DoubleTypeConverter(); 
            object value = -23.351d; 
            Type targetType = null; 
            object parameter = null;
            CultureInfo culture = null;
            object expected = "-23,351";
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            value = null;
            expected = null;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DoubleTypeConverter Constructor
        ///</summary>
        [Test]
        public void DoubleTypeConverterConstructorTest()
        {
            DoubleTypeConverter target = new DoubleTypeConverter();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
