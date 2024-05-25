using aliment_backend.Entities;
using aliment_backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace aliment_backend.Controllers
{
    // Ce contrôleur gère les opérations liées à l'historique des stocks.
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class HistoricalController : ControllerBase
    {
        public readonly IUnitOfWork _unitOfWork;

        // Constructeur du contrôleur avec l'unité de travail en tant que dépendance
        public HistoricalController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Méthode pour récupérer tous les historiques avec les informations associées aux stocks
        [HttpGet("api/[controller]s")]
        public async Task<IActionResult> GetAllAsync()
        {
            // Récupérer tous les historiques et tous les stocks de la base de données
            IEnumerable<Historical> historicals = await _unitOfWork.Historical.GetAllAsync();
            IEnumerable<Stock> stocks = await _unitOfWork.Stocks.GetAllAsync();

            // Extraire les identifiants uniques des produits associés aux stocks
            List<int> productIds = stocks.Select(s => s.ProductId).Distinct().ToList();

            // Récupérer tous les produits associés aux stocks en utilisant leurs identifiants
            IEnumerable<Product> products = await _unitOfWork.Products.GetAllAsync(p => productIds.Contains(p.Id));

            // Associer chaque stock à son produit correspondant
            foreach (var stock in stocks)
            {
                stock.Product = products.FirstOrDefault(p => p.Id == stock.ProductId);
            }

            // Compléter les historiques avec les informations sur les stocks
            foreach (var historical in historicals)
            {
                historical.Stock = stocks.FirstOrDefault(s => s.Id == historical.StockId);
            }
            return Ok(historicals);
        }
    }
}
