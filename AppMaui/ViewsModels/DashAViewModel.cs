using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using AppMaui.Core.Services;
using AppMaui.Core.Services.Local;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.ViewsModels
{
    public partial class DashAViewModel : ObservableObject
    {
        [ObservableProperty]
        private DashBoardADTO dash;

        private Usuario _usuario;
        private readonly AlunoService _alunoService;

        public DashAViewModel(AlunoService alunoService)
        {
            _alunoService = alunoService;
            _ = CarregarDashBoard();
        }

        public async Task CarregarDashBoard()
        {
            
            _usuario = SessaoService.UsuarioLogado;
            
            if (_usuario != null && _usuario.Tipo == "Aluno")
            {                
                Dash = await _alunoService.GerarDashBoard(_usuario.Id);
                Debug.WriteLine($"{Dash.Capa}");
            }
                
        }
    }
}
