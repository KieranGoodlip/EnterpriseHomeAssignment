using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using ShoppingCart.Data.Repositories;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Services;
using ShoppingCart.Data.Context;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ShoppingCart.Application.AutoMapper;

namespace ShoppingCart.IOC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services, string connectionString)
        {
            //When are the instances created?
            /*     
             *     Singleton: IoC container will create and share a single instance of a service throughout the application's lifetime.
             *                 ( many requests > 1 instance)
                   Transient: The IoC container will create a new instance of the specified service type every time you ask for it.
                               ( 1 request > multiple instances)
                   Scoped: IoC container will create an instance of the specified service type once per request and will be shared in a single request.
                           (1 request > 1 instance)
            */

            //Shortcut key Ctrl+.

            services.AddDbContext<ShoppingCartDbContext>(options =>
                options.UseSqlServer(connectionString )
                );

            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IProductsService, ProductsService>();

            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<ICategoriesService, CategoriesService>();

            services.AddScoped<IMembersRepository, MembersRepository>();
            services.AddScoped<IMembersService, MembersService>();


            //Move Initialization of ShoppingCartDbContext to here and refine the dependencies


            services.AddAutoMapper(typeof(AutoMapperConfiguration));
            AutoMapperConfiguration.RegisterMappings();


        }
    }
}
