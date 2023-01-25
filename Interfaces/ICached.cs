using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ICached<T>
    {
        public List<string> GetAll();
        public string GetByName(string name);
    }
}
