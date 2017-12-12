using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerkyTemp.Models;
using static PerkyTemp.Models.CurrentSession;
using System.Diagnostics;

namespace UnitTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing linear regression...");
            double m, b;

            LinearRegression(new List<KeyValuePair<double, double>> {
                new KeyValuePair<double, double>(1, 3),
                new KeyValuePair<double, double>(2, 6)
            }, out m, out b);
            Debug.Assert(m == 3);
            Debug.Assert(b == 0);

            LinearRegression(new List<KeyValuePair<double, double>> {
                new KeyValuePair<double, double>(1, 4),
                new KeyValuePair<double, double>(2, 7)
            }, out m, out b);
            Debug.Assert(m == 3);
            Debug.Assert(b == 1);

            LinearRegression(new List<KeyValuePair<double, double>> {
                new KeyValuePair<double, double>(-1, 6),
                new KeyValuePair<double, double>(5, 3.5),
                new KeyValuePair<double, double>(2, 4),
                new KeyValuePair<double, double>(8, 1)
                //{-1,6},{5,3.5},{2,4},{8,1}
            }, out m, out b);
            Console.WriteLine("m = {0}; b = {1}", m, b);
            Debug.Assert(Math.Abs(m - (-0.516667)) < 0.1);
            Debug.Assert(Math.Abs(b - 5.43333) < 0.1);





            CurrentSession s = new CurrentSession();

            Console.WriteLine("Setting vest threshold to 5 via mocked settings...");
            SettingsModel settings = SettingsModel.NewSettingsModel();
            settings.TemperatureThreshold = 5;
            PerkyTempDatabase.Database.MockedSettingsForTesting = settings;

            Console.WriteLine("Sending temp reading of 1, waiting 1s...");
            s.RecordTempReading(1);
            System.Threading.Thread.Sleep(1000);

            Console.WriteLine("Sending temp reading of 2, waiting 1s...");
            s.RecordTempReading(2);
            System.Threading.Thread.Sleep(1000);

            Console.WriteLine("Sending temp reading of 3...");
            s.RecordTempReading(3);

            Console.WriteLine("How much longer til temp reading of 6?");
            Console.WriteLine(s.GetTimeRemainingSec());

            Console.ReadKey();
        }
    }
}
