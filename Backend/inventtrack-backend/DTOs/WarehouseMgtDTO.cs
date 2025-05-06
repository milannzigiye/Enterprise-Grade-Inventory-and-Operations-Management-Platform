using System.ComponentModel.DataAnnotations;

public class WarehouseDto
{
    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public decimal CapacitySquareFeet { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public ICollection<WarehouseZoneDto> Zones { get; set; }
    public int TotalZones => Zones?.Count ?? 0;
    public int WorkerCount { get; set; }
    public decimal CurrentUtilizationPercent { get; set; }
}

public class WarehouseCreateDto
{
    [Required]
    [StringLength(100)]
    public string WarehouseName { get; set; }

    [Required]
    [StringLength(200)]
    public string Address { get; set; }

    [Required]
    [StringLength(100)]
    public string City { get; set; }

    [Required]
    [StringLength(50)]
    public string State { get; set; }

    [Required]
    [StringLength(20)]
    public string ZipCode { get; set; }

    [Required]
    [StringLength(50)]
    public string Country { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [Range(1, int.MaxValue)]
    public decimal CapacitySquareFeet { get; set; }
}

public class WarehouseUpdateDto
{
    [StringLength(100)]
    public string WarehouseName { get; set; }

    [StringLength(200)]
    public string Address { get; set; }

    [StringLength(100)]
    public string City { get; set; }

    [StringLength(50)]
    public string State { get; set; }

    [StringLength(20)]
    public string ZipCode { get; set; }

    [StringLength(50)]
    public string Country { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [Range(1, int.MaxValue)]
    public decimal? CapacitySquareFeet { get; set; }

    public bool? IsActive { get; set; }
}

public class WarehouseZoneDto
{
    public int ZoneId { get; set; }
    public int WarehouseId { get; set; }
    public string ZoneName { get; set; }
    public string ZoneType { get; set; }
    public int FloorLevel { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public ICollection<StorageLocationDto> Locations { get; set; }
    public int TotalLocations => Locations?.Count ?? 0;
    public decimal UtilizationPercent { get; set; }
}

public class WarehouseZoneCreateDto
{
    [Required]
    public int WarehouseId { get; set; }

    [Required]
    [StringLength(50)]
    public string ZoneName { get; set; }

    [Required]
    [StringLength(50)]
    public string ZoneType { get; set; }

    public int FloorLevel { get; set; }

    [StringLength(200)]
    public string Description { get; set; }
}

public class WarehouseZoneUpdateDto
{
    [StringLength(50)]
    public string ZoneName { get; set; }

    [StringLength(50)]
    public string ZoneType { get; set; }

    public int? FloorLevel { get; set; }

    [StringLength(200)]
    public string Description { get; set; }

    public bool? IsActive { get; set; }
}

public class StorageLocationDto
{
    public int LocationId { get; set; }
    public int ZoneId { get; set; }
    public string Aisle { get; set; }
    public string Rack { get; set; }
    public string Bin { get; set; }
    public string LocationCode { get; set; }
    public decimal MaxCapacity { get; set; }
    public decimal CurrentUtilization { get; set; }
    public decimal UtilizationPercent => MaxCapacity > 0 ? (CurrentUtilization / MaxCapacity) * 100 : 0;
    public bool IsActive { get; set; }
    public DateTime? LastCountDate { get; set; }
    public ICollection<InventoryBriefDto> StoredItems { get; set; }
}

public class StorageLocationCreateDto
{
    [Required]
    public int ZoneId { get; set; }

    [Required]
    [StringLength(20)]
    public string Aisle { get; set; }

    [Required]
    [StringLength(20)]
    public string Rack { get; set; }

    [Required]
    [StringLength(20)]
    public string Bin { get; set; }

    [Range(0.1, double.MaxValue)]
    public decimal MaxCapacity { get; set; }
}

public class StorageLocationUpdateDto
{
    [StringLength(20)]
    public string Aisle { get; set; }

    [StringLength(20)]
    public string Rack { get; set; }

    [StringLength(20)]
    public string Bin { get; set; }

    [Range(0.1, double.MaxValue)]
    public decimal? MaxCapacity { get; set; }

