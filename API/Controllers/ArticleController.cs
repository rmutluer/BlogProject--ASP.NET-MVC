using BUSINESS.Abstract;
using ENTITIES.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IGenericService<Article> _articleService;
        private readonly IGenericService<AppUser> _appUserService;
        private readonly IGenericService<AppRole> _appRoleService;

        public ArticleController(IGenericService<Article> articleService, IGenericService<AppUser> appUserService, IGenericService<AppRole> appRoleService)
        {
            _articleService = articleService;
            _appUserService = appUserService;
            _appRoleService = appRoleService;
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllArticle()
        {
            return Ok(_articleService.GetAll());
        }
        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult GetByArticle(int id)
        {
            return Ok(_articleService.GetById(id));
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllActiveArticle()
        {
            return Ok(_articleService.GetDefault(x=>x.Status==ENTITIES.Enums.Status.Active));
        }
        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult GetActiveArticle(int id)
        {
            return Ok(_articleService.GetByDefault(x => x.ArticleID==id && x.Status==ENTITIES.Enums.Status.Active));
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllPassiveArticle()
        {
            return Ok(_articleService.GetDefault(x => x.Status == ENTITIES.Enums.Status.Passive));
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllPendingArticle()
        {
            return Ok(_articleService.GetDefault(x => x.Status == ENTITIES.Enums.Status.Pending));
        }
        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateArticle(Article article)
        {
            return Ok(_articleService.Add(article));//article ile birlikte appUser dan rolu yazar olanı da göndermemiz gerekecek!!!!
        }
        [HttpPut]
        [Route("[action]")]
        public IActionResult UpdateArticle(Article article)
        {
            return Ok(_articleService.Update(article));
        }
        [HttpDelete]
        [Route("[action]/{id}")]

        public IActionResult PassiveArticle(int id)
        {
            
            return Ok(_articleService.SetPassive(id));
        }
        [HttpDelete]
        [Route("[action]")]

        public IActionResult PassiveArticle()
        {

            return Ok(_articleService.SetPassive(x=>x.Status==ENTITIES.Enums.Status.Active));
        }
        [HttpDelete]
        [Route("[action]")]

        public IActionResult RemoveArticle(Article article)
        {

            return Ok(_articleService.Remove(article));
        }
        //nul references alma ihtimalimiz var. id ile bağlantılı olması gerekebilir!!!!

    }
}
