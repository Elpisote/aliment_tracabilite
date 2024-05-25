using aliment_backend.DTOs;
using aliment_backend.Entities;
using AutoMapper;

namespace aliment_backend.Mappers
{
    /// <summary>
    /// Profile AutoMapper pour la configuration des mappings entre les DTO et les entités.
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="AutoMapperProfile"/>.
        /// </summary>
        public AutoMapperProfile()
        {
            // Mappage de CategoryDTO vers Category et vice versa
            CreateMap<CategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();

            // Mappage de Product vers ProductDTO et vice versa
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();

            // Mappage de StockDTO vers Stock et vice versa
            CreateMap<StockDTO, Stock>();
            CreateMap<Stock, StockDTO>();

            // Mappage de UserDTO vers User et vice versa
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();
        }
    }
}
