using System.ComponentModel.DataAnnotations;

namespace APBD_task_11.Models;

public class Account
{
    public int Id {get;set;}
    [Length(1,60)]
    public string Username {get;set;}
    [Length(1,100)]
    public string Password {get;set;}
    
    public int EmployeeId {get;set;}
    public Employee Employee {get;set;}
    public int RoleId {get;set;}
    public Role Role {get;set;}
}