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
    public class AppUserController : ControllerBase
    {
        private readonly IGenericService<Article> _articleService;
        private readonly IGenericService<AppUser> _appUserService;
        private readonly IGenericService<AppRole> _appRoleService;

        public AppUserController(IGenericService<Article> articleService, IGenericService<AppUser> appUserService, IGenericService<AppRole> appRoleService)
        {
            _articleService = articleService;
            _appUserService = appUserService;
            _appRoleService = appRoleService;
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllUser()
        {
            return Ok(_appUserService.GetAll());
        }
        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult GetByUser(int id)
        {
            return Ok(_appUserService.GetById(id));
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllActiveUser()
        {
            return Ok(_appUserService.GetDefault(x => x.Status == ENTITIES.Enums.Status.Active));
        }
        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult GetActiveUser(int id)
        {
            return Ok(_appUserService.GetByDefault(x => x.Id == id && x.Status == ENTITIES.Enums.Status.Active));
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllPassiveUser()
        {
            return Ok(_appUserService.GetDefault(x => x.Status == ENTITIES.Enums.Status.Passive));
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllPendingUser()
        {
            return Ok(_appUserService.GetDefault(x => x.Status == ENTITIES.Enums.Status.Pending));
        }
        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateUser(AppUser user)
        {
            return Ok(_appUserService.Add(user));//AppUserile birlikte appUser dan rolu yazar olanı da göndermemiz gerekecek!!!!
        }
        [HttpPut]
        [Route("[action]")]
        public IActionResult UpdateUser(AppUser user)
        {
            return Ok(_appUserService.Update(user));
        }
        [HttpDelete]
        [Route("[action]/{id}")]

        public IActionResult PassiveUser(int id)
        {

            return Ok(_appUserService.SetPassive(id));
        }
        [HttpDelete]
        [Route("[action]")]

        public IActionResult PassiveUser()
        {

            return Ok(_appUserService.SetPassive(x => x.Status == ENTITIES.Enums.Status.Active));
        }
        [HttpDelete]
        [Route("[action]")]

        public IActionResult RemoveUser(AppUser user)
        {

            return Ok(_appUserService.Remove(user));
        }
        //nul references alma ihtimalimiz var. id ile bağlantılı olması gerekebilir!!!!

    }
}

