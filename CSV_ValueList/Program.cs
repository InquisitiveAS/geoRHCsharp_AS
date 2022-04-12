using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSV_ValueList
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] rawcsv = System.IO.File.ReadAllLines(@"TLINECON 21-1005 CSV.csv");

            var employeeid = new List<string>();//0
            var paycode = new List<string>();//4
            var workhours = new List<double>();//6

            List<(string EmpID, string paycode, double workHours)> employeedata_vtuples_list = new List<(string, string , double)>();

            for(int i = 1; i < rawcsv.Length; i++)
            {
                string[] employeedataSplitData = rawcsv[i].Split(',');

                double castedworkhours = Convert.ToDouble(employeedataSplitData[6]);

                employeedata_vtuples_list.Add((employeedataSplitData[0], employeedataSplitData[4], castedworkhours));
            }

            Console.WriteLine("Original EmployeeList");
            for (int v = 0; v < employeedata_vtuples_list.Count; v++)
            {
                
                Console.WriteLine(employeedata_vtuples_list[v]);
               
            }

            Console.WriteLine("\n\r");

            Console.WriteLine("Processed EmployeeList");

            //Creating a new List for employee data
            var revisedemployeeData = from @record in employeedata_vtuples_list 
                                      group @record by @record.EmpID into employeeWorkhourRecords 
                                      let totalworkHours = employeeWorkhourRecords.Sum(r => r.workHours) 
                                      let ratio = totalworkHours < 40 ? 1 : 40 / totalworkHours 
                                      from employeeiD in employeeWorkhourRecords 
                                      select (employeeiD.EmpID, employeeiD.paycode, employeeiD.workHours * ratio);

            foreach (var item in revisedemployeeData) Console.WriteLine(item);

            Console.ReadKey();
        }
    }
}
