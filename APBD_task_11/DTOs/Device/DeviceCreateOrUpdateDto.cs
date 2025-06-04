using System.ComponentModel.DataAnnotations;

namespace APBD_task_11.DTOs.Device;

public class DeviceCreateOrUpdateDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string DeviceTypeName { get; set; }

    [Required]
    public bool IsEnabled { get; set; }

    [Required]
    public object AdditionalProperties { get; set; }
}