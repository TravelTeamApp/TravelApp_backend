using WebApplication2.Interfaces;

namespace WebApplication2.Services;

public class PlaceService
{
    private readonly ICommentRepository _commentRepo;
    private readonly IPlaceRepository _placeRepo;

    public PlaceService(ICommentRepository commentRepo, IPlaceRepository placeRepo)
    {
        _commentRepo = commentRepo;
        _placeRepo = placeRepo;
    }

    /// <summary>
    /// Updates the rating of a place based on its comments.
    /// </summary>
    /// <param name="placeId">The ID of the place to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdatePlaceRatingAsync(int placeId)
    {
        // Retrieve all comments for the given place
        var comments = await _commentRepo.GetByPlaceIdAsync(placeId);

        if (comments == null || !comments.Any())
        {
            // No comments, set rating to 0
            await _placeRepo.UpdateRatingAsync(placeId, 0);
            return;
        }

        // Calculate the average rating
        var totalRate = comments.Sum(c => c.Rate);
        var userCount = comments.Count;
        var averageRating = Math.Round((double)totalRate / userCount, 1); // Round to 1 decimal places
        // Update the place's rating
        await _placeRepo.UpdateRatingAsync(placeId, averageRating);
    }
}