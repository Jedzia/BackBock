using Jedzia.BackBock.ViewModel.Converters;
using MbUnit.Framework;
using System;
using System.Globalization;

// Todo Fix namespaces of the complete group.
namespace Jedzia.BackBock.ViewModel.Tests
{
    
    /// <summary>
    ///This is a test class for DisplayModeToVisibilityTest and is intended
    ///to contain all DisplayModeToVisibilityTest Unit Tests
    ///</summary>
    [TestFixture]
    public class DisplayModeToVisibilityTest
    {

        /// <summary>
        ///A test for Invert
        ///</summary>
        [Test]
        public void InvertTest()
        {
            DisplayModeToVisibility target = new DisplayModeToVisibility(); 
            bool expected = false; 
            bool actual;
            target.Invert = expected;
            actual = target.Invert;
            Assert.AreEqual(expected, actual);

            expected = true;
            target.Invert = expected;
            actual = target.Invert;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ConvertBack
        ///</summary>
        [Test]
        public void ConvertBackTest()
        {
            DisplayModeToVisibility target = new DisplayModeToVisibility();
            object value = null;
            Type targetType = null; 
            object parameter = null; 
            CultureInfo culture = null; 
            //object expected = null;
            object actual;
            Assert.Throws<NotImplementedException>(
                () => actual = target.ConvertBack(value, targetType, parameter, culture));
        }

        /// <summary>
        ///A test for Convert
        ///</summary>
        [Test]
        public void ConvertTest()
        {
            DisplayModeToVisibility target = new DisplayModeToVisibility();
            object value = true; 
            Type targetType = null;
            object parameter = null; 
            CultureInfo culture = null;
            object expected = "Visible"; 
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            value = false;
            expected = "Visible";
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            value = null;
            expected = "Collapsed";
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            target.Invert = true;

            expected = "Visible";
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            value = false;
            expected = "Collapsed";
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DisplayModeToVisibility Constructor
        ///</summary>
        [Test]
        public void DisplayModeToVisibilityConstructorTest()
        {
            DisplayModeToVisibility target = new DisplayModeToVisibility();
            Assert.IsFalse(target.Invert);
        }
    }
}
