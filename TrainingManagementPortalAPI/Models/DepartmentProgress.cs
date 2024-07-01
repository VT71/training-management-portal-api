namespace TrainingManagementPortalAPI
{
    public partial class DepartmentProgress
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int TotalTrainingsCount { get; set; }
        public int TotalMissedTrainingsCount { get; set; }
        public int TotalInProgressTrainingsCount { get; set; }
        public int TotalCompletedTrainingsCount { get; set; }
        public int TotalUpcomingTrainingsCount { get; set; }

        public DepartmentProgress()
        {
            if (DepartmentName == null)
            {
                DepartmentName = "";
            }

        }
    }
}