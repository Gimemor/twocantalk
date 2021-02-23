using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMSWeb.BusinessServices.Services.Interfaces;
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
        public async Task Post([FromBody] CreatePhraseDto value)
        {
             await PhrasebookService.CreatePhrase(value);
        }

        [HttpPost("category")]
        public async Task PostCategory([FromBody] CreateListDto value)
        {
            await PhrasebookService.CreateList(value);
        }

        [HttpPost("modify")]
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
        public async Task Delete([FromBody] DeletePhrasebookDto[] ids)
        {
            await PhrasebookService.Delete(ids);
        }
    }
}
