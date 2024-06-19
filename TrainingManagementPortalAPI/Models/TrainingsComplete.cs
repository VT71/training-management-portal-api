namespace TrainingManagementPortalAPI
{
    public partial class TrainingsComplete
    {
        public int TrainingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Individual { get; set; }
        public string Adress { get; set;}
        public DateTime Deadline { get; set; }
        public int Trainer { get; set; }
        public int ForDepartments { get; set; }
        public int ForEmployees { get; set; }
        public List<int> Departments { get; set; }  // Schimbat de la int la List<int>
        public List<int> Employees { get; set; }


      
        public TrainingsComplete()
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
            
          
            Departments = new List<int>();
            Employees = new List<int>();

        }
    }
}