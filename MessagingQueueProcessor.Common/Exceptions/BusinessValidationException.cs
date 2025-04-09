namespace MessagingQueueProcessor.Common.Exceptions
{
    public class BusinessValidationException : Exception
    {
        public BusinessValidationException()
        {
            Messages = new List<string>();
        }

        public BusinessValidationException(string message)
            : base(message)
        {
            Messages = new List<string>
            {
                message,
            };
        }

        public BusinessValidationException(IEnumerable<string> messages)
            : base(string.Join(",", messages))
        {
            Messages = new List<string>(messages);
        }

        public BusinessValidationException(string message, Exception? innerException)
            : base(message, innerException)
        {
            Messages = new List<string>
            {
                message,
            };
        }

        public IReadOnlyList<string> Messages { get; }
    }
}
