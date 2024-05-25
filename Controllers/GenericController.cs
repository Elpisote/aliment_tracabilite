using aliment_backend.DTOs;
using aliment_backend.Entities;
using aliment_backend.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace aliment_backend.Controllers
{
    // Contrôleur générique pour les opérations CRUD sur une entité de type T
    [ApiController]
    public class GenericController<T, TDTO> : ControllerBase where T : class where TDTO : class
    {
        // Dépendances requises pour le contrôleur
        protected readonly IUnitOfWork _unitOfWork; // Unité de travail pour accéder à la source de données
        private readonly IMapper _mapper; // Mapper pour la conversion entre entités et DTO

        // Constructeur du contrôleur
        public GenericController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Méthode pour récupérer toutes les entités de type T     
        [HttpGet("api/[controller]s")]  
        public virtual async Task<IActionResult> GetAllAsync()
        {
            // Récupère toutes les entités de type T à partir de la source de données
            IEnumerable<T> entities = await _unitOfWork.GetRepository<T>().GetAllAsync();

            // Traitement spécifique selon le type d'entité T
            if (typeof(T) == typeof(Category))
            {
                // Si l'entité est une catégorie, chargez les produits associés
                List<Category> categories = entities.Cast<Category>().ToList();
                _ = LoadProductsForCategoriesAsync(categories);
            }
            else if (typeof(T) == typeof(Product))
            {
                // Chargez les stocks et les catégories associées pour les produits
                List<Product> products = entities.Cast<Product>().ToList();
                await LoadStocksForProductsAsync(products);
                await LoadCategoriesForProductsAsync(products);
            }
            else if (typeof(T) == typeof(Stock))
            {
                // Si l'entité est un stock
                List<Stock> stocks = entities.Cast<Stock>().ToList();
                stocks = stocks.Where(s => s.Statuts == 0).ToList();
                foreach (Stock s in stocks)
                {
                    // Chargez les informations complémentaires pour chaque stock
                    s.Product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(s.ProductId);
                    if (s.Product?.DurationConservation != null)
                    {
                        s.ExpirationDate = s.OpeningDate.AddDays(s.Product.DurationConservation);
                    }
                }
                await LoadHistoricalsForStocksAsync(stocks);
                return Ok(stocks);
            }
            return Ok(entities);
        }

        // Action pour récupérer une entité de type T par son identifiant  
        [HttpGet("api/[controller]/{id}")]
        public virtual async Task<IActionResult> GetByIdAsync(int id)
        {
            // Récupération de l'entité de type T par son identifiant
            T? entity = await _unitOfWork.GetRepository<T>().GetByIdAsync(id);
            if (entity == null)
                return NotFound($"L'entité n°{id} n'a pas été trouvée.");

            // Traitement spécial selon le type d'entité T
            if (typeof(T) == typeof(Product))
            {
                // Si l'entité est un produit, chargez les informations supplémentaires telles que la catégorie associée
                if (entity is Product product)
                {
                    product.Category = _unitOfWork.GetRepository<Category>().GetById(product.CategoryId);
                }
            }
            else if (typeof(T) == typeof(Stock))
            {
                // Si l'entité est un stock, chargez les informations supplémentaires telles que le produit associé
                if (entity is Stock stock)
                {
                    stock.Product = _unitOfWork.GetRepository<Product>().GetById(stock.ProductId);
                    if (stock.Product?.DurationConservation != null)
                    {
                        stock.ExpirationDate = stock.OpeningDate.AddDays(stock.Product.DurationConservation);
                    }                  
                }
            }
            return Ok(entity);
        }

        // Méthode pour ajouter une nouvelle entité de type T
        [HttpPost("api/[controller]")]     
        public virtual async Task<IActionResult> AddOne([FromBody] TDTO dto)
        {     
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Traitement spécial en fonction du type d'entité T
            if (typeof(T) == typeof(Product) && dto != null)
            {
                // Si l'entité est un produit, effectuez les opérations spécifiques pour l'ajout de produit
                Product product = _mapper.Map<Product>(dto);
                product.Category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(product.CategoryId);
                await _unitOfWork.GetRepository<Product>().AddAsynch(product);
                await _unitOfWork.CompleteAsync();
                return Ok(product);
            }
            else if (typeof(T) == typeof(Stock))
            {
                // L'ajout de stock n'est pas autorisé via cette méthode
                return BadRequest("L'ajout de stock par la méthode AddOne n'est pas auttorisée.");
            }
            else
            {
                // Ajout d'une entité de type T autre que Product ou Stock
                T entityToAdd = _mapper.Map<T>(dto);
                await _unitOfWork.GetRepository<T>().AddAsynch(entityToAdd);
                await _unitOfWork.CompleteAsync();
                return Ok(entityToAdd);
            }            
        }

        // Méthode pour supprimer une entité de type T par son ID 
        [HttpDelete("api/[controller]/{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            // Vérification du type d'entité T
            if (typeof(T) == typeof(Stock))
            {
                // Empêcher la suppression des stocks
                return BadRequest("La suppression des stocks n'est pas autorisée.");
            }
            else
            {
                // Supprimer l'entité avec l'ID spécifié
                bool isDeleted = await _unitOfWork.GetRepository<T>().Delete(id);
                if (!isDeleted)
                    return NotFound($"L'entité avec l'ID {id} n'a pas été trouvée.");

                await _unitOfWork.CompleteAsync();
                return Ok(true);
            }
        }

        // Méthode pour mettre à jour une entité de type T par son ID
        [HttpPut("api/[controller]/{id}")]
        public virtual async Task<IActionResult> Update(int id, [FromBody] TDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Récupérer l'entité de type T par son ID
            T? entity = await _unitOfWork.GetRepository<T>().GetByIdAsync(id);
            if (entity == null)
                return NotFound($"L'entité n°{id} n'a pas été trouvée.");

            // Gérer les opérations spéciales en fonction du type d'entité T
            if (typeof(T) == typeof(Stock))
            {
                // Mettre à jour les informations d'un stock
                if (dto is StockDTO stockDTO)
                {
                    Stock? stock = await _unitOfWork.GetRepository<Stock>().GetByIdAsync(id);
                    if (stock == null)
                        throw new Exception($"Stock n°{id} non trouvé.");

                    // Mapper les données du DTO dans l'objet Stock
                    stockDTO.ProductId = stock.ProductId;
                    _mapper.Map(stockDTO, stock);
                    stock.UserModification = User.Identity?.Name;
                    Stock? stockUpdate = _unitOfWork.GetRepository<Stock>().Update(stock); 
                    stockUpdate.UserModification = User.FindFirstValue("Username");

                    // Ajouter un historique pour la modification
                    Historical historical = new(DateTime.Now, "Modification", stock.Id);
                    await _unitOfWork.GetRepository<Historical>().AddAsynch(historical);
                    await _unitOfWork.CompleteAsync();

                    return Ok(stockUpdate);
                }
            }
            else if (typeof(T) == typeof(Product))
            {
                // Mettre à jour les informations d'un produit
                if (entity is Product product)
                {
                    // Mapper les données du DTO dans l'objet Product
                    _mapper.Map(dto, product);
                    // Récupérer la catégorie associée
                    product.Category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(product.CategoryId);                    
                    _unitOfWork.GetRepository<Product>().Update(product);
                    await _unitOfWork.CompleteAsync();
                    return Ok(product);
                }
            }
            else
            {
                // Mettre à jour les informations d'une entité autre que Stock ou Product
                _mapper.Map(dto, entity);
                _unitOfWork.GetRepository<T>().Update(entity);
                await _unitOfWork.CompleteAsync();
            }           
            return Ok(entity);
        }

        // Méthode asynchrone pour charger les catégories associées à une collection de produits
        private async Task LoadCategoriesForProductsAsync(IEnumerable<Product> products)
        {
            foreach (Product product in products)
            {
                // Charger la catégorie associée au produit
                product.Category = await _unitOfWork.GetRepository<Category>()
                    .GetByIdAsync(product.CategoryId);
            }
        }

        // Méthode pour charger les produits associés à une collection de catégories
        private Task LoadProductsForCategoriesAsync(IEnumerable<Category> categories)
        {
            foreach (Category category in categories)
            {
                // Charger les produits associés à la catégorie
                category.Products =  _unitOfWork.GetRepository<Product>()
                    .FindAll(p => p.CategoryId == category.Id)
                    .ToList(); 
            }
            return Task.CompletedTask;
        }

        // Méthode pour charger les stocks associés à une collection de produits
        private Task LoadStocksForProductsAsync(IEnumerable<Product> products)
        {
            foreach (Product product in products)
            {
                // Charger les stocks associés au produit
                product.Stocks = _unitOfWork.GetRepository<Stock>()
                    .FindAll(p => p.ProductId == product.Id)
                    .ToList(); 
            }
            return Task.CompletedTask;
        }

        // Méthode pour charger les historiques associés à une collection de stocks
        private Task LoadHistoricalsForStocksAsync(IEnumerable<Stock> stocks)
        {
            foreach (Stock stock in stocks)
            {
                // Charger les historiques associés au stock
                stock.Historical = _unitOfWork.GetRepository<Historical>()
                    .FindAll(s => s.StockId == stock.Id)
                    .ToList();
            }
            return Task.CompletedTask;
        }       
    }   
}

