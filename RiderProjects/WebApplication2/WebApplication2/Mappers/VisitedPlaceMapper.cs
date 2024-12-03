using WebApplication2.Dtos.Comment;
using WebApplication2.Dtos.Place;
using WebApplication2.Dtos.PlaceType;
using WebApplication2.Models;

namespace WebApplication2.Mappers
{
    public static class VisitedPlaceMapper
    {
        public static VisitedPlaceDto ToVisitedPlaceDto(this VisitedPlace visitedPlace)
        {
            return new VisitedPlaceDto
            {
                VisitedPlaceId = visitedPlace.VisitedPlaceId,
                PlaceId = visitedPlace.Place?.PlaceId ?? 0, // Nullable kontrolü
                PlaceName = visitedPlace.Place?.PlaceName,
                PlaceAddress = visitedPlace.Place?.PlaceAddress,
                Description = visitedPlace.Place?.Description,
                Rating = visitedPlace.Place?.Rating,
                PlaceType = visitedPlace.Place?.PlaceType != null
                    ? new PlaceTypeDto
                    {
                        PlaceTypeId = visitedPlace.Place.PlaceType.PlaceTypeId,
                        PlaceTypeName = visitedPlace.Place.PlaceType.PlaceTypeName
                    }
                    : null,

                // Yorumları DTO'ya dönüştürüyoruz
                Comments = visitedPlace.Place?.Comments?.Select(c => c.ToCommentDto()).ToList() ?? new List<CommentDto>(),

                UserName = visitedPlace.User?.UserName // Kullanıcı adı kontrolü
            };
        }
    }
}