using Microsoft.AspNetCore.Mvc;
using TopupService.Domain;
using TopupService.Models.Response;
using TopupService.Facade.Interfaces;
using Monad;
using TopupService.Models.Request;
using Newtonsoft.Json;
using Error = TopupService.Domain.Error;

namespace TopupService.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class BeneficiaryController(IBeneficiaryFacade beneficiaryFacade, ILogger<BeneficiaryController> logger, IConfiguration configuration) : ControllerBase
    {
        private readonly IBeneficiaryFacade _beneficiaryFacade = beneficiaryFacade;
        private readonly ILogger<BeneficiaryController> _logger = logger;

        private readonly IConfiguration _configuration = configuration;

        [HttpPost("beneficiary")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BeneficiaryResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> AddBeneficiary([FromBody] AddBeneficiaryRequest request)
        {
            var benefiary = new Beneficiary()
            {
                UserId = request.UserId,
                PhoneNumber = request.PhoneNumber,
                NickName = request.NickName,
                IsActive = true,
            };

            var beneficiaryLimit = _configuration.GetSection("BeneficiaryLimit").Get<int>();

            var beneficiaryResult = await _beneficiaryFacade.AddBeneficiary(benefiary, beneficiaryLimit);

            return MapBeneficiaryResponse(beneficiaryResult);
        }

        [HttpGet("beneficiaries/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BeneficiaryResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public ActionResult GetActiveBeneficiaries([FromRoute] long userId)
        {
            var userBeneficiaries = _beneficiaryFacade.GetUserActiveBeneficiaries(userId);

            if (userBeneficiaries.IsRight()) // error thrown
            {
                var error = userBeneficiaries.Right();

                if (error.Code == ErrorCodes.InternalServerError)
                {
                    return StatusCode(500, MapErrorResponse(error));
                }
                else
                {
                    return BadRequest(
                    new ErrorResponse()
                    {
                        Code = error.Code,
                        Message = error.Message
                    });
                }
            }
            else
            {
                var beneficiaryList = userBeneficiaries.Left();
                List<BeneficiaryResponse> beneficiaryResponseList = [];

                foreach(var beneficiary in beneficiaryList)
                {
                    beneficiaryResponseList.Add(new BeneficiaryResponse()
                    {
                        UserId = beneficiary.UserId,
                        NickName = beneficiary.NickName,
                        PhoneNumber = beneficiary.PhoneNumber
                    });
                }

                return Ok(beneficiaryResponseList);

            };
        }

        private static ErrorResponse MapErrorResponse(Error error)
        {
            return new ErrorResponse()
            {
                Code = error.Code,
                Message = error.Message
            };
        }
        private ActionResult MapBeneficiaryResponse(Either<Beneficiary, Error> response)
        {
            if (response.IsRight()) // error thrown
            {
                var error = response.Right();

                if (error.Code == ErrorCodes.InternalServerError)
                {
                    return StatusCode(500, MapErrorResponse(error));
                }
                else
                {
                    return BadRequest(
                    new ErrorResponse()
                    {
                        Code = error.Code,
                        Message = error.Message
                    });
                }
            }
            else
            {
                var beneficiary = response.Left();

                return Ok(new BeneficiaryResponse()
                { 
                    UserId = beneficiary.UserId,
                    NickName = beneficiary.NickName,
                    PhoneNumber = beneficiary.PhoneNumber
                });
            }
        }
    }
}
