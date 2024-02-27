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
    public class BaseElementRepo : IRepo<BaseElement> {

        IDBAccess dbAccess;

        public BaseElementRepo(IDBAccess access) {
            dbAccess = access;
        }

        public void Delete(Guid rid, IEnumerable<Guid>? guids = null) {
            if (guids == null)
                guids = new List<Guid>();

            foreach (var guid in guids) {
                string query = @"DELETE FROM base_elements WHERE BID=@bid AND RID=@rid;";
                var args = new Dictionary<string, object>() {
                    { "@bid", guid.ToString() },
                    { "@rid", rid.ToString() }
                };

                dbAccess.ExecuteWrite(query, args);
            }
        }

        public void Delete(Guid rid, Guid guid) {
            Delete(rid, new Guid[] { guid });
        }

        public IEnumerable<BaseElement> Get(Guid rid, IEnumerable<Guid>? guids = null) {
            string query = @"SELECT * FROM base_elements;";

            var args = new Dictionary<string, object>();

            var resultTable = dbAccess.Execute(query, args);

            if (resultTable == null)
                return new BaseElement[0];

            var result = new List<BaseElementDTO>();

            foreach (DataRow row in resultTable.Rows) {
                var tmp = new BaseElementDTO();
                tmp.BID = Guid.Parse((string)row["BID"]);
                tmp.RID = Guid.Parse((string)row["RID"]);
                tmp.Name = (string)row["Name"];
                tmp.Description = (string)row["Description"];

                result.Add(tmp);
            }

            if (guids == null || guids.Count() == 0)
                return result.Select(x => x.FromDTO());
            else
                return result.Where(x => guids.Contains(x.BID)).ToList().Select(x => x.FromDTO());
        }

        public void Save(Guid rid, IEnumerable<BaseElement> elements) {
            foreach (var element in elements) {
                Save(rid, element);
            }
        }

        public void Save(Guid rid, BaseElement element) {
            var elementDTO = new BaseElementDTO();
            elementDTO.ToDTO(element);

            string query = @"SELECT 1 FROM base_elements WHERE BID=@bid AND RID=@rid;";
            var args = new Dictionary<string, object>() {
                { "@rid", rid.ToString() },
                { "@bid", elementDTO.BID.ToString() }
            };

            var result = dbAccess.Execute(query, args);

            if (result == null || result.Rows.Count == 0)
                query = @"INSERT INTO base_elements (BID, RID, name, description) VALUES (@bid, @rid, @name, @description);";
            else
                query = @"UPDATE base_elements SET RID=@rid, name=@name, description=@description WHERE BID=@bid;";

            args = new Dictionary<string, object>() {
                { "@rid", elementDTO.RID.ToString() },
                { "@bid", elementDTO.BID.ToString() },
                { "@name", elementDTO.Name },
                { "@description", elementDTO.Description }
            };

            dbAccess.ExecuteWrite(query, args);
        }
    }
}
