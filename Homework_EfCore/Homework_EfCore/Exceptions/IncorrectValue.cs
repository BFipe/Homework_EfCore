namespace Homework_EfCore.Exceptions
{
    public class IncorrectValue : Exception
    {
        public IncorrectValue(string value) : base($"Incorrect value - {value}")
        {

        }
    }
}
