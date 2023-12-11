namespace Grades.Tests
{
    [TestFixture]
    public class AverageCalculatorTests
    {
        private IAverageCalculator averageCalculator;

        [SetUp]
        public void SetUp()
        {
            averageCalculator = new AverageCalculator();
        }

        [Test]
        public void CalculateAverage_ShouldCalculateCorrectly()
        {
            var parametersList = new List<AverageCalculationParameters>
            {
                new AverageCalculationParameters { Value = 1.0m }
            };

            var result = averageCalculator.CalculateAverage(parametersList);

            Assert.That(result.AverageResult, Is.EqualTo(1.0m));
            Assert.That(result.CalculationProcess, Is.EqualTo("( 1.0 ) / 1 = 1.0"));
        }

        [Test]
        public void CalculateWeightedAverage_ShouldCalculateCorrectly()
        {
            var parametersList = new List<AverageCalculationParameters>
            {
                new AverageCalculationParameters { Value = 1.0m, Weight = 2 },
                new AverageCalculationParameters { Value = 2.0m, Weight = 3 },
                new AverageCalculationParameters { Value = 3.0m, Weight = 4 },
                new AverageCalculationParameters { Value = 4.0m, Weight = 1 }
            };

            var result = averageCalculator.CalculateWeightedAverage(parametersList);

            Assert.That(result.AverageResult, Is.EqualTo(2.4m));
            Assert.That(result.CalculationProcess, Is.EqualTo("( 1.0 * 2 + 2.0 * 3 + 3.0 * 4 + 4.0 * 1 ) / (2 + 3 + 4 + 1) = 2.4"));
        }
    }
}