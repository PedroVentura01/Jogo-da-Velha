using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jogo_da_Velha
{
    class Jogador
    {
        public string Nome { get; }
        public string CPF { get; }
        public int PartidasVencidas { get; set; }

        public Jogador(string nome)
        {
            Nome = nome;
        }
        public Jogador(string nome, string cpf)
        {
            Nome = nome;
            CPF = cpf;
        }
    }
}
