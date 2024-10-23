using JwtAuthASPNet7WebAPI.Core.Dtos;

namespace JwtAuthASPNet7WebAPI.Core.Contexts
{
    public class UserContext
    {
        private static readonly AsyncLocal<RuntimeContextInstance> _runtimeContext = new AsyncLocal<RuntimeContextInstance>();

        public static RuntimeContextInstance Current
        {
            get
            {
                if (_runtimeContext.Value == null)
                {
                    _runtimeContext.Value = new RuntimeContextInstance();
                }
                return _runtimeContext.Value;
            }
            set
            {
                _runtimeContext.Value = value;
            }
        }

        public static void SetUserData(CurrentUser user)
        {
            Current.User = user;
        }

        public static void ClearUserData()
        {
            Current.User = new CurrentUser();
        }
    }
}
