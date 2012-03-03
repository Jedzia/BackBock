using Jedzia.BackBock.ViewModel.Converters;
using MbUnit.Framework;
using System;
using System.Globalization;

namespace Jedzia.BackBock.ViewModel.Tests
{
    
    
    /// <summary>
    ///This is a test class for BoolToOppositeConverterTest and is intended
    ///to contain all BoolToOppositeConverterTest Unit Tests
    ///</summary>
    [TestFixture]
    public class BoolToOppositeConverterTest
    {

        /// <summary>
        ///A test for ConvertBack
        ///</summary>
        [Test]
        public void ConvertBackTest()
        {
            BoolToOppositeConverter target = new BoolToOppositeConverter();
            object value = true;
            Type targetType = typeof(bool);
            object parameter = null;
            CultureInfo culture = null;
            object expected = false;
            object actual;
            actual = target.ConvertBack(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            value = false;
            expected = true;
            actual = target.ConvertBack(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Convert
        ///</summary>
        [Test]
        public void ConvertTest()
        {
            BoolToOppositeConverter target = new BoolToOppositeConverter();
            object value = true; 
            Type targetType = typeof(bool); 
            object parameter = null; 
            CultureInfo culture = null;
            object expected = false; 
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            value = false;
            expected = true;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BoolToOppositeConverter Constructor
        ///</summary>
        [Test]
        public void BoolToOppositeConverterConstructorTest()
        {
            BoolToOppositeConverter target = new BoolToOppositeConverter();
        }
    }
}
