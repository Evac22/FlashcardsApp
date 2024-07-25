using FlashcardsApp.Domain.Entities;
using FlashcardsApp.Infrastructure.Data;
using FlashcardsApp.Models;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace FlashcardsApp.Tests
{
    public class CardsControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<IValidator<CreateCardViewModel>> _validatorMock;
        private readonly CardsController _controller;

        public CardsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);

            _validatorMock = new Mock<IValidator<CreateCardViewModel>>();
            _controller = new CardsController(_context, _validatorMock.Object);
        }

        [Fact]
        public async Task Create_Post_ValidModel_ShouldSaveCardAndRedirect()
        {
            // Arrange
            var model = new CreateCardViewModel
            {
                Question = "Test Question",
                Answer = "Test Answer",
                DeckId = 1,
                ImageFile = new FormFile(new MemoryStream(new byte[0]), 0, 0, "ImageFile", "image.jpg")
            };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateCardViewModel>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            // Act
            var result = await _controller.Create(model);

            // Assert
            _context.Cards.Should().ContainSingle(c => c.Question == model.Question && c.Answer == model.Answer);
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("Details");
            redirectResult.RouteValues["id"].Should().Be(model.DeckId);
        }

        [Fact]
        public async Task Edit_Post_ValidModel_ShouldUpdateCardAndRedirect()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var card = new Card { Id = 1, Question = "Original Question", Answer = "Original Answer", DeckId = 1, Image = new byte[0] };
                context.Cards.Add(card);
                await context.SaveChangesAsync();

                var controller = new CardsController(context, _validatorMock.Object);

                var model = new CreateCardViewModel
                {
                    Id = card.Id,
                    Question = "Updated Question",
                    Answer = "Updated Answer",
                    DeckId = 1,
                    ImageFile = null // оставляем null, чтобы не менять Image
                };

                _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateCardViewModel>(), default))
                    .ReturnsAsync(new FluentValidation.Results.ValidationResult());

                // Act
                var result = await controller.Edit(card.Id, model);

                // Assert
                var updatedCard = await context.Cards.FindAsync(card.Id);
                Assert.Equal(model.Question, updatedCard.Question);
                Assert.Equal(model.Answer, updatedCard.Answer);

                var redirectResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Details", redirectResult.ActionName);
                Assert.Equal(model.DeckId, redirectResult.RouteValues["id"]);
            }
        }

        [Fact]
        public async Task Delete_Post_ShouldRemoveCardAndRedirect()
        {
            // Arrange
            var card = new Card { Id = 1, Question = "Test Question", Answer = "Test Answer", DeckId = 1, Image = new byte[0] };
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteConfirmed(card.Id);

            // Assert
            _context.Cards.Should().BeEmpty();
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("Details");
            redirectResult.RouteValues["id"].Should().Be(card.DeckId);
        }
    }
}
