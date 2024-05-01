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
    [Route("api/v1/topup")]
    [ApiController]
    public class TopupController(ITopupFacade topupfacade, ILogger<TopupController> logger, IConfiguration configuration) : ControllerBase
    {
        private readonly ITopupFacade _topupfacade = topupfacade;
        private readonly ILogger<TopupController> _logger = logger;

        private readonly IConfiguration _configuration = configuration;

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TopupResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> CreateTopup([FromBody] TopupRequest request)
        {
            
            var topupLimits = _configuration.GetSection("TopupLimits").Get<IEnumerable<TopupLimit>>() ?? //fallback if there is no config
                               [new TopupLimit() { LimitCode = "UnverifiedUserMonthlyTopupLimit", Value = 500 },
                                new TopupLimit() { LimitCode = "VerifiedUserMonthlyTopupLimit", Value = 1000 },
                                new TopupLimit() { LimitCode = "MultipleBeneficiaryMonthlyLimit", Value = 3000 }];

            var topupFee = _configuration.GetSection("TopupFee").Get<decimal>();

            var userInfo = ExtractUserInfo();

            if (userInfo == null)
            {
                return BadRequest(
                new ErrorResponse()
                {
                        Code = ErrorCodes.InvalidUser,
                        Message = "Invalid User"
                    });
            }
            
            var topupResult = await _topupfacade.CreateTopup
            (
                new Topup() 
                { 
                    UserInfo = userInfo,
                    Amount = request.Amount, 
                    BeneficiaryId = request.BeneficiaryId 
                },
                topupLimits,
                topupFee
            );

            return MapTopupResponse(topupResult);
        }

        [HttpGet("options")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TopupOption>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public ActionResult GetTopupOptions()
        {
            try
            {
                var options = _configuration.GetSection("TopupOptions").Get<List<TopupOption>>();

                if (options == null || options?.Count == 0)
                {
                    return StatusCode(500, new ErrorResponse { Code = ErrorCodes.ConfigrationNotFoundError, Message = "No topup option found" });
                }

                return Ok(options);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in retrieving topup options from config. Detail: {ex.Message}");
                return StatusCode(500, new ErrorResponse { Code = ErrorCodes.ConfigrationNotFoundError, Message = "Topup options cannot be retrieved" });
            }
        }

        private User? ExtractUserInfo()
        {
            if (Request.Headers.ContainsKey("User-Info"))
            {
                Request.Headers.TryGetValue("User-Info", out var userInfoJson);
                return JsonConvert.DeserializeObject<User>(userInfoJson!) ?? null;
            }

            return null;
        }

        private static ErrorResponse MapErrorResponse(Error error)
        {
            return new ErrorResponse()
            {
                Code = error.Code,
                Message = error.Message
            };
        }
        private ActionResult MapTopupResponse(Either<Topup, Error> response)
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
                var topup = response.Left();

                return Ok(new TopupResponse()
                {
                    UserId = topup.UserInfo.Id,
                    Amount = topup.Amount
                });
            }
        }
    }
}
