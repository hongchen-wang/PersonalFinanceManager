import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Transaction } from 'src/app/models/transaction.model';
import { TransactionService } from 'src/app/services/transaction.service';

@Component({
  selector: 'app-transaction-form',
  templateUrl: './transaction-form.component.html',
  styleUrls: ['./transaction-form.component.css'],
})
export class TransactionFormComponent implements OnInit {
  transactionForm!: FormGroup;
  transactionId?: number;

  constructor(
    private fb: FormBuilder,
    private transactionService: TransactionService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.transactionForm = this.fb.group({
      description: ['', Validators.required],
      amount: [0, [Validators.required, Validators.min(0.01)]], // multi-validator case: FIELD_KEY: [INITIAL_VALUE, [LIST_OF_VALIDATORS]]
      date: ['', Validators.required],
      categoryId: [null, Validators.required],
    });

    this.transactionId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.transactionId) {
      this.transactionService
        .getTransactionById(this.transactionId)
        .subscribe((transaction) => {
          this.transactionForm.patchValue(transaction);
        });
    }
  }

  onSubmit(): void {
    if (this.transactionForm.invalid) {
      return;
    }
    const transaction: Transaction = this.transactionForm.value;

    if (this.transactionId) {
      //update existing transaction
      this.transactionService.updateTransaction(transaction).subscribe(() => {
        this.router.navigate(['/transactions']);
      });
    } else {
      // add new transaction
      this.transactionService.addTransaction(transaction).subscribe(() => {
        this.router.navigate(['/transactions']);
      });
    }
  }
}
