using Shared.Domain;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Shared.Contracts
{
    [ServiceContract]
    public interface ICruiseManagementService
    {
        [OperationContract]
        List<Voyage> GetAllVoyages();

        [OperationContract]
        List<Voyage> SearchVoyages(string searchTerm);

        [OperationContract]
        bool AddVoyage(Voyage voyage);

        [OperationContract]
        bool UpdateVoyage(Voyage voyage);

        [OperationContract]
        bool DeleteVoyage(string voyageCode);

        [OperationContract]
        bool AddShip(Ship ship);

        [OperationContract]
        bool AddPort(Port port);

        [OperationContract]
        List<Ship> GetAllShips();

        [OperationContract]
        List<Port> GetAllPorts();

        [OperationContract]
        bool SimulateStateChange(string voyageCode);

        [OperationContract]
        bool ValidateVoyage(Voyage voyage);

        [OperationContract]
        bool ValidateShip(Ship ship);

        [OperationContract]
        bool ValidatePort(Port port);
    }
}