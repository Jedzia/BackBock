using Jedzia.BackBock.ViewModel.Converters;
using MbUnit.Framework;
using System;
using System.Globalization;

namespace Jedzia.BackBock.ViewModel.Tests
{
    
    
    /// <summary>
    ///This is a test class for ImageConverterTest and is intended
    ///to contain all ImageConverterTest Unit Tests
    ///</summary>
    [TestFixture]
    public class ImageConverterTest
    {


        /// <summary>
        ///A test for DeleteObject
        ///</summary>
        [Test]
        public void DeleteObjectTest()
        {
            // Creation of the private accessor for 'Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly' failed
            Assert.Inconclusive("Creation of the private accessor for \'Microsoft.VisualStudio.TestTools.TypesAndSy" +
                    "mbols.Assembly\' failed");
        }

        /// <summary>
        ///A test for ConvertBack
        ///</summary>
        [Test]
        public void ConvertBackTest()
        {
            ImageConverter target = new ImageConverter(); // TODO: Initialize to an appropriate value
            object value = null; // TODO: Initialize to an appropriate value
            Type targetType = null; // TODO: Initialize to an appropriate value
            object parameter = null; // TODO: Initialize to an appropriate value
            CultureInfo culture = null; // TODO: Initialize to an appropriate value
            object expected = null; // TODO: Initialize to an appropriate value
            object actual;
            actual = target.ConvertBack(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Convert
        ///</summary>
        [Test]
        public void ConvertTest()
        {
            ImageConverter target = new ImageConverter(); // TODO: Initialize to an appropriate value
            object value = null; // TODO: Initialize to an appropriate value
            Type targetType = null; // TODO: Initialize to an appropriate value
            object parameter = null; // TODO: Initialize to an appropriate value
            CultureInfo culture = null; // TODO: Initialize to an appropriate value
            object expected = null; // TODO: Initialize to an appropriate value
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ImageConverter Constructor
        ///</summary>
        [Test]
        public void ImageConverterConstructorTest()
        {
            ImageConverter target = new ImageConverter();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
