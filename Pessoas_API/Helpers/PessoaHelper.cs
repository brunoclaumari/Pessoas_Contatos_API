using Pessoas_API.Entidades;

namespace Pessoas_API.Helpers
{
    public class PessoaHelper
    {
        public PessoaHelper()
        {
            
        }

        public async Task TransfereEntradaParaPessoa(Pessoa entrada, Pessoa pessoaExistente)
        {
            long pessoaId = pessoaExistente.Id;
            var contatosNovosEntrada = entrada.Contatos.FindAll(x => x.Id <= 0);
            var contatosExistentesEntrada = entrada.Contatos.FindAll(x => x.Id > 0);

            pessoaExistente.Nome = entrada.Nome;
            pessoaExistente.Email = entrada.Email;
            pessoaExistente.Contatos = entrada.Contatos;
            //pessoaExistente.Contatos.ForEach(c =>
            //{
            //    var contatoEntrada = contatosExistentesEntrada.FirstOrDefault(x => c.Id == x.Id);
            //    if(contatoEntrada != null && contatoEntrada.Id > 0)
            //    {
            //        c.Nome = contatoEntrada.Nome;
            //        c.Email = contatoEntrada.Email;
            //        c.Telefone = contatoEntrada.Telefone;
            //        c.Whatsapp = contatoEntrada.Whatsapp;
            //        c.PessoaId = contatoEntrada.PessoaId;
            //    }
            //});
            //contatosNovosEntrada.ForEach(c => c.PessoaId = pessoaId);
            //pessoaExistente.Contatos.AddRange(contatosNovosEntrada);
        }

        public async Task TransfereEntradaParaContato(Contato entrada, Contato contatoExistente)
        {
            long pessoaId = contatoExistente.Id;

            contatoExistente.Nome = entrada.Nome;
            contatoExistente.Email = entrada.Email;
            contatoExistente.Telefone = entrada.Telefone;
            contatoExistente.Whatsapp = entrada.Whatsapp;
        }


    }
}
