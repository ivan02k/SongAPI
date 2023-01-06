using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using ViewModels.Data.ViewModels;

namespace SongAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : BaseController<SongViewModel>
    {
        public SongController(IBaseService<SongViewModel> service) : base(service)
        {
        }
    }
}
