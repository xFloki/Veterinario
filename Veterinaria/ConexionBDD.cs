using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria
{
    public static class ConexionBDD
    {

        public static string rutaConexion { get; } = "Server=localhost; Database=veterinario; Uid=root; Pwd=root ; Port=3306";    }
}
