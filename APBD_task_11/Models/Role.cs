using System.ComponentModel.DataAnnotations;

namespace APBD_task_11.Models;

public class Role
{
    public int Id {get;set;}
    [Length(1,60)]
    public string Name {get;set;}
}