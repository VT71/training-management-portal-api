namespace TrainingManagementPortalAPI
{
    public partial class Trainings
    {
        public int TrainingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Online { get; set; }
        public DateTime Deadline { get; set; }
        public bool forDepartments { get; set; }
        public bool forEmployees { get; set; }

        public List<int> Departments { get; set; } // Departamentele selectate
        public List<int> Employees { get; set; } // Angajații selectați

        public Trainings()
        {

            if (Title == null)
            {
                Title = "";
            }
            if (Description == null)
            {
                Description = "";
            }
            Departments = new List<int>();
            Employees = new List<int>();

        }
    }
}