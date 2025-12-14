using CampLib.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampLib.Repository
{
    public class UserRepository
    {
        private static readonly List<User> _users = new();
        private static int _nextId = 1;

        public Task<List<User>> GetAllAsync()
        {
            return Task.FromResult(_users);
        }

        public Task<User?> GetByIdAsync(int id)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
        }

        public Task<User> AddAsync(User user)
        {
            if (!user.Email.EndsWith("@edu.zealand.dk"))
                throw new ArgumentException("Email skal ende på @edu.zealand.dk");

            user.Id = _nextId++;
            _users.Add(user);
            return Task.FromResult(user);
        }

        public Task<User?> UpdateAsync(int id, User updated)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return Task.FromResult<User?>(null);

            if (!updated.Email.EndsWith("@edu.zealand.dk"))
                throw new ArgumentException("Email skal ende på @edu.zealand.dk");

            user.Email = updated.Email;

            return Task.FromResult(user);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return Task.FromResult(false);

            _users.Remove(user);
            return Task.FromResult(true);
        }
    }
}