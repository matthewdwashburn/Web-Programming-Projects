using Portal.Models;

namespace Portal.Services
{
    public class UserDatabase
    {

        //Create user database
        private Dictionary<string, UserModel> users = new();

        public UserDatabase() { 
            //Initialize user database with usernames and passwords
            UserModel user1 = new UserModel();
            UserModel user2 = new UserModel();
            UserModel user3 = new UserModel();
            user1.UserName = "glados";
            user1.Password = "cake";
            user2.UserName = "Matthew";
            user2.Password = "Washburn";
            user3.UserName = "Joshua";
            user3.Password = "Washburn";

            users.Add(user1.UserName, user1);
            users.Add(user2.UserName, user2);
            users.Add(user3.UserName, user3);

        }

        //Get user with parameter username key
        public UserModel GetUser(string username)
        {
            if(!users.ContainsKey(username))
            {
                return null;
            }

            return users[username];
        }

        //Check if parameter password matches password stored in database for specified user
        public bool CheckPassword(UserModel user, string password)
        {
            if (user.Password == password)
            {
                return true;
            }

            return false;
        }

    }
}
