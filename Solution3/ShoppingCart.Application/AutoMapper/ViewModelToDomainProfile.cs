using AutoMapper;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.AutoMapper
{

    //convert
    //Domain (classes) >>>>>> Application (view models)

    public class ViewModelToDomainProfile: Profile
    {
        public ViewModelToDomainProfile()
        {
            CreateMap<ProductViewModel, Product>();//.ForMember(x=>x.Category, opt=> opt.Ignore());
            CreateMap<CategoryViewModel, Category>();
            CreateMap<MemberViewModel, Member>();
        }
    }
}
