namespace MessagingQueueProcessor.Data.Enums
{
    public enum MessageStatus
    {
        Pending,
        Processing,
        Sent,
        Failed,
        DeadLetter
    }
}
