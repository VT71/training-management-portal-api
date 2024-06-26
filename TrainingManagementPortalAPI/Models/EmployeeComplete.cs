namespace TrainingManagementPortalAPI
{
    public partial class EmployeeComplete
    {
        public int EmployeeId { get; set; }
        public int Trainer { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }

        public EmployeeComplete()
        {
            if (UserId == null)
            {
                UserId = "";
            }
            if (Email == null)
            {
                Email = "";
            }
            if (FullName == null)
            {
                FullName = "";
            }
            if (DepartmentName == null)
            {
                DepartmentName = "";
            }
        }
    }
}