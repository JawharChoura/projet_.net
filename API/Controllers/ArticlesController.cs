using API.Data;
using API.DTOs;
using API.Models;
using API.Repositories; // CORRECTION: "Repositories" pas "Respositories"
using API.Respositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;

        public ArticlesController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        // GET: api/Articles
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            try
            {
                return Ok(await _articleRepository.GetAllAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erreur lors de la récupération des articles");
            }
        }

        // GET: api/Articles/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            try
            {
                var result = await _articleRepository.GetByIdAsync(id);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erreur lors de la récupération de l'article");
            }
        }

        // POST: api/Articles
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Article>> CreateArticle(Article article)
        {
            try
            {
                if (article == null)
                    return BadRequest();

                var createdArticle = await _articleRepository.AddAsync(article);

                return CreatedAtAction(nameof(GetArticle),
                    new { id = createdArticle.Id },
                    createdArticle);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erreur lors de la création de l'article");
            }
        }

        // PUT: api/Articles/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Article>> UpdateArticle(int id, Article article)
        {
            try
            {
                if (id != article.Id)
                    return BadRequest("ID d'article ne correspond pas");

                var articleToUpdate = await _articleRepository.GetByIdAsync(id);
                if (articleToUpdate == null)
                    return NotFound($"Article avec ID = {id} non trouvé");

                var updatedArticle = await _articleRepository.UpdateAsync(article);
                if (updatedArticle == null)
                    return NotFound();

                return Ok(updatedArticle);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erreur lors de la mise à jour de l'article");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Article>> DeleteArticle(int id)
        {
            try
            {
                var articleToDelete = await _articleRepository.GetByIdAsync(id);
                if (articleToDelete == null)
                    return NotFound($"Article avec ID = {id} non trouvé");

                var deletedArticle = await _articleRepository.DeleteAsync(id);
                if (deletedArticle == null)
                    return NotFound();

                return Ok(deletedArticle);
            }
            catch (Exception ex)
            {
                // 🔹 Important : log l'erreur pour voir le vrai problème
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erreur lors de la suppression de l'article");
            }
        }


        // GET: api/Articles/search?name=xxx&categoryId=1
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Article>>> Search(string name, int? categoryId)
        {
            try
            {
                var result = await _articleRepository.SearchAsync(name, categoryId);
                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erreur lors de la recherche d'articles");
            }
        }
    }
}