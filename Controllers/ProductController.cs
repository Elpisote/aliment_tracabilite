using aliment_backend.DTOs;
using aliment_backend.Entities;
using aliment_backend.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aliment_backend.Controllers
{
    // Ce contrôleur gère les opérations CRUD pour l'entité de produit.
    // hérite de GenericController 
    [ApiController]
    public class ProductController : GenericController<Product, ProductDTO>
    {
        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }
     
        [HttpGet("api/[controller]s")]
        [Authorize(Roles = "Admin,User")] // Autorisation spécifique pour ProductController
        public override async Task<IActionResult> GetAllAsync()
        {
            return await base.GetAllAsync(); // Appeler la méthode de base du contrôleur générique
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("api/[controller]/{id}")]
        public override async Task<IActionResult> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("api/[controller]")]
        public override async Task<IActionResult> AddOne([FromBody] ProductDTO dto)
        {
            return await base.AddOne(dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("api/[controller]/{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("api/[controller]/{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] ProductDTO dto)
        {
            return await base.Update(id, dto);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("api/[controller]/ProductsByCategoryIds")]
        public IActionResult GetProductsByCategoryIds([FromQuery] List<int> categoryIds)
        {
            if (categoryIds.Count == 0)
            {
                return BadRequest("La liste d'IDs de catégorie ne peut pas être vide.");
            }

            // Vérifier si tous les IDs de catégorie sont valides
            foreach (var categoryId in categoryIds)
            {
                Category? category = _unitOfWork.GetRepository<Category>().GetById(categoryId);
                if (category == null)
                {
                    return BadRequest($"L'ID de catégorie {categoryId} n'existe pas.");
                }
            }

            // Récupérer les produits correspondant aux IDs de catégorie
            List<Product> products = _unitOfWork.Products
                .FindAll(p => categoryIds.Contains(p.CategoryId))
                .ToList();

            foreach (Product product in products)
            {
                product.Category = _unitOfWork.GetRepository<Category>().GetById(product.CategoryId);       
            }
            return Ok(products);
        }       
    }
}
