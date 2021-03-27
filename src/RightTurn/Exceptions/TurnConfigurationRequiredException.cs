namespace RightTurn.Exceptions
{
    public class TurnConfigurationRequiredException : TurnException
    {
        public TurnConfigurationRequiredException()
            : base("IConfiguration is required. Use RightTurn.Extensions.Configuration to provide configuration builder.")
        {

        }
    }
}
