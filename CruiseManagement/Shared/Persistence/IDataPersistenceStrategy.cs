using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Persistence
{
    public interface IDataPersistenceStrategy
    {
        void SaveData(List<Voyage> voyages, List<Ship> ships, List<Port> ports);

        (List<Voyage>, List<Ship>, List<Port>) LoadData();
    }
}