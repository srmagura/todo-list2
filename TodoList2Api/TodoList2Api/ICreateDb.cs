using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TodoList2Api
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICreateDb" in both code and config file together.
    [ServiceContract]
    public interface ICreateDb
    {
        [OperationContract]
        void DoWork();
    }
}
