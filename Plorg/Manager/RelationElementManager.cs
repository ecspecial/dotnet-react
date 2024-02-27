using Plorg.Filters;
using Plorg.Model;
using Plorg.Repo;
using Plorg.Repo.DTO;
using PlorgRepo.Repo.ElementRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plorg.Manager {
    public class RelationElementManager : ElementManager<RelationElement> {
        public RelationElementManager(IRepo<RelationElement> elementRepository) : base(elementRepository) { }
    }
}
