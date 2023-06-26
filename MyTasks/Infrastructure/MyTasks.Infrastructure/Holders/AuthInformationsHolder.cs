using MyTasks.Application.Common.Interfaces;

namespace MyTasks.Infrastructure.Holders
{
    class AuthInformationsHolder : IAuthInformationsHolder
    {
        public string IdToken { get; set; }

        public AuthInformationsHolder()
        {
            IdToken = String.Empty;
        }
    }
}
