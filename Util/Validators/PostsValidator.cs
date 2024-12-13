using BackendLaboratory.Constants;
using BackendLaboratory.Data.DTO;
using BackendLaboratory.Util.CustomExceptions.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace BackendLaboratory.Util.Validators
{
    public static class PostsValidator
    {
        public static void IsPostDataValid(CreatePostDto createPostDto)
        {
            if (createPostDto.ReadingTime <= 0)
            {
                throw new BadRequestException(ErrorMessages.IncorrectReadingTime);
            }

            if (createPostDto.Image != null)
            {
                string regex = @"^https?:\/\/[^\s/$.?#].[^\s]*$\/*";
                if (!Regex.IsMatch(createPostDto.Image, regex))
                {
                    throw new BadRequestException(ErrorMessages.IncorrectImage);
                }
            }

            if (createPostDto.Tags.IsNullOrEmpty()) 
            {
                throw new BadRequestException(ErrorMessages.IncorrectPostTags);
            }
        }
    }
}
