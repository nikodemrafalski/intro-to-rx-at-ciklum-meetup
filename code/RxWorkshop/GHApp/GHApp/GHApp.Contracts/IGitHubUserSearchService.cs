using System;
using System.Collections.Generic;
using GHApp.Contracts.Dto;

namespace GHApp.Contracts
{
    public interface IGitHubUserSearchService
    {
        IObservable<IEnumerable<User>> FindUser(string userNamePart);
    }
}