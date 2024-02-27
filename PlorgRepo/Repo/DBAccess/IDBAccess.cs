using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlorgRepo.Repo.DBAccess {
    public interface IDBAccess {
        public int ExecuteWrite(string query, Dictionary<string, object> args);
        public DataTable? Execute(string query, Dictionary<string, object> args);
    }
}
