using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using ViewModels.Data.ViewModels;

namespace SongAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongSpecificationController : BaseController<SongSpecificationViewModel>
    {
        public SongSpecificationController(IBaseService<SongSpecificationViewModel> service, ICached<SongSpecificationViewModel> cached) : base(service, cached)
        {
        }
    }
}
