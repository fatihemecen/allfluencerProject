using BlazorApp.Shared;

namespace BlazorApp.Client
{
    public class mnUserManager
    {
        public coUser user;
        public bool IsLogged
        {
            get
            {
                if (user != null && !String.IsNullOrEmpty(user.UserGuid))
                {
                    return true;
                }

                return false;
            }
        }
        public bool IsAdmin
        {
            get
            {
                return false;
            }
        }
        public coUser LoggedUser
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
            }
        }
    }
}
