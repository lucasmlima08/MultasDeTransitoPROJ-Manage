using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace Sistema_de_Multas_do_Detran
{
    public partial class aplicacao : Form
    {
        MySqlConnection conexao = new MySqlConnection("Server = localhost; Uid = root; Password = root; Database = Sistema; Port = 80");
        MySqlCommand comando = new MySqlCommand();
        MySqlDataReader reader;

        String[] arrayRadares;

        public aplicacao()
        {
            InitializeComponent();

            // Realizar Conexão com o Banco de Dados.
            conexao.Open();
            comando.Connection = conexao;

            // Limpar campos.
            limparLogin();
            limparCadastrarVeiculo();

            // Inclui a data..
            data.Text = "Acesso: " + String.Format("{0:dd/MM/yyyy}", DateTime.Now.Date) + " (" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ")";
        }

        /* FUNÇÕES SIMPLES */

        private void limparLogin()
        {
            entLogin.Text = "";
            entSenha.Text = "";
        }

        private void limparCadastrarRadar()
        {
            localizacao09.Text = "";
            detalhes09.Text = "";
            velocidade09.Text = "";
            peso09.Text = "";
        }

        private void limparCadastrarVeiculo()
        {
            placa06.Text = "";
            chassis06.Text = "";
            marca06.Text = "";
            modelo06.Text = "";
            cpf06.Text = "";
        }

        private void limparCadastrarProprietario()
        {
            cpfProprietario5.Text = "";
            rgProprietario5.Text = "";
            nomeProprietario5.Text = "";
            cidadeProprietario5.Text = "";
            estadoProprietario5.Text = "";
        }

        private void limparAlterarRadar()
        {
            localRadares18.Text = "";
            detalhesRadares18.Text = "";
            pesoRadares18.Text = "";
            velocidadeRadares18.Text = "";
        }

        private void limparAlterarProprietario()
        {
            cpfAntigo13.Text = "";
            rgAntigo13.Text = "";
            nomeAtual13.Text = "";
            cpfAtual13.Text = "";
            rgAtual13.Text = "";
            cidadeAtual13.Text = "";
            estadoAtual13.Text = "";
        }

        private void limparRemoverVeiculo()
        {
            chassisRemov.Text = "";
            placaRemov.Text = "";
            motivoRemov.Text = "";
        }

        private void chamarBotoes()
        {
            botInicio.Visible = true;
            botCadastrarRadares.Visible = true;
            botConsultarRadares.Visible = true;
            botAlterarLRadares.Visible = true;
            botHistoricoDeMultas.Visible = true;
            botAlterarProprietarios.Visible = true;
            botCadastrarVeiculos.Visible = true;
            botRemoverVeiculos.Visible = true;
            botConsultarVeiculos.Visible = true;
            botConsultarProprietarios.Visible = true;
            botCadastrarProprietarios.Visible = true;
            botSair.Visible = true;
        }


        /* ---------------- LOGIN DE ADMINISTRADOR -------------------- */

        private void logar_Click(object sender, EventArgs e)
        {

            try
            {
                String comando = "select count(*) from administrador where LOGIN = '" + entLogin.Text + "' and SENHA = '" + entSenha.Text + "'";

                // Entra aqui se encontra algum registro do Administrador.
                if (count(comando) >= 1)
                {
                    comando = "select NOME, CPF from administrador where LOGIN = '" + entLogin.Text + "' and SENHA = '" + entSenha.Text + "'";

                    String[] dados = { "NOME", "CPF" };

                    // Recebe os dados do administrador.
                    String[] saida = new String[dados.Length];
                    saida = select(comando, dados);

                    // Coloca os dados na página principal.
                    administrador.Text = "Administrador: " + saida[0];
                    cpfAdministrador.Text = "CPF: " + saida[1];

                    limparLogin();
                    chamarBotoes();

                    telas.SelectedTab = telaInicial;
                    reader.Close();
                }
                else
                {
                    MessageBox.Show("Administrador não encontrado!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossível estabelecer a conexão: " + ex.Message);
            }
        } // Realizar o Login.

        private void botSair_Click(object sender, EventArgs e) { Environment.Exit(0); } // Sair.

        /* ---------------- MUDANÇAS DE TELA [CADASTRO] -------------------- */

        private void botCadastrarRadares_Click(object sender, EventArgs e) { telas.SelectedTab = telaCadastrarRadares; } // Tela Cadastrar Local.

        private void botCadastrarVeiculos_Click(object sender, EventArgs e) { telas.SelectedTab = telaCadastrarVeiculos; } // Tela Cadastrar Veículo.
        
        private void botCadastrarProprietarios_Click(object sender, EventArgs e) { telas.SelectedTab = telaCadastrarProprietarios; } // Tela Cadastrar Proprietários.

        /* ---------------- MUDANÇAS DE TELA [ALTERAÇÕES] -------------------- */

        private void botAlterarRadares_Click(object sender, EventArgs e) { telas.SelectedTab = telaAlterarRadares; chamarRadares(); } // Tela Alterar Local.

        private void botAlterarProprietarios_Click(object sender, EventArgs e) { telas.SelectedTab = telaAlterarProprietarios; } // Tela Alterar Proprietário.

        /* ---------------- MUDANÇAS DE TELA [CONSULTAS] -------------------- */

        private void botConsultarRadares_Click(object sender, EventArgs e)
        {
            telas.SelectedTab = telaConsultarRadares;
            boxConsultarRadar();
        } // Tela Consultar Radares.   

        private void botConsultarVeiculos_Click(object sender, EventArgs e)
        {
            telas.SelectedTab = telaConsultarVeiculos;
            boxListarVeiculos();
        } // Tela Situação do Veículo.

        private void botConsultarProprietarios_Click(object sender, EventArgs e)
        {
            telas.SelectedTab = telaConsultarProprietarios;
            boxListarProprietarios();
        } // Tela Situação do Proprietário.

        private void botHistoricoDeMultas_Click(object sender, EventArgs e)
        {
            telas.SelectedTab = telaHistoricoDeMultas;
            boxListarMultas();
        } // Tela Histórico de Multas.

        /* ---------------- MUDANÇAS DE TELA [OUTRAS OPÇÕES] -------------------- */

        private void botInicio_Click(object sender, EventArgs e) { telas.SelectedTab = telaInicial; } // Tela Início.

        private void botRemoverVeiculos_Click(object sender, EventArgs e) { telas.SelectedTab = telaRemoverVeiculos; } // Tela Remover Veículo.

        
        /* ----------------- EVENTOS DE BOTÕES -------------------------- */

        private void cadastrarRadar_Click(object sender, EventArgs e) { funcaoCadastrarRadar(); }

        private void alterarRadar_Click(object sender, EventArgs e) { funcaoAlterarRadar(); }

        private void alterarProprietario_Click(object sender, EventArgs e) { funcaoAlterarProprietario(); }

        private void cadastrarVeiculo_Click(object sender, EventArgs e) { funcaoCadastrarVeiculo(); }

        private void removerVeiculo_Click(object sender, EventArgs e) { funcaoRemoverVeiculo(); }

        private void botCadastrarProprietario_Click(object sender, EventArgs e) { funcaoCadastrarProprietario(); }

        /* ----------------- FUNÇÕES [CADASTRO] -------------------------- */

        private void funcaoCadastrarRadar()
        {
            if (localizacao09.Text.Equals("") || detalhes09.Text.Equals("") || velocidade09.Text.Equals("") || peso09.Text.Equals(""))
            {
                MessageBox.Show("Preencha todos os campos");
            }
            else
            {
                String comando = "insert into radares(LOCALIZACAO, DETALHES, PESO) values('" + localizacao09.Text + "','" + detalhes09.Text + "','" + double.Parse(velocidade09.Text.ToString()) + "');";
                
                if (insert(comando))
                {
                    MessageBox.Show("Radar Cadastrado com Sucesso");
                }
                else
                {
                    MessageBox.Show("Problemas no Banco de Dados!");
                }
                limparCadastrarVeiculo();
            }
        }

        private void funcaoCadastrarProprietario()
        {
            if (cpfProprietario5.Text.Equals("") || rgProprietario5.Text.Equals("") || nomeProprietario5.Text.Equals("")
                                                 || cidadeProprietario5.Text.Equals("") || estadoProprietario5.Text.Equals(""))
            {
                MessageBox.Show("Preencha todos os campos");
            }
            else
            {
                String comando = "insert into proprietarios(CPF, RG, NOME, CIDADE, ESTADO, PONTOS, MULTAS) values('" + int.Parse(cpfProprietario5.Text) + "','" + int.Parse(rgProprietario5.Text) + "',"+
                                 "'" + nomeProprietario5.Text + "','" + cidadeProprietario5.Text + "','" + estadoProprietario5.Text + "','" + 0 + "','" + 0 + "')";

                if (insert(comando))
                {
                    MessageBox.Show("Proprietário Cadastrado com Sucesso");
                }
                else
                {
                    MessageBox.Show("Problemas no Banco de Dados!");
                }
                limparCadastrarProprietario();
            }
        }

        public void funcaoCadastrarVeiculo()
        {
            if (placa06.Text.Equals("") || marca06.Text.Equals("") || modelo06.Text.Equals("") || cpf06.Text.Equals(""))
            {
                MessageBox.Show("Preencha todos os campos");
            }
            else
            {
                String comando = "insert into veiculos(PLACA, MARCA, MODELO, RESTRICAO, PROPRIETARIO) values('" + placa06.Text + "','" + marca06.Text + "',"+
                                          "'" + modelo06.Text + "','******','" + int.Parse(cpf06.Text.ToString()) + "');";

                if (insert(comando))
                {
                    MessageBox.Show("Veículo Cadastrado com Sucesso");
                }
                else
                {
                    MessageBox.Show("Problemas no Banco de Dados!");
                }
                limparCadastrarVeiculo();
            }
        }

        /* ----------------- FUNÇÕES [ALTERAÇÕES] -------------------------- */ // CONCERTAR!!!!!!!!

        public void funcaoAlterarRadar()
        {
            if (localRadares18.Text.Equals("") || detalhesRadares18.Text.Equals("") || pesoRadares18.Text.Equals("")
                                               || listaDeRadares.Text.Equals("") || velocidadeRadares18.Text.Equals(""))
            {
                MessageBox.Show("Preencha todos os dados!");
            }
            else
            {
                String comando = "UPDATE radares set LOCALIZACAO='" + localRadares18.Text + "', DETALHES='" + detalhesRadares18.Text + "'," +
                                 "PESO='" + double.Parse(pesoRadares18.Text) + "' WHERE ID='" + numRadar() + "';";
                    
                if (update(comando))
                {
                    chamarRadares();
                    MessageBox.Show("Radar Alterado com Sucesso!");
                }
                else
                {
                    MessageBox.Show("Problemas no Banco de Dados!");
                }
            }
            limparAlterarRadar();
        }
        
        public void funcaoAlterarProprietario()
        {
            if (cpfAntigo13.Text.Equals("") || rgAntigo13.Text.Equals("") || nomeAtual13.Text.Equals("") || cpfAtual13.Text.Equals("")
                                            || rgAtual13.Text.Equals("") || cidadeAtual13.Text.Equals("") || estadoAtual13.Text.Equals(""))
            {
                MessageBox.Show("Preencha todos os campos");
            }
            else
            {
                String comando = "SELECT COUNT(*) FROM proprietarios WHERE CPF='" + int.Parse(cpfAntigo13.Text) + "' AND RG='" + int.Parse(rgAntigo13.Text) + "';";
                    
                if (count(comando) == 0) 
                {
                    MessageBox.Show("Proprietário não Encontrado");
                }
                else
                {
                    comando = "UPDATE proprietarios SET CPF='"+ int.Parse(cpfAtual13.Text) +"', RG='"+ int.Parse(rgAtual13.Text) +"',"+
                              "NOME='"+ nomeAtual13.Text +"', CIDADE='"+ cidadeAtual13.Text +"', ESTADO='"+ estadoAtual13.Text +"',"+
                              "PONTOS='"+0+"', MULTAS='"+0+"' WHERE CPF='"+ int.Parse(cpfAntigo13.Text) +"' and RG='"+ int.Parse(rgAntigo13.Text) +"';";
                    
                    if (update(comando) == true)
                    {
                        MessageBox.Show("Proprietário Alterado com Sucesso!");
                    }
                    else
                    {
                        MessageBox.Show("Houve algum problema!");
                    }
                }
                limparAlterarProprietario();
            }
        }

        /* ----------------- FUNÇÕES [REMOÇÃO] -------------------------- */

        private void funcaoRemoverVeiculo()
        {
            if (chassisRemov.Text.Equals("") || placaRemov.Equals("") || motivoRemov.Equals(""))
            {
                MessageBox.Show("Preencha todos os dados!");
            }
            else
            {
                String comando = "SELECT COUNT(*) FROM veiculos WHERE placa='"+placaRemov.Text+"';";

                if (count(comando) > 0)
                {
                    comando = "DELETE FROM veiculos WHERE placa='" + placaRemov.Text + "';";

                    if (delete(comando))
                    {
                        MessageBox.Show("Veículo deletado!");
                    }
                    else
                    {
                        MessageBox.Show("Problemas no banco de dados!");
                    }
                }
                else
                {
                    MessageBox.Show("Veículo não encontrado!");
                }
               
            }
            limparRemoverVeiculo();
        }
        
        /* ----------------- FUNÇÕES [CONSULTAS] -------------------------- */

        private void boxListarVeiculos()
        {
            consultarSituacaoVeiculos.Items.Clear();

            String[] dados = new String[4];

            String comando = "SELECT COUNT(*) FROM veiculos;";

            // Entra aqui se encontra algum registro do Administrador.
            if (count(comando) > 0)
            {
                // Comandos de listagem de registros.
                comando = "select * from veiculos;";
                String[] saida = {"PLACA","MARCA","MODELO","RESTRICAO"};

                boxVeiculos(comando, saida);
            }
            else
            {
                MessageBox.Show("Não existe veículo cadastrado!");
            }
        }

        private void boxListarProprietarios()
        {
            consultarSituacaoProprietarios.Items.Clear();

            String[] dados = new String[6];

            String comando = "SELECT COUNT(*) FROM proprietarios;";

            // Entra aqui se encontra algum registro do Administrador.
            if (count(comando) > 0)
            {
                // Comandos de listagem de registros.
                comando = "SELECT * FROM proprietarios;";
                String[] saida = { "NOME", "CPF", "CIDADE", "ESTADO", "PONTOS", "MULTAS" };

                boxProprietarios(comando, saida);

            }
            else
            {
            MessageBox.Show("Não existe proprietário cadastrado!");
            }
        }

        private void boxConsultarRadar()
        {
            consultarRadares.Items.Clear();

            String[] dados = new String[4];

            String comando = "SELECT COUNT(*) FROM radares;";
            
            // Entra aqui se encontra algum registro do Administrador.
            if (count(comando) > 0)
            {
                // Comandos de listagem de registros.
                comando = "SELECT * FROM radares;";
                String[] saida = { "ID", "LOCALIZACAO", "DETALHES", "PESO" };

                boxRadares(comando, saida);

            }
            else
            {
                MessageBox.Show("Não existe radares cadastrado!");
            }
        }
        
        private void chamarRadares()
        {

            String comando = "SELECT COUNT(*) FROM radares;";
            int quantRegistros = count(comando);

            // Entra aqui se encontra algum registro do Radar.
            if (quantRegistros > 0)
            {
                arrayRadares = new String[quantRegistros];

                comando = "SELECT * FROM radares";

                String[] dados = { "ID", "LOCALIZACAO", "DETALHES" };

                selectRadares(comando, dados);

            }
            else
            {
                MessageBox.Show("Não existe proprietário cadastrado!");
            }
        } // Chama a lista de radares.

        private int numRadar()
        {
            for (int i = 0; i < arrayRadares.Length; i++)
            {
                if (arrayRadares[i].Equals(listaDeRadares.Text))
                {
                    return i + 1;
                }
            }
            return 0;
        } // Encontra o número do radar pela escolha no comboBox.
        
        private void boxListarMultas()
        {
            historicoDeMultas.Items.Clear();

            String comando = "SELECT COUNT(*) FROM multas;";

            // Entra aqui se encontra algum registro do Administrador.
            if (count(comando) > 0)
            {
                // Comandos de listagem de registros.
                comando = "SELECT * FROM multas;";
                String[] saida = { "PLACA", "RADAR", "VELOCIDADE", "VALOR", "DATA", "HORA" };


                boxMultas(comando, saida);
            }
            else
            {
                MessageBox.Show("Não existe multas cadastrado!");
            }
        }


        /* ----------------- FUNÇÕES [SQL] -------------------------- */

        public Boolean insert(String comando)
        {
            try
            {
                this.comando.CommandText = comando;
                this.comando.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Boolean update(String comando)
        {
            try
            {
                this.comando.CommandText = comando;
                this.comando.ExecuteNonQuery();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public Boolean delete(String comando)
        {
            try
            {
                this.comando.CommandText = comando;
                this.comando.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public String[] select(String comando, String[] dados)
        {
            String[] saida = new String[dados.Length];

            try
            {
                this.comando.CommandText = comando;
                reader = this.comando.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < dados.Length; i++)
                    {
                        saida[i] = reader[dados[i]].ToString();
                    }
                }

                reader.Close();
                return saida;
            }
            catch (Exception ex)
            {
                reader.Close();
                return saida;
            }
        }

        public ListView selectRegistros(String comando, String[] dados)
        {
            String[] coluna = new String[dados.Length];
            
            ListView lista = new ListView();
            ListViewItem item = new ListViewItem();

            try
            {
                this.comando.CommandText = comando;
                reader = this.comando.ExecuteReader();

                int acm = 0;
                while (reader.Read())
                {
                    for (int i = 0; i < dados.Length; i++)
                    {
                        coluna[i] = reader[dados[i]].ToString();
                    }
                    item = new ListViewItem(coluna);
                    lista.Items.Add(item);
                }

                reader.Close();
                return lista;
            }
            catch (Exception ex)
            {
                reader.Close();
                return lista;
            }
        }

        public void boxProprietarios(String comando, String[] dados)
        {
            String[] coluna = new String[dados.Length];

            ListViewItem item = new ListViewItem();

            try
            {
                this.comando.CommandText = comando;
                reader = this.comando.ExecuteReader();

                int acm = 0;
                while (reader.Read())
                {
                    for (int i = 0; i < dados.Length; i++)
                    {
                        coluna[i] = reader[dados[i]].ToString();
                    }
                    item = new ListViewItem(coluna);
                    consultarSituacaoProprietarios.Items.Add(item);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                reader.Close();
            }
        }

        public void boxVeiculos(String comando, String[] dados)
        {
            String[] coluna = new String[dados.Length];

            ListViewItem item = new ListViewItem();

            try
            {
                this.comando.CommandText = comando;
                reader = this.comando.ExecuteReader();

                int acm = 0;
                while (reader.Read())
                {
                    for (int i = 0; i < dados.Length; i++)
                    {
                        coluna[i] = reader[dados[i]].ToString();
                    }
                    item = new ListViewItem(coluna);
                    consultarSituacaoVeiculos.Items.Add(item);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                reader.Close();
            }
        }

        public void boxRadares(String comando, String[] dados)
        {
            String[] coluna = new String[dados.Length];

            ListViewItem item = new ListViewItem();

            try
            {
                this.comando.CommandText = comando;
                reader = this.comando.ExecuteReader();

                int acm = 0;
                while (reader.Read())
                {
                    for (int i = 0; i < dados.Length; i++)
                    {
                        coluna[i] = reader[dados[i]].ToString();
                    }
                    item = new ListViewItem(coluna);
                    consultarRadares.Items.Add(item);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                reader.Close();
            }
        }

        public void boxMultas(String comando, String[] dados)
        {
            String[] coluna = new String[dados.Length];

            ListViewItem item = new ListViewItem();

            try
            {
                this.comando.CommandText = comando;
                reader = this.comando.ExecuteReader();

                int acm = 0;
                while (reader.Read())
                {
                    for (int i = 0; i < dados.Length; i++)
                    {
                        coluna[i] = reader[dados[i]].ToString();
                    }
                    item = new ListViewItem(coluna);
                    historicoDeMultas.Items.Add(item);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                reader.Close();
            }
        }

        public void selectRadares(String comando, String[] dados)
        {
            listaDeRadares.SelectedIndex = -1;
            listaDeRadares.Items.Clear();

            String[] saida = new String[dados.Length];
            String modelo = "";

            try
            {
                this.comando.CommandText = comando;
                reader = this.comando.ExecuteReader();

                int acm = 0;
                while (reader.Read())
                {
                    for (int i = 0; i < dados.Length; i++)
                    {
                        saida[i] = reader[dados[i]].ToString();
                    }
                    
                    modelo = "Radar " + saida[0] + " - " + saida[1] + " [" + saida[2] + "]";
                    listaDeRadares.Items.Add(modelo);
                    arrayRadares[acm] = modelo;
                    acm++;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                reader.Close();
            }
        }

        public int count(String comando)
        {
            try
            {
                this.comando.CommandText = comando;
                String acm = this.comando.ExecuteScalar().ToString();
                
                return int.Parse(acm);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
