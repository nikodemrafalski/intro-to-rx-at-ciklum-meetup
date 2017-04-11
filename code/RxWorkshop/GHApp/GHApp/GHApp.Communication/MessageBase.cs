using System;

namespace GHApp.Communication
{
    [Serializable]
    public abstract class MessageBase : IMessage
    {
        protected MessageBase(Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
        }

        public Guid Id { get; }
    }
}