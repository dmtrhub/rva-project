using Shared.Contracts;
using Shared.Domain;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Client.Services
{
    public class CruiseServiceProxy : ClientBase<ICruiseManagementService>, ICruiseManagementService
    {
        public CruiseServiceProxy(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        public List<Voyage> GetAllVoyages() => Channel.GetAllVoyages();

        public List<Voyage> SearchVoyages(string searchTerm) => Channel.SearchVoyages(searchTerm);

        public bool AddVoyage(Voyage voyage) => Channel.AddVoyage(voyage);

        public bool UpdateVoyage(Voyage voyage) => Channel.UpdateVoyage(voyage);

        public bool DeleteVoyage(string voyageCode) => Channel.DeleteVoyage(voyageCode);

        public bool AddShip(Ship ship) => Channel.AddShip(ship);

        public bool AddPort(Port port) => Channel.AddPort(port);

        public List<Ship> GetAllShips() => Channel.GetAllShips();

        public List<Port> GetAllPorts() => Channel.GetAllPorts();

        public bool SimulateStateChange(string voyageCode) => Channel.SimulateStateChange(voyageCode);

        public bool ValidateVoyage(Voyage voyage) => Channel.ValidateVoyage(voyage);

        public bool ValidateShip(Ship ship) => Channel.ValidateShip(ship);

        public bool ValidatePort(Port port) => Channel.ValidatePort(port);

        public List<string> GetVoyageValidationErrors(Voyage voyage)
            => Channel.GetVoyageValidationErrors(voyage);

        public List<string> GetShipValidationErrors(Ship ship)
            => Channel.GetShipValidationErrors(ship);

        public List<string> GetPortValidationErrors(Port port)
            => Channel.GetPortValidationErrors(port);
    }
}