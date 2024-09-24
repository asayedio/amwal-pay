import { Component } from '@angular/core';
import { TransactionService } from '../../services/transaction.service';

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.sass'],
})
export class TransactionComponent {
  transactionData = {
    ProcessingCode: '',
    SystemTraceNr: '',
    FunctionCode: '',
    CardNo: '',
    CardHolder: '',
    AmountTrxn: '',
    CurrencyCode: '',
  };
  response: any;

  constructor(private transactionService: TransactionService) {}

  async submitTransaction() {
    try {
      this.response = await this.transactionService.processTransaction(
        this.transactionData
      );
    } catch (error) {
      alert('Error processing transaction');
    }
  }
}
