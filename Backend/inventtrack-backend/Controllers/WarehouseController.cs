using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using inventtrack_backend.DTOs;
using inventtrack_backend.Service;
using Amazon.Runtime;

namespace omniflow_backend.Controllers
{
    
        [Route("api/[controller]")]
        [ApiController]
        [Produces(MediaTypeNames.Application.Json)]
        public class WarehousesController : ControllerBase
        {
            private readonly IWarehouseService _warehouseService;
            private readonly ILogger<WarehousesController> _logger;

            public WarehousesController(
                IWarehouseService warehouseService,
                ILogger<WarehousesController> logger)
            {
                _warehouseService = warehouseService;
                _logger = logger;
            }

            [HttpGet]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<PaginatedResponse<WarehouseDto>>> GetWarehouses(
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10)
            {
                try
                {
                    if (pageNumber < 1 || pageSize < 1)
                    {
                        return BadRequest("Page number and page size must be greater than 0");
                    }

                    var warehouses = await _warehouseService.GetWarehousesAsync(pageNumber, pageSize);
                    return Ok(warehouses);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting warehouses");
                    return StatusCode(500, "Error retrieving warehouses");
                }
            }

        
            [HttpGet("{id}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<WarehouseWithDetailsDto>> GetWarehouse(int id)
            {
                try
                {
                    var warehouse = await _warehouseService.GetWarehouseWithDetailsAsync(id);
                    if (warehouse == null)
                    {
                        return NotFound();
                    }
                    return Ok(warehouse);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error getting warehouse with ID {id}");
                    return StatusCode(500, "Error retrieving warehouse");
                }
            }

           
            [HttpPost]
            [Consumes(MediaTypeNames.Application.Json)]
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<WarehouseDto>> CreateWarehouse(
                [FromBody] WarehouseCreateDto warehouseCreateDto)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var createdWarehouse = await _warehouseService.CreateWarehouseAsync(warehouseCreateDto);
                    return CreatedAtAction(
                        nameof(GetWarehouse),
                        new { id = createdWarehouse.WarehouseId },
                        createdWarehouse);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating warehouse");
                    return StatusCode(500, "Error creating warehouse");
                }
            }

           
            [HttpPut("{id}")]
            [Consumes(MediaTypeNames.Application.Json)]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> UpdateWarehouse(
                int id,
                [FromBody] WarehouseUpdateDto warehouseUpdateDto)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    await _warehouseService.UpdateWarehouseAsync(id, warehouseUpdateDto);
                    return NoContent();
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error updating warehouse with ID {id}");
                    return StatusCode(500, "Error updating warehouse");
                }
            }

           
            [HttpDelete("{id}")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> DeleteWarehouse(int id)
            {
                try
                {
                    await _warehouseService.DeleteWarehouseAsync(id);
                    return NoContent();
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error deleting warehouse with ID {id}");
                    return StatusCode(500, "Error deleting warehouse");
                }
            }

            
            [HttpGet("{id}/stats")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<WarehouseStatsDto>> GetWarehouseStats(int id)
            {
                try
                {
                    var stats = await _warehouseService.GetWarehouseStatsAsync(id);
                    return Ok(stats);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error getting stats for warehouse with ID {id}");
                    return StatusCode(500, "Error getting warehouse stats");
                }
            }

           
            [HttpGet("{id}/zones")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<IEnumerable<WarehouseZoneDto>>> GetWarehouseZones(int id)
            {
                try
                {
                    var zones = await _warehouseService.GetWarehouseZonesAsync(id);
                    return Ok(zones);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error getting zones for warehouse with ID {id}");
                    return StatusCode(500, "Error getting warehouse zones");
                }
            }

            
            [HttpGet("{id}/locations")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<IEnumerable<StorageLocationDto>>> GetWarehouseLocations(int id)
            {
                try
                {
                    var locations = await _warehouseService.GetWarehouseLocationsAsync(id);
                    return Ok(locations);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error getting locations for warehouse with ID {id}");
                    return StatusCode(500, "Error getting warehouse locations");
                }
            }

         
            [HttpGet("{id}/workers")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<IEnumerable<WarehouseWorkerDto>>> GetWarehouseWorkers(int id)
            {
                try
                {
                    var workers = await _warehouseService.GetWarehouseWorkersAsync(id);
                    return Ok(workers);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error getting workers for warehouse with ID {id}");
                    return StatusCode(500, "Error getting warehouse workers");
                }
            }
        }
    }

