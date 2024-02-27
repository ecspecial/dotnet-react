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
    public class RelationElementRepo : IRepo<RelationElement> {

        IDBAccess dbAccess;

        public RelationElementRepo(IDBAccess access) {
            dbAccess = access;
        }

        public void Delete(Guid rid, IEnumerable<Guid>? guids = null) {
            if (guids == null)
                guids = new List<Guid>();

            foreach (var guid in guids) {
                string query = @"DELETE FROM relation_elements WHERE ID=@id AND RID=@rid;";
                var args = new Dictionary<string, object>() {
                    { "@id", guid.ToString() },
                    { "@rid", rid.ToString() }
                };

                dbAccess.ExecuteWrite(query, args);
            }
        }

        public void Delete(Guid rid, Guid guid) {
            Delete(rid, new Guid[] { guid });
        }

        public IEnumerable<RelationElement> Get(Guid rid, IEnumerable<Guid>? guids = null) {
            string query = @"SELECT * FROM relation_elements;";

            var args = new Dictionary<string, object>();

            var resultTable = dbAccess.Execute(query, args);

            if (resultTable == null)
                return new RelationElement[0];

            var result = new List<RelationElementDTO>();

            foreach (DataRow row in resultTable.Rows) {
                var tmp = new RelationElementDTO();
                tmp.ID = Guid.Parse((string)row["ID"]);
                tmp.BID = Guid.Parse((string)row["BID"]);
                tmp.RID = Guid.Parse((string)row["RID"]);

                tmp.Relative = Guid.Parse((string)row["relative"]);
                tmp.RelationType = (RelativeType)int.Parse((string)row["relationType"]);

                result.Add(tmp);
            }

            if (guids == null || guids.Count() == 0)
                return result.Select(x => x.FromDTO());
            else
                return result.Where(x => guids.Contains(x.ID)).ToList().Select(x => x.FromDTO());
        }

        public void Save(Guid rid, IEnumerable<RelationElement> elements) {
            foreach (var element in elements) {
                Save(rid, element);
            }
        }

        public void Save(Guid rid, RelationElement element) {
            var elementDTO = new RelationElementDTO();
            elementDTO.ToDTO(element);

            string query = @"SELECT 1 FROM relation_elements WHERE ID=@id AND RID=@rid;";
            var args = new Dictionary<string, object>() {
                { "@rid", rid.ToString() },
                { "@bid", elementDTO.BID.ToString() },
                { "@id", elementDTO.ID.ToString() }
            };

            var result = dbAccess.Execute(query, args);

            if (result == null || result.Rows.Count == 0)
                query = @"INSERT INTO relation_elements (ID, BID, RID, relative, relationType) VALUES (@id, @bid, @rid, @relative, @relationType);";
            else
                query = @"UPDATE relation_elements SET BID=@bid, RID=@rid, relative=@relative, relationType=@relationType WHERE ID=@id;";

            args = new Dictionary<string, object>() {
                { "@id", elementDTO.ID.ToString() },
                { "@rid", elementDTO.RID.ToString() },
                { "@bid", elementDTO.BID.ToString() },
                { "@relative", elementDTO.Relative.ToString() },
                { "@relationType", ((int)elementDTO.RelationType).ToString() }
            };

            dbAccess.ExecuteWrite(query, args);
        }
    }
}
