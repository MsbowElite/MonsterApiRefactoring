using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Battles
{
    public interface IBattleRepository
    {
        public Task InsertAsync(Battle battle, CancellationToken cancellationToken);
        public ValueTask<Battle?> FindAsync(int id, CancellationToken cancellationToken);
        public void Remove(Battle battle);
    }
}
