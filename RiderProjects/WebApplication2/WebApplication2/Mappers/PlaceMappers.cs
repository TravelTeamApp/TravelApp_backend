using WebApplication2.Dtos.Place;
using WebApplication2.Dtos.PlaceType;
using WebApplication2.Models;

namespace WebApplication2.Mappers
{
    public static class PlaceMappers
    {
        public static PlaceDto ToPlaceDto(this Place placeModel)
        {
            return new PlaceDto
            {
                PlaceId = placeModel.PlaceId,
                PlaceName = placeModel.PlaceName,
                PlaceAddress = placeModel.PlaceAddress,
                Description = placeModel.Description,
                Rating = placeModel.Rating,
                Longitude = placeModel.Longitude,
                Latitude = placeModel.Latitude,
                // PlaceType bilgisini ekliyoruz
                PlaceType = placeModel.PlaceType != null 
                    ? new PlaceTypeDto 
                    { 
                        PlaceTypeId = placeModel.PlaceType.PlaceTypeId,
                        PlaceTypeName = placeModel.PlaceType.PlaceTypeName 
                    }
                    : null,
                    
                 // Yorumları ekliyoruz
               Comments = placeModel.Comments.Select(c => c.ToCommentDto()).ToList()
            };
        }
    }
}