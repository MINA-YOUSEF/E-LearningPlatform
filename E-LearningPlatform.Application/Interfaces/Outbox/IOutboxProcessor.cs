using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Outbox
{
    public interface IOutboxProcessor
    {
        Task ProcessAsync ();
    }
}
