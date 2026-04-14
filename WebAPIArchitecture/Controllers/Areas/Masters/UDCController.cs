using API_Arch_Core.DataBaseObjects.Areas.Masters;
using API_Arch_DataAccessLayer.Interphases.Areas.Masters;
using API_Arch_Framework.Areas.Masters;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIArchitecture.Controllers.Areas.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class UDCController : ControllerBase
    {
        private readonly IUDCServices _UDCServices;
        private readonly IMapper _mapper;
        public UDCController(IUDCServices uDCServices, IMapper mapper) 
        {
            _UDCServices = uDCServices;
            _mapper = mapper;
        }

        [HttpGet("GetAllUDCValues")]
        public async Task<IActionResult> GetAllUDCValues()
        {
            return Ok(_UDCServices.GetAllAsync());
        }

        [HttpPost("AddUDCValue")]
        public async Task<IActionResult> AddUDCValue(UDCDto UDCDto)
        {
            var udc = _mapper.Map<UDCMaster>(UDCDto);
            return Ok(_UDCServices.CreateAsync(udc));
        }

    }
}
