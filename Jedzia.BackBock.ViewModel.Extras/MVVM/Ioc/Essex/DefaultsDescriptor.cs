// Copyright 2004-2011 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;
    using System.Linq;

    public class DefaultsDescriptor : IComponentModelDescriptor
    {
        private readonly Type implementation;
        private readonly ComponentName name;

        public DefaultsDescriptor(ComponentName name, Type implementation)
        {
            this.name = name;
            this.implementation = implementation;
        }

        public void BuildComponentModel(IKernel kernel, ComponentModel model)
        {
            if (model.Implementation == null)
            {
                model.Implementation = implementation ?? FirstService(model);
            }

            EnsureComponentName(model);
            EnsureComponentConfiguration(kernel, model);
        }

        public void ConfigureComponentModel(IKernel kernel, ComponentModel model)
        {
        }

        private void EnsureComponentConfiguration(IKernel kernel, ComponentModel model)
        {
            var configuration = kernel.ConfigurationStore.GetComponentConfiguration(model.Name);
            if (configuration == null)
            {
                configuration = new MutableConfiguration("component");
                kernel.ConfigurationStore.AddComponentConfiguration(model.Name, configuration);
            }
            if (model.Configuration == null)
            {
                model.Configuration = configuration;
            }
            return;
        }

        private void EnsureComponentName(ComponentModel model)
        {
            if (model.ComponentName != null)
            {
                return;
            }
            if (name != null)
            {
                model.ComponentName = name;
                return;
            }
            /*if (model.Implementation == typeof(LateBoundComponent))
            {
                model.ComponentName = new ComponentName("Late bound " + FirstService(model).FullName, false);
                return;
            }*/
            model.ComponentName = ComponentName.DefaultFor(model.Implementation);
        }

        private Type FirstService(ComponentModel model)
        {
            return model.Services.First();
        }
    }


    //using Castle.DynamicProxy.Generators.Emitters;

    /*/// <summary>
    ///   Describes how to select a types service.
    /// </summary>
    public class ServiceDescriptor
    {
        // Fields
        private readonly BasedOnDescriptor basedOnDescriptor;
        private ServiceSelector serviceSelector;

        // Methods
        internal ServiceDescriptor(BasedOnDescriptor basedOnDescriptor)
        {
            this.basedOnDescriptor = basedOnDescriptor;
        }

        private void AddFromInterface(Type type, Type implements, ICollection<Type> matches)
        {
            foreach (Type type2 in this.GetTopLevelInterfaces(type))
            {
                if (type2.GetInterface(implements.FullName, false) != null)
                {
                    matches.Add(type2);
                }
            }
        }

        public BasedOnDescriptor AllInterfaces()
        {
            return this.Select((ServiceSelector)((t, b) => t.GetAllInterfaces()));
        }

        public BasedOnDescriptor Base()
        {
            return this.Select((ServiceSelector)((t, b) => b));
        }

        public BasedOnDescriptor DefaultInterfaces()
        {
            return this.Select((ServiceSelector)((type, @base) => (from i in type.GetAllInterfaces()
                                                                   where type.Name.Contains(this.GetInterfaceName(i))
                                                                   select i)));
        }

        public BasedOnDescriptor FirstInterface()
        {
            return this.Select(delegate(Type type, Type[] @base)
            {
                Type type2 = type.GetInterfaces().FirstOrDefault<Type>();
                if (type2 == null)
                {
                    return null;
                }
                return new Type[] { type2 };
            });
        }

        public BasedOnDescriptor FromInterface()
        {
            return this.FromInterface(null);
        }

        public BasedOnDescriptor FromInterface(Type implements)
        {
            return this.Select(delegate(Type type, Type[] baseTypes)
            {
                HashSet<Type> matches = new HashSet<Type>();
                if (implements != null)
                {
                    this.AddFromInterface(type, implements, matches);
                }
                else
                {
                    foreach (Type type2 in baseTypes)
                    {
                        this.AddFromInterface(type, type2, matches);
                    }
                }
                if (matches.Count == 0)
                {
                    foreach (Type type3 in from t in baseTypes
                                           where t != typeof(object)
                                           select t)
                    {
                        if (type3.IsAssignableFrom(type))
                        {
                            matches.Add(type3);
                            return matches;
                        }
                    }
                }
                return matches;
            });
        }

        private string GetInterfaceName(Type @interface)
        {
            string name = @interface.Name;
            if (((name.Length > 1) && (name[0] == 'I')) && char.IsUpper(name[1]))
            {
                return name.Substring(1);
            }
            return name;
        }

        internal ICollection<Type> GetServices(Type type, Type[] baseType)
        {
            HashSet<Type> set = new HashSet<Type>();
            if (this.serviceSelector != null)
            {
                foreach (ServiceSelector selector in this.serviceSelector.GetInvocationList())
                {
                    IEnumerable<Type> source = selector(type, baseType);
                    if (source != null)
                    {
                        foreach (Type type2 in source.Select<Type, Type>(new Func<Type, Type>(ServiceDescriptor.WorkaroundCLRBug)))
                        {
                            set.Add(type2);
                        }
                    }
                }
            }
            return set;
        }

        private IEnumerable<Type> GetTopLevelInterfaces(Type type)
        {
            Type[] interfaces = type.GetInterfaces();
            List<Type> list = new List<Type>(interfaces);
            foreach (Type type2 in interfaces)
            {
                foreach (Type type3 in type2.GetInterfaces())
                {
                    list.Remove(type3);
                }
            }
            return list;
        }

        public BasedOnDescriptor Select(IEnumerable<Type> types)
        {
            return this.Select(delegate
            {
                return types;
            });
        }

 

 

        public BasedOnDescriptor Select(ServiceSelector selector)
        {
            this.serviceSelector = (ServiceSelector)Delegate.Combine(this.serviceSelector, selector);
            return this.basedOnDescriptor;
        }

        public BasedOnDescriptor Self()
        {
            return this.Select((ServiceSelector)((t, b) => new Type[] { t }));
        }

        private static Type WorkaroundCLRBug(Type serviceType)
        {
            if (serviceType.IsInterface)
            {
                if (!serviceType.IsGenericType || (serviceType.ReflectedType != null))
                {
                    return serviceType;
                }
                bool flag = false;
                foreach (Type type in serviceType.GetGenericArguments())
                {
                    flag |= type.IsGenericParameter;
                }
                if (flag)
                {
                    return serviceType.GetGenericTypeDefinition();
                }
            }
            return serviceType;
        }

        // Nested Types
        public delegate IEnumerable<Type> ServiceSelector(Type type, Type[] baseTypes);
    }*/
}