namespace TrainingManagementPortalAPI
{
    public partial class Trainings
    {
        public int TrainingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Online { get; set; }
        public DateTime Deadline { get; set; }

        public string Departament { get; set; }

        public string Employee { get; set; }

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
            if (Departament == null)
            {
                Departament = "";
            }
            if (Employee == null)
            {
                Employee = "";
            }

        }
    }
}