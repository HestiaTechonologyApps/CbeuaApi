using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Entities
{
    public class UserRoleRight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRoleRightId { get; set; }

        public DateTime ControllerName { get; set; }

        public String ActionName { get; set; } = "";

        public int UserTypeID { get; set; }

    }
}
