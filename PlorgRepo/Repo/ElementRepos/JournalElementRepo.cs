using Plorg.Model;
using PlorgRepo.Repo.DBAccess;
using PlorgRepo.Repo.DTO;
using System.Data;

namespace PlorgRepo.Repo.ElementRepos {
  public class JournalElementRepo : IRepo<JournalElement> {

    IDBAccess dbAccess;

    public JournalElementRepo(IDBAccess access) {
      dbAccess = access;
    }

    public void Delete(Guid rid, IEnumerable<Guid>? guids = null) {
      if (guids == null)
        guids = new List<Guid>();

      foreach (var guid in guids) {
        string query = @"DELETE FROM journal_elements WHERE ID=@id AND RID=@rid;";
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

    public IEnumerable<JournalElement> Get(Guid rid, IEnumerable<Guid>? guids = null) {
      string query = @"SELECT * FROM journal_elements;";

      var args = new Dictionary<string, object>();

      var resultTable = dbAccess.Execute(query, args);

      if (resultTable == null)
        return new JournalElement[0];

      var result = new List<JournalElementDTO>();

      foreach (DataRow row in resultTable.Rows) {
        var tmp = new JournalElementDTO();
        tmp.ID = Guid.Parse((string)row["ID"]);
        tmp.BID = Guid.Parse((string)row["BID"]);
        tmp.RID = Guid.Parse((string)row["RID"]);

        result.Add(tmp);
      }

      query = @"SELECT * FROM journal_pages WHERE journal=@id;";

      foreach (var journal in result) {
        args = new Dictionary<string, object>() {
                    { "@id", journal.ID.ToString() }
                };

        var pagesTable = dbAccess.Execute(query, args);

        if (pagesTable != null) {
          var pagesDTO = new List<PageDTO>();
          foreach (DataRow row in pagesTable.Rows) {
            var tmp = new PageDTO();
            tmp.name = (string)row["name"];
            tmp.content = (string)row["content"];
            tmp.pageId = int.Parse((string)row["pageId"]);
            pagesDTO.Add(tmp);
          }
          journal.Pages = pagesDTO.OrderByDescending(x => x.pageId).Reverse().ToList();
        }
      }

      if (guids == null || guids.Count() == 0)
        return result.Select(x => x.FromDTO());
      else
        return result.Where(x => guids.Contains(x.ID)).ToList().Select(x => x.FromDTO());
    }

    public void Save(Guid rid, IEnumerable<JournalElement> elements) {
      foreach (var element in elements) {
        Save(rid, element);
      }
    }

    public void SavePages(Guid id, IEnumerable<PageDTO> pages) {
      string query = @"DELETE FROM journal_pages WHERE journal=@journal;";

      var args = new Dictionary<string, object>() {
                { "@journal", id.ToString() }
            };

      dbAccess.Execute(query, args);

      query = @"INSERT INTO journal_pages (journal, pageId, name, content) VALUES (@journal, @pageId, @name, @content);";

      int i = 0;
      foreach (var page in pages) {
        args = new Dictionary<string, object>() {
                    { "@journal", id.ToString() },
                    { "@pageId", i },
                    { "@name", page.name },
                    { "@content", page.content }
                };
        dbAccess.Execute(query, args);
        i++;
      }
    }

    public void Save(Guid rid, JournalElement element) {
      var elementDTO = new JournalElementDTO();
      elementDTO.ToDTO(element);

      string query = @"SELECT 1 FROM journal_elements WHERE ID=@id AND RID=@rid;";
      var args = new Dictionary<string, object>() {
                { "@rid", rid.ToString() },
                { "@bid", elementDTO.BID.ToString() },
                { "@id", elementDTO.ID.ToString() }
            };

      var result = dbAccess.Execute(query, args);

      if (result == null || result.Rows.Count == 0)
        query = @"INSERT INTO journal_elements (ID, BID, RID) VALUES (@id, @bid, @rid);";
      else
        query = @"UPDATE journal_elements SET BID=@bid, RID=@rid WHERE ID=@id;";

      args = new Dictionary<string, object>() {
                { "@id", elementDTO.ID.ToString() },
                { "@bid", elementDTO.BID.ToString() },
                { "@rid", elementDTO.RID.ToString() }
            };
      dbAccess.ExecuteWrite(query, args);

      SavePages(elementDTO.ID, elementDTO.Pages);

    }
  }
}
