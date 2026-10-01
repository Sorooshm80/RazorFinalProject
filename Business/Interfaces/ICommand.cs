using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICommand
    {
        OperationResult Execute();
    }

    public interface ICommandInvoker
    {
        OperationResult Run(ICommand command);
    }
}
