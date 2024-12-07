using WebApplication2.DTOs;
using WebApplication2.Dtos.Place;
using WebApplication2.Models;

namespace WebApplication2.Mappers
{
    public static class UserPlaceTypeMapper
    {
        public static UserPlaceTypeDto ToUserPlaceTypeDto(this UserPlaceType userPlaceTypeModel)
        {
            return new UserPlaceTypeDto
            {
                PlaceTypeNames = new List<string> { userPlaceTypeModel.PlaceType.PlaceTypeName } // Wrap the string in a List<string>
            };
        }

    }
}