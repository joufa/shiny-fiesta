using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grades.Tests
{
    [TestFixture]
    internal class GradingValidatorTests
    {
        private CustomNumberValidationAttribute _validator;
        [SetUp]
        public void Setup()
        {
            _validator = new();
        }
        [TestCase("4,25")]
        [TestCase("5,50")]
        [TestCase("5")]
        [TestCase("5,0")]
        [TestCase("9,75")]
        public void Validate_Success(string input)
        {
            var result = _validator.GetValidationResult(input, new ValidationContext(input));
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        [TestCase("4,26")]
        [TestCase("3")]
        [TestCase("10,25")]
        [TestCase(null)]
        [TestCase("")]
        public void Validate_Failure(string input)
        {
            var result = _validator.IsValid(input);
            Assert.That(result, Is.EqualTo(false));
        }
    }
}
