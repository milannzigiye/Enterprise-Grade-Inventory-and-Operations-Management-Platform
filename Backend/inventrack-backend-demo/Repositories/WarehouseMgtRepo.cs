using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Model;
using inventrack_backend_demo.Data;
using Microsoft.EntityFrameworkCore;

namespace inventrack_backend_demo.Repositories
{
    #region Warehouse Management Repositories

    public interface IWarehouseRepository
    {
        Task<WarehouseDto> GetWarehouseByIdAsync(int warehouseId);
        Task<List<WarehouseDto>> GetAllWarehousesAsync();
        Task<WarehouseDto> CreateWarehouseAsync(WarehouseCreateDto warehouseCreateDto);
        Task<WarehouseDto> UpdateWarehouseAsync(int warehouseId, WarehouseUpdateDto warehouseUpdateDto);
        Task<bool> DeleteWarehouseAsync(int warehouseId);
    }

    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly ApplicationDbContext _context;

        public WarehouseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WarehouseDto> GetWarehouseByIdAsync(int warehouseId)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.Zones)
                .Include(w => w.Workers)
                    .ThenInclude(ww => ww.User)
                .FirstOrDefaultAsync(w => w.WarehouseId == warehouseId);

            if (warehouse == null) return null;

            return new WarehouseDto
            {
                WarehouseId = warehouse.WarehouseId,
                WarehouseName = warehouse.WarehouseName,
                Address = warehouse.Address,
                City = warehouse.City,
                State = warehouse.State,
                ZipCode = warehouse.ZipCode,
                Country = warehouse.Country,
                PhoneNumber = warehouse.PhoneNumber,
                Email = warehouse.Email,
                CapacitySquareFeet = warehouse.CapacitySquareFeet,
                IsActive = warehouse.IsActive,
                CreatedDate = warehouse.CreatedDate,
                LastModifiedDate = warehouse.LastModifiedDate,
                Zones = warehouse.Zones?.Select(z => new WarehouseZoneDto
                {
                    ZoneId = z.ZoneId,
                    WarehouseId = z.WarehouseId,
                    ZoneName = z.ZoneName,
                    ZoneType = z.ZoneType,
                    FloorLevel = z.FloorLevel,
                    Description = z.Description,
                    IsActive = z.IsActive
                }).ToList(),
                Workers = warehouse.Workers?.Select(ww => new WarehouseWorkerDto
                {
                    WorkerId = ww.WorkerId,
                    UserId = ww.UserId,
                    WarehouseId = ww.WarehouseId,
                    Position = ww.Position,
                    HireDate = ww.HireDate,
                    SupervisorId = ww.SupervisorId,
                    IsActive = ww.IsActive,
                    User = new UserDto
                    {
                        UserId = ww.User.UserId,
                        Username = ww.User.Username,
                        Email = ww.User.Email
                    }
                }).ToList()
            };
        }

        public async Task<List<WarehouseDto>> GetAllWarehousesAsync()
        {
            return await _context.Warehouses
                .Include(w => w.Zones)
                .Include(w => w.Workers)
                .Select(w => new WarehouseDto
                {
                    WarehouseId = w.WarehouseId,
                    WarehouseName = w.WarehouseName,
                    Address = w.Address,
                    City = w.City,
                    State = w.State,
                    ZipCode = w.ZipCode,
                    Country = w.Country,
                    PhoneNumber = w.PhoneNumber,
                    Email = w.Email,
                    CapacitySquareFeet = w.CapacitySquareFeet,
                    IsActive = w.IsActive,
                    CreatedDate = w.CreatedDate,
                    LastModifiedDate = w.LastModifiedDate,
                    Zones = w.Zones.Select(z => new WarehouseZoneDto
                    {
                        ZoneId = z.ZoneId,
                        WarehouseId = z.WarehouseId,
                        ZoneName = z.ZoneName,
                        ZoneType = z.ZoneType,
                        FloorLevel = z.FloorLevel,
                        Description = z.Description,
                        IsActive = z.IsActive
                    }).ToList(),
                    Workers = w.Workers.Select(ww => new WarehouseWorkerDto
                    {
                        WorkerId = ww.WorkerId,
                        UserId = ww.UserId,
                        WarehouseId = ww.WarehouseId,
                        Position = ww.Position,
                        HireDate = ww.HireDate,
                        SupervisorId = ww.SupervisorId,
                        IsActive = ww.IsActive
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<WarehouseDto> CreateWarehouseAsync(WarehouseCreateDto warehouseCreateDto)
        {
            var warehouse = new Warehouse
            {
                WarehouseName = warehouseCreateDto.WarehouseName,
                Address = warehouseCreateDto.Address,
                City = warehouseCreateDto.City,
                State = warehouseCreateDto.State,
                ZipCode = warehouseCreateDto.ZipCode,
                Country = warehouseCreateDto.Country,
                PhoneNumber = warehouseCreateDto.PhoneNumber,
                Email = warehouseCreateDto.Email,
                CapacitySquareFeet = warehouseCreateDto.CapacitySquareFeet,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();

            return await GetWarehouseByIdAsync(warehouse.WarehouseId);
        }

        public async Task<WarehouseDto> UpdateWarehouseAsync(int warehouseId, WarehouseUpdateDto warehouseUpdateDto)
        {
            var warehouse = await _context.Warehouses.FindAsync(warehouseId);
            if (warehouse == null) return null;

            warehouse.WarehouseName = warehouseUpdateDto.WarehouseName;
            warehouse.Address = warehouseUpdateDto.Address;
            warehouse.City = warehouseUpdateDto.City;
            warehouse.State = warehouseUpdateDto.State;
            warehouse.ZipCode = warehouseUpdateDto.ZipCode;
            warehouse.Country = warehouseUpdateDto.Country;
            warehouse.PhoneNumber = warehouseUpdateDto.PhoneNumber;
            warehouse.Email = warehouseUpdateDto.Email;
            warehouse.CapacitySquareFeet = warehouseUpdateDto.CapacitySquareFeet;
            warehouse.IsActive = warehouseUpdateDto.IsActive;
            warehouse.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await GetWarehouseByIdAsync(warehouseId);
        }

        public async Task<bool> DeleteWarehouseAsync(int warehouseId)
        {
            var warehouse = await _context.Warehouses.FindAsync(warehouseId);
            if (warehouse == null) return false;

            warehouse.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IWarehouseZoneRepository
    {
        Task<WarehouseZoneDto> GetZoneByIdAsync(int zoneId);
        Task<List<WarehouseZoneDto>> GetZonesByWarehouseAsync(int warehouseId);
        Task<WarehouseZoneDto> CreateZoneAsync(WarehouseZoneCreateDto zoneCreateDto);
        Task<WarehouseZoneDto> UpdateZoneAsync(int zoneId, WarehouseZoneDto zoneDto);
        Task<bool> DeleteZoneAsync(int zoneId);
    }

    public class WarehouseZoneRepository : IWarehouseZoneRepository
    {
        private readonly ApplicationDbContext _context;

        public WarehouseZoneRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WarehouseZoneDto> GetZoneByIdAsync(int zoneId)
        {
            var zone = await _context.WarehouseZones
                .Include(z => z.Locations)
                .FirstOrDefaultAsync(z => z.ZoneId == zoneId);

            if (zone == null) return null;

            return new WarehouseZoneDto
            {
                ZoneId = zone.ZoneId,
                WarehouseId = zone.WarehouseId,
                ZoneName = zone.ZoneName,
                ZoneType = zone.ZoneType,
                FloorLevel = zone.FloorLevel,
                Description = zone.Description,
                IsActive = zone.IsActive,
                Locations = zone.Locations?.Select(l => new StorageLocationDto
                {
                    LocationId = l.LocationId,
                    ZoneId = l.ZoneId,
                    Aisle = l.Aisle,
                    Rack = l.Rack,
                    Bin = l.Bin,
                    MaxCapacity = l.MaxCapacity,
                    CurrentUtilization = l.CurrentUtilization,
                    IsActive = l.IsActive,
                    LastCountDate = l.LastCountDate
                }).ToList()
            };
        }

        public async Task<List<WarehouseZoneDto>> GetZonesByWarehouseAsync(int warehouseId)
        {
            return await _context.WarehouseZones
                .Where(z => z.WarehouseId == warehouseId)
                .Select(z => new WarehouseZoneDto
                {
                    ZoneId = z.ZoneId,
                    WarehouseId = z.WarehouseId,
                    ZoneName = z.ZoneName,
                    ZoneType = z.ZoneType,
                    FloorLevel = z.FloorLevel,
                    Description = z.Description,
                    IsActive = z.IsActive
                }).ToListAsync();
        }

        public async Task<WarehouseZoneDto> CreateZoneAsync(WarehouseZoneCreateDto zoneCreateDto)
        {
            var zone = new WarehouseZone
            {
                WarehouseId = zoneCreateDto.WarehouseId,
                ZoneName = zoneCreateDto.ZoneName,
                ZoneType = zoneCreateDto.ZoneType,
                FloorLevel = zoneCreateDto.FloorLevel,
                Description = zoneCreateDto.Description,
                IsActive = true
            };

            _context.WarehouseZones.Add(zone);
            await _context.SaveChangesAsync();

            return await GetZoneByIdAsync(zone.ZoneId);
        }

        public async Task<WarehouseZoneDto> UpdateZoneAsync(int zoneId, WarehouseZoneDto zoneDto)
        {
            var zone = await _context.WarehouseZones.FindAsync(zoneId);
            if (zone == null) return null;

            zone.ZoneName = zoneDto.ZoneName;
            zone.ZoneType = zoneDto.ZoneType;
            zone.FloorLevel = zoneDto.FloorLevel;
            zone.Description = zoneDto.Description;
            zone.IsActive = zoneDto.IsActive;

            await _context.SaveChangesAsync();
            return await GetZoneByIdAsync(zoneId);
        }

        public async Task<bool> DeleteZoneAsync(int zoneId)
        {
            var zone = await _context.WarehouseZones.FindAsync(zoneId);
            if (zone == null) return false;

            zone.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IStorageLocationRepository
    {
        Task<StorageLocationDto> GetLocationByIdAsync(int locationId);
        Task<List<StorageLocationDto>> GetLocationsByZoneAsync(int zoneId);
        Task<StorageLocationDto> CreateLocationAsync(StorageLocationCreateDto locationCreateDto);
        Task<StorageLocationDto> UpdateLocationAsync(int locationId, StorageLocationDto locationDto);
        Task<bool> DeleteLocationAsync(int locationId);
    }

    public class StorageLocationRepository : IStorageLocationRepository
    {
        private readonly ApplicationDbContext _context;

        public StorageLocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StorageLocationDto> GetLocationByIdAsync(int locationId)
        {
            var location = await _context.StorageLocations.FindAsync(locationId);
            if (location == null) return null;

            return new StorageLocationDto
            {
                LocationId = location.LocationId,
                ZoneId = location.ZoneId,
                Aisle = location.Aisle,
                Rack = location.Rack,
                Bin = location.Bin,
                MaxCapacity = location.MaxCapacity,
                CurrentUtilization = location.CurrentUtilization,
                IsActive = location.IsActive,
                LastCountDate = location.LastCountDate
            };
        }

        public async Task<List<StorageLocationDto>> GetLocationsByZoneAsync(int zoneId)
        {
            return await _context.StorageLocations
                .Where(l => l.ZoneId == zoneId)
                .Select(l => new StorageLocationDto
                {
                    LocationId = l.LocationId,
                    ZoneId = l.ZoneId,
                    Aisle = l.Aisle,
                    Rack = l.Rack,
                    Bin = l.Bin,
                    MaxCapacity = l.MaxCapacity,
                    CurrentUtilization = l.CurrentUtilization,
                    IsActive = l.IsActive,
                    LastCountDate = l.LastCountDate
                }).ToListAsync();
        }

        public async Task<StorageLocationDto> CreateLocationAsync(StorageLocationCreateDto locationCreateDto)
        {
            var location = new StorageLocation
            {
                ZoneId = locationCreateDto.ZoneId,
                Aisle = locationCreateDto.Aisle,
                Rack = locationCreateDto.Rack,
                Bin = locationCreateDto.Bin,
                MaxCapacity = locationCreateDto.MaxCapacity,
                CurrentUtilization = 0,
                IsActive = true
            };

            _context.StorageLocations.Add(location);
            await _context.SaveChangesAsync();

            return await GetLocationByIdAsync(location.LocationId);
        }

        public async Task<StorageLocationDto> UpdateLocationAsync(int locationId, StorageLocationDto locationDto)
        {
            var location = await _context.StorageLocations.FindAsync(locationId);
            if (location == null) return null;

            location.Aisle = locationDto.Aisle;
            location.Rack = locationDto.Rack;
            location.Bin = locationDto.Bin;
            location.MaxCapacity = locationDto.MaxCapacity;
            location.CurrentUtilization = locationDto.CurrentUtilization;
            location.IsActive = locationDto.IsActive;
            location.LastCountDate = locationDto.LastCountDate;

            await _context.SaveChangesAsync();
            return await GetLocationByIdAsync(locationId);
        }

        public async Task<bool> DeleteLocationAsync(int locationId)
        {
            var location = await _context.StorageLocations.FindAsync(locationId);
            if (location == null) return false;

            location.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IWarehouseWorkerRepository
    {
        Task<WarehouseWorkerDto> GetWorkerByIdAsync(int workerId);
        Task<List<WarehouseWorkerDto>> GetWorkersByWarehouseAsync(int warehouseId);
        Task<WarehouseWorkerDto> CreateWorkerAsync(WarehouseWorkerCreateDto workerCreateDto);
        Task<WarehouseWorkerDto> UpdateWorkerAsync(int workerId, WarehouseWorkerDto workerDto);
        Task<bool> DeleteWorkerAsync(int workerId);
    }

    public class WarehouseWorkerRepository : IWarehouseWorkerRepository
    {
        private readonly ApplicationDbContext _context;

        public WarehouseWorkerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WarehouseWorkerDto> GetWorkerByIdAsync(int workerId)
        {
            var worker = await _context.WarehouseWorkers
                .Include(ww => ww.User)
                .Include(ww => ww.Warehouse)
                .Include(ww => ww.Supervisor)
                .FirstOrDefaultAsync(ww => ww.WorkerId == workerId);

            if (worker == null) return null;

            return new WarehouseWorkerDto
            {
                WorkerId = worker.WorkerId,
                UserId = worker.UserId,
                WarehouseId = worker.WarehouseId,
                Position = worker.Position,
                HireDate = worker.HireDate,
                SupervisorId = worker.SupervisorId,
                IsActive = worker.IsActive,
                User = new UserDto
                {
                    UserId = worker.User.UserId,
                    Username = worker.User.Username,
                    Email = worker.User.Email
                },
                Warehouse = new WarehouseDto
                {
                    WarehouseId = worker.Warehouse.WarehouseId,
                    WarehouseName = worker.Warehouse.WarehouseName
                },
                Supervisor = worker.Supervisor != null ? new UserDto
                {
                    UserId = worker.Supervisor.UserId,
                    Username = worker.Supervisor.Username,
                    Email = worker.Supervisor.Email
                } : null
            };
        }

        public async Task<List<WarehouseWorkerDto>> GetWorkersByWarehouseAsync(int warehouseId)
        {
            return await _context.WarehouseWorkers
                .Where(ww => ww.WarehouseId == warehouseId)
                .Include(ww => ww.User)
                .Select(ww => new WarehouseWorkerDto
                {
                    WorkerId = ww.WorkerId,
                    UserId = ww.UserId,
                    WarehouseId = ww.WarehouseId,
                    Position = ww.Position,
                    HireDate = ww.HireDate,
                    SupervisorId = ww.SupervisorId,
                    IsActive = ww.IsActive,
                    User = new UserDto
                    {
                        UserId = ww.User.UserId,
                        Username = ww.User.Username,
                        Email = ww.User.Email
                    }
                }).ToListAsync();
        }

        public async Task<WarehouseWorkerDto> CreateWorkerAsync(WarehouseWorkerCreateDto workerCreateDto)
        {
            var worker = new WarehouseWorker
            {
                UserId = workerCreateDto.UserId,
                WarehouseId = workerCreateDto.WarehouseId,
                Position = workerCreateDto.Position,
                HireDate = DateTime.UtcNow,
                SupervisorId = workerCreateDto.SupervisorId,
                IsActive = true
            };

            _context.WarehouseWorkers.Add(worker);
            await _context.SaveChangesAsync();

            return await GetWorkerByIdAsync(worker.WorkerId);
        }

        public async Task<WarehouseWorkerDto> UpdateWorkerAsync(int workerId, WarehouseWorkerDto workerDto)
        {
            var worker = await _context.WarehouseWorkers.FindAsync(workerId);
            if (worker == null) return null;

            worker.Position = workerDto.Position;
            worker.SupervisorId = workerDto.SupervisorId;
            worker.IsActive = workerDto.IsActive;

            await _context.SaveChangesAsync();
            return await GetWorkerByIdAsync(workerId);
        }

        public async Task<bool> DeleteWorkerAsync(int workerId)
        {
            var worker = await _context.WarehouseWorkers.FindAsync(workerId);
            if (worker == null) return false;

            worker.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IWorkerShiftRepository
    {
        Task<WorkerShiftDto> GetShiftByIdAsync(int shiftId);
        Task<List<WorkerShiftDto>> GetShiftsByWorkerAsync(int workerId);
        Task<WorkerShiftDto> CreateShiftAsync(WorkerShiftCreateDto shiftCreateDto);
        Task<WorkerShiftDto> UpdateShiftAsync(int shiftId, WorkerShiftDto shiftDto);
        Task<bool> DeleteShiftAsync(int shiftId);
    }

    public class WorkerShiftRepository : IWorkerShiftRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkerShiftRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WorkerShiftDto> GetShiftByIdAsync(int shiftId)
        {
            var shift = await _context.WorkerShifts
                .Include(s => s.Worker)
                .FirstOrDefaultAsync(s => s.ShiftId == shiftId);

            if (shift == null) return null;

            return new WorkerShiftDto
            {
                ShiftId = shift.ShiftId,
                WorkerId = shift.WorkerId,
                ShiftStart = shift.ShiftStart,
                ShiftEnd = shift.ShiftEnd,
                Status = shift.Status,
                Notes = shift.Notes,
                Worker = new WarehouseWorkerDto
                {
                    WorkerId = shift.Worker.WorkerId,
                    UserId = shift.Worker.UserId
                }
            };
        }

        public async Task<List<WorkerShiftDto>> GetShiftsByWorkerAsync(int workerId)
        {
            return await _context.WorkerShifts
                .Where(s => s.WorkerId == workerId)
                .Select(s => new WorkerShiftDto
                {
                    ShiftId = s.ShiftId,
                    WorkerId = s.WorkerId,
                    ShiftStart = s.ShiftStart,
                    ShiftEnd = s.ShiftEnd,
                    Status = s.Status,
                    Notes = s.Notes
                }).ToListAsync();
        }

        public async Task<WorkerShiftDto> CreateShiftAsync(WorkerShiftCreateDto shiftCreateDto)
        {
            var shift = new WorkerShift
            {
                WorkerId = shiftCreateDto.WorkerId,
                ShiftStart = shiftCreateDto.ShiftStart,
                Status = shiftCreateDto.Status
            };

            _context.WorkerShifts.Add(shift);
            await _context.SaveChangesAsync();

            return await GetShiftByIdAsync(shift.ShiftId);
        }

        public async Task<WorkerShiftDto> UpdateShiftAsync(int shiftId, WorkerShiftDto shiftDto)
        {
            var shift = await _context.WorkerShifts.FindAsync(shiftId);
            if (shift == null) return null;

            shift.ShiftStart = shiftDto.ShiftStart;
            shift.ShiftEnd = shiftDto.ShiftEnd;
            shift.Status = shiftDto.Status;
            shift.Notes = shiftDto.Notes;

            await _context.SaveChangesAsync();
            return await GetShiftByIdAsync(shiftId);
        }

        public async Task<bool> DeleteShiftAsync(int shiftId)
        {
            var shift = await _context.WorkerShifts.FindAsync(shiftId);
            if (shift == null) return false;

            _context.WorkerShifts.Remove(shift);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IWorkerTaskRepository
    {
        Task<WorkerTaskDto> GetTaskByIdAsync(int taskId);
        Task<List<WorkerTaskDto>> GetTasksByWorkerAsync(int workerId);
        Task<List<WorkerTaskDto>> GetTasksByWarehouseAsync(int warehouseId);
        Task<WorkerTaskDto> CreateTaskAsync(WorkerTaskCreateDto taskCreateDto);
        Task<WorkerTaskDto> UpdateTaskAsync(int taskId, WorkerTaskDto taskDto);
        Task<bool> DeleteTaskAsync(int taskId);
    }

    public class WorkerTaskRepository : IWorkerTaskRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkerTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WorkerTaskDto> GetTaskByIdAsync(int taskId)
        {
            var task = await _context.WorkerTasks
                .Include(t => t.Worker)
                .Include(t => t.Warehouse)
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null) return null;

            return new WorkerTaskDto
            {
                TaskId = task.TaskId,
                WorkerId = task.WorkerId,
                WarehouseId = task.WarehouseId,
                TaskType = task.TaskType,
                Status = task.Status,
                AssignedDate = task.AssignedDate,
                DueDate = task.DueDate,
                CompletedDate = task.CompletedDate,
                Priority = task.Priority,
                Notes = task.Notes,
                Worker = new WarehouseWorkerDto
                {
                    WorkerId = task.Worker.WorkerId,
                    UserId = task.Worker.UserId
                },
                Warehouse = new WarehouseDto
                {
                    WarehouseId = task.Warehouse.WarehouseId,
                    WarehouseName = task.Warehouse.WarehouseName
                }
            };
        }

        public async Task<List<WorkerTaskDto>> GetTasksByWorkerAsync(int workerId)
        {
            return await _context.WorkerTasks
                .Where(t => t.WorkerId == workerId)
                .Select(t => new WorkerTaskDto
                {
                    TaskId = t.TaskId,
                    WorkerId = t.WorkerId,
                    WarehouseId = t.WarehouseId,
                    TaskType = t.TaskType,
                    Status = t.Status,
                    AssignedDate = t.AssignedDate,
                    DueDate = t.DueDate,
                    CompletedDate = t.CompletedDate,
                    Priority = t.Priority,
                    Notes = t.Notes
                }).ToListAsync();
        }

        public async Task<List<WorkerTaskDto>> GetTasksByWarehouseAsync(int warehouseId)
        {
            return await _context.WorkerTasks
                .Where(t => t.WarehouseId == warehouseId)
                .Select(t => new WorkerTaskDto
                {
                    TaskId = t.TaskId,
                    WorkerId = t.WorkerId,
                    WarehouseId = t.WarehouseId,
                    TaskType = t.TaskType,
                    Status = t.Status,
                    AssignedDate = t.AssignedDate,
                    DueDate = t.DueDate,
                    CompletedDate = t.CompletedDate,
                    Priority = t.Priority,
                    Notes = t.Notes
                }).ToListAsync();
        }

        public async Task<WorkerTaskDto> CreateTaskAsync(WorkerTaskCreateDto taskCreateDto)
        {
            var task = new WorkerTask
            {
                WorkerId = taskCreateDto.WorkerId,
                WarehouseId = taskCreateDto.WarehouseId,
                TaskType = taskCreateDto.TaskType,
                Status = "Assigned",
                AssignedDate = DateTime.UtcNow,
                DueDate = taskCreateDto.DueDate,
                Priority = taskCreateDto.Priority,
                Notes = taskCreateDto.Notes
            };

            _context.WorkerTasks.Add(task);
            await _context.SaveChangesAsync();

            return await GetTaskByIdAsync(task.TaskId);
        }

        public async Task<WorkerTaskDto> UpdateTaskAsync(int taskId, WorkerTaskDto taskDto)
        {
            var task = await _context.WorkerTasks.FindAsync(taskId);
            if (task == null) return null;

            task.TaskType = taskDto.TaskType;
            task.Status = taskDto.Status;
            task.DueDate = taskDto.DueDate;
            task.CompletedDate = taskDto.CompletedDate;
            task.Priority = taskDto.Priority;
            task.Notes = taskDto.Notes;

            await _context.SaveChangesAsync();
            return await GetTaskByIdAsync(taskId);
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var task = await _context.WorkerTasks.FindAsync(taskId);
            if (task == null) return false;

            _context.WorkerTasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    #endregion
}
