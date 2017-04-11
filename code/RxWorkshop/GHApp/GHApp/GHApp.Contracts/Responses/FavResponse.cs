using System;
using GHApp.Communication;

namespace GHApp.Contracts.Responses
{
    [Serializable]
    public class FavResponse : MessageBase
    {
        public FavResponse(Guid id)
            : base(id)
        {
        }
    }
}