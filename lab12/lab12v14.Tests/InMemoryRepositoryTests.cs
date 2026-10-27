using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using lab12v14;

namespace lab12v14.Tests
{
    public class InMemoryRepositoryTests
    {
        private readonly InMemoryRepository<Course> _repository;

        public InMemoryRepositoryTests()
        {
            _repository = new InMemoryRepository<Course>();
        }

        [Fact]
        public void Add_ValidItem_ShouldAddItemToRepository()
        {
            // Arrange
            var course = new Course { Id = 1, Title = "Math", Credits = 4, TeacherId = 10 };

            // Act
            _repository.Add(course);

            // Assert
            var result = _repository.GetById(1);
            Assert.NotNull(result);
            Assert.Equal("Math", result.Title);
        }

        [Fact]
        public void Add_DuplicateId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var course1 = new Course { Id = 1, Title = "Math", Credits = 4, TeacherId = 10 };
            var course2 = new Course { Id = 1, Title = "Physics", Credits = 3, TeacherId = 11 };
            _repository.Add(course1);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _repository.Add(course2));
        }

        [Fact]
        public void GetById_ExistingId_ShouldReturnItem()
        {
            // Arrange
            var course = new Course { Id = 1, Title = "C# Programming", Credits = 5, TeacherId = 1 };
            _repository.Add(course);

            // Act
            var result = _repository.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetById_NonExistingId_ShouldReturnNull()
        {
            // Act
            var result = _repository.GetById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAll_ShouldReturnAllItems()
        {
            // Arrange
            _repository.Add(new Course { Id = 1, Title = "Course 1", Credits = 2, TeacherId = 1 });
            _repository.Add(new Course { Id = 2, Title = "Course 2", Credits = 3, TeacherId = 2 });

            // Act
            var items = _repository.GetAll();

            // Assert
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public void Update_ExistingItem_ShouldUpdateData()
        {
            // Arrange
            var course = new Course { Id = 1, Title = "Old Title", Credits = 3, TeacherId = 1 };
            _repository.Add(course);

            var updatedCourse = new Course { Id = 1, Title = "New Title", Credits = 5, TeacherId = 1 };

            // Act
            _repository.Update(updatedCourse);

            // Assert
            var result = _repository.GetById(1);
            Assert.NotNull(result);
            Assert.Equal("New Title", result.Title);
            Assert.Equal(5, result.Credits);
        }

        [Fact]
        public void Update_NonExistingItem_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var course = new Course { Id = 999, Title = "Ghost Course", Credits = 1, TeacherId = 1 };

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.Update(course));
        }

        [Fact]
        public void Delete_ExistingId_ShouldRemoveItem()
        {
            // Arrange
            var course = new Course { Id = 1, Title = "To Delete", Credits = 2, TeacherId = 1 };
            _repository.Add(course);

            // Act
            _repository.Delete(1);

            // Assert
            Assert.Null(_repository.GetById(1));
        }

        [Fact]
        public void Delete_NonExistingId_ShouldThrowKeyNotFoundException()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.Delete(999));
        }
    }
}
