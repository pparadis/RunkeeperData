using System.Threading.Tasks;
using HealthGraphNet;
using Microsoft.Azure.WebJobs;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;


namespace RunkeeperData
{
    public class HowManyIRanJob
    {
        static void Main(string[] args)
        {
            var host = new JobHost();
            Task callTask = host.CallAsync(typeof(HowManyIRanJob).GetMethod("Calculate"), new { value = 20 });

            Console.WriteLine("Waiting for async operation...");
            callTask.Wait();
            Console.WriteLine("Task completed: " + callTask.Status);
        }

        [NoAutomaticTrigger]
        public static void Calculate([Blob("runs/{name}")] TextWriter output)
        {
            var tokenManager = new AccessTokenManager("7303a9bd039f4484af3fd83d5f60d72c", ConfigurationManager.AppSettings["ClientSecret"], "https://www.frenchcoding.com/", ConfigurationManager.AppSettings["AccessToken"]);
            var userRequest = new UsersEndpoint(tokenManager);
            var user = userRequest.GetUser();

            var activitiesRequest = new FitnessActivitiesEndpoint(tokenManager, user);
            var activities = activitiesRequest.GetFeedPage(null, null, new DateTime(2013, 04, 01), DateTime.Now);

            var totalDistance = activities.Items.Sum(activity => activity.TotalDistance);
            Console.WriteLine("Total Distance: " + totalDistance);
            output.Write(totalDistance);
        }
    }
}
