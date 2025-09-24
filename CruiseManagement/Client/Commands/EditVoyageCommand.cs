using Shared.Commands;
using Client.ViewModels;
using System.Collections.ObjectModel;
using Client.Services;

namespace Client.Commands;

public class EditVoyageCommand : ICommand
{
    private readonly ObservableCollection<VoyageViewModel> _voyages;
    private readonly VoyageViewModel _oldVoyage;
    private readonly VoyageViewModel _newVoyage;
    private readonly MainViewModel _parentVm;
    private readonly CruiseServiceProxy _service;

    public EditVoyageCommand(ObservableCollection<VoyageViewModel> voyages, VoyageViewModel oldVoyage, VoyageViewModel newVoyage, MainViewModel parentVm, CruiseServiceProxy service)
    {
        _voyages = voyages;
        _oldVoyage = oldVoyage;
        _newVoyage = newVoyage;
        _parentVm = parentVm;
        _service = service;
    }

    public void Execute()
    {
        int index = _voyages.IndexOf(_oldVoyage);
        if (index >= 0)
        {
            _voyages[index] = _newVoyage;
            _parentVm.SelectedVoyage = _newVoyage;

            _service.UpdateVoyage(_newVoyage.Model);
        }
    }

    public void Undo()
    {
        int index = _voyages.IndexOf(_newVoyage);
        if (index >= 0)
        {
            _voyages[index] = _oldVoyage;
            _parentVm.SelectedVoyage = _oldVoyage;

            _service.UpdateVoyage(_oldVoyage.Model);
        }
    }

    public string Description => $"Edit Voyage {_oldVoyage.Code}";
}