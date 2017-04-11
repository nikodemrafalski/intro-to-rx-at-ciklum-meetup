using System;

namespace GHApp.Communication
{
    public interface IMessage
    {
        Guid Id { get; }
    }
}