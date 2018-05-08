using Horseplay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Horseplay.ViewModels
{
    public class AddingUserViewModel
    {
        public User User { get; set; }
        public Invitation Invitation { get; set; }
    }
}