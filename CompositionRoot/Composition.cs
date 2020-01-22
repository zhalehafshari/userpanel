using Autofac;
using Infrastructure.AspcoreIdentity;
using Common.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Efcore;

using WebsiteCore.services;


namespace CompositionRoot
{
    public  static class Composition
    {
        public static void RegisterDependencies(ContainerBuilder Builder)
        {
            Builder.RegisterType<ProjectUserStor>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            Builder.RegisterType<ProjectRoleStore>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
           Builder.RegisterType<RenderViewToString>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            Builder.Register(r => ProjectDbContextFactory.CreateDbContext())
                .As<ProjectDbContext>()
                .InstancePerLifetimeScope();  
               
        }
    }
}
