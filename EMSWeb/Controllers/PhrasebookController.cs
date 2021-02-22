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
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Phrasebook/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