    public bool? IsActive { get; set; }
}

public class WarehouseWorkerDto
{
    public int WorkerId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public string Position { get; set; }
    public DateTime HireDate { get; set; }
    public int? SupervisorId { get; set; }
    public string SupervisorName { get; set; }
    public bool IsActive { get; set; }
    public ICollection<WorkerShiftDto> RecentShifts { get; set; }
    public ICollection<WorkerTaskDto> ActiveTasks { get; set; }
    public int CompletedTasksCount { get; set; }
    public decimal AverageCompletionTime { get; set; }
}

public class WarehouseWorkerCreateDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int WarehouseId { get; set; }

    [Required]
    [StringLength(50)]
    public string Position { get; set; }

    public DateTime HireDate { get; set; } = DateTime.UtcNow;

    public int? SupervisorId { get; set; }
}

public class WarehouseWorkerUpdateDto
{
    [StringLength(50)]
    public string Position { get; set; }

    public int? SupervisorId { get; set; }

    public bool? IsActive { get; set; }
}

public class WorkerShiftDto
{
    public int ShiftId { get; set; }
    public int WorkerId { get; set; }
    public string WorkerName { get; set; }
    public DateTime ShiftStart { get; set; }
    public DateTime ShiftEnd { get; set; }
    public TimeSpan Duration => ShiftEnd - ShiftStart;
    public string Status { get; set; }
    public string Notes { get; set; }
}

public class WorkerShiftCreateDto
{
    [Required]
    public int WorkerId { get; set; }

    [Required]
    public DateTime ShiftStart { get; set; }

    [Required]
    public DateTime ShiftEnd { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; }

    [StringLength(500)]
    public string Notes { get; set; }
}

public class WorkerShiftUpdateDto
{
    public DateTime? ShiftStart { get; set; }

    public DateTime? ShiftEnd { get; set; }

    [StringLength(20)]
    public string Status { get; set; }

    [StringLength(500)]
    public string Notes { get; set; }
}

public class WorkerTaskDto
{
    public int TaskId { get; set; }
    public int WorkerId { get; set; }
    public string WorkerName { get; set; }
    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public string TaskType { get; set; }
    public string Status { get; set; }
    public DateTime AssignedDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public TimeSpan? CompletionTime => CompletedDate.HasValue ? CompletedDate.Value - AssignedDate : null;
    public string Priority { get; set; }
    public string Notes { get; set; }
    public object ReferenceData { get; set; }  // Flexible storage for task-specific data
}

public class WorkerTaskCreateDto
{
    [Required]
    public int WorkerId { get; set; }

    [Required]
    public int WarehouseId { get; set; }

    [Required]
    [StringLength(50)]
    public string TaskType { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Assigned";

    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    [StringLength(20)]
    public string Priority { get; set; } = "Normal";

    [StringLength(500)]
    public string Notes { get; set; }

    public object ReferenceData { get; set; }
}

public class WorkerTaskUpdateDto
{
    [StringLength(20)]
    public string Status { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    [StringLength(20)]
    public string Priority { get; set; }

    [StringLength(500)]
    public string Notes { get; set; }

    public object ReferenceData { get; set; }
}

// A brief version of inventory for use in other DTOs
public class InventoryBriefDto
{
    public int InventoryId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string SKU { get; set; }
    public int? VariantId { get; set; }
    public string VariantName { get; set; }
    public decimal QuantityOnHand { get; set; }
    public decimal QuantityAvailable { get; set; }
}

public class WarehouseWithDetailsDto : WarehouseDto
{
    public ICollection<WarehouseZoneDto> Zones { get; set; }
    public ICollection<WarehouseWorkerDto> Workers { get; set; }
    public decimal UtilizationPercent { get; set; }
}

public class WarehouseStatsDto
{
    public int TotalZones { get; set; }
    public int TotalLocations { get; set; }
    public int TotalWorkers { get; set; }
    public int TotalProducts { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public decimal UtilizationPercent { get; set; }
    public int ActiveTasks { get; set; }
    public int CompletedTasksToday { get; set; }
}