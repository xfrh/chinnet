using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Teams;

namespace ManageSystem.Services.Teams
{
    public class TeamCREContentService : BaseService<Team_CREContent>, ITeamCREContentService
    {
        public TeamCREContentService(IRepository<Team_CREContent> repository) : base(repository)
        {
        }
    }
}
