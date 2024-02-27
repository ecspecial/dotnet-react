using Plorg.Model;
using Plorg.Repo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PlorgRepo.Repo.DTO {
    public class RootElementDTO : IElementDTO<RootElement> {
        public Guid ID { get; set; }
        public string Name { get; set; } = "";
        public string Password { get; set; } = "";

        public RootElement FromDTO() {
            return new RootElement(ID, Name, Password);
        }

        public void ToDTO(RootElement element) {
            ID = element.ID;
            Name = element.Name;
            Password = element.Password;
        }
    }
}
