using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Common;
using Common.Entities;
using Newtonsoft.Json;

namespace Kuscotopia.Services
{

    //Loop over number from controller, creating messages for each one
    public class QueueService
    {
        private static BasicAWSCredentials credentials;

        private static AmazonSQSClient sqsClient;
        //Keeps the same random seed for each new random call in a workentity
        private readonly Random random = new Random();

        public QueueService()
        {
            credentials = new BasicAWSCredentials(Credentials.QueueKeyId, Credentials.QueueKey);
            sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.USEast2);
        }

        public async Task QueueWorkAsync(int workCount)
        {
            for (int i = 0; i < workCount; i++)
            {
                var sendMessageRequest = new SendMessageRequest()
                {
                    QueueUrl = Credentials.QueueUrl,
                    MessageBody = JsonConvert.SerializeObject(CreateRandomWorkEntity())
                };
                await sqsClient.SendMessageAsync(sendMessageRequest);
            }
        }

        private WorkEntity CreateRandomWorkEntity()
        {
            int typeChoice = random.Next(1,4); //1, 2, or 3
            int buildSteps = random.Next(1,6); //1, 2, 3, 4, or 5
            int surveyTime = random.Next(500,1001); //500 - 1000
            

            WorkEntity workEntity = new WorkEntity();

            switch (typeChoice)
            {
                case 1:
                    workEntity.WorkType = "Carry";
                    int carryObject = random.Next(1, 6); //1, 2, 3, 4, or 5
                    switch(carryObject)
                    {
                        case 1:
                            workEntity.Message = "Lumber!";
                            break;
                        case 2:
                            workEntity.Message = "Steel!";
                            break;
                        case 3:
                            workEntity.Message = "Concrete!";
                            break;
                        case 4:
                            workEntity.Message = "Bricks!";
                            break;
                        case 5:
                            workEntity.Message = "Sheetrock!";
                            break;
                    }
                    workEntity.Data = null;
                    break;
                case 2:
                    workEntity.WorkType = "Build";
                    switch (buildSteps)
                    {
                        case 1:
                            workEntity.Message = "A Chair!";
                            break;
                        case 2:
                            workEntity.Message = "A Dresser!";
                            break;
                        case 3:
                            workEntity.Message = "A Shed!";
                            break;
                        case 4:
                            workEntity.Message = "A Kitchen!";
                            break;
                        case 5:
                            workEntity.Message = "A House!";
                            break;
                    }
                    workEntity.Data = buildSteps.ToString();
                    break;
                case 3:
                    workEntity.WorkType = "Survey";
                    workEntity.Message = "Looks Good!";
                    workEntity.Data = surveyTime.ToString();
                    break;
            }
            return workEntity;
        }
    }
}
