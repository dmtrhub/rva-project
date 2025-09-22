using Shared.Commands;
using Client.ViewModels;
using System.Collections.ObjectModel;
using Client.Services;

namespace Client.Commands;

public class AddVoyageCommand : ICommand
{
    private readonly ObservableCollection<VoyageViewModel> _voyages;
    private readonly VoyageViewModel _voyage;
    private readonly CruiseServiceProxy _service;

    public AddVoyageCommand(ObservableCollection<VoyageViewModel> voyages, VoyageViewModel voyage, CruiseServiceProxy service)
    {
        _voyages = voyages;
        _voyage = voyage;
        _service = service;
    }

    public void Execute()
    {
        if (_service.AddVoyage(_voyage.Model))
            _voyages.Add(_voyage);
    }

    public void Undo()
    {
        if (_service.DeleteVoyage(_voyage.Code))
            _voyages.Remove(_voyage);
    }

    public string Description => $"Add Voyage {_voyage.Code}";
}