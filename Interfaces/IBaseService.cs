using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IBaseService<T>
    {
        public bool Create(T obj);
        public List<string> GetAll();
        public string GetByName(string name);
        public bool Update(T obj, string name);
        public bool Delete(string name);
    }
}
