using Cbeua.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices.Common
{
    public interface IMonthlyContributionService
    {
        Task< List<IndividualContrbutionDTO>> GetIndividualContrbution(int MemeberId);

    }


    
}