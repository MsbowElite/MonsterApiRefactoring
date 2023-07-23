using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Test.Fixtures
{
    public class ApiApplicationFixture : IDisposable
    {
        public ApiApplication Application;

        public ApiApplicationFixture()
        {
            Application = new ApiApplication();
        }

        public void Dispose()
        {
            Application.Dispose();
        }
    }
}
