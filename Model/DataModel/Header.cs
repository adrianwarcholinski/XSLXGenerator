using System;
using System.Globalization;
using System.Linq;

namespace Model.DataModel
{
    public class Header
    {
        private const string DATE_TIME_FORMAT = "dd.MM.yyyy HH:mm:ss";
        private const string DATE_FORMAT = "dd.MM.yyyy";

        public string Model { get; private set; }
        public string Project { get; private set; }
        public DateTime Date { get; private set; }

        public Header(string content)
        {
            bool hasTime = false;
            string dateTimeString = "";

            string[] lines = content.Split('\n');

            foreach (string line in lines)
            {
                if (line.Contains("Model") && Model == null)
                {
                    Model = line.Split("Model:").Last().Split("  ")[0].Trim();
                }

                if (line.Contains("Project") && Project == null)
                {
                    Project = line.Split("Project:").Last().Trim();
                }

                if (line.Contains("Date"))
                {
                    string dateString = line.Split("Date:").Last().Trim();
                    dateTimeString = dateString;
                }

                if (line.Contains("Time"))
                {
                    string timeString = line.Split("Time:").Last().Trim();
                    dateTimeString += " " + timeString;
                    hasTime = true;
                }
            }

            Date = DateTime.ParseExact(dateTimeString, hasTime ? DATE_TIME_FORMAT : DATE_FORMAT, CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            return $"Model: {Model};Project: {Project}";
        }
    }
}