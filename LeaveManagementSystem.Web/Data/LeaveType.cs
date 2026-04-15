using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace LeaveManagementSystem.Web.Data
{
    public class LeaveType
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(150)")] // [MaxLength(150)]
        public string Name { get; set; }
        public int NumberOfDays { get; set; }
    }
}
