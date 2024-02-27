using Plorg.Filters;
using Plorg.Model;
using Plorg.Repo.DTO;
using PlorgRepo.Repo.ElementRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Plorg.Manager
{
    public abstract class ElementManager<T>: IElementManager<T> {
        public IRepo<T> ElementRepository { get; set; }

        public ElementManager(IRepo<T> elementRepository) {
            ElementRepository = elementRepository;
        }

        public IEnumerable<T> Find(Guid rid, IEnumerable<Guid>? bids = null, IElementFilter<T>? filter = null) {
            var elements = ElementRepository.Get(rid, bids);
            if (filter != null) {
                elements = filter.Filter(elements);
            }
            return elements;
        }

        public void Save(Guid rid, IEnumerable<T> elements) {
            ElementRepository.Save(rid, elements);
        }

        public void Save(Guid rid, T element) {
            ElementRepository.Save(rid, element);
        }

        public void Delete(Guid rid, IEnumerable<Guid>? bids = null) {
            ElementRepository.Delete(rid, bids);
        }

        public void Delete(Guid rid, Guid bid) {
            ElementRepository.Delete(rid, bid);
        }
    }
}
