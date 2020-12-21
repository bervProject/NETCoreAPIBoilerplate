namespace BervProject.WebApi.Boilerplate.ConfigModel
{
    public class AWSConfiguration
    {
        public AWSBasicConfiguration Basic { get; set; }
        public AWSEmailConfiguration Email { get; set; }
        public AWSDynamoConfiguration Dynamo { get; set; }
    }

    public class AWSAuth
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }

    public class AWSBasicConfiguration
    {
        public AWSAuth Auth { get; set; }
    }

    public class AWSDynamoConfiguration
    {
        public string Location { get; set; }
    }

    public class AWSEmailConfiguration
    {
        public string Location { get; set; }
    }
}
