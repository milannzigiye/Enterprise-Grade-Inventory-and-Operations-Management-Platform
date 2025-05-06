using System.ComponentModel.DataAnnotations;

namespace inventtrack_backend.Model
{
    #region Warehouse Management Domain

    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }

        [Required]
        [StringLength(100)]
        public string WarehouseName { get; set; }

        [StringLength(255)]
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

        public decimal CapacitySquareFeet { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastModifiedDate { get; set; }

        // Navigation properties
        public virtual ICollection<WarehouseZone> Zones { get; set; }
        public virtual ICollection<WarehouseWorker> Workers { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<WorkerTask> Tasks { get; set; }
    }

    public class WarehouseZone
    {
        [Key]
        public int ZoneId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        [StringLength(50)]
        public string ZoneName { get; set; }

        [Required]
        [StringLength(50)]
        public string ZoneType { get; set; }

        public int FloorLevel { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Warehouse Warehouse { get; set; }
        public virtual ICollection<StorageLocation> Locations { get; set; }
    }

    public class StorageLocation
    {
        [Key]
        public int LocationId { get; set; }

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

        public decimal MaxCapacity { get; set; }

        public decimal CurrentUtilization { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? LastCountDate { get; set; }

        // Navigation properties
        public virtual WarehouseZone Zone { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<InventoryTransaction> SourceTransactions { get; set; }
        public virtual ICollection<InventoryTransaction> DestinationTransactions { get; set; }
    }

    public class WarehouseWorker
    {
        [Key]
        public int WorkerId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        [StringLength(50)]
        public string Position { get; set; }

        public DateTime HireDate { get; set; }

        public int? SupervisorId { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual User Supervisor { get; set; }
        public virtual ICollection<WorkerShift> Shifts { get; set; }
        public virtual ICollection<WorkerTask> Tasks { get; set; }
    }

    public class WorkerShift
    {
        [Key]
        public int ShiftId { get; set; }

        [Required]
        public int WorkerId { get; set; }

        [Required]
        public DateTime ShiftStart { get; set; }

        public DateTime? ShiftEnd { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        // Navigation properties
        public virtual WarehouseWorker Worker { get; set; }
    }

    public class WorkerTask
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        public int WorkerId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        [StringLength(20)]
        public string TaskType { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

        public DateTime? DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public int Priority { get; set; } = 3;

        [StringLength(255)]
        public string Notes { get; set; }

        // Navigation properties
        public virtual WarehouseWorker Worker { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }

    #endregion
}
