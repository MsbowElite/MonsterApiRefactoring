using Lib.Repository.Entities;
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
        public Monster Monster; 

        public ApiApplicationFixture()
        {
            Application = new ApiApplication();

            Monster = new()
            {
                Name = "Monster Test",
                Attack = 50,
                Defense = 40,
                Hp = 80,
                Speed = 60,
                ImageUrl = ""
            };
        }

        public void Dispose()
        {
            Application.Dispose();
        }
    }
}
