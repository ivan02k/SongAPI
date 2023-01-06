using Data.Entities.IdentityClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace SongAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        public readonly IDataService _dataService;
        public readonly IPDFService _iPDFService;
        public DataController(IDataService dataService, IPDFService iPDFService)
        {
            _dataService = dataService;
            _iPDFService = iPDFService;
        }
        [HttpDelete]
        public bool DeleteAll()
        {
            return _dataService.DeleteAll();
        }
        [HttpGet]
        public Task GetPDF()
        {
            return _iPDFService.GetPDF();
        }
    }
}
