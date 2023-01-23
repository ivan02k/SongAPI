﻿using Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SongAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        public readonly IPDFService _IPDFService;
        public StatisticsController(IPDFService iPDFService)
        {
            _IPDFService = iPDFService;
        }
        [HttpGet]
        public IActionResult GetAllSongs()
        {
            return _IPDFService.GetPDF();
        }
    }
}
