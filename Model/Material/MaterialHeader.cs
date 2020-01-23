using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace Model.Material
{
    public class MaterialHeader
    {
        private const string DATE_TIME_FORMAT = "dd.MM.yyyy HH:mm:ss";

        public string Model { get; private set; }
        public string Project { get; private set; }
        public DateTime Date { get; private set; }

        public MaterialHeader(string[] lines)
        {
            string dateTimeString = "";

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
                }
            }

            Date = DateTime.ParseExact(dateTimeString, DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            return $"Model: {Model};Project: {Project}";
        }
    }
}