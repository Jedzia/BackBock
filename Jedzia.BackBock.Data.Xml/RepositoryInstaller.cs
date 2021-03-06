﻿using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Jedzia.BackBock.DataAccess;
namespace Jedzia.BackBock.Data.Xml
{

    public class RepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //    SimpleIoc.Default.Register<ITaskService>(() => { return TaskRegistry.GetInstance(); });
            container.Register(Component.For<BackupDataRepository>().ImplementedBy<XmlDataRepository>().LifestyleTransient());
        }
    }
}
