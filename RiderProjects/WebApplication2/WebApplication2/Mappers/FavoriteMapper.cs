using WebApplication2.Dtos.Comment;
using WebApplication2.Dtos.Place;
using WebApplication2.Dtos.PlaceType;
using WebApplication2.Models;

namespace WebApplication2.Mappers
{
    public static class FavoriteMapper
    {
        public static FavoriteDto ToFavoriteDto(this Favorite favorite)
        {
            return new FavoriteDto
            {
                FavoriteId = favorite.FavoriteId,
                PlaceId = favorite.Place?.PlaceId ?? 0, // Nullable kontrolü
                PlaceName = favorite.Place?.PlaceName,
                PlaceAddress = favorite.Place?.PlaceAddress,
                Description = favorite.Place?.Description,
                Rating = favorite.Place?.Rating,
                PlaceType = favorite.Place?.PlaceType != null
                    ? new PlaceTypeDto
                    {
                        PlaceTypeId = favorite.Place.PlaceType.PlaceTypeId,
                        PlaceTypeName = favorite.Place.PlaceType.PlaceTypeName
                    }
                    : null,

                // Yorumları DTO'ya dönüştürüyoruz
                Comments = favorite.Place?.Comments?.Select(c => c.ToCommentDto()).ToList() ?? new List<CommentDto>(),

                UserName = favorite.User?.UserName // Kullanıcı adı kontrolü
            };
        }
    }
}