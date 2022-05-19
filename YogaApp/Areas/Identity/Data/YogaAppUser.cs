using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace YogaApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the YogaAppUser class
public class YogaAppUser : IdentityUser
{

    public YogaAppUser() : base()
    {
        
    }

    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int UserType { get; set; }

   [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int YogaUserID { get; set; }





}

