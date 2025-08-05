using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpMate.Infrastructure.Logging
{
    public interface IAppLogger<T>
    {
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(string message, params object[] args);
        void LogError(Exception exception, string message, params object[] args);
        void LogDebug(string message, params object[] args);
        void LogCritical(string message, params object[] args);
    }
}
