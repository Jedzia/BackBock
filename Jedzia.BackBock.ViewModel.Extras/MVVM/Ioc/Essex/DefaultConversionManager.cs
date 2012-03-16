namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex.Conversion;

    /// <summary>
    ///   Implements a conversion logic to a type of a
    ///   set of types.
    /// </summary>
    public interface ITypeConverter
    {
        ITypeConverterContext Context { get; set; }

        /// <summary>
        ///   Returns true if this instance of <c>ITypeConverter</c>
        ///   is able to handle the specified type.
        /// </summary>
        /// <param name = "type"></param>
        /// <returns></returns>
        bool CanHandleType(Type type);

        /// <summary>
        ///   Returns true if this instance of <c>ITypeConverter</c>
        ///   is able to handle the specified type with the specified 
        ///   configuration
        /// </summary>
        /// <param name = "type"></param>
        /// <param name = "configuration"></param>
        /// <returns></returns>
        bool CanHandleType(Type type, IConfiguration configuration);

        /// <summary>
        ///   Should perform the conversion from the
        ///   string representation specified to the type
        ///   specified.
        /// </summary>
        /// <param name = "value"></param>
        /// <param name = "targetType"></param>
        /// <returns></returns>
        object PerformConversion(String value, Type targetType);

        /// <summary>
        ///   Should perform the conversion from the
        ///   configuration node specified to the type
        ///   specified.
        /// </summary>
        /// <param name = "configuration"></param>
        /// <param name = "targetType"></param>
        /// <returns></returns>
        object PerformConversion(IConfiguration configuration, Type targetType);

        TTarget PerformConversion<TTarget>(String value);

        TTarget PerformConversion<TTarget>(IConfiguration configuration);
    }

    public interface ITypeConverterContext
    {
        ITypeConverter Composition { get; }

        CreationContext CurrentCreationContext { get; }
        ComponentModel CurrentModel { get; }
        IKernelInternal Kernel { get; }

        void Pop();

        void Push(ComponentModel model, CreationContext context);
    }

    /// <summary>
    ///   Composition of all available conversion managers
    /// </summary>
    [Serializable]
    public class DefaultConversionManager : AbstractSubSystem, IConversionManager, ITypeConverterContext, ITypeConverter
    {
#if (!SILVERLIGHT)
        private static readonly LocalDataStoreSlot slot = Thread.AllocateDataSlot();
#else
		[ThreadStatic]
		private static Stack<Pair<ComponentModel,CreationContext>> slot;
#endif
        private readonly IList<ITypeConverter> converters = new List<ITypeConverter>();
        private readonly IList<ITypeConverter> standAloneConverters = new List<ITypeConverter>();

        public DefaultConversionManager()
        {
            InitDefaultConverters();
        }

        protected virtual void InitDefaultConverters()
        {
            Add(new PrimitiveConverter());
            /*Add(new TimeSpanConverter());
            Add(new TypeNameConverter(new TypeNameParser()));
            Add(new EnumConverter());
            Add(new ListConverter());
            Add(new DictionaryConverter());
            Add(new GenericDictionaryConverter());
            Add(new GenericListConverter());
            Add(new ArrayConverter());
            Add(new ComponentConverter());
            Add(new AttributeAwareConverter());
#if (SILVERLIGHT)
			Add(new NullableConverter(this));
#endif
            */
            Add(new ComponentModelConverter());
        }

        public void Add(ITypeConverter converter)
        {
            converter.Context = this;

            converters.Add(converter);

            if (!(converter is IKernelDependentConverter))
            {
                standAloneConverters.Add(converter);
            }
        }

        public ITypeConverterContext Context
        {
            get { return this; }
            set { throw new NotImplementedException(); }
        }

        public bool CanHandleType(Type type)
        {
            foreach (var converter in converters)
            {
                if (converter.CanHandleType(type))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanHandleType(Type type, IConfiguration configuration)
        {
            foreach (var converter in converters)
            {
                if (converter.CanHandleType(type, configuration))
                {
                    return true;
                }
            }

            return false;
        }

        public object PerformConversion(String value, Type targetType)
        {
            foreach (var converter in converters)
            {
                if (converter.CanHandleType(targetType))
                {
                    return converter.PerformConversion(value, targetType);
                }
            }

            var message = String.Format("No converter registered to handle the type {0}",
                                        targetType.FullName);

            throw new ConverterException(message);
        }

        public object PerformConversion(IConfiguration configuration, Type targetType)
        {
            foreach (var converter in converters)
            {
                if (converter.CanHandleType(targetType, configuration))
                {
                    return converter.PerformConversion(configuration, targetType);
                }
            }

            var message = String.Format("No converter registered to handle the type {0}",
                                        targetType.FullName);

            throw new ConverterException(message);
        }

        public TTarget PerformConversion<TTarget>(string value)
        {
            return (TTarget)PerformConversion(value, typeof(TTarget));
        }

        public TTarget PerformConversion<TTarget>(IConfiguration configuration)
        {
            return (TTarget)PerformConversion(configuration, typeof(TTarget));
        }

        IKernelInternal ITypeConverterContext.Kernel
        {
            get { return Kernel; }
        }

        public void Push(ComponentModel model, CreationContext context)
        {
            CurrentStack.Push(new Pair<ComponentModel, CreationContext>(model, context));
        }

        public void Pop()
        {
            CurrentStack.Pop();
        }

        public ComponentModel CurrentModel
        {
            get
            {
                if (CurrentStack.Count == 0)
                {
                    return null;
                }

                return CurrentStack.Peek().First;
            }
        }

        public CreationContext CurrentCreationContext
        {
            get
            {
                if (CurrentStack.Count == 0)
                {
                    return null;
                }

                return CurrentStack.Peek().Second;
            }
        }

        public ITypeConverter Composition
        {
            get { return this; }
        }

        private Stack<Pair<ComponentModel, CreationContext>> CurrentStack
        {
            get
            {
#if (SILVERLIGHT)
				if(slot == null)
				{
					slot = new Stack<Pair<ComponentModel,CreationContext>>();
				}

				return slot;
#else
                var stack = (Stack<Pair<ComponentModel, CreationContext>>)Thread.GetData(slot);
                if (stack == null)
                {
                    stack = new Stack<Pair<ComponentModel, CreationContext>>();
                    Thread.SetData(slot, stack);
                }

                return stack;

#endif
            }
        }
    }

}