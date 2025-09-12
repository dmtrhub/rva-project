using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Commands
{
    public interface ICommand
    {
        void Execute();

        void Undo();

        string Description { get; }
    }
}