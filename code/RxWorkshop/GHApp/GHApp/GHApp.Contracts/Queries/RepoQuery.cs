using System;
using GHApp.Communication;
using GHApp.Contracts.Dto;

namespace GHApp.Contracts.Queries
{
    [Serializable]
    public class RepoQuery : MessageBase
    {
        public RepoQuery(User user)
            : base(null)
        {
            User = user;
        }

        public User User { get; }
    }
}