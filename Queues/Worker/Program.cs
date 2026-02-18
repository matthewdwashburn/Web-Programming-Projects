using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Common;
using Common.Entities;
using Newtonsoft.Json;

namespace Worker
{
    internal class Program
    {
        private static BasicAWSCredentials credentials;

        private static AmazonSQSClient sqsClient;

        static async Task Main(string[] args)
        {
            credentials = new BasicAWSCredentials(Credentials.QueueKeyId, Credentials.QueueKey);
            sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.USEast2);

            Console.WriteLine("The peasants are waiting for work...");

            await ReadMessagesAsync();
        }

        public static async Task ReadMessagesAsync()
        {
            var request = new ReceiveMessageRequest()
            {
                QueueUrl = Credentials.QueueUrl,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 10,
            };

            while (true)
            {
                var messages = await sqsClient.ReceiveMessageAsync(request);

                if (messages.Messages == null)
                {
                    continue;
                }
                //Catch any errors that happen when processing each task
                try
                {
                    foreach (var message in messages.Messages)
                    {
                        WorkEntity? workEntity = JsonConvert.DeserializeObject<WorkEntity>(message.Body);
                        //Skip if the work type is null
                        if (workEntity == null)
                        {
                            continue;
                        }

                        if (workEntity.WorkType == "Carry")
                        {
                            Console.WriteLine("I'm Carrying " + workEntity.Message);
                        }
                        if (workEntity.WorkType == "Build")
                        {
                            Console.Write("I'm Building " + workEntity.Message);
                            //Check if work data is null
                            if (workEntity.Data == null)
                            {
                                continue;
                            }
                            //If work data is greater than 0, there is still work to be done!
                            if (int.Parse((workEntity.Data)) > 0)
                            {
                                //Let console know there is still work to be done
                                int buildStepsLeft = int.Parse(workEntity.Data);
                                Console.WriteLine(" (Not Done, " + buildStepsLeft + " steps to go.)");
                                //Decrement number of steps to go
                                int decrementedBuildStepsLeft = buildStepsLeft - 1;
                                //Request the same work entity with one less step
                                workEntity.Data = decrementedBuildStepsLeft.ToString();
                                var sendMessageRequest = new SendMessageRequest()
                                {
                                    QueueUrl = Credentials.QueueUrl,
                                    MessageBody = JsonConvert.SerializeObject(workEntity)
                                };
                                await sqsClient.SendMessageAsync(sendMessageRequest);
                            }
                            else
                            {
                                Console.WriteLine("(Done!)");
                            }
                        }
                        if (workEntity.WorkType == "Survey")
                        {
                            //Check if work data is null
                            if (workEntity.Data == null)
                            {
                                continue;
                            }
                            Console.Write("Surveying the work....");
                            //Wait for surveying time
                            await Task.Delay(int.Parse(workEntity.Data));
                            //Write surveying message
                            Console.WriteLine(workEntity.Message);
                        }

                        _ = sqsClient.DeleteMessageAsync(new DeleteMessageRequest()
                        {
                            QueueUrl = Credentials.QueueUrl,
                            ReceiptHandle = message.ReceiptHandle
                        }).ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                            {
                                Console.WriteLine("Work Task Failed to Delete from the Queue!");
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    // Write out the exception that occured during the task
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
