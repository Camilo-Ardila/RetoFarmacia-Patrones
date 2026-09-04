using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibFarmacia.Interfaces
{
    public interface IAutenticador
    {
        bool Autenticar(
            string user,
            string password);
    }
}
