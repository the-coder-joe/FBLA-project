namespace FBLA_project
{
    public class MessageException(string msg) : Exception
    {
        public string Msg { get; set; } = msg;
    }
}