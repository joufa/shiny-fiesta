using Blazored.LocalStorage;
using Moq;
using System.Text.Json;

namespace Grades.Tests
{

    [TestFixture]
    internal class StorageServiceTests
    {
        private IStorageService _storageService;
        private Mock<ILocalStorageService> _storageServiceMock;
        private IGuidService _guidService;

        [SetUp]
        public void Setup()
        {
            _storageServiceMock = new Mock<ILocalStorageService>();
            _storageService = new StorageService(_storageServiceMock.Object);
            _guidService = new SequentialGuidService();
        }

        [Test]
        public void SerializeGradingIssue()
        {
            var issue = new GradingIssue()
            {
                Id = _guidService.GetGuid(),
                Name = "Maths",
                Percentage = 80,
                Locked = true,
            };

            string expected = "{\"Id\":\"00000000-0000-0000-0000-000000000001\",\"Name\":\"Maths\",\"Percentage\":80,\"Locked\":true}";

            var json = JsonSerializer.Serialize(issue);
            
            Assert.That(json, Is.EqualTo(expected));
        }

        [Test]
        public void DeserializeGradingIssue()
        {

            string json = "{\"Id\":\"00000000-0000-0000-0000-000000000001\",\"Name\":\"Maths\",\"Percentage\":80,\"Locked\":true}";

            GradingIssue? deserialized = JsonSerializer.Deserialize<GradingIssue>(json);

            var issue = new GradingIssue()
            {
                Id = _guidService.GetGuid(),
                Name = "Maths",
                Percentage = 80,
                Locked = true,
            };

            Assert.That(issue, Is.EqualTo(deserialized));
        }

        [Test]
        public void SerializeListOfGradingIssues()
        {
            List<GradingIssue> issues = new List<GradingIssue>
            {
                new GradingIssue()
                {
                    Id = _guidService.GetGuid(),
                    Name = "Maths",
                    Percentage = 30,
                    Locked = true,
                },
                new GradingIssue()
                {
                    Id = _guidService.GetGuid(),
                    Name = "Physics",
                    Percentage = 70,
                    Locked = true,
                }
            };

            string json = JsonSerializer.Serialize(issues);

            var expected = @"[{""Id"":""00000000-0000-0000-0000-000000000001"",""Name"":""Maths"",""Percentage"":30,""Locked"":true},{""Id"":""00000000-0000-0000-0000-000000000002"",""Name"":""Physics"",""Percentage"":70,""Locked"":true}]";

            Assert.That(expected, Is.EqualTo(json));
        }

        [Test]
        public async Task Load()
        {
            _storageServiceMock
                .Setup(x => x.GetItemAsStringAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(@"[{""Id"":""00000000-0000-0000-0000-000000000001"",""Name"":""Maths"",""Percentage"":30,""Locked"":true},{""Id"":""00000000-0000-0000-0000-000000000002"",""Name"":""Physics"",""Percentage"":70,""Locked"":true}]");

            var issues = await _storageService.GetGradingIssues();

            Assert.That(issues.Count == 2);
        }
    }
}
