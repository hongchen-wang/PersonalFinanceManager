import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TransactionFormComponent } from './components/transaction-form/transaction-form.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [AppComponent, TransactionFormComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent, TransactionFormComponent],
})
export class AppModule {}
