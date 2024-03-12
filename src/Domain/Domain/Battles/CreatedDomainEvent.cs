using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Battles;

public sealed record CreatedDomainEvent(Guid monsterId, int damage) : IDomainEvent;