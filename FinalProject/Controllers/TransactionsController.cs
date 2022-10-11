using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly FinalProjectContext _context;

        public TransactionsController(FinalProjectContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransaction()
        {
            return await _context.Transaction.Include(t => t.ToAccount).Include(t => t.FromAccount).ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transaction.Include(t => t.ToAccount).Include(t => t.FromAccount).FirstAsync(t => t.TransactionId == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        [HttpGet("Account/{accountid}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByAccount(int accountid)
        {
            return await _context.Transaction.Include(t => t.ToAccount).Include(t => t.FromAccount).Where(t => t.ToAccount.AccountId == accountid || t.FromAccount.AccountId == accountid).ToListAsync();
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            if (transaction.ToAccount != null)
            {
                if (transaction.ToAccount.AccountId == 0)
                {
                    transaction.ToAccount = null;
                }
                else
                {

                    var toaccount = await _context.Account.Include(a => a.Customer).FirstAsync(a => a.AccountId == transaction.ToAccount.AccountId);

                    if (toaccount == null)
                    {
                        return NotFound();
                    }

                    transaction.ToAccount = toaccount;
                }

            }

            if (transaction.FromAccount != null)
            {
                if (transaction.FromAccount.AccountId == 0)
                {
                    transaction.FromAccount = null;
                }
                else
                {

                    var fromaccount = await _context.Account.Include(a => a.Customer).FirstAsync(a => a.AccountId == transaction.FromAccount.AccountId);

                    if (fromaccount == null)
                    {
                        return NotFound();
                    }

                    transaction.FromAccount = fromaccount;
                }

            }

            if (transaction.FromAccount != null && (transaction.FromAccount.CurrAmount < transaction.Amount))
            {
                return BadRequest();
            }
            else if (transaction.FromAccount != null)
            {
                transaction.FromAccount.CurrAmount -= transaction.Amount;
            }

            if (transaction.ToAccount != null)
            {
                transaction.ToAccount.CurrAmount += transaction.Amount;
            }

            _context.Transaction.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaction", new { id = transaction.TransactionId }, transaction);
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.TransactionId == id);
        }
    }
}
