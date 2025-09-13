using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Persistence
{
    public interface IDataPersistenceStrategy
    {
        void SaveData(IEnumerable<Voyage> voyages, IEnumerable<Ship> ships, IEnumerable<Port> ports);

        (IEnumerable<Voyage>, IEnumerable<Ship>, IEnumerable<Port>) LoadData();
    }
}