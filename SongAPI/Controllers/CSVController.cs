//using Data;
//using Data.Entities;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Repository.Interfaces;
//using ViewModels;

//namespace SongAPI.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class CSVController : Controller
//    {
//        private readonly SongContext _context;
//        private readonly ICSVService _csvService;
//        private static Dictionary<string,List<string>> result = new Dictionary<string, List<string>>();
//        public CSVController(SongContext songContext, ICSVService csvService)
//        {
//            _csvService = csvService;
//            _context = songContext;
//        }

//        [HttpPost("read-csv")]
//#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
//        public async Task<IActionResult> GetCSV([FromForm] IFormFileCollection file)
//#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
//        {
//            var csv = _csvService.ReadCSV<CSVViewModel>(file[0].OpenReadStream()).ToList();
           
//            foreach (CSVViewModel model in csv)
//            {
//                var artist=_context.Artists.FirstOrDefault(x => x.Name == model.artist);
//                if (artist != null)
//                {
//                   var checkSong= artist.Song.FirstOrDefault(x => x.Title == model.title);
//                    if (checkSong==null)
//                    {
//                        artist.Song.Add(new Song() { Title = model.title });    
//                    }
//                }
//                else
//                {

//                }
//                //if (result.ContainsKey(model.artist))
//                //{
//                //    var checkSOng = result[model.artist].FirstOrDefault(x => x == model.title);
                   
//                //}
//                //else
//                //{
//                //    result.Add(model.artist, new List<string>());
//                //}
               
//            }
           
//            //var isValidArtist = _context.Artists.Where(x => x.Name == csv);
//            //if (isValidArtist)
//            //{
//            //    _context.Artists.Add(csv);
//            //    _context.SaveChanges();
//            //}

//            return Ok(csv);
//        }
//    }
//}
