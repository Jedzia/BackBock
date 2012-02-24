namespace Jedzia.BackBock.ViewModel.Tests.Commands
{
    using System.Windows.Controls;
    using System.Windows.Input;
    using System;
    using System.Windows;
    using Gallio.Framework;
    using Jedzia.BackBock.ViewModel.Commands;
    using MbUnit.Framework;

    /// <summary>
    ///This is a test class for CommandKeyGestureAttributeTest and is intended
    ///to contain all CommandKeyGestureAttributeTest Unit Tests
    ///</summary>
    [TestFixture]
    public class CommandKeyGestureAttributeTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


       /// <summary>
        ///A test for ApplyKeyGestures
        ///</summary>
        [Test, Ignore]
        public void ApplyKeyGesturesTest1()
        {
            // Todo: MockMe
            Type type = null; // TODO: Initialize to an appropriate value
            ICanInputBind inputBinder = null; // TODO: Initialize to an appropriate value
            object instance = null; // TODO: Initialize to an appropriate value
            CommandKeyGestureAttribute.ApplyKeyGestures(type, inputBinder, instance);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }


        private class MyClass
        {
            public readonly Window mainWindow = new Window();
            public readonly Window subWindow = new Window();

            private RelayCommand firstCommand;

            [CommandKeyGesture(Key.A, ModifierKeys.Control, "mainWindow")]
            public ICommand FirstCommand
            {
                get
                {
                    if (this.firstCommand == null)
                    {
                        this.firstCommand = new RelayCommand(this.FirstExecuted);
                    }

                    return this.firstCommand;
                }
            }

            private void FirstExecuted(object e)
            {
            }

            private RelayCommand secondCommand;

            [CommandKeyGesture(Key.B, ModifierKeys.Alt, "subWindow")]
            public ICommand SecondCommand
            {
                get
                {
                    if (this.secondCommand == null)
                    {
                        this.secondCommand = new RelayCommand(this.SecondExecuted);
                    }

                    return this.secondCommand;
                }
            }

            private void SecondExecuted(object e)
            {
            }

            [CommandKeyGesture(Key.C, ModifierKeys.Alt, "mainWindow")]
            public ICommand NullCommand
            {
                get
                {
                    return null;
                }
            }

            [CommandKeyGesture(Key.D, ModifierKeys.Alt, "mainWindow")]
            public string NoCommand
            {
                get
                {
                    return "Fail!";
                }
            }

        }
        /// <summary>
        ///A test for ApplyKeyGestures
        ///</summary>
        [Test()]
        public void ApplyKeyGesturesTest()
        {
            var instance = new MyClass();
            Type type = instance.GetType();
            UIElement inputBinder = instance.mainWindow;
            UIElement inputBinder2 = instance.subWindow;

            Assert.IsEmpty(inputBinder.InputBindings);
            Assert.IsEmpty(((RelayCommand)instance.FirstCommand).InputGestures);
            Assert.IsEmpty(inputBinder2.InputBindings);
            Assert.IsEmpty(((RelayCommand)instance.SecondCommand).InputGestures);

            // Apply gestures from "mainWindow"-targets to the mainWindow field.
            CommandKeyGestureAttribute.ApplyKeyGestures(type, inputBinder, instance);

            Assert.IsNotEmpty(inputBinder.InputBindings);
            Assert.AreEqual(1, inputBinder.InputBindings.Count);
            var inpBinding = inputBinder.InputBindings[0];
            Assert.AreSame(instance.FirstCommand, inpBinding.Command);
            var gesture = (KeyGesture)inpBinding.Gesture;
            Assert.AreEqual(Key.A, gesture.Key);
            Assert.AreEqual(ModifierKeys.Control, gesture.Modifiers);

            var rlay = (RelayCommand)instance.FirstCommand;
            Assert.IsNotEmpty(rlay.InputGestures);
            Assert.AreEqual(1, rlay.InputGestures.Count);
            var inpGesture = rlay.InputGestures[0];
            gesture = (KeyGesture)inpGesture;
            Assert.AreEqual(Key.A, gesture.Key);
            Assert.AreEqual(ModifierKeys.Control, gesture.Modifiers);

            Assert.IsEmpty(inputBinder2.InputBindings);
            Assert.IsEmpty(((RelayCommand)instance.SecondCommand).InputGestures);

            // Apply gestures from "subWindow"-targets to the subWindow field.
            CommandKeyGestureAttribute.ApplyKeyGestures(type, inputBinder2, instance);

            Assert.IsNotEmpty(inputBinder2.InputBindings);
            Assert.AreEqual(1, inputBinder2.InputBindings.Count);
            inpBinding = inputBinder2.InputBindings[0];
            Assert.AreSame(instance.SecondCommand, inpBinding.Command);
            gesture = (KeyGesture)inpBinding.Gesture;
            Assert.AreEqual(Key.B, gesture.Key);
            Assert.AreEqual(ModifierKeys.Alt, gesture.Modifiers);

            rlay = (RelayCommand)instance.SecondCommand;
            Assert.IsNotEmpty(rlay.InputGestures);
            Assert.AreEqual(1, rlay.InputGestures.Count);
            inpGesture = rlay.InputGestures[0];
            gesture = (KeyGesture)inpGesture;
            Assert.AreEqual(Key.B, gesture.Key);
            Assert.AreEqual(ModifierKeys.Alt, gesture.Modifiers);
        }


        /// <summary>
        ///A test for CommandKeyGestureAttribute Constructor
        ///</summary>
        [Test()]
        public void CommandKeyGestureAttributeConstructorTest6()
        {
            const Key key = Key.D;
            const ModifierKeys modifiers = ModifierKeys.Alt | ModifierKeys.Shift;
            const string target1 = "MyTarget";
            var target = new CommandKeyGestureAttribute(key, modifiers, target1);
            Assert.AreEqual(key, target.Key.Key);
            Assert.AreEqual(modifiers, target.Key.Modifiers);
            Assert.AreEqual(string.Empty, target.Key.DisplayString);
            Assert.AreEqual(target1, target.Target);
        }

        /// <summary>
        ///A test for CommandKeyGestureAttribute Constructor
        ///</summary>
        [Test]
        public void CommandKeyGestureAttributeConstructorTest5()
        {
            var key = new KeyGesture(Key.F1); 
            var target = new CommandKeyGestureAttribute(key);
            Assert.AreEqual(key, target.Key);
            Assert.AreEqual(string.Empty, target.Key.DisplayString);
            Assert.IsNull(target.Target);
            
            Assert.Throws<ArgumentNullException>(() => target = new CommandKeyGestureAttribute(null));
        }

        /// <summary>
        ///A test for CommandKeyGestureAttribute Constructor
        ///</summary>
        [Test]
        public void CommandKeyGestureAttributeConstructorTest1()
        {
            var key = new KeyGesture(Key.F1);
            const string target1 = "TestTarget";
            var target = new CommandKeyGestureAttribute(key, target1);
            Assert.AreEqual(key, target.Key);
            Assert.AreEqual(string.Empty, target.Key.DisplayString);
            Assert.AreEqual(target1, target.Target);

            Assert.Throws<ArgumentNullException>(() => target = new CommandKeyGestureAttribute(null, target1));
        }

        /// <summary>
        ///A test for CommandKeyGestureAttribute Constructor
        ///</summary>
        [Test]
        public void CommandKeyGestureAttributeConstructorTest4()
        {
            const Key key = Key.F1;
            const string target1 = "TestTarget"; 
            var target = new CommandKeyGestureAttribute(key, target1);
            Assert.AreEqual(key, target.Key.Key);
            Assert.AreEqual(ModifierKeys.None, target.Key.Modifiers);
            Assert.AreEqual(string.Empty, target.Key.DisplayString);
            Assert.AreEqual(target1, target.Target);
            
            Assert.Throws<ArgumentNullException>(() => target = new CommandKeyGestureAttribute(null, target1));
        }

        /// <summary>
        ///A test for CommandKeyGestureAttribute Constructor
        ///</summary>
        [Test]
        public void CommandKeyGestureAttributeConstructorTest3()
        {
            const Key key = Key.A;
            const ModifierKeys modifiers = ModifierKeys.Alt; 
            var target = new CommandKeyGestureAttribute(key, modifiers);
            Assert.AreEqual(key, target.Key.Key);
            Assert.AreEqual(modifiers, target.Key.Modifiers);
            Assert.AreEqual(string.Empty, target.Key.DisplayString);
            Assert.IsNull(target.Target);
        }

        /// <summary>
        ///A test for CommandKeyGestureAttribute Constructor
        ///</summary>
        [Test]
        public void CommandKeyGestureAttributeConstructorTest2()
        {
            const Key key = Key.F1;
            var target = new CommandKeyGestureAttribute(key);
            Assert.AreEqual(key, target.Key.Key);
            Assert.AreEqual(ModifierKeys.None, target.Key.Modifiers);
            Assert.AreEqual(string.Empty, target.Key.DisplayString);
            Assert.IsNull(target.Target);
        }

        /// <summary>
        ///A test for CommandKeyGestureAttribute Constructor
        ///</summary>
        [Test]
        public void CommandKeyGestureAttributeConstructorTest()
        {
            const Key key = Key.D;
            const ModifierKeys modifiers = ModifierKeys.Alt | ModifierKeys.Shift;
            const string displayString = "Alt+Shift+O";
            const string target1 = "MyTarget";
            var target = new CommandKeyGestureAttribute(key, modifiers, displayString, target1);
            Assert.AreEqual(key, target.Key.Key);
            Assert.AreEqual(modifiers, target.Key.Modifiers);
            Assert.AreEqual(displayString, target.Key.DisplayString);
            Assert.AreEqual(target1, target.Target);
        }
    }
}
