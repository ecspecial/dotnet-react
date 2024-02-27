using Plorg.Model;
using Plorg.Repo;
using Plorg.Repo.DTO;
using PlorgRepo.Repo.ElementRepos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Plorg.Manager {
    public class RootElementManager : ElementManager<RootElement> {
        public RootElementManager(IRepo<RootElement> elementRepository) : base(elementRepository) { }

        public RootElement? Authenticate(Guid rid, string password = "") {
            var root = Find(Guid.Empty, new Guid[] { rid }).First();

            if (root == null) { return null; }

            var hashedPassword = GeneratePasswordHash(rid, password);

            if (hashedPassword == root.Password) {
                return root;
            } else {
                return null;
            }
        }

        public new void Save(Guid rid, IEnumerable<RootElement> elements) {
            foreach (var element in elements) {
                element.Password = GeneratePasswordHash(element.ID, element.Password);
            }

            ElementRepository.Save(rid, elements);
        }

        public new void Save(Guid rid, RootElement element) {
            element.Password = GeneratePasswordHash(element.ID, element.Password);
            ElementRepository.Save(rid, element);
        }

        public static string GeneratePasswordHash(Guid rid, string password) {
            var hash = Encoding.UTF8.GetBytes(rid.ToString() + password);
            SHA1 sha1 = SHA1.Create();
            var sha1hash = sha1.ComputeHash(hash);
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetString(sha1hash).Replace('\u0000', '0');
        }

        public RootElement Create(string name, string password, Guid? guid = null) {
            Guid rootGuid;
            if (guid != null)
                rootGuid = (Guid)guid;
            else
                rootGuid = Guid.NewGuid();

            var root = new RootElement(rootGuid, name, password);
            Save(rootGuid, root);
            return root;
        }
    }
}
