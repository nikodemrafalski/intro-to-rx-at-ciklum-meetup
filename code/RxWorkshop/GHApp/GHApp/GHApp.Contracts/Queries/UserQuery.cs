using System;
using GHApp.Communication;

namespace GHApp.Contracts.Queries
{
    [Serializable]
    public class UserQuery : MessageBase
    {
        public UserQuery(string name)
            : base(null)
        {
            Name = name;
        }

        public string Name { get; }
    }
}