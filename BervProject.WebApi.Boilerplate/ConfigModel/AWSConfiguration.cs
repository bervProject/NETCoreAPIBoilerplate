namespace BervProject.WebApi.Boilerplate.ConfigModel
{
    public class AWSConfiguration
    {
        public AWSBasicConfiguration Basic { get; set; }
        public AWSEmailConfiguration Email { get; set; }
        public AWSNotificationConfiguration Notification { get; set; }
        public AWSDynamoConfiguration Dynamo { get; set; }
    }
}
