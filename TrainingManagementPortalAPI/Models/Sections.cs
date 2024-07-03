namespace TrainingManagementPortalAPI
{
    public partial class Sections
    {
        public int SectionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }


        public Sections()
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