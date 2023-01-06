using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using ViewModels.Data.ViewModels;

namespace SongAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : BaseController<ArtistViewModel>
    {
        public ArtistController(IBaseService<ArtistViewModel> service) : base(service)
        {
        }
    }
}
