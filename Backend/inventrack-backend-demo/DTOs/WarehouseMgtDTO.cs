namespace inventrack_backend_demo.DTOs
{
    #region Warehouse Management DTOs

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
        public DateTime? LastModifiedDate { get; set; }
        public List<WarehouseZoneDto> Zones { get; set; }
        public List<WarehouseWorkerDto> Workers { get; set; }
    }

    public class WarehouseCreateDto
    {
        public string WarehouseName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal CapacitySquareFeet { get; set; }
    }

    public class WarehouseUpdateDto
    {
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
        public List<StorageLocationDto> Locations { get; set; }
    }

    public class WarehouseZoneCreateDto
    {
        public int WarehouseId { get; set; }
        public string ZoneName { get; set; }
        public string ZoneType { get; set; }
        public int FloorLevel { get; set; }
        public string Description { get; set; }
    }

    public class StorageLocationDto
    {
        public int LocationId { get; set; }
        public int ZoneId { get; set; }
        public string Aisle { get; set; }
        public string Rack { get; set; }
        public string Bin { get; set; }
        public decimal MaxCapacity { get; set; }
        public decimal CurrentUtilization { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastCountDate { get; set; }
    }

    public class StorageLocationCreateDto
    {
        public int ZoneId { get; set; }
        public string Aisle { get; set; }
        public string Rack { get; set; }
        public string Bin { get; set; }
        public decimal MaxCapacity { get; set; }
    }

    public class WarehouseWorkerDto
    {
        public int WorkerId { get; set; }
        public int UserId { get; set; }
        public int WarehouseId { get; set; }
        public string Position { get; set; }
        public DateTime HireDate { get; set; }
        public int? SupervisorId { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public UserDto User { get; set; }
        public WarehouseDto Warehouse { get; set; }
        public UserDto Supervisor { get; set; }
    }

    public class WarehouseWorkerCreateDto
    {
        public int UserId { get; set; }
        public int WarehouseId { get; set; }
        public string Position { get; set; }
        public int? SupervisorId { get; set; }
    }

    public class WorkerShiftDto
    {
        public int ShiftId { get; set; }
        public int WorkerId { get; set; }
        public DateTime ShiftStart { get; set; }
        public DateTime? ShiftEnd { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public WarehouseWorkerDto Worker { get; set; }
    }

    public class WorkerShiftCreateDto
    {
        public int WorkerId { get; set; }
        public DateTime ShiftStart { get; set; }
        public string Status { get; set; }
    }

    public class WorkerTaskDto
    {
        public int TaskId { get; set; }
        public int WorkerId { get; set; }
        public int WarehouseId { get; set; }
        public string TaskType { get; set; }
        public string Status { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int Priority { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public WarehouseWorkerDto Worker { get; set; }
        public WarehouseDto Warehouse { get; set; }
    }

    public class WorkerTaskCreateDto
    {
        public int WorkerId { get; set; }
        public int WarehouseId { get; set; }
        public string TaskType { get; set; }
        public DateTime? DueDate { get; set; }
        public int Priority { get; set; }
        public string Notes { get; set; }
    }

    #endregion
}
