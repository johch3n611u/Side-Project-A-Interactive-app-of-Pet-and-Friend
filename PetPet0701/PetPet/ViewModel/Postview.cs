using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetPet.Models;

namespace PetPet.ViewModel
{
    public class Postview
    {
        public List<Post> post_no { get; set; }
        public List<Post_img> postimg { get; set; }
    }
}