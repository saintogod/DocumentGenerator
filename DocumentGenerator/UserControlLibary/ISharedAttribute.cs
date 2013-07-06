using DataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserControlLibary
{
    public interface ISharedAttribute<T> where T: Field, new()
    {
        T Info { get; set; }
    }
}
