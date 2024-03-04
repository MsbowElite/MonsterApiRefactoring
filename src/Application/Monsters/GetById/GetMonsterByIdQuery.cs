using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Monsters.GetById;

public sealed record GetMonserByIdQuery(Guid MonsterId) : IQuery<MonsterResponse>;
