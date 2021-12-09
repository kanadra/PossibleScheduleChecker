using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PossibleScheduleChecker
{
    internal class Program
    {
        static public List<Course> _courses;

        static void Main(string[] args)
        {
            _courses = new List<Course>();
            Console.WriteLine("Program initialized, awaiting input. Type \"HELP\" for a list of commands");
            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "HELP":
                        Console.WriteLine("\"VIEWALL\":   View all currently added courses, professors and schedules");
                        Console.WriteLine("\"ADDCOURSE\": Adds a new course, a course can contain various professors");
                        Console.WriteLine("\"DELCOURSE\": Removes a course");
                        Console.WriteLine("\"ADDPROF\":   Adds a new professor to a course, each professor has a schedule for his course");
                        Console.WriteLine("\"DELPROF\":   Removes a professor");
                        Console.WriteLine("\"ADDCLASS\":  Adds a new class to a professors schedule");
                        Console.WriteLine("\"DELCLASS\":  Removes a class");
                        Console.WriteLine("\"RESET\":     Clears all courses, professors and their schedules");
                        Console.WriteLine("\"START\":     Starts the algorythm to find all possible answers");
                        Console.WriteLine("\"HELP\":      You are already using this one dumdum :)");
                        break;
                    case "VIEWALL":
                        ViewAll();
                        break;
                    case "ADDCOURSE":
                        AddCourse();
                        break;
                    case "DELCOURSE":
                        DelCourse();
                        break;
                    case "ADDPROF":
                        AddProf();
                        break;
                    case "DELPROF":
                        DelProf();
                        break;
                    case "ADDCLASS":
                        AddClass();
                        break;
                    case "DELCLASS":
                        DelClass();
                        break;
                    case "RESET":
                        _courses = new List<Course>();
                        break;
                    case "START":
                        Calculate();
                        break;
                    default:
                        Console.WriteLine("Unrecognized Command");
                        break;
                }

            }
        }

        static void Calculate()
        {
            var answers = new List<Answer>();
            var listCorrect = new List<Answer>();
            var listProf = new List<Professor>();
            var listInt = new List<int>();
            var listCount = new List<int>();
            foreach (var course in _courses)
            {
                listInt.Add(course.Professors.Count - 1);
                listCount.Add(0);
            }
            var an = new Answer() { Professors = new List<Professor>() };
            for (int i = 0; i < _courses.Count; i++)
            {
                an.Professors.Add(_courses[i].Professors[listCount[i]]);
            }
            answers.Add(an);
            while (listCount.Sum() < listInt.Sum())
            {
                for (int i = 0; i < listInt.Count; i++)
                {
                    if (listCount[i] < listInt[i])
                    {
                        listCount[i]++;
                        break;
                    }
                    else
                    {
                        listCount[i] = 0;
                    }
                }
                var ans = new Answer() { Professors = new List<Professor>() };
                for (int i = 0; i < _courses.Count; i++)
                {
                    ans.Professors.Add(_courses[i].Professors[listCount[i]]);
                }
                answers.Add(ans);
            }
            foreach (var answer in answers)
            {
                var correct = true;
                var listSchedule = new List<Schedule>();
                foreach (var prof in answer.Professors)
                {
                    foreach (var schedule in prof.Schedule)
                    {
                        listSchedule.Add(schedule);
                    }
                }
                foreach (var schedule in listSchedule)
                {
                    if (listSchedule.Exists(a => 
                        a.ScheduleStart < schedule.ScheduleEnd && 
                        a.ScheduleEnd > schedule.ScheduleStart && 
                        a.Weekday == schedule.Weekday &&
                        a != schedule))
                    {
                        correct = false;
                    }
                }
                if (correct)
                {
                    listCorrect.Add(answer);
                }
            }
            Console.WriteLine("Analized " + answers.Count + " possible schedules and produced " + listCorrect.Count + " possible schedules");
            int sol = 1;
            foreach (var corr in listCorrect)
            {
                Console.WriteLine("Solution " + sol + "/" + listCorrect.Count + ":");
                for (int i = 0; i < _courses.Count; i++)
                {
                    Console.WriteLine("  " + _courses[i].Name + " with " + corr.Professors[i].Name);
                    foreach (var schedule in corr.Professors[i].Schedule)
                    {
                        Console.WriteLine("    " + schedule.Weekday + " " + schedule.ScheduleStart.TimeOfDay.ToString().Remove(5) + " - " + schedule.ScheduleEnd.TimeOfDay.ToString().Remove(5));
                    }
                }
                sol++;
                Console.WriteLine("See next? y/n");
                if (Console.ReadLine() == "n")
                {
                    return;
                }
            }
        }

        static void ViewAll()
        {
            foreach(var course in _courses)
            {
                var cs = "Course:    ";
                var ps = "Professor: ";
                var ss = "Class:     ";
                Console.WriteLine(cs + course.Name);
                foreach(var professor in course.Professors)
                {
                    Console.WriteLine(ps + "  " + professor.Name);
                    foreach(var schedule in professor.Schedule)
                    {
                        Console.WriteLine(ss + "    " + schedule.Weekday + " " + schedule.ScheduleStart.TimeOfDay.ToString().Remove(5) + " - " + schedule.ScheduleEnd.TimeOfDay.ToString().Remove(5));
                    }
                }
            }
        }

        static void AddCourse()
        {
            Console.WriteLine("Adding new course, currently added courses are:");
            foreach(var course in _courses)
            {
                Console.WriteLine(course.Name);
            }
            Console.WriteLine("Input Course Name");
           _courses.Add(new Course() { Name = Console.ReadLine(), Professors = new List<Professor>()});
            Console.WriteLine("Course Added!");
        }

        static void DelCourse()
        {
            Console.WriteLine("Select a course:");
            for (int i = 0; i < _courses.Count; i++)
            {
                Console.WriteLine("(" + (i + 1) + ") " + _courses[i].Name);
            }
            var input = 0;
            try
            {
                input = Convert.ToInt32(Console.ReadLine()) - 1;
                _courses.RemoveAt(input);
            }
            catch
            {
                Console.WriteLine("Not found, returning to menu");
                return;
            }
        }

        static void AddProf()
        {
            if (_courses.Count == 0)
            {
                Console.WriteLine("No courses found");
                return;
            }
            Console.WriteLine("Select a course:");
            for (int i = 0; i < _courses.Count; i++)
            {
                Console.WriteLine("(" + (i + 1) + ") " + _courses[i].Name);
            }
            var input = 0;
            try
            {
                input = Convert.ToInt32(Console.ReadLine()) - 1;
                var test = _courses[input].Professors.Count == 0;
            }
            catch
            {
                Console.WriteLine("Not found, returning to menu");
                return;
            }
            Console.WriteLine("Select a professor");
            foreach(var prof in _courses[input].Professors)
            {
                Console.WriteLine(prof.Name);
            }
            // TODO: ADD LIST OF PROFS
            Console.WriteLine("Please type the name of the professor to add:");
            _courses[input].Professors.Add(new Professor() { Name = Console.ReadLine(), Schedule = new List<Schedule>() });
            Console.WriteLine("Added professor");
        }

        static void DelProf()
        {
            Console.WriteLine("Select a course:");
            for (int i = 0; i < _courses.Count; i++)
            {
                Console.WriteLine("(" + (i + 1) + ") " + _courses[i].Name);
            }
            var input = 0;
            try
            {
                input = Convert.ToInt32(Console.ReadLine()) - 1;
                var test = _courses[input];
            }
            catch
            {
                Console.WriteLine("Not found, returning to menu");
                return;
            }
            var listprofs = _courses[input].Professors;
            Console.WriteLine("Select a professor:");
            for (int i = 0; i < listprofs.Count; i++)
            {
                Console.WriteLine("(" + (i + 1) + ") " + listprofs[i].Name);
            }
            var input2 = 0;
            try
            {
                input2 = Convert.ToInt32(Console.ReadLine()) - 1;
                _courses[input].Professors.RemoveAt(input2);
            }
            catch
            {
                Console.WriteLine("Not found, returning to menu");
                return;
            }
        }

        static void AddClass()
        {
            if (_courses.Count == 0)
            {
                Console.WriteLine("No courses found");
                return;
            }
            Console.WriteLine("Select a course:");
            for (int i = 0; i < _courses.Count; i++)
            {
                Console.WriteLine("(" + (i + 1) + ") " + _courses[i].Name);
            }
            var input = 0;
            try
            {
                input = Convert.ToInt32(Console.ReadLine()) - 1;
                if (_courses[input].Professors.Count == 0)
                {
                    Console.WriteLine("Course has no professors, returning to menu");
                    return;
                }
            }
            catch
            {
                Console.WriteLine("Not found, returning to menu");
                return;
            }
            var listprofs = _courses[input].Professors;
            Console.WriteLine("Select a professor:");
            for (int i = 0; i < listprofs.Count; i++)
            {
                Console.WriteLine("(" + (i + 1) + ") " + listprofs[i].Name);
            }
            var input2 = 0;
            try
            {
                input2 = Convert.ToInt32(Console.ReadLine()) - 1;
                var test = listprofs[input2];
            }
            catch
            {
                Console.WriteLine("Not found, returning to menu");
                return;
            }
            Console.WriteLine("Select a day of the week:");
            var listDays = new List<string>() 
            { 
                "Monday   ", 
                "Tuesday  ", 
                "Wednesday", 
                "Thursday ", 
                "Friday   ", 
                "Saturday ", 
                "Sunday   " 
            };
            for (int i = 0; i < listDays.Count; i++)
            {
                Console.WriteLine("(" + (i + 1) + ") " + listDays[i]);
            }
            var dow = 0;
            try
            {
                dow = Convert.ToInt32(Console.ReadLine()) - 1;
            }
            catch
            {
                Console.WriteLine("Invalid, returning to menu");
                return;
            }
            Console.WriteLine("Type class start time (\"HH:MM\") 24hr format:");
            var cs = Console.ReadLine();
            DateTime csd = new DateTime(0);
            try
            {
                csd = csd.AddHours(Convert.ToInt32(cs.Remove(2)));
                csd = csd.AddMinutes(Convert.ToInt32(cs.Remove(0, 3)));
            }
            catch
            {
                Console.WriteLine("Invalid, returning to menu");
                return;
            }
            Console.WriteLine("Type class end time (\"HH:MM\") 24hr format:");
            var ce = Console.ReadLine();
            DateTime ced = new DateTime(0);
            try
            {
                ced = ced.AddHours(Convert.ToInt32(ce.Remove(2)));
                ced = ced.AddMinutes(Convert.ToInt32(ce.Remove(0, 3)));
            }
            catch
            {
                Console.WriteLine("Invalid, returning to menu");
                return;
            }
            _courses[input].Professors[input2].Schedule.Add(new Schedule() { Weekday = listDays[dow], ScheduleStart = csd, ScheduleEnd = ced });
            Console.WriteLine("Added class");
        }

        static void DelClass()
        {
            Console.WriteLine("Select a course:");
            for (int i = 0; i < _courses.Count; i++)
            {
                Console.WriteLine("(" + (i + 1) + ") " + _courses[i].Name);
            }
            var input = 0;
            try
            {
                input = Convert.ToInt32(Console.ReadLine()) - 1;
                var test = _courses[input];
            }
            catch
            {
                Console.WriteLine("Not found, returning to menu");
                return;
            }
            var listprofs = _courses[input].Professors;
            Console.WriteLine("Select a professor:");
            for (int i = 0; i < listprofs.Count; i++)
            {
                Console.WriteLine("(" + (i + 1) + ") " + listprofs[i].Name);
            }
            var input2 = 0;
            try
            {
                input2 = Convert.ToInt32(Console.ReadLine()) - 1;
                var test = _courses[input].Professors[input2];
            }
            catch
            {
                Console.WriteLine("Not found, returning to menu");
                return;
            }
            var listClasses = _courses[input].Professors[input2].Schedule;
            Console.WriteLine("Select a professor:");
            for (int i = 0; i < listprofs.Count; i++)
            {
                Console.WriteLine("(" + (i + 1) + ") " + listClasses[i].Weekday + listClasses[i].ScheduleStart.TimeOfDay.ToString().Remove(5) + " - " + listClasses[i].ScheduleEnd.TimeOfDay.ToString().Remove(5));
            }
            var input3 = 0;
            try
            {
                input3 = Convert.ToInt32(Console.ReadLine()) - 1;
                _courses[input].Professors[input2].Schedule.RemoveAt(input3);
            }
            catch
            {
                Console.WriteLine("Not found, returning to menu");
                return;
            }
        }
    }
    public class Course
    {
        public string Name;

        public List<Professor> Professors;
    }

    public class Professor
    {
        public string Name;

        public List<Schedule> Schedule;
    }

    public class Schedule
    {
        public DateTime ScheduleStart;

        public DateTime ScheduleEnd;

        public string Weekday;
    }

    public class Answer
    {
        public List<Professor> Professors;
    }
}
