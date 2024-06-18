using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffEasy.AppFiles
{
    class CombineData
    {
        public string FullName { get; set; }
        public string Position { get; set; }
        public int TotalWorkExperience { get; set; }
        public int TeachingExperience { get; set; }
        public List<string> Disciplines { get; set; }

        public CombineData(string fullName, string position, int totalWorkExperience, int teachingExperience, List<string> disciplines)
        {
            FullName = fullName;
            Position = position;
            TotalWorkExperience = totalWorkExperience;
            TeachingExperience = teachingExperience;
            Disciplines = disciplines;
        }
    }
}
