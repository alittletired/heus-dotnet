namespace Heus.Core.Internal;

using System.Security.Claims;
    [Service]
    internal class CurrentUser : ICurrentUser
    {
        private static readonly AsyncLocal<ClaimsPrincipal?> CurrentPrincipal = new();
        public bool IsAuthenticated => Principal != null;

        public ClaimsPrincipal? Principal => CurrentPrincipal.Value;

        public long? UserId => this.FindClaimValue<long>(ClaimTypes.Name);

        public string? UserName => this.FindClaimValue(ClaimTypes.NameIdentifier);



        public IDisposable SetCurrent(ClaimsPrincipal principal)
        {
            var parent = Principal;
            CurrentPrincipal.Value = principal;
            return DisposeAction.Create(() =>
            {
                CurrentPrincipal.Value = parent;
            });
        }
    }

