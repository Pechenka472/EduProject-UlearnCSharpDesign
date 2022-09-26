using System;
using System.Collections.Generic;
using System.Linq;

namespace Incapsulation.Failures
{
    public class Common
    {
        public static int IsFailureSerious(int failureType)
        {
            if (failureType%2==0) return 1;
            return 0;
        }
        public static int Earlier(object[] v, int day, int month, int year)
        {
            int vYear = (int)v[2];
            int vMonth = (int)v[1];
            int vDay = (int)v[0];
            if (vYear < year) return 1;
            if (vYear > year) return 0;
            if (vMonth < month) return 1;
            if (vMonth > month) return 0;
            if (vDay < day) return 1;
            return 0;
        }
    }

    public class Device
    {
        public readonly int Id;
        public readonly string Name;

        public Device(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class Failure
    {
        public enum FailureType
        {
            A,
            B,
            C,
            D
        }
        
        public readonly FailureType Type;
        public readonly DateTime Date;
        public readonly Device Device;

        public Failure(FailureType type, DateTime date, Device device)
        {
            Type = type;
            Date = date;
            Device = device;
        }
        
        public bool IsFailureSerious()
        {
            return (int)Type%2==0;
        }
    }

    public class Date
    {
        public readonly DateTime DateTime;

        public Date(int day, int month, int year)
        {
            this.DateTime = new DateTime(year, month, day);
        }

        public bool IsEarlier(Date dateBeforeWhich)
        {
            var beforeYear = dateBeforeWhich.DateTime.Year;
            var beforeMonth = dateBeforeWhich.DateTime.Month;
            var beforeDay = dateBeforeWhich.DateTime.Day;
            if (DateTime.Year != beforeYear) return DateTime.Year < beforeYear;
            if (DateTime.Month != beforeMonth) return DateTime.Month < beforeMonth;
            if (DateTime.Day != beforeDay) return DateTime.Day < beforeDay;
            return false;
        }
    }

    public class ReportMaker
    {
        /// <summary>
        /// </summary>
        /// <param name="day"></param>
        /// <param name="failureTypes">
        /// 0 for unexpected shutdown, 
        /// 1 for short non-responding, 
        /// 2 for hardware failures, 
        /// 3 for connection problems
        /// </param>
        /// <param name="deviceId"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes, 
            int[] deviceId, 
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            var date = new DateTime(year, month, day);
            var failures = new List<Failure>();

            for (var i = 0; i < devices.Count; i++)
            {
                var failureType = (Failure.FailureType)failureTypes[i];
                var failuresDate = new DateTime((int)times[i][2], (int)times[i][1], (int)times[i][0]);
                var device = new Device((int)devices[i]["DeviceId"], (string)devices[i]["Name"]); 
                failures.Add(new Failure(failureType, failuresDate, device));
            }

            return FindDevicesFailedBeforeDate(date, failures);
        }

        public static List<string> FindDevicesFailedBeforeDate(DateTime date, List<Failure> failures)
        {
            var problematicDeviceNames = new HashSet<string>();
            
            foreach (Failure failure in failures)
            {
                if (failure.IsFailureSerious() && failure.Date < date)
                {
                    problematicDeviceNames.Add(failure.Device.Name);
                }
            }

            return problematicDeviceNames.ToList();
        }
    }
}
