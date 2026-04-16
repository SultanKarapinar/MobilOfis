using AutoMapper;
using DTO.CategoryDTOs;
using DTO.EmailNotificationDTOs;
using DTO.ProductDTOs;
using DTO.StockTransactionDTOs;
using DTO.SupplierDTOs;
using DTO.UserDTOs;
using OfisUrunTakip.WebApi.Entity;

namespace Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Category, CategoryAddDto>().ReverseMap();
            CreateMap<Category, CategoryDeleteDto>().ReverseMap();
            CreateMap<Category, CategoryListDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();

            CreateMap<EmailNotification, EmailNotificationAddDto>().ReverseMap();
            CreateMap<EmailNotification, EmailNotificationDeleteDto>().ReverseMap();
            CreateMap<EmailNotification, EmailNotificationListDto>()
     .ForMember(d => d.UserName, o => o.MapFrom(s => s.User != null ? s.User.Name : null))
     .ForMember(d => d.UserEmail, o => o.MapFrom(s => s.User != null ? s.User.Email : null))
     .ForMember(d => d.UserRole, o => o.MapFrom(s => s.User != null ? s.User.Role : null));

            CreateMap<EmailNotification, EmailNotificationUpdateDto>().ReverseMap();

            CreateMap<Product, ProductAddDto>().ReverseMap();
            CreateMap<Product, ProductDeleteDto>().ReverseMap();
            CreateMap<Product, ProductListDto>()
                  .ForMember(dest => dest.CategoryName,
               opt => opt.MapFrom(src => src.Category.Name)).ReverseMap();
            CreateMap<Product, ProductUpdateDto>().ReverseMap();

            CreateMap<StockTransaction, StockTransactionAddDto>().ReverseMap();
            CreateMap<StockTransaction, StockTransactionDeleteDto>().ReverseMap();
            CreateMap<StockTransaction, StockTransactionListDto>()
                     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name)).ReverseMap()
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

          
            CreateMap<StockTransaction, StockTransactionUpdateDto>().ReverseMap();

            CreateMap<Supplier, SupplierAddDto>().ReverseMap();
            CreateMap<Supplier, SupplierDeleteDto>().ReverseMap();
            CreateMap<Supplier, SupplierListDto>().ReverseMap();
            CreateMap<Supplier, SupplierUpdateDto>().ReverseMap();

            CreateMap<User, UserAddDto>().ReverseMap();
            CreateMap<User, UserDeleteDto>().ReverseMap();
            CreateMap<User, UserListDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();


            //CreateMap< entity ismi , kullandıgın dtoların ismi >().ReverseMap 
            //ReverseMap() ile metodu ıkı yonlu yaptım CreateMap<UserAdd,user> yazmama gerek kalmadı


        }
    }
}
