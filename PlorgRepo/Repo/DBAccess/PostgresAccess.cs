using System.Data;
using Npgsql;
using System.Configuration;

namespace PlorgRepo.Repo.DBAccess
{

  public class PostgresAccess: IDBAccess {

    public string connString = "";
    protected bool _readOnly = false;

    public void InitializeDatabase(bool readOnly=false) {
      _readOnly = readOnly;

      var connStringParsed = connString.Split(';').Where(x => !x.StartsWith("Database=")).Aggregate((x, y) => x + ";" + y);
      var dbname = connString.Split(';').Where(x => x.StartsWith("Database=")).FirstOrDefault();
      if (dbname == null)
        throw new ConfigurationErrorsException("Incorrect postgres configuration: database name not specified");
      dbname = dbname.Replace("Database=", "");

      using (var db = new NpgsqlConnection(connStringParsed)) {
        try {
          db.Open();
        } catch(Exception e) {
          throw new Exception($"Database connection exception: {e.Message}");
        }

        string query = $"SELECT * FROM pg_database WHERE datname = '{dbname}';";
        var command = new NpgsqlCommand(query, db);

        var da = new NpgsqlDataAdapter(command);
        var dt = new DataTable();

        da.Fill(dt);
        da.Dispose();

        if (dt.Rows.Count == 0) {
          query = $"CREATE DATABASE {dbname};";
          command = new NpgsqlCommand(query, db);
          command.ExecuteNonQuery();
        }
      }

      using (var db = new NpgsqlConnection(this.connString)) {
        db.Open();

        string createRootElementsTable = "CREATE TABLE IF NOT EXISTS " +
          "root_elements (" +
          "RID TEXT NOT NULL PRIMARY KEY," +
          "name TEXT," +
          "password TEXT" +
          ");";

        string createBaseElementsTable = "CREATE TABLE IF NOT EXISTS " +
          "base_elements (" +
          "BID TEXT NOT NULL PRIMARY KEY," +
          "RID TEXT NOT NULL," +
          "name TEXT," +
          "description TEXT," +
          "FOREIGN KEY(RID) REFERENCES root_elements(RID) ON DELETE CASCADE);";

        string createChecklistElementsTable = "CREATE TABLE IF NOT EXISTS " +
          "checklist_elements (" +
          "ID TEXT NOT NULL PRIMARY KEY," +
          "BID TEXT NOT NULL," +
          "RID TEXT NOT NULL," +
          "ParentID TEXT NOT NULL," +
          "checked TEXT," +
          "resetOnComplete TEXT," +
          "FOREIGN KEY(RID) REFERENCES root_elements(RID) ON DELETE CASCADE," +
          "FOREIGN KEY(BID) REFERENCES base_elements(BID) ON DELETE CASCADE);";

        string createJournalElementsTable = "CREATE TABLE IF NOT EXISTS " +
          "journal_elements (" +
          "ID TEXT NOT NULL PRIMARY KEY," +
          "BID TEXT NOT NULL," +
          "RID TEXT NOT NULL," +
          "FOREIGN KEY(RID) REFERENCES root_elements(RID) ON DELETE CASCADE," +
          "FOREIGN KEY(BID) REFERENCES base_elements(BID) ON DELETE CASCADE);";

        string createJournalElementPagesTable = "CREATE TABLE IF NOT EXISTS " +
          "journal_pages (" +
          "journal TEXT NOT NULL," +
          "pageId TEXT NOT NULL," +
          "name TEXT NOT NULL," +
          "content TEXT," +
          "FOREIGN KEY(journal) REFERENCES journal_elements(ID) ON DELETE CASCADE);";

        string createRelationElementsTable = "CREATE TABLE IF NOT EXISTS " +
          "relation_elements (" +
          "ID TEXT NOT NULL PRIMARY KEY," +
          "BID TEXT NOT NULL," +
          "RID TEXT NOT NULL," +
          "relative TEXT NOT NULL," +
          "relationType TEXT NOT NULL," +
          "FOREIGN KEY(RID) REFERENCES root_elements(RID) ON DELETE CASCADE," +
          "FOREIGN KEY(BID) REFERENCES base_elements(BID) ON DELETE CASCADE," +
          "FOREIGN KEY(relative) REFERENCES base_elements(BID) ON DELETE CASCADE);";

        string createTimeLimitElementsTable = "CREATE TABLE IF NOT EXISTS " +
          "time_limit_elements (" +
          "ID TEXT NOT NULL PRIMARY KEY," +
          "BID TEXT NOT NULL," +
          "RID TEXT NOT NULL," +
          "deadline TEXT," +
          "executionTime TEXT," +
          "warningTime TEXT," +
          "repeatYears TEXT," +
          "repeatMonths TEXT," +
          "repeatSpan TEXT," +
          "autoRepeat TEXT," +
          "repeatCount TEXT," +
          "active TEXT," +
          "repeated TEXT," +
          "FOREIGN KEY(RID) REFERENCES root_elements(RID) ON DELETE CASCADE," +
          "FOREIGN KEY(BID) REFERENCES base_elements(BID) ON DELETE CASCADE);";

        string tableCommand = createRootElementsTable +
          createBaseElementsTable +
          createChecklistElementsTable +
          createJournalElementsTable + createJournalElementPagesTable +
          createRelationElementsTable +
          createTimeLimitElementsTable;

        var initializeTable = new NpgsqlCommand(tableCommand, db);

        initializeTable.ExecuteReader();
      }
    }

    public int ExecuteWrite(string query, Dictionary<string, object> args) {
      if (_readOnly) {
        throw new AccessViolationException("Access to database was denied due to app being launched in readonly mode");
      }

      int numberOfRowsAffected;

      //setup the connection to the database
      using (var con = new NpgsqlConnection(connString)) {
        con.Open();

        //open a new command
        using (var cmd = new NpgsqlCommand(query, con)) {
          //set the arguments given in the query
          foreach (var pair in args) {
            cmd.Parameters.AddWithValue(pair.Key, pair.Value);
          }

          //execute the query and get the number of row affected
          numberOfRowsAffected = cmd.ExecuteNonQuery();
        }

        return numberOfRowsAffected;
      }
    }

    public DataTable? Execute(string query, Dictionary<string, object> args) {
      if (string.IsNullOrEmpty(query.Trim()))
        return null;

      using (var con = new NpgsqlConnection(connString)) {
        con.Open();
        using (var cmd = new NpgsqlCommand(query, con)) {
          foreach (KeyValuePair<string, object> entry in args) {
            cmd.Parameters.AddWithValue(entry.Key, entry.Value);
          }

          var da = new NpgsqlDataAdapter(cmd);

          var dt = new DataTable();
          da.Fill(dt);

          da.Dispose();
          return dt;
        }
      }
    }
  }
}
