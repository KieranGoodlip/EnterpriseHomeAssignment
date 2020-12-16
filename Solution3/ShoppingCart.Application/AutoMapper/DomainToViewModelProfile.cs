using AutoMapper;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.AutoMapper
{
    public class DomainToViewModelProfile: Profile
    {

        //convert
        //Application (view models) >> Domain (classes)
        public DomainToViewModelProfile()
        {
            CreateMap<Product, ProductViewModel>(); //.ForMember(x=>x.DestColumn, opt=> opt.MapFrom(src=>src.SrcColumn)) ; 
            //informing the automapper library that we are mapping /linking Product onto ProductViewModel

            CreateMap<Category, CategoryViewModel>();

            CreateMap<Member, MemberViewModel>();

        }

    }
}
