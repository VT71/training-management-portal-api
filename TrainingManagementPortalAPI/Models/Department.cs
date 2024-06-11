namespace TrainingManagementPortalAPI
{
    public partial class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public Department()
        {
            if (DepartmentName == null)
            {
                DepartmentName = "";
            }

        }
    }
}