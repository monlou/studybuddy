using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBuddy.Models;

namespace StudyBuddy.Services.Contracts
{
    public interface IGoogleManager
    {
        void Login(Action<GoogleUser, string> OnLoginComplete);
        void Logout();
    }
}