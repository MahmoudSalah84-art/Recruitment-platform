using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Interfaces
{
    public interface IMatchingService
    {
        /// <summary>
        /// returns score 0..1 for given cv and job
        /// </summary>
        double Score(CV cv, Job job);
    }
}
