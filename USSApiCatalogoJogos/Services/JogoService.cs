using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USSApiCatalogoJogos.Entities;
using USSApiCatalogoJogos.InputModel;
using USSApiCatalogoJogos.Repositories;
using USSApiCatalogoJogos.ViewModel;

namespace USSApiCatalogoJogos.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public async Task<List<JogoViewModel>> Obter(int pagina, int quantidade)
        {
            var jogos = await _jogoRepository.Obter(pagina, quantidade);

            return jogos.Select(jogo => new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            }).ToList();
        }

        public async Task<JogoViewModel> Obter(Guid id)
        {
            var jogo = await _jogoRepository.Obter(id);

            if (jogo == null)
                return null;

            return new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
        }

        public async Task<JogoViewModel> Inserir(JogoInputModel jogo)
        {
            var entidadeJogo = await _jogoRepository.Obter(jogo.Nome, jogo.Produtora);

            if (entidadeJogo.Count > 0)
                throw new Exception();

            var jogoInsert = new Jogo
            {
                Id = Guid.NewGuid(),
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };

            await _jogoRepository.Inserir(jogoInsert);

            return new JogoViewModel
            {
                Id = jogoInsert.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
        }

        public async Task Atualizar(Guid id, JogoInputModel jogo)
        {
            var entidadeJogo = await _jogoRepository.Obter(id);

            if (entidadeJogo == null)
                throw new Exception();

            entidadeJogo.Nome = jogo.Nome;
            entidadeJogo.Produtora = jogo.Produtora;
            entidadeJogo.Preco = jogo.Preco;

            await _jogoRepository.Atualizar(entidadeJogo);
        }

        public async Task Atualizar(Guid id, double preco)
        {
            var entidadeJogo = await _jogoRepository.Obter(id);

            if (entidadeJogo == null)
                throw new Exception();

            entidadeJogo.Preco = preco;

            await _jogoRepository.Atualizar(entidadeJogo);
        }

        public async Task Remover(Guid id)
        {
            var jogo = await _jogoRepository.Obter(id);

            if (jogo == null)
                throw new Exception();

            await _jogoRepository.Remover(id);
        }

        public void Dispose()
        {
            _jogoRepository?.Dispose();
        }
    }
}
