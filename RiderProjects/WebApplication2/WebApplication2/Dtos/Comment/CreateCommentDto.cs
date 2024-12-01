using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Dtos.Comment
{
    public class CreateCommentDto
    {
        [MinLength(5, ErrorMessage = "Content must be at least 5 characters.")]
        [MaxLength(280, ErrorMessage = "Content cannot exceed 280 characters.")]
        public string Text { get; set; } = string.Empty;


    }
}
