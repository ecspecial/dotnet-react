using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace PlorgRepo.Repo.DBAccess
{
  public class MySQLAccess : IDBAccess
  {

    public string connString = "";
    protected bool _readOnly = false;

    public void InitializeDatabase(bool readOnly = false)
    {
      _readOnly = readOnly;

      var connStringParsed = connString.Split(';').Where(x => !x.StartsWith("Database=")).Aggregate((x, y) => x + ";" + y);
      var dbname = connString.Split(';').Where(x => x.StartsWith("Database=")).FirstOrDefault();
      if (dbname == null)
        throw new ConfigurationErrorsException("Incorrect mysql configuration: database name not specified");
      dbname = dbname.Replace("Database=", "");

      using (var db = new MySqlConnection(connStringParsed))
      {
        try
        {
          db.Open();
        }
        catch (Exception e)
        {
          throw new Exception($"Database connection exception: {e.Message}");
        }

        string query = $"CREATE DATABASE IF NOT EXISTS {dbname};";
        var command = new MySqlCommand(query, db);
        command.ExecuteNonQuery();
      }

      using (var db = new MySqlConnection(this.connString))
      {
        db.Open();

        string createRootElementsTable = "CREATE TABLE IF NOT EXISTS " +
          "root_elements (" +
          "RID VARCHAR(64) NOT NULL," +
          "name TEXT," +
          "password TEXT," +
          "PRIMARY KEY (RID)" +
          ");";

        string createBaseElementsTable = "CREATE TABLE IF NOT EXISTS " +
          "base_elements (" +
          "BID VARCHAR(64) NOT NULL," +
          "RID VARCHAR(64) NOT NULL," +
          "name TEXT," +
          "description TEXT," +
          "PRIMARY KEY (BID)," +
          "FOREIGN KEY (RID) REFERENCES root_elements (RID) ON DELETE CASCADE);";

        string createChecklistElementsTable = "CREATE TABLE IF NOT EXISTS " +
          "checklist_elements (" +
          "ID VARCHAR(64) NOT NULL," +
          "BID VARCHAR(64) NOT NULL," +
          "RID VARCHAR(64) NOT NULL," +
          "ParentID VARCHAR(64) NOT NULL," +
          "checked TEXT," +
          "resetOnComplete TEXT," +
          "PRIMARY KEY (ID)," +
          "FOREIGN KEY(RID) REFERENCES root_elements(RID) ON DELETE CASCADE," +
          "FOREIGN KEY(BID) REFERENCES base_elements(BID) ON DELETE CASCADE);";

        string createJournalElementsTable = "CREATE TABLE IF NOT EXISTS " +
          "journal_elements (" +
          "ID VARCHAR(64) NOT NULL," +
          "BID VARCHAR(64) NOT NULL," +
          "RID VARCHAR(64) NOT NULL," +
          "PRIMARY KEY (ID)," +
          "FOREIGN KEY(RID) REFERENCES root_elements(RID) ON DELETE CASCADE," +
          "FOREIGN KEY(BID) REFERENCES base_elements(BID) ON DELETE CASCADE);";

        string createJournalElementPagesTable = "CREATE TABLE IF NOT EXISTS " +
          "journal_pages (" +
          "journal VARCHAR(64) NOT NULL," +
          "pageId TEXT NOT NULL," +
          "name TEXT NOT NULL," +
          "PRIMARY KEY (journal)," +
          "content TEXT," +
          "FOREIGN KEY(journal) REFERENCES journal_elements(ID) ON DELETE CASCADE);";

        string createRelationElementsTable = "CREATE TABLE IF NOT EXISTS " +
          "relation_elements (" +
          "ID VARCHAR(64) NOT NULL," +
          "BID VARCHAR(64) NOT NULL," +
          "RID VARCHAR(64) NOT NULL," +
          "relative VARCHAR(64) NOT NULL," +
          "relationType TEXT NOT NULL," +
          "PRIMARY KEY (ID)," +
          "FOREIGN KEY(RID) REFERENCES root_elements(RID) ON DELETE CASCADE," +
          "FOREIGN KEY(BID) REFERENCES base_elements(BID) ON DELETE CASCADE," +
          "FOREIGN KEY(relative) REFERENCES base_elements(BID) ON DELETE CASCADE);";

        string createTimeLimitElementsTable = "CREATE TABLE IF NOT EXISTS " +
          "time_limit_elements (" +
          "ID VARCHAR(64) NOT NULL," +
          "BID VARCHAR(64) NOT NULL," +
          "RID VARCHAR(64) NOT NULL," +
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
          "PRIMARY KEY (ID)," +
          "FOREIGN KEY(RID) REFERENCES root_elements(RID) ON DELETE CASCADE," +
          "FOREIGN KEY(BID) REFERENCES base_elements(BID) ON DELETE CASCADE);";

        string tableCommand = createRootElementsTable +
          createBaseElementsTable +
          createChecklistElementsTable +
          createJournalElementsTable + createJournalElementPagesTable +
          createRelationElementsTable +
          createTimeLimitElementsTable;

        var initializeTable = new MySqlCommand(tableCommand, db);

        initializeTable.ExecuteReader();
      }
    }

    public int ExecuteWrite(string query, Dictionary<string, object> args)
    {
      if (_readOnly)
      {
        throw new AccessViolationException("Access to database was denied due to app being launched in readonly mode");
      }

      int numberOfRowsAffected;

      //setup the connection to the database
      using (var con = new MySqlConnection(connString))
      {
        con.Open();

        //open a new command
        using (var cmd = new MySqlCommand(query, con))
        {
          //set the arguments given in the query
          foreach (var pair in args)
          {
            cmd.Parameters.AddWithValue(pair.Key, pair.Value);
          }

          //execute the query and get the number of row affected
          numberOfRowsAffected = cmd.ExecuteNonQuery();
        }

        return numberOfRowsAffected;
      }
    }

    public DataTable? Execute(string query, Dictionary<string, object> args)
    {
      if (string.IsNullOrEmpty(query.Trim()))
        return null;

      using (var con = new MySqlConnection(connString))
      {
        con.Open();
        using (var cmd = new MySqlCommand(query, con))
        {
          foreach (KeyValuePair<string, object> entry in args)
          {
            cmd.Parameters.AddWithValue(entry.Key, entry.Value);
          }

          var da = new MySqlDataAdapter(cmd);

          var dt = new DataTable();
          da.Fill(dt);

          da.Dispose();
          return dt;
        }
      }
    }
  }
}
