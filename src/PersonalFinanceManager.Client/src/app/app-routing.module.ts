import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TransactionFormComponent } from './components/transaction-form/transaction-form.component';

const routes: Routes = [
  { path: 'transactions/add', component: TransactionFormComponent },
  { path: 'transactions/edit/:id', component: TransactionFormComponent },
  { path: '**', redirectTo: 'transactions/add' }, // default route
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
