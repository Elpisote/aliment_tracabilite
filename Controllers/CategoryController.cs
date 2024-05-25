using aliment_backend.DTOs;
using aliment_backend.Entities;
using aliment_backend.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aliment_backend.Controllers
{
    // Ce contrôleur gère les opérations CRUD pour l'entité de catégorie.
    // hérite de GenericController
    [ApiController]
    public class CategoryController : GenericController<Category, CategoryDTO>
    {
        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("api/Categories")]       
        public override async Task<IActionResult> GetAllAsync()
        {
            return await base.GetAllAsync(); // Appel la méthode de base du contrôleur générique
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("api/[controller]/{id}")]
        public override async Task<IActionResult> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("api/[controller]")]
        public override async Task<IActionResult> AddOne([FromBody] CategoryDTO dto)
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
        public override async Task<IActionResult> Update(int id, [FromBody] CategoryDTO dto)
        {
            return await base.Update(id, dto);
        }
    }
}
