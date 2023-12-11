namespace Grades.Tests
{
    public class GradeServiceTests
    {
        private IGradeService _gradeService;

        [SetUp]
        public void Setup()
        {
            _gradeService = new GradeService(new SequentialGuidService());
        }

        [Test]
        public void Test2()
        {
            List<GradeInputModel> model = new List<GradeInputModel>()
            {
                new GradeInputModel("Eka", 40)
                {
                    Rating = "8,25",
                },
                new GradeInputModel("Toka", 60)
                {
                    Rating = "7",
                }
            };
            var xx = _gradeService.GetGrade(model);
        }
    }
}