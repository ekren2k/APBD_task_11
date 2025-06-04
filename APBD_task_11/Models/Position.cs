﻿namespace APBD_task_11.Models;

public partial class Position
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int MinExpYears { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
