using Plorg.Model;
using PlorgRepo.Repo.ElementRepos;
using PlorgRepo.Repo.DBAccess;
using PlorgRepo.Repo.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlorgRepo.Repo.ElementRepos {
    public class TimeLimitElementRepo : IRepo<TimeLimitElement> {
        IDBAccess dbAccess;

        public TimeLimitElementRepo(IDBAccess access) {
            dbAccess = access;
        }

        public void Delete(Guid rid, IEnumerable<Guid>? guids = null) {
            if (guids == null)
                guids = new List<Guid>();

            foreach (var guid in guids) {
                string query = @"DELETE FROM time_limit_elements WHERE ID=@id AND RID=@rid;";
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

        public IEnumerable<TimeLimitElement> Get(Guid rid, IEnumerable<Guid>? guids = null) {
            string query = @"SELECT * FROM time_limit_elements;";

            var args = new Dictionary<string, object>();

            var resultTable = dbAccess.Execute(query, args);

            if (resultTable == null)
                return new TimeLimitElement[0];

            var result = new List<TimeLimitElementDTO>();

            foreach (DataRow row in resultTable.Rows) {
                var tmp = new TimeLimitElementDTO();
                tmp.ID = Guid.Parse((string)row["ID"]);
                tmp.BID = Guid.Parse((string)row["BID"]);
                tmp.RID = Guid.Parse((string)row["RID"]);

                tmp.Deadline = DateTime.Parse((string)row["deadline"]);
                tmp.ExecutionTime = TimeSpan.Parse((string)row["executionTime"]);
                tmp.WarningTime = TimeSpan.Parse((string)row["warningTime"]);
                tmp.repeatYears = int.Parse((string)row["repeatYears"]);
                tmp.repeatMonths = int.Parse((string)row["repeatMonths"]);
                tmp.repeatSpan = TimeSpan.Parse((string)row["repeatSpan"]);
                tmp.AutoRepeat = bool.Parse((string)row["autoRepeat"]);
                tmp.RepeatCount = int.Parse((string)row["repeatCount"]);
                tmp.Active = bool.Parse((string)row["active"]);
                tmp.Repeated = bool.Parse((string)row["repeated"]);


                result.Add(tmp);
            }

            if (guids == null || guids.Count() == 0)
                return result.Select(x => x.FromDTO());
            else
                return result.Where(x => guids.Contains(x.ID)).ToList().Select(x => x.FromDTO());
        }

        public void Save(Guid rid, IEnumerable<TimeLimitElement> elements) {
            foreach (var element in elements) {
                Save(rid, element);
            }
        }

        public void Save(Guid rid, TimeLimitElement element) {
            var elementDTO = new TimeLimitElementDTO();
            elementDTO.ToDTO(element);

            string query = @"SELECT 1 FROM time_limit_elements WHERE ID=@id AND BID=@bid AND RID=@rid;";
            var args = new Dictionary<string, object>() {
                { "@rid", rid.ToString() },
                { "@bid", elementDTO.BID.ToString() },
                { "@id", elementDTO.ID.ToString() }
            };

            var result = dbAccess.Execute(query, args);

            if (result == null || result.Rows.Count == 0)
                query = @"INSERT INTO time_limit_elements (ID, BID, RID, deadline, executionTime," +
                    @" warningTime, repeatYears, repeatMonths, repeatSpan, autoRepeat, repeatCount, active, repeated)" +
                    @" VALUES (@id, @bid, @rid, @deadline, @executionTime," +
                    @" @warningTime, @repeatYears, @repeatMonths, @repeatSpan, @autoRepeat, @repeatCount, @active, @repeated);";
            else
                query = @"UPDATE time_limit_elements SET BID=@bid, RID=@rid, deadline=@deadline, executionTime=@executionTime," +
                    @" warningTime=@warningTime, repeatYears=@repeatYears, repeatMonths=@repeatMonths, repeatSpan=@repeatSpan," +
                    @" autoRepeat=@autoRepeat, repeatCount=@repeatCount, active=@active, repeated=@repeated" +
                    @" WHERE ID=@id;";

            args = new Dictionary<string, object>() {
                { "@id", elementDTO.ID.ToString() },
                { "@rid", elementDTO.RID.ToString() },
                { "@bid", elementDTO.BID.ToString() },
                { "@deadline", elementDTO.Deadline.ToString() },
                { "@executionTime", elementDTO.ExecutionTime.ToString() },
                { "@warningTime", elementDTO.WarningTime.ToString() },
                { "@repeatYears", elementDTO.repeatYears.ToString() },
                { "@repeatMonths", elementDTO.repeatMonths.ToString() },
                { "@repeatSpan", elementDTO.repeatSpan.ToString() },
                { "@autoRepeat", elementDTO.AutoRepeat.ToString() },
                { "@repeatCount", elementDTO.RepeatCount.ToString() },
                { "@active", elementDTO.Active.ToString() },
                { "@repeated", elementDTO.Repeated.ToString() }
            };

            dbAccess.ExecuteWrite(query, args);
        }
    }
}
