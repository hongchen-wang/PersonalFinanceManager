using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using PersonalFinanceManager.Server.Models;
using PersonalFinanceManager.Server.Services;
using System.Threading.Tasks;


namespace PersonalFinanceManager.Server.Controllers
{
    // CRUD: Create, Read, Update, Delete

    [Route("api/transactions")]
    [ApiController]
    [Authorize] // require authentication for all endpoints
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // GET: api/transactions/getAll
        [HttpGet("getAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllTransactions();
            return Ok(transactions);
        }

        // GET: api/transactions/get/{id}
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var transaction = await _transactionService.GetTransactionById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // POST: api/transactions/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            // verify the model validation by using attributes 
            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState);
            }
            
            var createdTransaction = await _transactionService.CreateTransactionAsync(transaction);
            // return a StatusCodes.Status201Created
            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, createdTransaction);
        }

        // PUT: api/transactions/update/{id}
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedTransaction = await _transactionService.UpdateTransactionAsync(id, transaction);
            if (updatedTransaction == null)
            { 
                return NotFound();
            }

            return Ok(updatedTransaction);
        }

        // DELETE: api/transactions/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        { 
            var success = await _transactionService.DeleteTransactionAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
