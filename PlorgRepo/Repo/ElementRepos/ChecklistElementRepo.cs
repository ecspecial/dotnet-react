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
    public class ChecklistElementRepo : IRepo<ChecklistElement> {

        IDBAccess dbAccess;

        public ChecklistElementRepo(IDBAccess access) {
            dbAccess = access;
        }

        public void Delete(Guid rid, IEnumerable<Guid>? guids = null) {
            if (guids == null)
                guids = new List<Guid>();

            foreach (var guid in guids) {
                string query = @"DELETE FROM checklist_elements WHERE ID=@id AND RID=@rid;";
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

        public IEnumerable<ChecklistElement> Get(Guid rid, IEnumerable<Guid>? guids = null) {
            string query = @"SELECT * FROM checklist_elements;";

            var args = new Dictionary<string, object>();

            var resultTable = dbAccess.Execute(query, args);

            if (resultTable == null)
                return new ChecklistElement[0];

            var result = new List<ChecklistElementDTO>();

            foreach (DataRow row in resultTable.Rows) {
                var tmp = new ChecklistElementDTO();
                tmp.ID = Guid.Parse((string)row["ID"]);
                tmp.BID = Guid.Parse((string)row["BID"]);
                tmp.RID = Guid.Parse((string)row["RID"]);
                tmp.ParentID = Guid.Parse((string)row["ParentID"]);
                tmp.Checked = bool.Parse((string)row["checked"]);
                tmp.ResetOnComplete = bool.Parse((string)row["resetOnComplete"]);

                result.Add(tmp);
            }

            if (guids == null || guids.Count() == 0)
                return result.Select(x => x.FromDTO());
            else
                return result.Where(x => guids.Contains(x.ID)).ToList().Select(x => x.FromDTO());
        }

        public void Save(Guid rid, IEnumerable<ChecklistElement> elements) {
            foreach (var element in elements) {
                Save(rid, element);
            }
        }

        public void Save(Guid rid, ChecklistElement element) {
            var elementDTO = new ChecklistElementDTO();
            elementDTO.ToDTO(element);

            string query = @"SELECT 1 FROM checklist_elements WHERE ID=@id AND BID=@bid AND RID=@rid;";
            var args = new Dictionary<string, object>() {
                { "@rid", rid.ToString() },
                { "@bid", elementDTO.BID.ToString() },
                { "@id", elementDTO.ID.ToString() }
            };

            var result = dbAccess.Execute(query, args);

            if (result == null || result.Rows.Count == 0)
                query = @"INSERT INTO checklist_elements (ID, BID, RID, ParentID, checked, resetOnComplete) VALUES (@id, @bid, @rid, @pid, @checked, @resetOnComplete);";
            else
                query = @"UPDATE checklist_elements SET BID=@bid, RID=@rid, ParentID=@pid, checked=@checked, resetOnComplete=@resetOnComplete WHERE ID=@id;";

            args = new Dictionary<string, object>() {
                { "@id", elementDTO.ID.ToString() },
                { "@rid", elementDTO.RID.ToString() },
                { "@bid", elementDTO.BID.ToString() },
                { "@pid", elementDTO.ParentID.ToString() },
                { "@checked", elementDTO.Checked.ToString() },
                { "@resetOnComplete", elementDTO.ResetOnComplete.ToString() }
            };

            dbAccess.ExecuteWrite(query, args);
        }
    }
}
