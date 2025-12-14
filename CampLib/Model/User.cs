namespace CampLib.Model
{
    public class User
    {
        public int Id { get; set; }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (!value.EndsWith("@edu.zealand.dk"))
                    throw new ArgumentException("Email skal ende på '@edu.zealand.dk'.");
                _email = value;
            }
        }

        // Constructor (til API POST)
        public User() { }

        // Constructor
        public User(string email)
        {
            Email = email;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Email: {Email}";
        }
    }
}