namespace TrainingManagementPortalAPI
{
    public partial class Trainings
    {
        public int TrainingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Individual { get; set; }
        public string Adress { get; set; }
        public DateTime Deadline { get; set; }
        public int Trainer { get; set; }
        public int ForDepartments { get; set; }
        public int ForEmployees { get; set; }
        public string Title1 { get; set; }
        public string Description1 { get; set; }


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
            if (Adress == null)
            {
                Adress = "";
            }
            if (Title1 == null)
            {
                Title1 = "";
            }
            if (Description1 == null)
            {
                Description1 = "";
            }


        }
    }
}