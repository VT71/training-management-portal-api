namespace TrainingManagementPortalAPI
{
    public partial class User
    {
        public int UserId {get; set;}
        public string FullName {get; set;}
        public string Email {get; set;}
        public string Role {get; set;}

        public User()
        {
            if (FullName == null)
            {
                FullName = "";
            }
            if (Email == null)
            {
                Email = "";
            }
            if (Role == null)
            {
                Role  = "";
            }
        }
    }
}