using BackendLaboratory.Constants;
using BackendLaboratory.Util.CustomExceptions.Exceptions;

namespace BackendLaboratory.Util.Validators
{
    public static class QueryValidation
    {
        public static void IsPostDataValid(int page, int size, int? min, int? max)
        {
            if (size < 1) { throw new BadRequestException(ErrorMessages.InvalidSize); }

            if (page < 1) { throw new BadRequestException(ErrorMessages.InvalidPage); }

            if (min < 0) { throw new BadRequestException(ErrorMessages.InvalidMin); }

            if (max < 0) { throw new BadRequestException(ErrorMessages.InvalidMax); }
        }

        public static void IsCommunityDataValid(int page, int size)
        {
            if (size < 1) { throw new BadRequestException(ErrorMessages.InvalidSize); }

            if (page < 1) { throw new BadRequestException(ErrorMessages.InvalidPage); }
        }
    }
}
