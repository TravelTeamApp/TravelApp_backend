using WebApplication2.Dtos.Comment;
using WebApplication2.Models;

namespace WebApplication2.Mappers;

public static class CommentMapper
{
    public static CommentDto ToCommentDto(this Comment commentModel)
    {
        return new CommentDto
        {
            CommentId = commentModel.CommentId,
            Text = commentModel.Text,
            CreatedOn = commentModel.CreatedOn,
            Rate = commentModel.Rate,
            CreatedBy = commentModel.User?.UserName ?? "Unknown",
            PlaceId = commentModel.PlaceId,
            UserID = commentModel.UserID
        };
    }

    public static Comment ToCommentFromCreate(this CreateCommentDto commentDto, int placeId)
    {
        return new Comment
        {
            Text = commentDto.Text,
            Rate = commentDto.Rate,
            PlaceId = placeId
        };
    }

    public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto commentDto, int placeId)
    {
        return new Comment
        {

            Text = commentDto.Text,
            Rate = commentDto.Rate,
            PlaceId = placeId
        };
    }

}