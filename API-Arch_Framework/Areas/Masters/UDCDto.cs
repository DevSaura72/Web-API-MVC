using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Arch_Framework.Areas.Masters
{
    public class UDCDto
    {
        public int Id { get; set; }
        public string? Header { get; set; } = string.Empty;
        public bool IsHeader { get; set; } = false;

        public string? Value1 { get; set; } = string.Empty;
        public string? Value2 { get; set; } = string.Empty;
        public int? ParentId { get; set; } 
        public int? RelationId { get; set; }
    }
}
