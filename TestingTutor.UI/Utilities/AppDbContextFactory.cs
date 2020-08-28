using System;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.UI.Data;

namespace TestingTutor.UI.Utilities
{
    public class AppDbContextFactory : IAppDbContextFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AppDbContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Dev.Data.DataAccess.ApplicationDbContext Create()
        {
            _serviceProvider.CreateScope();
            return _serviceProvider.GetService<Dev.Data.DataAccess.ApplicationDbContext>();
        }
    }
}