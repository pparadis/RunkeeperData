using System.IO;
using HealthGraphNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Azure.WebJobs;

namespace RunkeeperData
{
    public class HowManyIRanJob
    {
        static void Main(string[] args)
        {
            JobHost host = new JobHost();
            host.RunAndBlock();
        }

        public static void Calculate([Blob("output/{name}")] TextWriter output)
        {
            var tokenManager = new AccessTokenManager("7303a9bd039f4484af3fd83d5f60d72c", ConfigurationManager.AppSettings["ClientSecret"], "https://www.frenchcoding.com/", ConfigurationManager.AppSettings["AccessToken"]);
            var userRequest = new UsersEndpoint(tokenManager);
            var user = userRequest.GetUser();

            var activitiesRequest = new FitnessActivitiesEndpoint(tokenManager, user);
            var activities = activitiesRequest.GetFeedPage(null, null, new DateTime(2013, 04, 01), DateTime.Now);

            var totalDistance = activities.Items.Sum(activity => activity.TotalDistance);

            //save total distance
        }
    }
}
