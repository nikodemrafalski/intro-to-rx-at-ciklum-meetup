using System;
using System.Collections.Generic;
using GHApp.Communication;
using GHApp.Contracts.Dto;

namespace GHApp.Contracts.Responses
{
    [Serializable]
    public class UserResponse : MessageBase
    {
        public UserResponse(Guid id, List<User> users)
            : base(id)
        {
            Users = users;
        }

        public List<User> Users { get; }
    }
}