using Plorg.Model;
using Plorg.Repo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlorgRepo.Repo.DTO {
    public class RelationElementDTO : IElementDTO<RelationElement> {

        public Guid ID { get; set; }
        public Guid RID { get; set; }
        public Guid BID { get; set; }

        public Guid Relative { get; set; }

        public RelativeType RelationType { get; set; }

        public RelationElement FromDTO() {
            var relation = new RelationElement(ID, RID, BID);
            relation.Relative = Relative;
            relation.RelationType = RelationType;
            return relation;
        }

        public void ToDTO(RelationElement element) {
            ID = element.ID;
            RID = element.RID;
            BID = element.BID;
            Relative = element.Relative;
            RelationType = element.RelationType;
        }
    }
}
