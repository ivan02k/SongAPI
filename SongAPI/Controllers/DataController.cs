using Data.Entities.IdentityClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using Service.Interfaces;
using System.Data;
using ViewModels;

namespace SongAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
    public class DataController : ControllerBase
    {
        public readonly IDataService _dataService;
        public readonly ICSVService _iCSVService;
        public DataController(IDataService dataService, IPDFService iPDFService, ICSVService iCSVService)
        {
            _dataService = dataService;
            _iCSVService = iCSVService;
        }
        [HttpGet("GetEmptyFile")]
        public IActionResult GetEmptyFile()
        {
            List<CSVViewModel> list = new List<CSVViewModel>();
            return _iCSVService.WriteCSV(list);
        }
        [HttpDelete("Delete Data")]
        public bool DeleteAll()
        {
            return _dataService.DeleteAll();
        }
        [HttpPost]
        public IActionResult GetCSV()
        {
            List<CSVViewModel> list = _iCSVService.ConvertDataToCSV();
            return _iCSVService.WriteCSV(list);
        }
        [HttpPut("Upload CSV")]
        public void UploadCSV([FromForm] IFormFileCollection file) =>
           _iCSVService.CSVDataFilling(_iCSVService.DownloadCSV(file[0].OpenReadStream()), true);

    }
}
