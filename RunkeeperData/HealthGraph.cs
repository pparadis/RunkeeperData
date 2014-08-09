using System.Configuration;
using HealthGraphNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RunkeeperData
{
    public class HealthGraph
    {
        static void Main(string[] args)
        {
            var tokenManager = new AccessTokenManager("7303a9bd039f4484af3fd83d5f60d72c", ConfigurationManager.AppSettings["ClientSecret"], "https://www.frenchcoding.com/", ConfigurationManager.AppSettings["AccessToken"]);
            var userRequest = new UsersEndpoint(tokenManager);
            var user = userRequest.GetUser();

            var activitiesRequest = new FitnessActivitiesEndpoint(tokenManager, user);
            var activities = activitiesRequest.GetFeedPage(null, 500, new DateTime(2014, 07, 20), DateTime.Now);

            var totalDistance = 0.0;
            var lines = new List<string>();
            foreach (var activity in activities.Items.OrderBy(p => p.StartTime))
            {
                totalDistance += activity.TotalDistance;
                var date = activity.StartTime;
                var distance = activity.TotalDistance;
                lines.Add(date.ToShortDateString() + " / " + ToKilometers(distance));
            }

            System.IO.File.WriteAllLines("runs.txt", lines);
            Console.WriteLine("Distance totale : " + ToKilometers(totalDistance));
            Console.ReadKey();
        }

        private static double ToKilometers(double distance)
        {
            return Math.Round(distance / 1000, 2);
        }
    }
}