using FinalExam.Models;

namespace FinalExam.ViewModels
{
    public class HomeVM
    {
        public List<Employee> Employees { get; set; }
        public Dictionary<string,string> Settings { get; set; }
    }
}
