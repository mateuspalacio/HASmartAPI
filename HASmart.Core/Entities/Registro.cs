using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Architecture;
using HASmart.Core.Extensions;
using HASmart.Core.Validation;


namespace HASmart.Core.Entities {
    public class Registro : AggregateRoot {
        
        public Cidadao cidadao{ get; set;}

        public Medicao medicao{ get; set;}
    }
}