namespace TrainingManagementPortalAPI
{
    public partial class User
    {
        public string UserId {get; set;}
        public string FullName {get; set;}
        public string Email {get; set;}
        public string Role {get; set;}

        public User()
        {
            if (UserId == null)
            {
                UserId = "";
            }
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