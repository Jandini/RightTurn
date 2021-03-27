using System;

namespace RightTurn.Exceptions
{
    public class TurnException : Exception
    {
        public TurnException(string message) : base(message)
        {
        }
    }
}
