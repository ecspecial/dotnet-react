using Plorg.Filters;
using Plorg.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plorg.Manager {
    public interface IElementManager<T> {
        public IEnumerable<T> Find(Guid rid, IEnumerable<Guid>? bids = null, IElementFilter<T>? filter = null);
        
        public void Save(Guid rid, IEnumerable<T> elements);
        
        public void Save(Guid rid, T element);

        public void Delete(Guid rid, IEnumerable<Guid>? bids = null);
    }
}
