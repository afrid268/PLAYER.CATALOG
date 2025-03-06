using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        //variable for our repository
        private readonly IItemRepository itemsRepository;

        //constructor class , dependency inj
        public ItemsController(IItemRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        //api's
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync())
            .Select(x => x.AsDto());//transforms entity to dto here
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);//repository values are get here , then transformed later to as dto
            if (item == null)
            {
                return NotFound();
            }
            else
            {
                return item.AsDto();//entity to dto;
            }
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreatedItemDto createItemDto)
        {
            //assign dto values to entities value
            var item = new Item
            {
                Name = createItemDto.itemName,
                Description = createItemDto.itemDescription,
                Price = createItemDto.itemPrice,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateAsync(item);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);//action result
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await itemsRepository.GetAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.itemName;
            existingItem.Description = updateItemDto.itemDescription;
            existingItem.Price = updateItemDto.itemPrice;

            await itemsRepository.UpdateAsync(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var existingItem = await itemsRepository.GetAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            await itemsRepository.RemoveAsync(existingItem.Id);

            return NoContent();
        }
    }
}