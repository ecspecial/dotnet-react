using Plorg.Model;
using PlorgRepo.Repo.ElementRepos;
using PlorgRepo.Repo.DBAccess;
using PlorgRepo.Repo.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlorgRepo.Repo.ElementRepos {
    public class RootElementRepo : IRepo<RootElement> {

        IDBAccess dbAccess;

        public RootElementRepo(IDBAccess access) {
            dbAccess = access;
        }

        public void Delete(Guid rid, IEnumerable<Guid>? guids = null) {
            if (guids == null)
                guids = new List<Guid>();

            foreach (var guid in guids) {
                string query = @"DELETE FROM root_elements WHERE RID=@rid;";
                var args = new Dictionary<string, object>() {
                    { "@rid", guid.ToString() }
                };

                dbAccess.ExecuteWrite(query, args);
            }
        }

        public void Delete(Guid rid, Guid guid) {
            Delete(rid, new Guid[] { guid });
        }

        public IEnumerable<RootElement> Get(Guid rid, IEnumerable<Guid>? guids = null) {
            string query = @"SELECT * FROM root_elements;";

            var args = new Dictionary<string, object>();

            var resultTable = dbAccess.Execute(query, args);

            if (resultTable == null)
                return new RootElement[0];

            var result = new List<RootElementDTO>();

            foreach (DataRow row in resultTable.Rows) {
                var tmp = new RootElementDTO();
                tmp.ID = Guid.Parse((string)row["RID"]);
                tmp.Name = (string)row["Name"];
                tmp.Password = (string)row["Password"];

                result.Add(tmp);
            }

            if (guids == null || guids.Count() == 0)
                return result.Select(x => x.FromDTO());
            else
                return result.Where(x => guids.Contains(x.ID)).ToList().Select(x => x.FromDTO());
        }

        public void Save(Guid rid, IEnumerable<RootElement> elements) {
            foreach (var element in elements) {
                Save(rid, element);
            }
        }

        public void Save(Guid rid, RootElement element) {
            var elementDTO = new RootElementDTO();
            elementDTO.ToDTO(element);

            string query = @"SELECT * FROM root_elements WHERE RID=@rid;";
            var args = new Dictionary<string, object>() {
                { "@rid", elementDTO.ID.ToString() }
            };

            var result = dbAccess.Execute(query, args);

            if (result == null || result.Rows.Count == 0)
                query = @"INSERT INTO root_elements (RID, name, password) VALUES (@rid, @name, @password);";
            else
                query = @"UPDATE root_elements SET name=@name, password=@password WHERE RID=@rid;";

            args = new Dictionary<string, object>() {
                { "@rid", elementDTO.ID.ToString() },
                { "@name", elementDTO.Name },
                { "@password", elementDTO.Password }
            };

            dbAccess.ExecuteWrite(query, args);
        }
    }
}
