
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Grades
{
    public class GradingIssue
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Kuvaus tarvitaan")]
        public string Name { get; set; } = "";

        [Range(1, 100, ErrorMessage = "Prosentin tulee olla 1-100")]
        public int Percentage { get; set; }
        public bool Locked { get; set; }

        [JsonConstructor] 
        public GradingIssue() 
        {
            Name = "";
        }

        public override bool Equals(object? obj)
        {
            return obj is GradingIssue issue &&
                   Id.Equals(issue.Id) &&
                   Name == issue.Name &&
                   Percentage == issue.Percentage &&
                   Locked == issue.Locked;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Percentage, Locked);
        }

        public static bool operator ==(GradingIssue? left, GradingIssue? right)
        {
            return EqualityComparer<GradingIssue>.Default.Equals(left, right);
        }

        public static bool operator !=(GradingIssue? left, GradingIssue? right)
        {
            return !(left == right);
        }
    }
}
