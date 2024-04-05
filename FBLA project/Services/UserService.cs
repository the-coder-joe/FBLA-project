using Microsoft.AspNetCore.DataProtection;
using System.Diagnostics.Eventing.Reader;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text.Json;

namespace FBLA_project
{
    public static class UserService
    {

        private static readonly string _path = @".\Private\Users.txt";
        private static readonly IDataProtectionProvider _dataProtectionProvider;
        private static readonly string _sessionTokenKey = "Session Token Key";
        private static readonly string _userStoragekey = "User Storage Key";

        private static readonly IDataProtector _sessionTokenProtector;
        private static readonly IDataProtector _userProtector;

        static UserService()
        {
            _dataProtectionProvider = DataProtectionProvider.Create("ApplicationName");
            _sessionTokenProtector = _dataProtectionProvider.CreateProtector(_sessionTokenKey);
            _userProtector = _dataProtectionProvider.CreateProtector(_userStoragekey);
        }
        public static User? AuthenticateUser(string username, string password)
        {
            List<User>? userList = getUsers();
            if (userList is null) { return null; }
            foreach (User user in userList)
            {
                if (user.UnprotectedInfo.Username == username && user.ProtectedInfo.Password != password)
                {
                    throw new Exception("User does not exist");
                }
                if (user.UnprotectedInfo.Username == username && user.ProtectedInfo.Password == password)
                {
                    return user;
                }
            }
            return null;
        }

        public static string GenerateSessionToken(User user)
        {
            string userData = JsonSerializer.Serialize<User>(user);

            string token = _sessionTokenProtector.Protect(userData);

            return token;
        }

        public static User? GetUserFromHttpContext(HttpContext httpContext)
        {
            string? token = httpContext.Session.GetString("SessionToken");

            if (token == null)
            {
                return null;
            }

            string userData = _sessionTokenProtector.Unprotect(token);

            User? user = JsonSerializer.Deserialize<User>(userData);

            if (user is null)
            {
                return null;
            }

            return user;
        }

        public static void CreateNewUser(ProtectedData protectedData, UnprotectedData unprotected)
        {

            User user = new User
            {
                ProtectedInfo = protectedData,
                UnprotectedInfo = unprotected,
                Id = GenerateUserId()
            };


            List<User> users = getUsers() ?? new List<User>();
            users.Add(user);
            setUsers(users);
        }

        public static void ModifyUser(int userId, User newUser)
        {
            List<User>? users = getUsers();
            if(users is null)
            {
                return;
            }
            for (int i = 0; i < users.Count; i++)
            {
                User user = users[i];
                if(user.Id == userId)
                {
                    users[i] = newUser;
                    setUsers(users);
                    return;
                }
            }
        }

        public static int? GetUserIdByUsername(string username)
        {
            var users = getUsers();
            if (users == null) return null;
            foreach (var user in users)
            {
                if (user.UnprotectedInfo.Username == username) { return user.Id; }
            }
            return null;
        }

        public static User? GetUserById(int id)
        {
            var users = getUsers();
            foreach (User user in users)
            {
                if (user.Id == id)
                {
                    return user;
                }
            }
            return null;
        }
        private static List<User>? getUsers()
        {
            string encrptedUserData = File.ReadAllText(_path);
            if (string.IsNullOrEmpty(encrptedUserData)) { return null; }
            //    string rawUserData = _userProtector.Unprotect(encrptedUserData);
            var rawUserData = encrptedUserData;
            List<User>? users = JsonSerializer.Deserialize<List<User>>(rawUserData);

            return users;
        }

        private static void setUsers(List<User> users)
        {
            string rawUserData = JsonSerializer.Serialize(users);
           // string encriptedUserData = _userProtector.Protect(rawUserData);
            var encriptedUserData = rawUserData;
            File.WriteAllText(_path, encriptedUserData);

        }
        private static int GenerateUserId()
        {
            byte[] randomBytes = new byte[sizeof(Int16)];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return BitConverter.ToInt16(randomBytes);

        }

        private static bool AuthenticatePassword(string password, string salt, string hash)
        {
            var hashingAlgorithm = System.Security.Cryptography.SHA512.Create();
            int iterations = 1000;
            return false;
        }
    }
}
