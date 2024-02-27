using Plorg.Model;
using Plorg.Repo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlorgRepo.Repo.ElementRepos
{
    /// <summary>
    /// Repository interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepo<T>
    {
        public IEnumerable<T> Get(Guid rid, IEnumerable<Guid>? guids = null);
        public void Save(Guid rid, IEnumerable<T> elements); // doubles as update
        public void Save(Guid rid, T element);
        public void Delete(Guid rid, IEnumerable<Guid>? guids = null);
        public void Delete(Guid rid, Guid guid);
    }
}