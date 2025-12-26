using AutoMapper;
using E_commerce_c_charp.ViewModels;
using E_commerce_c_charp.Models;
using E_commerce_c_charp.ViewModels.Admin;

namespace E_commerce_c_charp.Mapping
{
    public class ProfileOrderAdmin : Profile
    {
        public ProfileOrderAdmin()
        {
            // CheckoutViewModel -> Order
            //AutoMapper saura copier les propriétés qui ont le même nom (FullName, Email, Address, City, Phone, PrixHT, PrixTVA, PrixTTC).
            /* CreateMap<OrderRowViewModel, Order>()
                // TotalAmount doit prendre la valeur du PrixTTC du ViewModel
                .ForMember(
                    o => o.TotalAmount, 
                    //opt => opt.MapFrom(src => src.PrixTTC)
                ); */

            // CartItemViewModel -> OrderItem
            /* CreateMap<CartItemViewModel, OrderItem>()
                .ForMember(
                    oi => oi.ProductId, 
                    opt => opt.MapFrom(src => src.Product!.Id)
                )
                .ForMember(
                    oi => oi.UnitPrice, 
                    opt => opt.MapFrom(src => src.Product!.Price)
                ); */
        }
    }
}
