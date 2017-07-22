using System;
using Microsoft.AspNetCore.Http;
using StravaReporter.Extensions;

namespace StravaReporter.Services
{
    public class UserSessionStateManager : IUserSessionStateManager
    {
        private readonly ISession _session;

        public UserSessionStateManager(IHttpContextAccessor session)
        {
            _session = session?.HttpContext?.Session;
            if (_session == null) throw new ArgumentException(nameof(_session));
        }

        public class Wrapper : IActivityState
        {
            public DateTime LastFetchDateTime { get; set; }
            public string Test { get; set; }
        }

        public Wrapper Current
        {
            get
            {
                if (_session == null) throw new ArgumentException(nameof(_session));
                Wrapper session = _session.GetObjectFromJson<Wrapper>(nameof(Wrapper));
                if (session == null)
                {
                    session = new Wrapper();
                    _session.SetObjectAsJson(nameof(Wrapper), session);
                }
                return session;
            }
        }
    }
}
