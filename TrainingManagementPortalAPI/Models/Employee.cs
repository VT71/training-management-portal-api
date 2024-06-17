namespace TrainingManagementPortalAPI
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public int Trainer { get; set; }
        public string UserId { get; set; }
        public int DepartmentId { get; set; }

        public Employee()
        {
            if (UserId == "")
            {
                UserId = "";
            }
        }
    }
}