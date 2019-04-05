using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrimeAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            //Check if inputs are valid
            //
            if (args.Length != 2)
            {
              
                Console.WriteLine("Incorrect number of file paths given. Program should be run:\nCrimeAnalyzer <crime_csv_file_path> <report_file_path>");
                return;
            }

            string filePath = args[0];
            string outputFile = args[1];

            foreach (string input in args)
            {
                bool doc1 = IsValid(input);
                if (doc1 is false)
                {
                    return;
                }
                bool doc2 = Readable(input);
                if (doc2 is false)
                {
                    return;
                }
            }

            bool doc3 = Writable(outputFile);
            if (doc3 is false)
            {
                return;
            }

            //
            //Read from .csv file and save to list of instances of class CrimeStats
            //
            List<CrimeStats> crimeStatsList = new List<CrimeStats>();

            StreamReader reader = new StreamReader(filePath);
            try
            {
                reader.ReadLine();
                while (reader.Peek() != -1)
                {
                    var line = reader.ReadLine();
                    var values = Array.ConvertAll(line.Split(','), int.Parse).ToList();
                    if (values.Count != 11)
                    {
                        return;
                    }
                    CrimeStats a = new CrimeStats { year = values[0], population = values[1], violentCrime = values[2], murder = values[3], rape = values[4], robbery = values[5], aggravatedAssault = values[6], propertyCrime = values[7], burglary = values[8], theft = values[9], motorVehicleTheft = values[10] };
                    crimeStatsList.Add(a);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }


            //
            //Answer questions using information from .csv file
            //
            string content = "";


            //Questions 1 and 2
            var years = from a in crimeStatsList
                        select a.year;

            int num = years.Count();
            int range1 = years.First();
            int range2 = years.Last();

            content = content + "Period: " + range1 + " - " + range2 + " (" + num + " years)\n\n";


            //Question 3
            content = content + "Years murders per year < 15000: ";

            var murders = from b in crimeStatsList
                          where b.murder < 15000
                          select b.year;

            int num1 = murders.Count();
            int numb = 0;

            foreach (var item in murders)
            {
                content = content + item;
                if (numb < num1-1)
                {
                    content = content + ", ";
                }
                else
                {
                    content = content + "\n";
                }
                numb++;
            }


            //Question 4 
            content = content + "Robberies per year > 500000: ";

            var robberies = from c in crimeStatsList
                            where c.robbery > 500000
                            select new { z = c.year, y = c.robbery };

            int num2 = robberies.Count();
            int numc = 0;

            foreach (var yr in robberies)
            {
                content = content + yr.z + " = " + yr.y;
                if (numc < num2-1)
                {
                    content = content + ", ";
                }
                else
                {
                    content = content + "\n";
                }
                numc++;
            }


            //Question 5
            content = content + "Violent crime per capita rate (2010): ";

            var vCrime = from d in crimeStatsList
                         where d.year == 2010
                         select new { x = d.violentCrime, w = d.population };

            foreach (var crime in vCrime)
            {
                double perCapita = (double)crime.x / crime.w;
                content = content + perCapita + "\n";
            }


            //Question 6
            var avgMurders1 = from e in crimeStatsList
                              select e.murder;
            var avg1 = avgMurders1.Average();

            content = content + "Average murders per year (all years): " + avg1 + "\n";


            //Question 7
            var avgMurders2 = from f in crimeStatsList
                              where f.year >= 1994 && f.year <= 1997
                              select f.murder;

            var avg2 = avgMurders2.Average();

            content = content + "Average murders per year (1994-1997): " + avg2 + "\n";


            //Question 8
            var avgMurders3 = from g in crimeStatsList
                              where g.year >= 2010 && g.year <= 2013
                              select g.murder;

            var avg3 = avgMurders3.Average();

            content = content + "Average murders per year (2010-2013): " + avg3 + "\n";


            //Questions 9 and 10
            var numThefts = from h in crimeStatsList
                            where h.year <= 2004 && h.year >= 1999
                            select h.theft;

            var maxThefts = numThefts.Max();
            var minThefts = numThefts.Min();

            content = content + "Minimum thefts per year (1999-2004): " + minThefts + "\n";
            content = content + "Maximum thefts per year (1999-2004): " + maxThefts + "\n";


            //Question 11
            var motorThefts = from i in crimeStatsList
                        //orderby i
                        select i.year;

            int motor = motorThefts.First();

            content = content + "Year of highest number of motor vehicle thefts: " + motor + "\n";


            //
            //Write content to output file
            //
            StreamWriter writer = new StreamWriter(outputFile);
            try
            {
                writer.WriteLine(content);
                Console.WriteLine("{0} was successfully saved.", outputFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        static bool IsValid(string doc)
        {
            if (File.Exists(doc) is false)
            {
                Console.WriteLine("File {0} does not exist", doc);
                return false;
            }
            return true;
        }

        static bool Readable(string doc)
        {
            using (FileStream fs = new FileStream(doc, FileMode.Open))
            {
                if (!fs.CanRead)
                {
                    Console.WriteLine("File {0} cannot be opened.", doc);
                    return false;
                }
                return true;
            }
        }

        static bool Writable(string doc)
        {
            using (FileStream fs = new FileStream(doc, FileMode.Open))
            {
                if (!fs.CanWrite)
                {
                    Console.WriteLine("File {0} cannot be written to.", doc);
                    return false;
                }
                return true;
            }
        }
    }

    public class CrimeStats
    {
        public int year;
        public int population;
        public int violentCrime;
        public int murder;
        public int rape;
        public int robbery;
        public int aggravatedAssault;
        public int propertyCrime;
        public int burglary;
        public int theft;
        public int motorVehicleTheft;
    }

}
