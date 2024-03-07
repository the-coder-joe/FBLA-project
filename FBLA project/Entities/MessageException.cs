namespace FBLA_project
{
    public class MessageException : Exception
    {
        public string Messge { get; set; }

        public MessageException(string msg)
        {
            Messge = msg;
        }
    }
}