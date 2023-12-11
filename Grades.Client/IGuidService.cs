namespace Grades
{
    public interface IGuidService
    {
        Guid GetGuid();
    }
    public class GuidService : IGuidService
    {
        public Guid GetGuid() => Guid.NewGuid();
    }
}
