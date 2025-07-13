using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.InterfaceAdapters.Models
{
    public class PurchaseOutputModel
    {
        public string ProductName { get; set; }
        public int TotalSpent { get; set; }
        public List<int> Change { get; set; }
    }
}
