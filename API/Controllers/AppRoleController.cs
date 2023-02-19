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
    public class AppRoleController : ControllerBase
    {
        private readonly IGenericService<Article> _articleService;
        private readonly IGenericService<AppUser> _appUserService;
        private readonly IGenericService<AppRole> _appRoleService;

        public AppRoleController(IGenericService<Article> articleService, IGenericService<AppUser> appUserService, IGenericService<AppRole> appRoleService)
        {
            _articleService = articleService;
            _appUserService = appUserService;
            _appRoleService = appRoleService;
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllRole()
        {
            return Ok(_appRoleService.GetAll());
        }
        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult GetByRole(int id)
        {
            return Ok(_appRoleService.GetById(id));
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllActiveRole()
        {
            return Ok(_appRoleService.GetDefault(x => x.Status == ENTITIES.Enums.Status.Active));
        }
        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult GetActiveRole(int id)
        {
            return Ok(_appRoleService.GetByDefault(x => x.Id == id && x.Status == ENTITIES.Enums.Status.Active));
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllPassiveRole()
        {
            return Ok(_appRoleService.GetDefault(x => x.Status == ENTITIES.Enums.Status.Passive));
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllPendingRole()
        {
            return Ok(_appRoleService.GetDefault(x => x.Status == ENTITIES.Enums.Status.Pending));
        }
        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateRole(AppRole role)
        {
            return Ok(_appRoleService.Add(role));//AppRoleile birlikte appRole dan rolu yazar olanı da göndermemiz gerekecek!!!!
        }
        [HttpPut]
        [Route("[action]")]
        public IActionResult UpdateRole(AppRole role)
        {
            return Ok(_appRoleService.Update(role));
        }
        [HttpDelete]
        [Route("[action]/{id}")]

        public IActionResult PassiveRole(int id)
        {

            return Ok(_appRoleService.SetPassive(id));
        }
        [HttpDelete]
        [Route("[action]")]

        public IActionResult PassiveRole()
        {

            return Ok(_appRoleService.SetPassive(x => x.Status == ENTITIES.Enums.Status.Active));
        }
        [HttpDelete]
        [Route("[action]")]

        public IActionResult RemoveRole(AppRole role)
        {

            return Ok(_appRoleService.Remove(role));
        }
    }
}
