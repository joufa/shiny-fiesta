using System.Globalization;

namespace Grades
{
    // Result class to hold the calculation result for a single value and its average
    public class AverageResultItem
    {
        public decimal AverageResult { get; set; }
        public required string CalculationProcess { get; set; }
    }

    // Parameters class for average calculations
    public class AverageCalculationParameters
    {
        public decimal Value { get; set; }
        public int? Weight { get; set; }
    }

    // Interface for average calculation
    public interface IAverageCalculator
    {
        AverageResultItem CalculateAverage(List<AverageCalculationParameters> parametersList);
        AverageResultItem CalculateWeightedAverage(List<AverageCalculationParameters> parametersList);
    }

    // Combined implementation
    public class AverageCalculator : IAverageCalculator
    {
        public AverageResultItem CalculateAverage(List<AverageCalculationParameters> parametersList)
        {
            if (parametersList == null || parametersList.Count == 0)
            {
                throw new ArgumentException("Input list is empty or null.");
            }

            decimal average = parametersList.Average(p => p.Value);
            string process = $"( {string.Join(" + ", parametersList.Select(p => p.Value.ToString(CultureInfo.InvariantCulture)))} ) / {parametersList.Count} = {average.ToString(CultureInfo.InvariantCulture)}";

            return new AverageResultItem { AverageResult = average, CalculationProcess = process };
        }

        public AverageResultItem CalculateWeightedAverage(List<AverageCalculationParameters> parametersList)
        {
            if (parametersList == null || parametersList.Count == 0)
            {
                throw new ArgumentException("Input list is empty or null.");
            }

            decimal weightedSum = parametersList.Sum(p => p.Value * (p.Weight ?? 1));
            decimal totalWeight = parametersList.Sum(p => p.Weight ?? 1);

            // Generating the step-by-step process string for weighted average calculation
            string process = $"( {string.Join(" + ", parametersList.Select(p => $"{p.Value.ToString(CultureInfo.InvariantCulture)} * {(p.Weight ?? 1).ToString(CultureInfo.InvariantCulture)}"))} ) / {totalWeight.ToString(CultureInfo.InvariantCulture)}";
            process += $" = {weightedSum.ToString(CultureInfo.InvariantCulture)} / {totalWeight.ToString(CultureInfo.InvariantCulture)}";
            process += $" = {(weightedSum / totalWeight).ToString("0.0", CultureInfo.InvariantCulture)}";

            return new AverageResultItem { AverageResult = weightedSum / totalWeight, CalculationProcess = process };
        }
    }
}
