﻿using Plorg.Model;
using Plorg.Repo;
using Plorg.Repo.DTO;
using PlorgRepo.Repo.ElementRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plorg.Manager {
    public class JournalElementManager : ElementManager<JournalElement> {
        public JournalElementManager(IRepo<JournalElement> elementRepository) : base(elementRepository) { }
    }
}
