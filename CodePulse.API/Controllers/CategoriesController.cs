using CodePulse.Api.Models.DTO;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace CodePulse.API.Controllers {
    // https://localhost:xxxx/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository){
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request){
            // Map DTO to Domain Model
            var category = new Category{
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };            

            await categoryRepository.CreateAsync(category);

            // Map Domain Model to DTO
            var response = new CategoryDto{
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }

        //GET: http://localhost:5102/api/categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories(){
            var categories = await categoryRepository.GetAllAsync();        
        
            //Map Domain model to DTO
            var response = new List<CategoryDto>();
            foreach(var category in categories){
                response.Add(new CategoryDto {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }
            return Ok(response);
        }

    }
}

