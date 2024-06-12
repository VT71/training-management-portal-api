namespace TrainingManagementPortalAPI
{
    public partial class Trainings
    {
        
        public string Title { get; set; }
        public string Description { get; set; }
        public int Online { get; set; }
        public string Deadline { get; set; }
        
        // public string Department { get; set; }
        // public string Employee { get; set; }

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
            if(Deadline == null) 
            {
                Deadline = "";
            }
            // if (Department == null)
            // {
            //     Department = "";
            // }
            // if (Employee == null)
            // {
            //     Employee = "";
            // }

        }
    }
}