using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Monsters.Create;

public sealed record CreateMonsterRequest(
    string Name,
    int Attack,
    int Defense,
    int Hp,
    string ImageUrl,
    int Speed);
