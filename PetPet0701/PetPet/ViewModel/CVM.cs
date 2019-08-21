using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetPet.Models;

namespace PetPet.ViewModel
{
    public class CVM
    {
        public List<PetType> ptype { get; set; }
        public List<PetVariety> variety { get; set; }
    }
}