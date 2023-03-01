using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SLMM.Models;
using SLMM.Services;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SLMM.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class SLMMController : ControllerBase
    {
        private IOperate _operater;
        public SLMMController(IOperate slmmService)
        {
            _operater = slmmService;
        }

        /// <summary>
        /// Get Position of SLMM
        /// </summary>
        /// <returns>X,Y and Orientation</returns>
        [HttpGet]
        public Task<Position> Get()
        {
            return _operater.GetPosition();
        }

        // POST api/<SLMMController> /{length}/{width}
        [HttpPost("Reset")]
        public async Task<ActionResult<string>> Reset(int length, int width)
        {
            try
            {
                if (length < 1)
                    throw new ArgumentException("Length is invalid " + length);
                if (width < 1)
                    throw new ArgumentException("Width is invalid " + width);

                await _operater.Reset(length, width);
                return Ok("Reset Success");
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }

        }

        [HttpPost("Turn")]
        public async Task<ActionResult<Position>> Turn(bool isLeft = false)
        {
            try
            {
                var curPosition = await _operater.Turn(isLeft);
                return Ok(curPosition);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
        }


        [HttpPost("Forward")]
        public async Task<ActionResult<Position>> MoveForward()
        {
            try
            {
                var curPosition = await _operater.MoveForward();
                return Ok(curPosition);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
        }


    }
}
