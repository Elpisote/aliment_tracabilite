using aliment_backend.DTOs;
using aliment_backend.Entities;
using aliment_backend.Interfaces;
using aliment_backend.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace aliment_backend.Controllers
{
    // Ce contrôleur gère les opérations CRUD pour l'entité de stock
    // hérite de GenericController
    [Authorize(Roles = "Admin, User")]
    [ApiController]
    public class StockController : GenericController<Stock, StockDTO>
    {
        // Constructeur du contrôleur avec l'unité de travail et le mapper en tant que dépendances
        public StockController(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        // Méthode pour ajouter plusieurs stocks simultanément
        [HttpPost("api/[controller]/AddMany")]
        public async Task<IActionResult> AddMany([FromBody] List<int> productIds)
        {
            // Liste pour stocker les stocks ajoutés
            List<Stock> addedStocks = new();
            foreach (int productId in productIds)
            {
                // Récupérer le produit correspondant à l'ID
                Product? product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
                if (product == null)
                {
                    return NotFound($"Le produit avec l'identifiant {productId} n'a pas été trouvé.");
                }
                else
                {                   
                    // Créer un nouveau stock
                    Stock stock = new()
                    {
                        Product = product,
                        ProductId = productId,
                        Statuts = Statuts.Inprogess, // Statut initial du stock
                        OpeningDate = DateTime.Now, // Date d'ouverture du stock (maintenant)
                        UserCreation = User.FindFirstValue("Username") // Utilisateur qui a créé le stock (à partir du jeton d'authentification)
                    };
                    // Calculer la date d'expiration du stock en fonction de la durée de conservation du produit
                    stock.ExpirationDate = stock.OpeningDate.AddDays(product.DurationConservation);
                    // Ajouter le stock à la base de données
                    await _unitOfWork.GetRepository<Stock>().AddAsynch(stock);
                    // Ajouter le stock à la liste des stocks ajoutés
                    addedStocks.Add(stock);

                    // Ajouter un historique pour le stock nouvellement créé
                    Historical historical = new(stock.OpeningDate, "Création", stock.Id)
                    {
                        Stock = stock,
                        StockId = stock.Id
                    };
                    await _unitOfWork.GetRepository<Historical>().AddAsynch(historical);
                }
            }
            await _unitOfWork.CompleteAsync();
            return Ok(addedStocks);
        }
    }
}
