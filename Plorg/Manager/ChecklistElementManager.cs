using Plorg.Model;
using Plorg.Repo.DTO;
using PlorgRepo.Repo.ElementRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plorg.Manager
{
    public class ChecklistElementManager: ElementManager<ChecklistElement> {

        public ChecklistElementManager(IRepo<ChecklistElement> elementRepository) : base(elementRepository) { }

    }
}
