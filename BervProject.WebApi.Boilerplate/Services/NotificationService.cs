using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using BervProject.WebApi.Boilerplate.ConfigModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AmazonSimpleNotificationServiceClient _notifClient;
        public NotificationService(AWSConfiguration awsConfig)
        {
            var credentials = new BasicAWSCredentials(awsConfig.Basic.Auth.AccessKey, awsConfig.Basic.Auth.SecretKey);
            _notifClient = new AmazonSimpleNotificationServiceClient(credentials, RegionEndpoint.GetBySystemName(""));
        }
        public void SendNotification()
        {

        }
    }
}
