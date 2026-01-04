namespace CampLib.Model
{
    public class Staff
    {
        public int Id { get; set; }


        public string Username { get; set; } 


        public string Password { get; set; }
        
        public Staff() { }

        public Staff(int id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }
    }
}