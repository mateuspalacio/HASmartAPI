using System;
using System.Collections.Generic;
using HASmart.Core.Architecture;
using HASmart.Core.Repositories;


namespace HASmart.Core.Services {
    interface IServiceBase<T> where T : AggregateRoot { }
}
