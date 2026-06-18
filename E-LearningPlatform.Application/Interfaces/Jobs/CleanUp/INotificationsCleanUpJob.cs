using E_LearningPlatform.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Jobs.CleanUp
{
    public interface INotificationsCleanUpJob
    {

        Task ExecuteAsync ( );
    }
}
