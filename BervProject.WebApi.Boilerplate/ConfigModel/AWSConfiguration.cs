namespace BervProject.WebApi.Boilerplate.ConfigModel;

/// <summary>
/// AWS Config
/// </summary>
public class AwsConfiguration
{
    /// <summary>
    /// Email
    /// </summary>
    public AwsEmailConfiguration Email { get; set; }
    /// <summary>
    /// Dynamo
    /// </summary>
    public AwsDynamoConfiguration Dynamo { get; set; }
}

/// <summary>
/// AWS Dynamo Config
/// </summary>
public class AwsDynamoConfiguration
{
    /// <summary>
    /// Location
    /// </summary>
    public string Location { get; set; }
}

/// <summary>
/// AWS Email Config
/// </summary>
public class AwsEmailConfiguration
{
    /// <summary>
    /// Location
    /// </summary>
    public string Location { get; set; }
}