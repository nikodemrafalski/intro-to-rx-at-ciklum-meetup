using System;
using GHApp.Communication;
using GHApp.Contracts.Dto;

namespace GHApp.Contracts.Queries
{
    [Serializable]
    public class FavQuery : MessageBase
    {
        public FavQuery(Repo repo)
            : base(null)
        {
            Repo = repo;
        }

        public Repo Repo { get; }
    }
}