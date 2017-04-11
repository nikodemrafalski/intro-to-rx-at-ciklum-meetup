using System;
using System.Collections.Generic;
using GHApp.Communication;
using GHApp.Contracts.Dto;

namespace GHApp.Contracts.Responses
{
    [Serializable]
    public class RepoResponse : MessageBase
    {
        public RepoResponse(Guid id, List<Repo> repos)
            : base(id)
        {
            Repos = repos;
        }

        public List<Repo> Repos { get; }
    }
}