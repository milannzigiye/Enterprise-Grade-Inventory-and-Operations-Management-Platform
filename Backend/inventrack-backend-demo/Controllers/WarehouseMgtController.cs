using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inventrack_backend_demo.Controllers
{
    #region Warehouse Management Controllers

    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseController(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        [HttpGet("{warehouseId}")]
        public async Task<ActionResult<WarehouseDto>> GetWarehouseById(int warehouseId)
        {
            var warehouse = await _warehouseRepository.GetWarehouseByIdAsync(warehouseId);
            if (warehouse == null)
                return NotFound();

            return Ok(warehouse);
        }

        [HttpGet]
        public async Task<ActionResult<List<WarehouseDto>>> GetAllWarehouses()
        {
            var warehouses = await _warehouseRepository.GetAllWarehousesAsync();
            return Ok(warehouses);
        }

        [HttpPost]
        public async Task<ActionResult<WarehouseDto>> CreateWarehouse(WarehouseCreateDto warehouseCreateDto)
        {
            var warehouse = await _warehouseRepository.CreateWarehouseAsync(warehouseCreateDto);
            return CreatedAtAction(nameof(GetWarehouseById), new { warehouseId = warehouse.WarehouseId }, warehouse);
        }

        [HttpPut("{warehouseId}")]
        public async Task<ActionResult<WarehouseDto>> UpdateWarehouse(int warehouseId, WarehouseUpdateDto warehouseUpdateDto)
        {
            var warehouse = await _warehouseRepository.UpdateWarehouseAsync(warehouseId, warehouseUpdateDto);
            if (warehouse == null)
                return NotFound();

            return Ok(warehouse);
        }

        [HttpDelete("{warehouseId}")]
        public async Task<ActionResult> DeleteWarehouse(int warehouseId)
        {
            var result = await _warehouseRepository.DeleteWarehouseAsync(warehouseId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseZoneController : ControllerBase
    {
        private readonly IWarehouseZoneRepository _zoneRepository;

        public WarehouseZoneController(IWarehouseZoneRepository zoneRepository)
        {
            _zoneRepository = zoneRepository;
        }

        [HttpGet("{zoneId}")]
        public async Task<ActionResult<WarehouseZoneDto>> GetZoneById(int zoneId)
        {
            var zone = await _zoneRepository.GetZoneByIdAsync(zoneId);
            if (zone == null)
                return NotFound();

            return Ok(zone);
        }

        [HttpGet("warehouse/{warehouseId}")]
        public async Task<ActionResult<List<WarehouseZoneDto>>> GetZonesByWarehouse(int warehouseId)
        {
            var zones = await _zoneRepository.GetZonesByWarehouseAsync(warehouseId);
            return Ok(zones);
        }

        [HttpPost]
        public async Task<ActionResult<WarehouseZoneDto>> CreateZone(WarehouseZoneCreateDto zoneCreateDto)
        {
            var zone = await _zoneRepository.CreateZoneAsync(zoneCreateDto);
            return CreatedAtAction(nameof(GetZoneById), new { zoneId = zone.ZoneId }, zone);
        }

        [HttpPut("{zoneId}")]
        public async Task<ActionResult<WarehouseZoneDto>> UpdateZone(int zoneId, WarehouseZoneDto zoneDto)
        {
            var zone = await _zoneRepository.UpdateZoneAsync(zoneId, zoneDto);
            if (zone == null)
                return NotFound();

            return Ok(zone);
        }

        [HttpDelete("{zoneId}")]
        public async Task<ActionResult> DeleteZone(int zoneId)
        {
            var result = await _zoneRepository.DeleteZoneAsync(zoneId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class StorageLocationController : ControllerBase
    {
        private readonly IStorageLocationRepository _locationRepository;

        public StorageLocationController(IStorageLocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [HttpGet("{locationId}")]
        public async Task<ActionResult<StorageLocationDto>> GetLocationById(int locationId)
        {
            var location = await _locationRepository.GetLocationByIdAsync(locationId);
            if (location == null)
                return NotFound();

            return Ok(location);
        }

        [HttpGet("zone/{zoneId}")]
        public async Task<ActionResult<List<StorageLocationDto>>> GetLocationsByZone(int zoneId)
        {
            var locations = await _locationRepository.GetLocationsByZoneAsync(zoneId);
            return Ok(locations);
        }

        [HttpPost]
        public async Task<ActionResult<StorageLocationDto>> CreateLocation(StorageLocationCreateDto locationCreateDto)
        {
            var location = await _locationRepository.CreateLocationAsync(locationCreateDto);
            return CreatedAtAction(nameof(GetLocationById), new { locationId = location.LocationId }, location);
        }

        [HttpPut("{locationId}")]
        public async Task<ActionResult<StorageLocationDto>> UpdateLocation(int locationId, StorageLocationDto locationDto)
        {
            var location = await _locationRepository.UpdateLocationAsync(locationId, locationDto);
            if (location == null)
                return NotFound();

            return Ok(location);
        }

        [HttpDelete("{locationId}")]
        public async Task<ActionResult> DeleteLocation(int locationId)
        {
            var result = await _locationRepository.DeleteLocationAsync(locationId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseWorkerController : ControllerBase
    {
        private readonly IWarehouseWorkerRepository _workerRepository;

        public WarehouseWorkerController(IWarehouseWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        [HttpGet("{workerId}")]
        public async Task<ActionResult<WarehouseWorkerDto>> GetWorkerById(int workerId)
        {
            var worker = await _workerRepository.GetWorkerByIdAsync(workerId);
            if (worker == null)
                return NotFound();

            return Ok(worker);
        }

        [HttpGet("warehouse/{warehouseId}")]
        public async Task<ActionResult<List<WarehouseWorkerDto>>> GetWorkersByWarehouse(int warehouseId)
        {
            var workers = await _workerRepository.GetWorkersByWarehouseAsync(warehouseId);
            return Ok(workers);
        }

        [HttpPost]
        public async Task<ActionResult<WarehouseWorkerDto>> CreateWorker(WarehouseWorkerCreateDto workerCreateDto)
        {
            var worker = await _workerRepository.CreateWorkerAsync(workerCreateDto);
            return CreatedAtAction(nameof(GetWorkerById), new { workerId = worker.WorkerId }, worker);
        }

        [HttpPut("{workerId}")]
        public async Task<ActionResult<WarehouseWorkerDto>> UpdateWorker(int workerId, WarehouseWorkerDto workerDto)
        {
            var worker = await _workerRepository.UpdateWorkerAsync(workerId, workerDto);
            if (worker == null)
                return NotFound();

            return Ok(worker);
        }

        [HttpDelete("{workerId}")]
        public async Task<ActionResult> DeleteWorker(int workerId)
        {
            var result = await _workerRepository.DeleteWorkerAsync(workerId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class WorkerShiftController : ControllerBase
    {
        private readonly IWorkerShiftRepository _shiftRepository;

        public WorkerShiftController(IWorkerShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        [HttpGet("{shiftId}")]
        public async Task<ActionResult<WorkerShiftDto>> GetShiftById(int shiftId)
        {
            var shift = await _shiftRepository.GetShiftByIdAsync(shiftId);
            if (shift == null)
                return NotFound();

            return Ok(shift);
        }

        [HttpGet("worker/{workerId}")]
        public async Task<ActionResult<List<WorkerShiftDto>>> GetShiftsByWorker(int workerId)
        {
            var shifts = await _shiftRepository.GetShiftsByWorkerAsync(workerId);
            return Ok(shifts);
        }

        [HttpPost]
        public async Task<ActionResult<WorkerShiftDto>> CreateShift(WorkerShiftCreateDto shiftCreateDto)
        {
            var shift = await _shiftRepository.CreateShiftAsync(shiftCreateDto);
            return CreatedAtAction(nameof(GetShiftById), new { shiftId = shift.ShiftId }, shift);
        }

        [HttpPut("{shiftId}")]
        public async Task<ActionResult<WorkerShiftDto>> UpdateShift(int shiftId, WorkerShiftDto shiftDto)
        {
            var shift = await _shiftRepository.UpdateShiftAsync(shiftId, shiftDto);
            if (shift == null)
                return NotFound();

            return Ok(shift);
        }

        [HttpDelete("{shiftId}")]
        public async Task<ActionResult> DeleteShift(int shiftId)
        {
            var result = await _shiftRepository.DeleteShiftAsync(shiftId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class WorkerTaskController : ControllerBase
    {
        private readonly IWorkerTaskRepository _taskRepository;

        public WorkerTaskController(IWorkerTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<WorkerTaskDto>> GetTaskById(int taskId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet("worker/{workerId}")]
        public async Task<ActionResult<List<WorkerTaskDto>>> GetTasksByWorker(int workerId)
        {
            var tasks = await _taskRepository.GetTasksByWorkerAsync(workerId);
            return Ok(tasks);
        }

        [HttpGet("warehouse/{warehouseId}")]
        public async Task<ActionResult<List<WorkerTaskDto>>> GetTasksByWarehouse(int warehouseId)
        {
            var tasks = await _taskRepository.GetTasksByWarehouseAsync(warehouseId);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<WorkerTaskDto>> CreateTask(WorkerTaskCreateDto taskCreateDto)
        {
            var task = await _taskRepository.CreateTaskAsync(taskCreateDto);
            return CreatedAtAction(nameof(GetTaskById), new { taskId = task.TaskId }, task);
        }

        [HttpPut("{taskId}")]
        public async Task<ActionResult<WorkerTaskDto>> UpdateTask(int taskId, WorkerTaskDto taskDto)
        {
            var task = await _taskRepository.UpdateTaskAsync(taskId, taskDto);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpDelete("{taskId}")]
        public async Task<ActionResult> DeleteTask(int taskId)
        {
            var result = await _taskRepository.DeleteTaskAsync(taskId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    #endregion
}
