
using FBLA_project;
using System.Security.Cryptography;

namespace FBLA_project
{
    public class UserService
    {
        private string _path;
        public UserService(string path) { _path = path; }
        public User? AuthenticateUser(string username, string password)
        {
            var userList = new JsonUtil<List<User>>(_path);
            foreach (var user in userList.Access())
            {
                if (user is not null)
                {
                    if (user.Username == username && user.Password != password)
                    {
                        throw new Exception("User does not exist");
                    }
                    if (user.Username == username && user.Password == password)
                    {
                        return user;
                    }
                }
            }
            return null;
        }


        public string GenerateSessionToken(User user)
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return $"{user.Id}:{Convert.ToBase64String(randomBytes)}";
        }

        public int GenerateUserId()
        {
            var randomBytes = new byte[2];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToInt16(randomBytes);

        }
    }
}