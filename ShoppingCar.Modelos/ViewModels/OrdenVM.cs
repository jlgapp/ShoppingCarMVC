﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCar.Modelos.ViewModels
{
    public class OrdenVM
    {
        public Orden Orden { get; set; }
        public IEnumerable<OrdenDetalle> OrdenDetalle { get; set; }
    }
}
