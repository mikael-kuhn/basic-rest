using Xunit;

namespace Finance.DomainTest.Repositories
{
    using Finance.Domain.Domain;
    using Finance.Domain.Repositories;

    public sealed class FileRepositoryTest
    {
        private readonly FileRepository testSubject;

        public FileRepositoryTest()
        {
            testSubject = new FileRepository();
        }

        [Fact]
        public void Should_Implement_IRepository_File()
        {
            Assert.IsAssignableFrom<IRepository<File>>(testSubject);
        }

        [Fact]
        public void Get_should_return_a_file()
        {
            // Act
            var response = testSubject.Get("1");

            // Assert
            Assert.NotNull(response);
        }

        [Fact]
        public void Get_should_return_null()
        {
            // Act
            var response = testSubject.Get("unknown");

            // Assert
            Assert.Null(response);
        }

        [Fact]
        public void Exists_should_return_true()
        {
            Assert.True(testSubject.Exists("1"));
        }

        [Fact]
        public void Exists_should_return_false()
        {
            Assert.False(testSubject.Exists("unknown"));
        }

        [Fact]
        public void Delete()
        {
            // Act
            testSubject.Delete("1");

            // Assert
            Assert.False(testSubject.Exists("1"));
        }
    }
}