using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMSWeb.BusinessServices.Services.Interfaces;
using EMSWeb.Filters;
using EMSWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMSWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhrasebookController : ControllerBase
    {
        private IPhrasebookService PhrasebookService { get; set; }
        public PhrasebookController(IPhrasebookService phrasebookService)
        {
            PhrasebookService = phrasebookService;
        }

        // GET: api/Phrasebook
        // -SELECT * FROM emas.phrase_lists;
        //--SELECT* FROM emas.phrases;
        //
        [HttpGet]
        public async Task<PhrasebookList> Get()
        {
            return await PhrasebookService.GetPhrasebook();
        }

        // GET: api/Phrasebook/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Phrasebook
        [HttpPost]
        [ClaimRequirement(ClaimType.Admin)]
        public async Task<IActionResult> Post([FromBody] CreatePhraseDto value)
        {
             return Ok(await PhrasebookService.CreatePhrase(value));
        }

        [HttpPost("category")]
        [ClaimRequirement(ClaimType.Admin)]
        public async Task<IActionResult> PostCategory([FromBody] CreateListDto value)
        {
            return Ok(await PhrasebookService.CreateList(value));
        }

        [HttpPost("modify")]
        [ClaimRequirement(ClaimType.Admin)]
        public async Task Modify([FromBody] ModifyNodeDto node) 
        {
            await PhrasebookService.Modify(node);
        }

        // PUT: api/Phrasebook/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Phrasebook/delete
        [HttpPost("delete")]
        [ClaimRequirement(ClaimType.Admin)]
        public async Task Delete([FromBody] DeletePhrasebookDto[] ids)
        {
            await PhrasebookService.Delete(ids);
        }
    }
}
