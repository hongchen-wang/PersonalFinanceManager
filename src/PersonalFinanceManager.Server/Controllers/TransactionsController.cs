using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PersonalFinanceManager.Server.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetTransactions()
        {
            return Ok(new {Message = "This is a protected endpoint"});
        }


        //private readonly ITransactionService _transactionService;
        //public TransactionsController(ITransactionService transactionService)
        //{
        //    _transactionService = transactionService;
        //}
        //[HttpGet]
        //public async Task<IActionResult> GetTransactions()
        //{
        //    var transactions = await _transactionService.GetTransactions();
        //    return Ok(transactions);
        //}
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetTransaction(int id)
        //{
        //    var transaction = await _transactionService.GetTransaction(id);
        //    if (transaction == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(transaction);
        //}
        //[HttpPost]
        //public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
        //{
        //    await _transactionService.AddTransaction(transaction);
        //    return StatusCode(StatusCodes.Status201Created);
        //}
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateTransaction(int id, [FromBody] Transaction transaction)
        //{
        //    await _transactionService.UpdateTransaction(id, transaction);
        //    return Ok();
        //}
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTransaction(int id)
        //{
        //    await _transactionService.DeleteTransaction(id);
        //    return Ok();
        //}
    }
}
