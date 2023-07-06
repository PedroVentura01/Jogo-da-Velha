using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JogoDaVelha
{
    class Program
    {
        static string rankingFilePath = "ranking.txt";
        static string jogadoresFilePath = "jogadores.txt";
        static List<Jogador> jogadores = new List<Jogador>();

        static void Main(string[] args)
        {
            CarregarRanking();
            CarregarJogadores();

            bool sair = false;
            while (!sair)
            {
                Console.WriteLine("------------------------------------");
                Console.WriteLine("1 ----------------- Player VS Player");
                Console.WriteLine("2 ---------------- Player VS Maquina");
                Console.WriteLine("3 ---------------- Cadastrar jogador");
                Console.WriteLine("4 -------------- Ver Top 10 Vendores");
                Console.WriteLine("5 ----------------------------- Sair");
                Console.WriteLine("------------------------------------");
                Console.WriteLine();
                Console.WriteLine("Qual opção deseja agora? ");
                string opcao = Console.ReadLine();
                Console.WriteLine();

                switch (opcao)
                {
                    case "1":
                        JogarVSAmigo();
                        break;
                    case "2":
                        JogarVSComputador();
                        break;
                    case "3":
                        CadastrarJogador();
                        break;
                    case "4":
                        MostrarRanking();
                        break;
                    case "5":
                        sair = true;
                        break;
                    default:
                        Console.WriteLine("Opção indisponível. Escolha uma opção válida.");
                        Console.WriteLine();
                        break;
                }
            }
            SalvarRanking();
            SalvarJogadores();
        }
        static void JogarVSAmigo()
        {
            Console.WriteLine("Jogador 1, digite seu nome: ");
            string nomeJogador1 = Console.ReadLine();
            Jogador jogador1 = ObterJogador(nomeJogador1);
            if (jogador1 == null)
            {
                jogador1 = new Jogador(nomeJogador1);
                jogadores.Add(jogador1);
            }

            Console.WriteLine("Jogador 2, digite seu nome: ");
            string nomeJogador2 = Console.ReadLine();
            Jogador jogador2 = ObterJogador(nomeJogador2);
            if (jogador2 == null)
            {
                jogador2 = new Jogador(nomeJogador2);
                jogadores.Add(jogador2);
            }

            Jogo jogo = new Jogo(jogador1, jogador2);
            jogo.IniciarJogo();

            AtualizarRanking(jogo.Vencedor);
        }
        static void JogarVSComputador()
        {
            Console.WriteLine("Digite seu nome: ");
            string nomeJogador = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Início do jogo!");
            Console.WriteLine();

            bool fimDeJogo = false;
            char jogadorAtual = 'X';

            while (!fimDeJogo)
            {
                Tabuleiro();

                if (jogadorAtual == 'X')
                {
                    Console.WriteLine($"Sua vez, {nomeJogador}.");
                    Console.WriteLine("Digite a linha (0-2): ");
                    int linha = LerPosicao();
                    Console.WriteLine("Digite a coluna (0-2): ");
                    int coluna = LerPosicao();
                    Console.WriteLine();

                    if (ValidarPosicao(linha, coluna))
                    {
                        tabuleiro[linha, coluna] = jogadorAtual;

                        if (VerificarVitoria(jogadorAtual))
                        {
                            fimDeJogo = true;
                            Console.WriteLine($"Parabéns, {nomeJogador}! Você venceu.");
                        }
                        else if (VerificarEmpate())
                        {
                            fimDeJogo = true;
                            Console.WriteLine("Empate!");
                        }

                        jogadorAtual = 'O';
                    }
                    else
                    {
                        Console.WriteLine("Posição inválida. Tente novamente.");
                    }
                }
                else
                {
                    Console.WriteLine("Vez do computador...");
                    Console.WriteLine();

                    Random random = new Random();
                    int linha, coluna;

                    do
                    {
                        linha = random.Next(0, 3);
                        coluna = random.Next(0, 3);
                    }
                    while (!ValidarPosicao(linha, coluna));

                    tabuleiro[linha, coluna] = jogadorAtual;

                    if (VerificarVitoria(jogadorAtual))
                    {
                        fimDeJogo = true;
                        Console.WriteLine("O computador venceu!");
                    }
                    else if (VerificarEmpate())
                    {
                        fimDeJogo = true;
                        Console.WriteLine("Empate!");
                    }

                    jogadorAtual = 'X';
                }
            }
        }
        static void CadastrarJogador()
        {
            Console.WriteLine("Digite seu nome: ");
            string nome = Console.ReadLine();

            Console.WriteLine("Digite seu CPF(somente números): ");
            string cpf = Console.ReadLine();

            if (ValidarCPF(cpf))
            {
                Jogador jogador = new Jogador(nome, cpf);
                jogadores.Add(jogador);
                Console.WriteLine("Jogador cadastrado com sucesso!");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("CPF inválido. O jogador não foi cadastrado.");
                Console.WriteLine();
            }
        }

        static void MostrarRanking()
        {
            if (jogadores.Count == 0)
            {
                Console.WriteLine("Não tem jogadores cadastrados ainda.");
                Console.WriteLine();
                return;
            }

            Console.WriteLine("Ranking dos 10 jogadores com mais partidas vencidas:");
            Console.WriteLine();

            jogadores.Sort((j1, j2) => j2.PartidasVencidas.CompareTo(j1.PartidasVencidas));

            int count = Math.Min(10, jogadores.Count);
            for (int i = 0; i < count; i++)
            {
                Jogador jogador = jogadores[i];
                Console.WriteLine($"{i + 1}. {jogador.Nome} - Partidas vencidas: {jogador.PartidasVencidas}");
            }

            Console.WriteLine();
        }

        static Jogador ObterJogador(string nome)
        {
            return jogadores.Find(j => j.Nome == nome);
        }

        static void AtualizarRanking(Jogador vencedor)
        {
            if (vencedor != null)
            {
                vencedor.PartidasVencidas++;
            }
        }

        static bool ValidarCPF(string cpf)
        {
            if (cpf.Length != 11)
            {
                return false;
            }

            for (int i = 0; i < cpf.Length; i++)
            {
                if (!char.IsDigit(cpf[i]))
                {
                    return false;
                }
            }
            return true;
        }

        static void CarregarRanking()
        {
            if (File.Exists(rankingFilePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(rankingFilePath);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(';');
                        if (parts.Length == 2)
                        {
                            string nome = parts[0];
                            int partidasVencidas = int.Parse(parts[1]);
                            Jogador jogador = new Jogador(nome);
                            jogador.PartidasVencidas = partidasVencidas;
                            jogadores.Add(jogador);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocorreu um erro ao carregar o ranking: " + ex.Message);
                    Console.WriteLine();
                }
            }
        }

        static void SalvarRanking()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(rankingFilePath))
                {
                    foreach (Jogador jogador in jogadores)
                    {
                        writer.WriteLine($"{jogador.Nome};{jogador.PartidasVencidas}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao salvar o ranking: " + ex.Message);
                Console.WriteLine();
            }
        }

        static void CarregarJogadores()
        {
            if (File.Exists(jogadoresFilePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(jogadoresFilePath);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(';');
                        if (parts.Length == 2)
                        {
                            string nome = parts[0];
                            string cpf = parts[1];
                            Jogador jogador = new Jogador(nome, cpf);
                            jogadores.Add(jogador);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocorreu um erro ao carregar os jogadores: " + ex.Message);
                    Console.WriteLine();
                }
            }
        }
        static void SalvarJogadores()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(jogadoresFilePath))
                {
                    foreach (Jogador jogador in jogadores)
                    {
                        writer.WriteLine($"{jogador.Nome};{jogador.CPF}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao salvar os jogadores: " + ex.Message);
                Console.WriteLine();
            }
        }
    }

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

    class Jogo
    {
        public Jogador jogador1;
        public Jogador jogador2;
        public Jogador jogadorAtual;
        public char[,] tabuleiro;
        public int jogadas;

        public Jogador Vencedor { get; private set; }

        public Jogo(Jogador jogador1, Jogador jogador2)
        {
            this.jogador1 = jogador1;
            this.jogador2 = jogador2;
            jogadorAtual = jogador1;
            tabuleiro = new char[3, 3];
            jogadas = 0;
        }

        public void IniciarJogo()
        {
            bool fimDeJogo = false;

            while (!fimDeJogo)
            {
                Tabuleiro();

                Console.WriteLine($"{jogadorAtual.Nome}, é sua vez.");
                Console.WriteLine("Digite a linha (0-2): ");
                int linha = LerPosicao();
                Console.WriteLine("Digite a coluna (0-2): ");
                int coluna = LerPosicao();
                Console.WriteLine();

                if (ValidarPosicao(linha, coluna))
                {
                    tabuleiro[linha, coluna] = jogadorAtual == jogador1 ? 'X' : 'O';
                    jogadas++;

                    if (VerificarVitoria())
                    {
                        fimDeJogo = true;
                        Vencedor = jogadorAtual;
                        Console.WriteLine($"Parabéns, {jogadorAtual.Nome}! Você venceu.");
                        Console.WriteLine();
                    }
                    else if (jogadas == 9)
                    {
                        fimDeJogo = true;
                        Console.WriteLine("Empate!");
                        Console.WriteLine();
                    }

                    jogadorAtual = jogadorAtual == jogador1 ? jogador2 : jogador1;
                }
                else
                {
                    Console.WriteLine("Posição inválida. Tente novamente.");
                    Console.WriteLine();
                }
            }
        }

        public int LerPosicao()
        {
            int posicao;
            while (!int.TryParse(Console.ReadLine(), out posicao) || posicao < 0 || posicao > 2)
            {
                Console.WriteLine("Valor inválido. Digite novamente: ");
            }
            return posicao;
        }

        public bool ValidarPosicao(int linha, int coluna)
        {
            return tabuleiro[linha, coluna] == '\0';
        }

        public bool VerificarVitoria()
        {
            for (int i = 0; i < 3; i++)
            {
                if (tabuleiro[i, 0] != '\0' && tabuleiro[i, 0] == tabuleiro[i, 1] && tabuleiro[i, 1] == tabuleiro[i, 2])
                {
                    return true;
                }
                if (tabuleiro[0, i] != '\0' && tabuleiro[0, i] == tabuleiro[1, i] && tabuleiro[1, i] == tabuleiro[2, i])
                {
                    return true;
                }
            }
            if (tabuleiro[0, 0] != '\0' && tabuleiro[0, 0] == tabuleiro[1, 1] && tabuleiro[1, 1] == tabuleiro[2, 2])
            {
                return true;
            }
            if (tabuleiro[0, 2] != '\0' && tabuleiro[0, 2] == tabuleiro[1, 1] && tabuleiro[1, 1] == tabuleiro[2, 0])
            {
                return true;
            }
            return false;
        }


        public void Tabuleiro()
        {
            Console.WriteLine();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(tabuleiro[i, j] == '\0' ? " - " : $" {tabuleiro[i, j]} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
