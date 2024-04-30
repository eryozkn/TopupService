using Microsoft.Extensions.Logging;
using Monad;
using System.Text.RegularExpressions;
using TopupService.DAL;
using TopupService.DAL.Interfaces;
using TopupService.Domain;
using TopupService.Facade.Interfaces;

namespace TopupService.Facade.Implementations
{
    public class BeneficiaryFacade(IBeneficiaryRepository beneficiaryRepository, ILogger<BeneficiaryFacade> logger) : IBeneficiaryFacade
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository = beneficiaryRepository;
        private readonly ILogger<BeneficiaryFacade> _logger = logger;
        public async ValueTask<Either<Beneficiary, Error>> AddBeneficiary(Beneficiary beneficiary, int beneficiaryLimit)
        {
            try
            {
                // Validate phone number
                if (!IsValidUAEPhoneNumber(beneficiary.PhoneNumber))
                {
                    return () => new Error() { Code = ErrorCodes.InvalidPhoneNumber, Message = $"Beneficiary phone number is invalid" };
                }

                // Check user beneficiary count and add new beneficiary
                var userBeneficiaries = _beneficiaryRepository.GetActiveBeneficiaries(beneficiary.UserId);

                if (userBeneficiaries.Count() >= beneficiaryLimit)
                {
                    return () => new Error() { Code = ErrorCodes.BeneficiaryLimitExceed, Message = $"User can add maximum {beneficiaryLimit} beneficiary" };
                }

                var addedBeneficiary = await _beneficiaryRepository.AddBeneficiary(MapToBeneficiaryEntity(beneficiary));

                return () => MapFromBeneficiaryEntity(addedBeneficiary);
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex.Message, $"{nameof(AddBeneficiary)}");
                return () => new Error() { Code = ErrorCodes.InternalServerError, Message = $"Something went wrong when adding a beneficiary" };
            }
        }

        public Either<IEnumerable<Beneficiary>, Error> GetUserActiveBeneficiaries(long userId)
        {
            try
            {
                var userBeneficiaries = _beneficiaryRepository.GetActiveBeneficiaries(userId);

                List<Beneficiary> beneficiaries = [];

                foreach (var beneficiary in userBeneficiaries)
                {
                    var beneficiaryDomainModel = MapFromBeneficiaryEntity(beneficiary);
                    beneficiaries.Add(beneficiaryDomainModel);
                }

                return () => beneficiaries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{nameof(GetUserActiveBeneficiaries)}");
                return () => new Error() { Code = ErrorCodes.InternalServerError, Message = $"Error on fetching user's beneficiaries" };
            }
        }
        private static bool IsValidUAEPhoneNumber(string phoneNumber) // can move to a helper class
        {
            // Regular expression pattern for UAE phone numbers
            string pattern = @"^\+971\d{9}$";

            // Check if the phone number matches the pattern
            return Regex.IsMatch(phoneNumber, pattern);
        }

        #region mappings
        private static BeneficiaryEntity MapToBeneficiaryEntity(Beneficiary beneficiary)
        {
            return new BeneficiaryEntity()
            {
                UserId = beneficiary.UserId,
                NickName = beneficiary.NickName,
                PhoneNumber = beneficiary.PhoneNumber,
                IsActive = beneficiary.IsActive
            };
        }

        private static Beneficiary MapFromBeneficiaryEntity(BeneficiaryEntity addedBeneficiary)
        {
            return new Beneficiary()
            {
                UserId = addedBeneficiary.UserId,
                NickName = addedBeneficiary.NickName,
                PhoneNumber = addedBeneficiary.PhoneNumber,
                IsActive = addedBeneficiary.IsActive
            };
        }
        #endregion
    }
}