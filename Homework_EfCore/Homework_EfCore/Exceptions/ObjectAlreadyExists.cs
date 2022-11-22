namespace Homework_EfCore.Exceptions
{
    public class ObjectAlreadyExists : Exception
    {
        public ObjectAlreadyExists(string objName) : base($"{objName} already exists")
        {

        }
    }
}
