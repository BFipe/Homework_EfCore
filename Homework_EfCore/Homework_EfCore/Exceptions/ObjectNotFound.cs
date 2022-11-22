namespace Homework_EfCore.Exceptions
{
    public class ObjectNotFound : Exception
    {
        public ObjectNotFound(string objName) : base($"{objName} not found qwe")
        {

        }
    }
}
