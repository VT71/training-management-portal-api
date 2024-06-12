namespace TrainingManagementPortalAPI
{
    public partial class Trainings
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public int Online { get; set; }
        public DateTime Deadline { get; set; }

        public int ForEmployees { get; set; }

        public int ForDepartments { get; set; }

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

        }
    }
}