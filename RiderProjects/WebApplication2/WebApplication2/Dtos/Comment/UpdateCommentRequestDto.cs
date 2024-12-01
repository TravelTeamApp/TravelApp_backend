using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Dtos.Comment;

public class UpdateCommentRequestDto
{
       
    [MinLength(5, ErrorMessage = "Content must be 5 characters")]
    [MaxLength(280, ErrorMessage = "Content cannot be over 280 characters")]
    public string Text { get; set; } = string.Empty;
}