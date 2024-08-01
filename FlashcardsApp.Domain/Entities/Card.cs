using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashcardsApp.Domain.Entities;

public class Card
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int DeckId { get; set; }
    public Deck Deck { get; set; }
    public byte[]? Image { get; set; }

    [NotMapped]
    [FileExtensions(Extensions = "jpg,jpeg,png,gif", ErrorMessage = "Please upload a valid image file (jpg, jpeg, png, gif).")]
    public IFormFile ImageFile { get; set; }
}

