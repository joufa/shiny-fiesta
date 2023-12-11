using System.Globalization;
using System.Text;

namespace Grades
{
    public record GradeResponse(string WeightedGrade, string RegularGrade, AverageCalculations Calculations);
    public record AverageCalculations(string WeightedAverageCalculation, string AvegrageCalculation);

    public class GradeInputModel
    {
        public string Label { get; }
        public int Percentage { get; }
        
        [CustomNumberValidation]
        public string Rating { get; set; } = "";

        public GradeInputModel(string label, int percentage)
        {
            Label = label;
            Percentage = percentage;
        }
    }

    public record ValidateResult(bool IsSuccess, string? ErrorText);

    public interface IGradeService
    {
        public GradingIssue GetNew();
        public ValidateResult ValidateCollection(List<GradingIssue> gradingIssues);

        public GradeResponse GetGrade(List<GradeInputModel> grades);
    }

    public class GradeService : IGradeService
    {
        private readonly IGuidService _guidService;
        private readonly IAverageCalculator _averageCalculator = new AverageCalculator();

        public GradeService(IGuidService guidService) 
        {
            _guidService = guidService;
        }

        public GradingIssue GetNew()
        {
            return new GradingIssue()
            {
                Id = _guidService.GetGuid(),
            };
        }

        public ValidateResult ValidateCollection(List<GradingIssue> gradingIssues)
        {
            // Has to be at least 2
            if (gradingIssues.Count < 3)
            {
                return new ValidateResult(false, "Lisää enemmän arvosanatyyppejä");
            }

            // Sum of percentages 100
            bool isValidPercentageSum = gradingIssues.Sum(x => x.Percentage) == 100;

            if (!isValidPercentageSum)
            {
                return new ValidateResult(false, "Painokertoimet tulee olla yhteensä 100");
            }
            
            return new ValidateResult(true, null);
        }

        private bool IsTotalPercetageAHundred(IEnumerable<int> percerages) => percerages.Sum() == 100;
        
        private static decimal ConvertStringToDecimal(string input)
        {
            // Replace the comma with a dot and parse as a decimal
            if (decimal.TryParse(input.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException("Invalid decimal string format.");
            }
        }
       
        private (decimal, decimal, AverageCalculations) CalculateWeightedAverage(List<GradeInputModel> items)
        {
            if (items == null || items.Count == 0)
            {
                throw new ArgumentException("The list of items is null or empty.");
            }

            var weightedResult = _averageCalculator.CalculateWeightedAverage(items.Select(x => new AverageCalculationParameters()
            {
                Value = ConvertStringToDecimal(x.Rating),
                Weight = x.Percentage,
            })
                .ToList());

            var regularResult = _averageCalculator.CalculateAverage(items.Select(x => new AverageCalculationParameters()
            {
                Value = ConvertStringToDecimal(x.Rating),
            })
               .ToList());

            var roundedAverage = RoundWeightedAverage(weightedResult.AverageResult, 0.25m);
            var roundedRegularAverage = RoundWeightedAverage(regularResult.AverageResult, 0.25m);
            return (roundedAverage, roundedRegularAverage, new AverageCalculations(weightedResult.CalculationProcess, regularResult.CalculationProcess));
        }

        private static decimal RoundWeightedAverage(decimal weightedAverage, decimal precision)
        {
            return Math.Ceiling(weightedAverage / precision) * precision;
        }

        public GradeResponse GetGrade(List<GradeInputModel> grades)
        {
            var average = CalculateWeightedAverage(grades);
            string weightedGrade = ConvertToLetterFormat(average.Item1);
            string regularGrade = ConvertToLetterFormat(average.Item2);
            return new GradeResponse(weightedGrade, regularGrade, average.Item3);
        }

        static string ConvertToLetterFormat(decimal roundedWeightedAverage)
        {
            // Extract the decimal part
            var firstPart = (int)roundedWeightedAverage;
            decimal decimalPart = roundedWeightedAverage % 1;

            // Check the decimal part and convert to letters
            string letterPart = "";
            if (decimalPart == 0.25m)
            {
                letterPart = "+";
            }
            else if (decimalPart == 0.50m)
            {
                letterPart = " ½";
            }
            else if (decimalPart == 0.75m)
            {
                firstPart = (int)roundedWeightedAverage + 1;
                letterPart = "-";
            }

            // Convert to string and concatenate with the integer part
            string result = (firstPart).ToString() + letterPart;
            return result;
        }
    }
}
