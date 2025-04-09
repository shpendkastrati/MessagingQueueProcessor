namespace MessagingQueueProcessor.Common
{
    public static class Check
    {
        public static T IsNotNull<T>(T? dependency)
            where T : class
        {
            return dependency ?? throw new ArgumentNullException($"{dependency} is null!");
        }

        public static T IsNotNull<T>(T? parameterValue, string parameterName)
        {
            return parameterValue == null
                ? throw new ArgumentNullException($"{parameterName} is null!")
                : parameterValue;
        }

        public static void IsNotEmpty(string parameterValue, string parameterName)
        {
            if (parameterValue == null)
            {
                throw new ArgumentNullException($"{parameterName} is null!");
            }

            if (string.IsNullOrWhiteSpace(parameterValue))
            {
                throw new ArgumentException($"{parameterName} is empty!");
            }
        }
    }
}
