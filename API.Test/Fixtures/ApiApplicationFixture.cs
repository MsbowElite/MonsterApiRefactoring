using Domain.Monsters;

namespace API.Test.Fixtures
{
    public class ApiApplicationFixture : IDisposable
    {
        public ApiApplication Application;
        public Monster Monster;

        public ApiApplicationFixture()
        {
            Application = new ApiApplication();

            Monster = new(
                Guid.NewGuid(),
                "Monster Test",
                50,
                40,
                80,
                "",
                60
            );
        }

        public void Dispose()
        {
            Application.Dispose();
        }
    }
}
