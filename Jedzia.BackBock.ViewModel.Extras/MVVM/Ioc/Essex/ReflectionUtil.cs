namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.IO;
    using System.Diagnostics;

    public static class ReflectionUtil
    {

        public static Assembly GetAssemblyNamed(string assemblyName)
        {
            Debug.Assert(string.IsNullOrEmpty(assemblyName) == false);

            try
            {
                Assembly assembly;
                if (IsAssemblyFile(assemblyName))
                {
#if (SILVERLIGHT)
					assembly = Assembly.Load(Path.GetFileNameWithoutExtension(assemblyName));
#else
                    if (Path.GetDirectoryName(assemblyName) == AppDomain.CurrentDomain.BaseDirectory)
                    {
                        assembly = Assembly.Load(Path.GetFileNameWithoutExtension(assemblyName));
                    }
                    else
                    {
                        assembly = Assembly.LoadFile(assemblyName);
                    }
#endif
                }
                else
                {
                    assembly = Assembly.Load(assemblyName);
                }
                return assembly;
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (FileLoadException)
            {
                throw;
            }
            catch (BadImageFormatException)
            {
                throw;
            }
            catch (Exception e)
            {
                // in theory there should be no other exception kind
                throw new Exception(string.Format("Could not load assembly {0}", assemblyName), e);
            }
        }

        public static bool IsAssemblyFile(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

            string extension;
            try
            {
                extension = Path.GetExtension(filePath);
            }
            catch (ArgumentException)
            {
                // path contains invalid characters...
                return false;
            }
            return IsDll(extension) || IsExe(extension);
        }

        private static bool IsDll(string extension)
        {
            return ".dll".Equals(extension, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsExe(string extension)
        {
            return ".exe".Equals(extension, StringComparison.OrdinalIgnoreCase);
        }




        public static TAttribute[] GetAttributes<TAttribute>(this MemberInfo item) where TAttribute : Attribute
        {
            return (TAttribute[])Attribute.GetCustomAttributes(item, typeof(TAttribute), true);
        }

        public static IEnumerable<Assembly> GetApplicationAssemblies(Assembly rootAssembly)
        {
            int length = rootAssembly.FullName.IndexOfAny(new char[] { '.', ',' });
            if (length < 0)
            {
                throw new ArgumentException(string.Format("Could not determine application name for assembly \"{0}\". Please use a different method for obtaining assemblies.", rootAssembly.FullName));
            }
            string applicationName = rootAssembly.FullName.Substring(0, length);
            HashSet<Assembly> assemblies = new HashSet<Assembly>();
            AddApplicationAssemblies(rootAssembly, assemblies, applicationName);
            return assemblies;
        }

        /*public static Type[] GetAvailableTypes(this Assembly assembly)
        {
            bool includeNonExported = false;
            return GetAvailableTypes(assembly, includeNonExported);
        }*/

        /*public static Type[] GetAvailableTypes(this Assembly assembly, bool includeNonExported)
        {
            try
            {
                if (includeNonExported)
                {
                    return assembly.GetTypes();
                }
                return assembly.GetExportedTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.FindAll(t => t != null);
                throw;
                // NOTE: perhaps we should not ignore the exceptions here, and log them?
            }
        }*/

        private static void AddApplicationAssemblies(Assembly assembly, HashSet<Assembly> assemblies, string applicationName)
        {
            if (assemblies.Add(assembly))
            {
                foreach (AssemblyName name in assembly.GetReferencedAssemblies())
                {
                    if (IsApplicationAssembly(applicationName, name.FullName))
                    {
                        try
                        { 
                            AddApplicationAssemblies(LoadAssembly(name), assemblies, applicationName);
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show("Can't load Assembly: " + name.FullName);
                        }
                    }
                }
            }
        }

        private static bool IsApplicationAssembly(string applicationName, string assemblyName)
        {
            return assemblyName.StartsWith(applicationName);
        }

        private static Assembly LoadAssembly(AssemblyName assemblyName)
        {
            // Todo: VS2008 loading problem
            return Assembly.Load(assemblyName);
        }

        public static TBase CreateInstance<TBase>(this Type subtypeofTBase, params object[] ctorArgs)
        {
            EnsureIsAssignable<TBase>(subtypeofTBase);
            return Instantiate<TBase>(subtypeofTBase, ctorArgs ?? new object[0]);
        }

        private static void EnsureIsAssignable<TBase>(Type subtypeofTBase)
        {
            if (!subtypeofTBase.Is<TBase>())
            {
                string str;
                if (typeof(TBase).IsInterface)
                {
                    str = string.Format("Type {0} does not implement the interface {1}.", subtypeofTBase.FullName, typeof(TBase).FullName);
                }
                else
                {
                    str = string.Format("Type {0} does not inherit from {1}.", subtypeofTBase.FullName, typeof(TBase).FullName);
                }
                throw new InvalidCastException(str);
            }
        }

        public static bool Is<TType>(this Type type)
        {
            return typeof(TType).IsAssignableFrom(type);
        }




        private static TBase Instantiate<TBase>(Type subtypeofTBase, object[] ctorArgs)
        {
            Func<object, Type> selector = null;
            TBase local;
            ctorArgs = ctorArgs ?? new object[0];
            Type[] types = ctorArgs.ConvertAll<object, Type>(delegate(object a)
                                                                 {
                                                                     if (a != null)
                                                                     {
                                                                         return a.GetType();
                                                                     }
                                                                     return typeof(object);
                                                                 });
            ConstructorInfo ctor = subtypeofTBase.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, types, null);
            if (ctor != null)
            {
                return (TBase)ctor.Instantiate(ctorArgs);
            }
            try
            {
                local = (TBase)Activator.CreateInstance(subtypeofTBase, ctorArgs);
            }
            catch (MissingMethodException exception)
            {
                string str;
                if (ctorArgs.Length == 0)
                {
                    str = string.Format("Type {0} does not have a public default constructor and could not be instantiated.", subtypeofTBase.FullName);
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine(string.Format("Type {0} does not have a public constructor matching arguments of the following types:", subtypeofTBase.FullName));
                    if (selector == null)
                    {
                        selector = o => o.GetType();
                    }
                    foreach (Type type in ctorArgs.Select<object, Type>(selector))
                    {
                        builder.AppendLine(type.FullName);
                    }
                    str = builder.ToString();
                }
                throw new ArgumentException(str, exception);
            }
            catch (Exception exception2)
            {
                throw new Exception(string.Format("Could not instantiate {0}.", subtypeofTBase.FullName), exception2);
            }
            return local;
        }

        public static TResult[] ConvertAll<T, TResult>(this T[] items, Converter<T, TResult> transformation)
        {
            return Array.ConvertAll<T, TResult>(items, transformation);
        }

        public static object Instantiate(this ConstructorInfo ctor, object[] ctorArgs)
        {
            Func<object[], object> func;
            if (!factories.TryGetValue(ctor, out func))
            {
                //using (@lock.ForWriting())
                {
                    if (!factories.TryGetValue(ctor, out func))
                    {
                        func = BuildFactory(ctor);
                        factories[ctor] = func;
                    }
                }
            }
            return func(ctorArgs);
        }

        private static Func<object[], object> BuildFactory(ConstructorInfo ctor)
        {
            var parameterInfos = ctor.GetParameters();
            var parameterExpressions = new Expression[parameterInfos.Length];
            var argument = Expression.Parameter(typeof(object[]), "parameters");
            for (var i = 0; i < parameterExpressions.Length; i++)
            {
                parameterExpressions[i] = Expression.Convert(
                    Expression.ArrayIndex(argument, Expression.Constant(i, typeof(int))),
                    parameterInfos[i].ParameterType.IsByRef ? parameterInfos[i].ParameterType.GetElementType() : parameterInfos[i].ParameterType);
            }
            return Expression.Lambda<Func<object[], object>>(
                Expression.New(ctor, parameterExpressions),
                new[] { argument }).Compile();
        }

        private static readonly Lock @lock;
        private static readonly IDictionary<ConstructorInfo, Func<object[], object>> factories;
        static ReflectionUtil()
        {
            factories = new Dictionary<ConstructorInfo, Func<object[], object>>();
            @lock = Lock.Create();
        }
    }
}