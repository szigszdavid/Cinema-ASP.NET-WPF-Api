using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Persistence
{
    public class Hall
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public int RowCount { get; set; }

        public int ColumnCount { get; set; }
    }
}
