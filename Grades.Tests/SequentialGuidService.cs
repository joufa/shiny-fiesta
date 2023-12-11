namespace Grades.Tests
{
    internal class SequentialGuidService : IGuidService
    {
        private Guid _currentGuid = Guid.Parse("00000000-0000-0000-0000-000000000000");

        public Guid GetGuid()
        {
            byte[] bytes = _currentGuid.ToByteArray();
            bytes[15] += 1;
            _currentGuid = new Guid(bytes);
            return _currentGuid;
        }
    }
}
