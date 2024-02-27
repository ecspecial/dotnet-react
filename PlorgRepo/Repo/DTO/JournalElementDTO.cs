using Plorg.Model;
using Plorg.Repo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlorgRepo.Repo.DTO {

    public class PageDTO {
        public string name = "";
        public string content = "";
        public int pageId = 0;

        public PageDTO() { }

        public PageDTO(string name, string content) {
            this.name = name;
            this.content = content;
        }

        public Page FromDTO() {
            var page = new Page();
            page.name = name;
            page.content = content;
            return page;
        }

        public void ToDTO(Page page) {
            name = page.name;
            content = page.content;
        }
    }

    public class JournalElementDTO : IElementDTO<JournalElement> {

        public Guid ID { get; set; }
        public Guid RID { get; set; }
        public Guid BID { get; set; }

        public List<PageDTO> Pages { get; set; } = new List<PageDTO>();

        public JournalElement FromDTO() {
            var journal = new JournalElement(ID, RID, BID);
            journal.Pages = Pages.Select(x => x.FromDTO()).ToList();
            return journal;
        }

        public void ToDTO(JournalElement element) {
            ID = element.ID;
            BID = element.BID;
            RID = element.RID;

            Pages = element.Pages.Select(x => new PageDTO(x.name, x.content)).ToList();
        }
    }
}
