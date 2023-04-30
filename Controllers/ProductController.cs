﻿using Manero_Backend.Helpers.Services;
using Manero_Backend.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Manero_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var product = await _productService.GetProductAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create( ProductRequest productRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Created("", await _productService.CreateAsync(productRequest));
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(Guid id, ProductRequest productRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var updatedProduct = await _productService.UpdateAsync(id, productRequest);

            if (updatedProduct == null)
            {
                return NotFound();
            }

            return Ok(updatedProduct);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var products = await _productService.SearchAsync(searchTerm);
            return Ok(products);
        }


        [HttpGet("tag/{tag}")]
        public async Task<IActionResult> GetAllByTag(string tag)
        {
            var product = await _productService.GetAllByTagAsync(tag);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("genre/{genre}")]
        public async Task<IActionResult> GetAllByGenre(string genre)
        {
            var product = await _productService.GetAllByTagAsync(genre);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }








    }
}
