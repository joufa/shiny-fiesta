using Blazored.LocalStorage;
using System.Text.Json;

namespace Grades
{
    public interface IStorageService
    {
        Task<List<GradingIssue>> GetGradingIssues();
        Task SaveGradingIssues(List<GradingIssue> gradingIssues);
    }

    public class StorageService : IStorageService
    {
        private const string LocalStorageKey = "grades.app.storage";

        private readonly ILocalStorageService _localStorageService;

        public StorageService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<List<GradingIssue>> GetGradingIssues()
        {
            var items = await _localStorageService.GetItemAsStringAsync(LocalStorageKey);

            if (items == null)
            {
                return [];
            }

            var deserialized = JsonSerializer.Deserialize<List<GradingIssue>>(items);

            return deserialized ?? [];
        }

        public async Task SaveGradingIssues(List<GradingIssue> gradingIssues)
        {
            if (gradingIssues == null || gradingIssues.Count == 0)
            {
                return;
            }

            var serialized = JsonSerializer.Serialize(gradingIssues);

            await _localStorageService.SetItemAsStringAsync(LocalStorageKey, serialized);
        }
    }
}
