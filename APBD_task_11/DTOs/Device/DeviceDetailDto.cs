namespace APBD_task_11.DTOs.Device;

public class DeviceDetailDto
{
    public string Name { get; set; }
    public string DeviceTypeName { get; set; }
    public bool IsEnabled { get; set; }
    public object AdditionalProperties { get; set; }

    public CurrentEmployeeDto? CurrentEmployee { get; set; }

    public class CurrentEmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }
}