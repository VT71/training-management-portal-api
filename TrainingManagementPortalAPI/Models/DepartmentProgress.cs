namespace TrainingManagementPortalAPI
{
    public partial class DepartmentProgress
    {
        public string DepartmentName { get; set; }
        public decimal DepartmentCompletionRate { get; set; }

        public DepartmentProgress()
        {
            if (DepartmentName == null)
            {
                DepartmentName = "";
            }

        }
    }
}